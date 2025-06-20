using ECommerce514.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace ECommerce514.Repository
{
    public class CategoryRepository
    {
        private ApplicationDbContext _context = new();

        // CRUD
        public async Task<bool> CreateAsync(Category category)
        {
            try
            {
                await _context.AddAsync(category);
                await _context.SaveChangesAsync();

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Ex: {ex}");
                return false;
            } 
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            try
            {
                _context.Update(category);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex: {ex}");
                return false;
            }
        }

        public async Task<bool> DeleteAsync(Category category)
        {
            try
            {
                _context.Remove(category);
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex: {ex}");
                return false;
            }
        }

        public async Task<IEnumerable<Category>> GetAsync(Expression<Func<Category, bool>>? expression = null, Expression<Func<Category, object>>[]? includes = null, bool tracked = true)
        {
            IQueryable<Category> categories = _context.Categories;

            if (expression is not null)
            {
                categories = categories.Where(expression);
            }

            if (includes is not null)
            {
                foreach (var item in includes)
                {
                    categories = categories.Include(item);
                }
            }

            if(!tracked)
            {
                categories = categories.AsNoTracking();
            }

            return (await categories.ToListAsync());
        }

        public async Task<Category?> GetOneAsync(Expression<Func<Category, bool>>? expression = null, Expression<Func<Category, object>>[]? includes = null, bool tracked = true)
        {
            return (await GetAsync(expression, includes, tracked)).FirstOrDefault();
        }

        public async Task<bool> CommitAsync()
        {
            try
            {
                await _context.SaveChangesAsync();

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ex: {ex}");
                return false;
            }
        }
    }
}
