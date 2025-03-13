using ProductProject.Infrastructure.Context;
using ProductProject.Infrastructure.Interfaces;
using ProductProjrect.Data.Entities;

namespace ProductProject.Infrastructure.Repositories
{
    public class ProductCategoryRepository : GenericRepository<ProductCategory>, IProductCategoryRepository
    {
        public ProductCategoryRepository(ApplicationDbContext context) : base(context) { }
    }
}
