using Microsoft.EntityFrameworkCore.Storage;
using ProductProject.Infrastructure.Context;
using ProductProject.Infrastructure.Interfaces;

namespace ProductProject.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private bool _disposed = false;
        private readonly ApplicationDbContext _context;

        private readonly Lazy<IProductRepository> _products;
        private readonly Lazy<ICategoryRepository> _categories;
        private readonly Lazy<IOrderRepository> _orders;
        private readonly Lazy<IOrderDetailRepository> _orderDetails;
        private readonly Lazy<IProductCategoryRepository> _productCategories;

        public IProductRepository Products => _products.Value;
        public ICategoryRepository Categories => _categories.Value;
        public IOrderRepository Orders => _orders.Value;
        public IOrderDetailRepository OrderDetails => _orderDetails.Value;
        public IProductCategoryRepository ProductCategories => _productCategories.Value;
        public UnitOfWork(ApplicationDbContext context)
        {
            _context = context;
            _products = new Lazy<IProductRepository>(() => new ProductRepository(_context));
            _categories = new Lazy<ICategoryRepository>(() => new CategoryRepository(_context));
            _orders = new Lazy<IOrderRepository>(() => new OrderRepository(_context));
            _orderDetails = new Lazy<IOrderDetailRepository>(() => new OrderDetailRepository(_context));
            _productCategories = new Lazy<IProductCategoryRepository>(() => new ProductCategoryRepository(_context));
        }
        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public async Task<IDbContextTransaction> BeginTransactionAsync() => await _context.Database.BeginTransactionAsync();
        public async Task<bool> CommitTransactionAsync()
        {
            if (_context.Database.CurrentTransaction != null)
            {
                bool isSaved = await SaveChangesAsync() > 0;
                if (!isSaved) return false;

                await _context.Database.CommitTransactionAsync();
                return true;
            }
            return false;
        }
        public async Task RollBackAsync()
        {
            if (_context.Database.CurrentTransaction != null)
                await _context.Database.RollbackTransactionAsync();
        }

        public void Dispose()
        {
            if (!_disposed)
            {
                _context.Dispose();
                _disposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}