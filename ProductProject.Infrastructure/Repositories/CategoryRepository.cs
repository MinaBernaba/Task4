using ProductProject.Infrastructure.Context;
using ProductProject.Infrastructure.Interfaces;
using ProductProjrect.Data.Entities;

namespace ProductProject.Infrastructure.Repositories
{
    public class CategoryRepository : GenericRepository<Category>, ICategoryRepository
    {
        public CategoryRepository(ApplicationDbContext context) : base(context) { }
    }
}
