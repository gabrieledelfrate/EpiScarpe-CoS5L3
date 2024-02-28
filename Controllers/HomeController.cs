using EpiScarpe_Co.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace EpiScarpe_Co.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public ActionResult Index()
        {
            var displayedProducts = ProductRepository.GetAllProducts().Where(p => p.IsDisplayedOnHomePage).ToList();
            return View(displayedProducts);
        }

        public ActionResult Details(int id)
        {
            var product = ProductRepository.GetAllProducts().FirstOrDefault(p => p.Id == id);
            return View(product);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
