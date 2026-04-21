using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Tender_parsing.Models;
using Tender_parsing.Services;

namespace Tender_parsing.Controllers
{
    public class HomeController : Controller
    {
        readonly IMarketMosregApiClient _tenderService;
        readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger, IMarketMosregApiClient tenderService)
        {
            _logger = logger;
            _tenderService = tenderService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public async Task<IActionResult> Test(string id)
        {
            var ans = await _tenderService.GetBasicTenderInfoAsync(id);
            var html = await _tenderService.GetTradePageHtmlAsync(id);
            var documents = await _tenderService.GetTenderDocumentsAsync(id);
            return Ok(documents);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
