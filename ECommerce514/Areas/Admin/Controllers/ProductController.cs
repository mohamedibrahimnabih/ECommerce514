using ECommerce514.Data;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce514.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private ApplicationDbContext _context = new();

        public IActionResult Index()
        {
            var result = _context.Products;

            return View(result.ToList());
        }

        public IActionResult Create()
        {
            var categories = _context.Categories;
            var brands = _context.Brands;

            CategoriesWithBrandsVM categoriesWithBrandsVM = new()
            {
                Categories = categories.ToList(),
                Brands = brands.ToList()
            };

            return View(categoriesWithBrandsVM);
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            _context.Products.Add(product);
            _context.SaveChanges();

            TempData["success-notification"] = "Add Product Successfully";

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Edit(int productId)
        {
            var product = _context.Products.Find(productId);

            if(product is not null)
            {
                var categories = _context.Categories;
                var brands = _context.Brands;

                CategoriesWithBrandsWithProductVM categoriesWithBrandsWithProductVM = new()
                {
                    Product = product,
                    Categories = categories.ToList(),
                    Brands = brands.ToList()
                };

                return View(categoriesWithBrandsWithProductVM);
            }

            return NotFound();
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            //var result = _context.Products.Any(e => e.ProductId == product.ProductId);

            //if(result)
            //{
                _context.Products.Update(product);
                _context.SaveChanges();

                TempData["success-notification"] = "Remove Product Successfully";

                return RedirectToAction(nameof(Index));
            //}

            //return NotFound();
        }

        public IActionResult Delete(int id)
        {
            var product = _context.Products.Find(id);

            if (product is not null)
            {
                _context.Products.Remove(product);
                _context.SaveChanges();

                TempData["success-notification"] = "Delete Product Successfully";

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}
