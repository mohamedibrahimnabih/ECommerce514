using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace ECommerce514.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private ApplicationDbContext _context = new();
        private CategoryRepository _categoryRepository = new();

        public async Task<IActionResult> Index()
        {
            var result = await _categoryRepository.GetAsync();

            return View(result);
        }

        public IActionResult Create()
        {
            return View(new Category());
        }

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

        public IActionResult Edit(int id)
        {
            var category = _categoryRepository.GetOne(e => e.Id == id);

            if(category is not null)
            {
                return View(category);
            }

            return NotFound();
        }

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

        public async Task<IActionResult> Delete(int id)
        {
            var category = _categoryRepository.GetOne(e => e.Id == id);

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
