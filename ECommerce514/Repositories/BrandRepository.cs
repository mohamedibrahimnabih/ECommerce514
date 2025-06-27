using ECommerce514.Models;
using ECommerce514.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ECommerce514.Repositories
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {

        public BrandRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
