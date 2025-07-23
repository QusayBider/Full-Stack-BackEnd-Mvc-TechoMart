using Azure.Core;
using Ecommerce.Data;
using Ecommerce.Models;
using Ecommerce.Services;
using Microsoft.AspNetCore.Components.Sections;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoriesController : Controller
    {
        ApplicationDbContext context = new ApplicationDbContext();
        public IActionResult ShowCategories()
        {
            var categories = context.Categories.ToList();
            return View(categories);
        }
        public IActionResult DeleteCategory(int id) { 
            var remove_category = context.Categories.FirstOrDefault(x => x.Id == id);
            if (remove_category is not null)
            {
                var imageServices = new ImageServices();
                var imageName = remove_category.Image;
                imageServices.DeleteFile(imageName);
                context.Categories.Remove(remove_category);
                context.SaveChanges();
               
            }
            return RedirectToAction("ShowCategories");

        }
        public IActionResult ViewEditCategory(int id){
            var category = context.Categories.FirstOrDefault(category => category.Id == id);
                return View(category);
        }
        public IActionResult UpdateCategory(Category request,IFormFile Image){
            ModelState.Remove("Image");
            var category = context.Categories.AsNoTracking().FirstOrDefault(c => c.Id == request.Id);
            if (ModelState.IsValid && !context.Categories.Any(c => c.Name == request.Name && c.Id != request.Id) && (Image is null))
            {
                request.Image = category.Image;
                Console.WriteLine(request.Name, request.Id);
                context.Categories.Update(request);
                context.SaveChanges();
                return RedirectToAction("ShowCategories");
            }
            else if (ModelState.IsValid && !context.Categories.Any(c => c.Name == request.Name && c.Id != request.Id) && (Image is not null)) { 
                var imageServices = new ImageServices();
                var fileName = imageServices.UploadFile(Image);
                request.Image = fileName;
                context.Categories.Update(request);
                context.SaveChanges();
                return RedirectToAction("ShowCategories");
            }
            else
            {
                ModelState.AddModelError("Name", "The Name is exiest");
                return View("ViewEditCategory", request);
            }
        }
        public IActionResult CreateCategory() { 
            return View(new Category());
        }
        public IActionResult AddCategory(Category request , IFormFile Image) {
            if (ModelState.IsValid&& !context.Categories.Any(c => c.Name == request.Name))
            {
                var imageSerives = new ImageServices();
                var fileName = imageSerives.UploadFile(Image);
                request.Image = fileName;
                context.Categories.Add(request);
                context.SaveChanges();
                return RedirectToAction("ShowCategories");
            }
            else {

                ModelState.AddModelError("Name","The Category exist");
                return View("CreateCategory",request);
            }
        }

    }
}
