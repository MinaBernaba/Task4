using ProductProject.Infrastructure.Context;
using ProductProject.Infrastructure.Interfaces;
using ProductProjrect.Data.Entities;

namespace ProductProject.Infrastructure.Repositories
{
    public class ProductRepository : GenericRepository<Product>, IProductRepository
    {
        public ProductRepository(ApplicationDbContext context) : base(context) { }
    }
}
