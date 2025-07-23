using Ecommerce.Data;
using Ecommerce.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Ecommerce.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class CartController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();

        public void SaveCartToCookies(HttpContext context, Cart cart)
        {
            var json = JsonConvert.SerializeObject(cart);
            context.Response.Cookies.Append("cart", json, new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(7),
                IsEssential = true
            });

        }
        public Cart GetCartFromCookies(HttpContext context)
        {
            if (context.Request.Cookies.TryGetValue("cart", out string json))
            {

                return JsonConvert.DeserializeObject<Cart>(json);
            }
            return new Cart(); 
        }
        public IActionResult Cart() {
            var cart = GetCartFromCookies(HttpContext);
            ViewBag.Products = context.Products.ToList();
            ViewBag.Categories = context.Categories.ToList();
            ViewBag.Total = cart.TotalAmount.ToString();
            return View(cart);
        }
        public IActionResult AddToCart(int Id)
        {
            var product = context.Products.FirstOrDefault(p => p.Id == Id);

            var cart = GetCartFromCookies(HttpContext);

            var existingItem = cart.Items.FirstOrDefault(i => i.Id == Id);

            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Items.Add(new CartItem
                {
                    Id = Id,
                    Name = product.Name,
                    UnitPrice = product.Price,
                    Quantity = 1,
                    ImageUrl = product.Image
                });
                ViewBag.Total = cart.TotalAmount.ToString();
            }

                SaveCartToCookies(HttpContext, cart);
                return RedirectToAction("Index", "Home");

            }
        

        public IActionResult RemoveFromCart(int Id)
        {
            var cart = GetCartFromCookies(HttpContext);
            var itemToRemove = cart.Items.FirstOrDefault(i => i.Id == Id);
            if (itemToRemove != null)
            {
                if (itemToRemove.Quantity > 1)
                { 
                    itemToRemove.Quantity--;
                }
                else
                {
 
                    cart.Items.Remove(itemToRemove);
                    ViewBag.Total = cart.TotalAmount.ToString();
                }
                SaveCartToCookies(HttpContext, cart);
            }
            return RedirectToAction("Cart");
        }
       
            

    }

}
