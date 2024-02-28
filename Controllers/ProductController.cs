using EpiScarpe_Co.Models;
using Microsoft.AspNetCore.Mvc;

namespace EpiScarpe_Co.Controllers
{
    public class ProductController : Controller
    {
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
    }
}
