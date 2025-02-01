using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Repositories.Categories
{
    public class CategoryRepository : GenericRepository<Category, int>, ICategoryRepository
    {
        public CategoryRepository(AppDbContext context) : base(context)
        {
        }
        public async  Task<Category?> GetCategoryWithProductsAsync(int id)
        {
            var category = await Context.Categories.Include(x => x.Products).SingleOrDefaultAsync(x => x.Id == id);

            return category;
        }

        public IQueryable<Category> GetCategoryWithProducts()
        {
            var categories = Context.Categories.Include(x => x.Products).AsQueryable();

            return categories;
        }

    }
}
