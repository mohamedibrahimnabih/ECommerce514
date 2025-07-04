using ECommerce514.Utility;
using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ECommerce514.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{SD.SuperAdmin},{SD.Admin}")]
    public class UserController : Controller
    {
        //private ApplicationDbContext _context = new();
        private UserManager<ApplicationUser> _userManager;// = new CategoryRepository();
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }
        

        public async Task<IActionResult> Index()
        {
            var result = _userManager.Users.ToList();

            Dictionary<ApplicationUser, string> keyValuePairs = new();

            foreach (var item in result)
            {
                keyValuePairs.Add(item, String.Join(",", await _userManager.GetRolesAsync(item)));
            }

            return View(keyValuePairs.ToDictionary());
        }



        public async Task<IActionResult> Edit(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            var registerVM = user.Adapt<AdminRegisterVM>();

            if (user is not null)
            {
                registerVM.Roles = _roleManager.Roles.Select(e=>new SelectListItem()
                {
                    Text = e.Name,
                    Value = e.Name
                }).ToList();
                return View(registerVM);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminRegisterVM adminRegisterVM, List<string> roles)
        {
            if (!ModelState.IsValid)
            {
                return View(adminRegisterVM);
            }

            var user = await _userManager.FindByIdAsync(adminRegisterVM.Id);

            if(user is not null)
            {
                user.FirstName = adminRegisterVM.FirstName;
                user.LastName = adminRegisterVM.LastName;
                user.UserName = adminRegisterVM.UserName;
                user.Email = adminRegisterVM.Email;
                user.Address = adminRegisterVM.Address;
                await _userManager.UpdateAsync(user);

                var userRoles = await _userManager.GetRolesAsync(user);

                await _userManager.RemoveFromRolesAsync(user, userRoles);

                await _userManager.AddToRolesAsync(user, roles);

                TempData["success-notification"] = "Update Data Successfully";
            }

            return RedirectToAction(nameof(Index));
        }


        public async Task<IActionResult> Delete(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is not null)
            {
                await _userManager.DeleteAsync(user);

                TempData["success-notification"] = "Delete User Data Successfully";

                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }

        public async Task<IActionResult> LockUnLock(string id)
        {
            var user = await _userManager.FindByIdAsync(id);

            if (user is not null)
            {
                if(user.LockoutEnabled)
                {
                    user.LockoutEnabled = false;
                    user.LockoutEnd = DateTime.UtcNow.AddMonths(1);
                    TempData["success-notification"] = "Block User Successfully";
                }
                else
                {
                    user.LockoutEnabled = true;
                    user.LockoutEnd = null;
                    TempData["success-notification"] = "UnBlock User Successfully";
                }

                await _userManager.UpdateAsync(user);
                return RedirectToAction(nameof(Index));
            }

            return NotFound();
        }
    }
}
