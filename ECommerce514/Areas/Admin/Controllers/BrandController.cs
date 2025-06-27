using ECommerce514.Data;
using ECommerce514.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce514.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {
        private readonly IBrandRepository _brandRepository;

        //private ApplicationDbContext _context = new();

        public BrandController(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _brandRepository.GetAsync();

            return View(result);
        }

        public IActionResult Create()
        {
            return View(new Brand());
        }

        [HttpPost]
        public async Task<IActionResult> Create(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View(brand);
            }

            await _brandRepository.CreateAsync(brand);

            TempData["success-notification"] = "Add Brand Successfully";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var brand = await _brandRepository.GetOneAsync(e => e.Id == id);

            if(brand is not null)
            {
                return View(brand);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Brand brand)
        {
            if (!ModelState.IsValid)
            {
                return View(brand);
            }

            await _brandRepository.UpdateAsync(brand);

            TempData["success-notification"] = "Update Brand Successfully";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var brand = await _brandRepository.GetOneAsync(e => e.Id == id);

            if (brand is not null)
            {
                await _brandRepository.DeleteAsync(brand);

                TempData["success-notification"] = "Delete Brand Successfully";

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}
