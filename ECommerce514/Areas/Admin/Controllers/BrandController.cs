using ECommerce514.Data;
using ECommerce514.Models;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce514.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private ApplicationDbContext _context = new();

        public IActionResult Index()
        {
            var result = _context.Brands;

            return View(result.ToList());
        }

        public IActionResult Create()
        {
            return View(new Brand());
        }

        [HttpPost]
        public IActionResult Create(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View(brand);
            }

            _context.Brands.Add(brand);
            _context.SaveChanges();

            TempData["success-notification"] = "Add Brand Successfully";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int id)
        {
            var brand = _context.Brands.Find(id);

            if(brand is not null)
            {
                return View(brand);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View(brand);
            }

            _context.Brands.Update(brand);
            _context.SaveChanges();

            TempData["success-notification"] = "Update Brand Successfully";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var brand = _context.Brands.Find(id);

            if (brand is not null)
            {
                _context.Brands.Remove(brand);
                _context.SaveChanges();

                TempData["success-notification"] = "Delete Brand Successfully";

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}
