using ECommerce514.Models;
using ECommerce514.Repositories.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ECommerce514.Repositories
{
    public class CartRepository : Repository<Cart>, ICartRepository
    {

        public CartRepository(ApplicationDbContext context) : base(context)
        {
        }
    }
}
