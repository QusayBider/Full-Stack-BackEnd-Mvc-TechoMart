using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductsController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult ShowProducts()
        {
            var products=context.Products.ToList();
            return View(products);
        }
        [HttpGet]
        public IActionResult CreateProduct() { 
            ViewBag.Categories=context.Categories.ToList();
            return View(new Product());
        }
        [HttpPost]
        public IActionResult CreateProduct(Product request,IFormFile Image) {
            if (ModelState.IsValid)
            {
                var imageSerives = new ImageServices();
                var fileName = imageSerives.UploadFile(Image);
                request.Image = fileName;
                context.Products.Add(request);
                context.SaveChanges();
                return RedirectToAction("ShowProducts");
            }
            else {
                ViewBag.Categories = context.Categories.ToList();

                return View(request);
            }
            

        }
        public IActionResult DeleteProduct(int id)
        {
            var product = context.Products.FirstOrDefault(x=>x.Id == id);

                var imageServices = new ImageServices();
                var imageName = product.Image;
                if (imageName != null)
                {
                    imageServices.DeleteFile(imageName);
            }
            context.Products.Remove(product);
                context.SaveChanges();
            
            return RedirectToAction("ShowProducts");
        }
        [HttpGet]
        public IActionResult ViewEditProduct(int id)
        {
            var product = context.Products.FirstOrDefault(product => product.Id == id);
            ViewBag.Categories = context.Categories.ToList();
            return View(product);
        }
        [HttpPost]
        public IActionResult EditProduct(Product request, IFormFile Image) {

            var ProductImageName= context.Products.AsNoTracking().FirstOrDefault(x=>x.Id==request.Id).Image;

            if (Image is not null)
            {
                var imageServices = new ImageServices();
                imageServices.DeleteFile(ProductImageName);
                var fileName = imageServices.UploadFile(Image);
                request.Image = fileName;
                context.Products.Update(request);
                context.SaveChanges();
                return RedirectToAction("ShowProducts");
            }
            else { 
                request.Image = ProductImageName;
                context.Products.Update(request);
                context.SaveChanges();
                return RedirectToAction("ShowProducts");
            }
            
        }
    }
}
