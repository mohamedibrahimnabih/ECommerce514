using ECommerce514.Data;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce514.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private ApplicationDbContext _context = new();

        public IActionResult Index()
        {
            var result = _context.Categories;

            return View(result.ToList());
        }

        public IActionResult Create()
        {
            return View(new Category());
        }

        [HttpPost]
        public IActionResult Create(Category category)
        {
            if(!ModelState.IsValid)
            {
                return View(category);
            }

            _context.Categories.Add(category);
            _context.SaveChanges();

            TempData["success-notification"] = "Add Category Successfully";

            //Response.Cookies.Append("success-notification", "Add Category Successfully");

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var category = _context.Categories.Find(id);

            if(category is not null)
            {
                return View(category);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            _context.Categories.Update(category);
            _context.SaveChanges();

            TempData["success-notification"] = "Update Category Successfully";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var category = _context.Categories.Find(id);

            if (category is not null)
            {
                _context.Categories.Remove(category);
                _context.SaveChanges();

                TempData["success-notification"] = "Delete Category Successfully";

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}
