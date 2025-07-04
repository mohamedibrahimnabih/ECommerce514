using ECommerce514.Repositories.IRepositories;
using ECommerce514.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce514.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private ApplicationDbContext _context = new();
        private ICategoryRepository _categoryRepository;// = new CategoryRepository();

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _categoryRepository.GetAsync();

            return View(result);
        }

        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
        public IActionResult Create()
        {
            return View(new Category());
        }

        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            if(!ModelState.IsValid)
            {
                return View(category);
            }

            await _categoryRepository.CreateAsync(category);

            TempData["success-notification"] = "Add Category Successfully";

            //Response.Cookies.Append("success-notification", "Add Category Successfully");

            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

        public async Task<IActionResult> Edit(int id)
        {
            var category = await _categoryRepository.GetOneAsync(e => e.Id == id);

            if(category is not null)
            {
                return View(category);
            }

            return NotFound();
        }

        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            if (!ModelState.IsValid)
            {
                return View(category);
            }

            await _categoryRepository.UpdateAsync(category);

            TempData["success-notification"] = "Update Category Successfully";

            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]

        public async Task<IActionResult> Delete(int id)
        {
            var category = await _categoryRepository.GetOneAsync(e => e.Id == id);

            if (category is not null)
            {
                await _categoryRepository.DeleteAsync(category);

                TempData["success-notification"] = "Delete Category Successfully";

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}
