using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace ECommerce514.Utility.DBInitilizer
{
    public class DBInitializer : IDBInitializer
    {
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public DBInitializer(ApplicationDbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
        }

        public void Initialize()
        {
            if(_context.Database.GetPendingMigrations().Any())
            {
                _context.Database.Migrate();
            }

            if(_roleManager.Roles.IsNullOrEmpty() /*&& _userManager.Users.IsNullOrEmpty()*/)
            {
                _roleManager.CreateAsync(new(SD.SuperAdmin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new(SD.Admin)).GetAwaiter().GetResult(); ;
                _roleManager.CreateAsync(new(SD.Employee)).GetAwaiter().GetResult(); ;
                _roleManager.CreateAsync(new(SD.Company)).GetAwaiter().GetResult(); ;
                _roleManager.CreateAsync(new(SD.Customer)).GetAwaiter().GetResult(); ;

                _userManager.CreateAsync(new()
                {
                    FirstName = "Super",
                    LastName = "Admin",
                    UserName = "SuperAdmin",
                    Email = "SuperAdmin@eraasoft.com",
                    EmailConfirmed = true,
                }, "Admin123$").GetAwaiter().GetResult(); ;

                var user = _userManager.FindByNameAsync("SuperAdmin").GetAwaiter().GetResult();

                _userManager.AddToRoleAsync(user, SD.SuperAdmin).GetAwaiter().GetResult();
            }
        }
    }
}
