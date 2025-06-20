using ECommerce514.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

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
        public async Task<IActionResult> Create(Product product, IFormFile mainImg)
        {
            if (mainImg is not null && mainImg.Length > 0)
            {
                // Save Img in wwwroot
                var fileName = Guid.NewGuid().ToString() + Path.GetExtension(mainImg.FileName);
                var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                using (var stream = System.IO.File.Create(filePath))
                {
                    await mainImg.CopyToAsync(stream);
                }

                // Save Img in DB
                product.MainImg = fileName;
            }

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
        public async Task<IActionResult> Edit(Product product, IFormFile mainImg)
        {
            //var result = _context.Products.Any(e => e.ProductId == product.ProductId);

            //if(result)
            //{

            var productInDB = _context.Products.AsNoTracking().FirstOrDefault(e=>e.ProductId == product.ProductId);

            if(productInDB is not null)
            {
                if (mainImg is not null && mainImg.Length > 0)
                {
                    // Save new Img in wwwroot
                    var fileName = Guid.NewGuid().ToString() + Path.GetExtension(mainImg.FileName);
                    var filePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", fileName);

                    using (var stream = System.IO.File.Create(filePath))
                    {
                        await mainImg.CopyToAsync(stream);
                    }

                    // Delete old Img in wwwroot
                    var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\images", productInDB.MainImg);
                    if (System.IO.File.Exists(oldFilePath))
                    {
                        System.IO.File.Delete(oldFilePath);
                    }

                    // Update Img in DB
                    product.MainImg = fileName;
                }
                else
                {
                    product.MainImg = productInDB.MainImg;
                }

                _context.Products.Update(product);
                _context.SaveChanges();

                TempData["success-notification"] = "Update Product Successfully";

                return RedirectToAction(nameof(Index));
            }

            return NotFound();

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
