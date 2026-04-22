using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Reflection;
using Tender_parsing.Models;
using Tender_parsing.Services;

namespace Tender_parsing.Controllers
{
    public class HomeController : Controller
    {
        readonly ILogger<HomeController> _logger;
        readonly ITenderService _tenderService;

        public HomeController(ILogger<HomeController> logger, ITenderService tenderService)
        {
            _logger = logger;
            _tenderService = tenderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Search(TenderSearchViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", model);
            }

            try
            {
                return RedirectToAction("Details", new { id = model.TenderId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при поиске тендера {TenderId}", model.TenderId);
                TempData["ErrorMessage"] = "Произошла ошибка при получении данных. Пожалуйста, попробуйте позже.";
                return RedirectToAction(nameof(Index));
            }
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> DetailsAsync(string id)
        {
            try
            {
                var tenderInfo = await _tenderService.GetCompleteTenderInfoAsync(id);

                if (tenderInfo == null || tenderInfo.BasicInfo == null)
                {
                    TempData["ErrorMessage"] = $"Тендер №{id} не найден";
                    return RedirectToAction(nameof(Index));
                }

                return View(tenderInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке деталей тендера {Id}", id);
                TempData["ErrorMessage"] = "Ошибка при загрузке данных";
                return RedirectToAction(nameof(Index));
            }
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
