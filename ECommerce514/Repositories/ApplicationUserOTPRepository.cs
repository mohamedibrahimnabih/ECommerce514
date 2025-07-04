using ECommerce514.Models;
using ECommerce514.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ECommerce514.Repositories
{
    public class ApplicationUserOTPRepository : Repository<ApplicationUserOTP>, IApplicationUserOTPRepository
    {

        public ApplicationUserOTPRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
