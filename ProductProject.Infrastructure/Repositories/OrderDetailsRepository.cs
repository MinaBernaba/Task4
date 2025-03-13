using ProductProject.Infrastructure.Context;
using ProductProject.Infrastructure.Interfaces;
using ProductProjrect.Data.Entities;

namespace ProductProject.Infrastructure.Repositories
{
    public class OrderDetailRepository : GenericRepository<OrderDetail>, IOrderDetailRepository
    {
        public OrderDetailRepository(ApplicationDbContext context) : base(context) { }
    }
}
