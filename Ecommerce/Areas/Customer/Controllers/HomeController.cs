using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Ecommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        ApplicationDbContext context = new ApplicationDbContext();
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }
        public Cart GetCartFromCookies(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue("cart", out string json))
            {

                return JsonConvert.DeserializeObject<Cart>(json);
            }
            return new Cart();
        }

        public IActionResult Index()
        {
            var cart = GetCartFromCookies(HttpContext);
            ViewBag.Total = cart.TotalAmount.ToString();

            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Products = context.Products.ToList();
            return View();
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
        public IActionResult viewProductByCategory(int id)
        {
            ViewBag.Category = context.Categories.FirstOrDefault(c => c.Id == id);
            var products = context.Products.Where(p => p.CategoryId == id).ToList();
            if (products.Count == 0)
            {
                ViewBag.Message = "No products found in this category.";
            }
            else
            {
                ViewBag.Message = null;
            }
            ViewBag.Products = products;
            return View();
        }
        public IActionResult ViewProductsDetiles(int id)
        {
            var product = context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                ViewBag.Message = "Product not found.";
            }
            ViewBag.Product = product;
            return View();
        }
        public IActionResult Cart() { 
            return View();
        }
    }
}
