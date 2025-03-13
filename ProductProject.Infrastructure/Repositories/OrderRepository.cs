using ProductProject.Infrastructure.Context;
using ProductProject.Infrastructure.Interfaces;
using ProductProjrect.Data.Entities;

namespace ProductProject.Infrastructure.Repositories
{
    public class OrderRepository : GenericRepository<Order>, IOrderRepository
    {
        public OrderRepository(ApplicationDbContext context) : base(context) { }
    }
}
