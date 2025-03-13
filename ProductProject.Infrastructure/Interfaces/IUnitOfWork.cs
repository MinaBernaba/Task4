using Microsoft.EntityFrameworkCore.Storage;

namespace ProductProject.Infrastructure.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IProductRepository Products { get; }
        ICategoryRepository Categories { get; }
        IOrderRepository Orders { get; }
        IOrderDetailRepository OrderDetails { get; }
        IProductCategoryRepository ProductCategories { get; }
        Task<int> SaveChangesAsync();
        Task<IDbContextTransaction> BeginTransactionAsync();
        Task<bool> CommitTransactionAsync();
        Task RollBackAsync();
    }
}
