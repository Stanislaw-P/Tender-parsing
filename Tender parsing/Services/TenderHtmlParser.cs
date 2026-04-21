using HtmlAgilityPack;
using System.Globalization;
using System.Text.RegularExpressions;
using Tender_parsing.Models;

namespace Tender_parsing.Services
{
    public class TenderHtmlParser : ITenderHtmlParser
    {
        readonly ILogger<TenderHtmlParser> _logger;

        public TenderHtmlParser(ILogger<TenderHtmlParser> logger)
        {
            _logger = logger;
        }

        public Task<TenderAdditionalInfo> ParseAdditionalInfoAsync(string html)
        {
            var result = new TenderAdditionalInfo();

            try
            {
                var doc = new HtmlDocument();
                doc.LoadHtml(html);

                // Парсинг места поставки
                result.DeliveryPlace = ParseDeliveryPlace(doc);

                // Парсинг состава лота
                result.LotItems = ParseLotItems(doc);

                _logger.LogInformation("Successfully parsed {LotCount} lot items", result.LotItems.Count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing tender HTML");
            }

            return Task.FromResult(result);
        }

        private List<LotItem> ParseLotItems(HtmlDocument doc)
        {
            var lotItems = new List<LotItem>();

            try
            {
                // Ищем все блоки с классом outputResults__oneResult (каждый блок = один лот)
                var lotBlocks = doc.DocumentNode.SelectNodes(
                    "//div[contains(@class, 'outputResults__oneResult')]");

                if (lotBlocks == null || !lotBlocks.Any())
                {
                    _logger.LogWarning("No lot blocks found");
                    return lotItems;
                }

                foreach (var lotBlock in lotBlocks)
                {
                    var item = new LotItem();

                    // 2.1 Наименование товара
                    var nameNode = lotBlock.SelectSingleNode(
                        ".//div[contains(@class, 'leftPart')]//p[contains(@class, 'leftPart__parag')]/span[contains(@class, 'grayText') and contains(text(), 'Наименование')]/..");

                    if (nameNode != null)
                    {
                        var nameText = nameNode.InnerText?.Replace("Наименование товара, работ, услуг:", "")?.Trim();
                        if (!string.IsNullOrEmpty(nameText))
                            item.Name = HtmlEntity.DeEntitize(nameText);
                    }

                    // Альтернативный способ - если структура другая
                    //if (string.IsNullOrEmpty(item.Name))
                    //{
                    //    var altNameNode = lotBlock.SelectSingleNode(
                    //        ".//div[contains(@class, 'leftPart')]//p[contains(@class, 'leftPart__parag')]");
                    //    if (altNameNode != null)
                    //    {
                    //        var text = altNameNode.InnerText?.Trim();
                    //        if (!string.IsNullOrEmpty(text))
                    //        {
                    //            // Убираем возможные префиксы
                    //            text = Regex.Replace(text, @"^[^:]+:\s*", "");
                    //            item.Name = HtmlEntity.DeEntitize(text);
                    //        }
                    //    }
                    //}

                    // 2.2 Единица измерения
                    var unitNode = lotBlock.SelectSingleNode(
                        ".//div[contains(@class, 'centerPart')]//p[contains(@class, 'centerPart__contentResult-parag')]/span[contains(@class, 'grayText') and contains(text(), 'Единицы измерения')]/..");

                    if (unitNode != null)
                    {
                        var unitText = unitNode.InnerText?.Replace("Единицы измерения:", "")?.Trim();
                        if (!string.IsNullOrEmpty(unitText))
                            item.Unit = HtmlEntity.DeEntitize(unitText);
                    }

                    // 2.3 Количество
                    var quantityNode = lotBlock.SelectSingleNode(
                        ".//div[contains(@class, 'centerPart')]//p[contains(@class, 'centerPart__contentResult-parag')]/span[contains(@class, 'grayText') and contains(text(), 'Количество')]/..");

                    if (quantityNode != null)
                    {
                        var quantityText = quantityNode.InnerText?.Replace("Количество:", "")?.Trim();
                        item.Quantity = ParseDecimal(quantityText);
                    }

                    // 2.4 Цена за единицу
                    var priceNode = lotBlock.SelectSingleNode(
                        ".//div[contains(@class, 'rightPart')]//p[contains(@class, 'rightPart__contentResult-parag')]/span[contains(@class, 'grayText') and contains(text(), 'Стоимость единицы')]/..");

                    if (priceNode != null)
                    {
                        var priceText = priceNode.InnerText?.Replace("Стоимость единицы продукции ( в т.ч. НДС при наличии):", "")?.Trim();
                        item.UnitPrice = ParseDecimal(priceText);
                    }

                    // Добавляем только если есть наименование
                    if (!string.IsNullOrEmpty(item.Name))
                    {
                        lotItems.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error parsing lot items");
            }

            return lotItems;
        }

        private string ParseDeliveryPlace(HtmlDocument doc)
        {
            var addressBlock = doc.DocumentNode.SelectSingleNode(
                "//div[contains(@class, 'informationAboutCustomer__data-infoBlock')]" +
                "[.//span[contains(text(), 'Адрес места нахождения')]]" +
                "//p[contains(@class, 'infoBlock__text')]");

            if (addressBlock != null)
            {
                var address = HtmlEntity.DeEntitize(addressBlock.InnerText).Trim();
                if (!string.IsNullOrEmpty(address) && address != "-")
                {
                    return address;
                }
            }

            return "Не указано (вероятно, адрес заказчика)";
        }

        private decimal ParseDecimal(string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return 0;

            var cleaned = value.Replace(" ", "").Replace(",", ".");
            if (decimal.TryParse(cleaned, NumberStyles.Any, CultureInfo.InvariantCulture, out var result))
                return result;

            return 0;
        }
    }
}
