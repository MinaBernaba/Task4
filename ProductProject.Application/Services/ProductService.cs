using Microsoft.EntityFrameworkCore;
using ProductProject.Application.Features.Products.Commands.Responses;
using ProductProject.Application.Features.Products.Queries.Responses;
using ProductProject.Application.ServiceInterfaces;
using ProductProject.Infrastructure.Interfaces;
using ProductProjrect.Data.Entities;
using System.Linq.Expressions;

namespace ProductProject.Application.Services
{
    public class ProductService(IUnitOfWork _unitOfWork) : IProductService
    {
        public IQueryable<Product> GetAll()
            => _unitOfWork.Products.GetAllAsNoTracking();
        public async Task<IEnumerable<Product>> GetAllAsync()
            => await _unitOfWork.Products.GetAllAsync();

        public async Task<Product> GetByIdAsync(int productId)
            => await _unitOfWork.Products.GetByIdAsync(productId);

        public async Task<List<Product>> GetByIdsAsync(HashSet<int> productIds)
            => await GetAll()
            .Where(x => productIds.Contains(x.ProductId))
            .ToListAsync();
        public async Task<IEnumerable<ProductInfoResponse>> GetAllProductsInfoAsync()
        {
            var productsInfo = await GetAll()
                .Select(x => new ProductInfoResponse
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    StockQuantity = x.StockQuantity,
                    Categories = x.ProductCategories.Select(x => x.Category.CategoryName).ToHashSet()
                }).ToListAsync();

            return productsInfo;
        }

        public async Task<ProductInfoResponse> GetProductInfoAsync(int productId)
        {
            var productInfo = await GetAll()
                .Where(x => x.ProductId == productId)
                .Select(x => new ProductInfoResponse
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    Price = x.Price,
                    StockQuantity = x.StockQuantity,
                    Categories = x.ProductCategories.Select(x => x.Category.CategoryName).ToHashSet()
                }).SingleAsync();

            return productInfo;
        }

        public async Task<bool> IsExistAsync(int productId)
            => await _unitOfWork.Products.IsExistAsync(x => x.ProductId == productId);

        public async Task<bool> IsExistAsync(params Expression<Func<Product, bool>>[] conditions)
            => await _unitOfWork.Products.IsExistAsync(conditions);

        public async Task<SetProductCategoriesResponse?> SetProductCategoriesAsync(int productId, HashSet<int> newCategoryIds)
        {
            await _unitOfWork.BeginTransactionAsync();

            // Get current product category IDs from the database
            var currentCategoryIdsOfTheProduct = _unitOfWork.ProductCategories
                .GetAllAsNoTracking()
                .Where(pc => pc.ProductId == productId)
                .Select(pc => pc.CategoryId)
                .ToHashSet();

            // Categories to remove: current IDs not in the new set
            var categoryIdsToRemove = currentCategoryIdsOfTheProduct.Except(newCategoryIds).ToHashSet();

            // Categories to add: new IDs not in current set
            var categoriesToAdd = newCategoryIds
                .Except(currentCategoryIdsOfTheProduct)
                .Select(categoryId => new ProductCategory { ProductId = productId, CategoryId = categoryId })
                .ToHashSet();

            // Remove categories if any
            if (categoryIdsToRemove.Count != 0)
            {
                // Create placeholder entities with keys and mark for deletion
                var categoriesToRemove = _unitOfWork.ProductCategories
                    .GetAllAsNoTracking()
                    .Where(pc => pc.ProductId == productId && categoryIdsToRemove.Contains(pc.CategoryId))
                    .ToHashSet();
                _unitOfWork.ProductCategories.DeleteRange(categoriesToRemove);
            }

            // Add new categories if any
            if (categoriesToAdd.Count != 0)
                await _unitOfWork.ProductCategories.AddRangeAsync(categoriesToAdd);

            // Save changes and populate response
            if (await _unitOfWork.CommitTransactionAsync())
            {
                return new SetProductCategoriesResponse()
                {
                    AddedCategories = categoriesToAdd.Select(pc => pc.CategoryId).ToHashSet(),
                    RemovedCategories = categoryIdsToRemove
                };
            }
            else
                return null;
        }

        #region crud

        public async Task<bool> AddAsync(Product product, HashSet<int> categoryIds)
        {
            if (!await AddAsync(product))
                return false;

            HashSet<ProductCategory> productCategories = new HashSet<ProductCategory>();
            foreach (var categoryId in categoryIds)
            {
                productCategories.Add(new ProductCategory
                {
                    ProductId = product.ProductId,
                    CategoryId = categoryId
                });
            }
            await _unitOfWork.ProductCategories.AddRangeAsync(productCategories);

            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Product product, HashSet<int> categoryIds)
        {
            await _unitOfWork.BeginTransactionAsync();

            // Get existing category IDs
            var existingCategoryIds = product.ProductCategories.Select(pc => pc.CategoryId).ToHashSet();


            // Find categories to remove
            var categoriesToRemove = product.ProductCategories
                .Where(pc => !categoryIds.Contains(pc.CategoryId))
                .ToList();

            // Remove old categories
            _unitOfWork.ProductCategories.DeleteRange(categoriesToRemove);

            // Find categories to add
            var categoriesToAdd = categoryIds.Except(existingCategoryIds)
                .Select(categoryId => new ProductCategory
                {
                    ProductId = product.ProductId,
                    CategoryId = categoryId
                }).ToList();

            // Add new categories
            await _unitOfWork.ProductCategories.AddRangeAsync(categoriesToAdd);

            _unitOfWork.Products.Update(product);

            return await _unitOfWork.CommitTransactionAsync();
        }

        public async Task<bool> AddAsync(Product product)
        {
            await _unitOfWork.Products.AddAsync(product);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<Product> products)
        {
            await _unitOfWork.Products.AddRangeAsync(products);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Product product)
        {
            _unitOfWork.Products.Update(product);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<Product> products)
        {
            _unitOfWork.Products.UpdateRange(products);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int productId)
        {
            await _unitOfWork.BeginTransactionAsync();

            var product = await _unitOfWork.Products.GetByIdAsync(productId);

            // Remove related categories from junction table
            var productCategories = product.ProductCategories.ToList();

            if (productCategories.Count != 0)
                _unitOfWork.ProductCategories.DeleteRange(productCategories);

            // Delete product
            _unitOfWork.Products.Delete(product);

            return await _unitOfWork.CommitTransactionAsync();
        }

        public async Task<bool> DeleteRangeAsync(HashSet<int> productIds)
        {
            await _unitOfWork.BeginTransactionAsync();

            var products = await GetAll().Where(p => productIds.Contains(p.ProductId)).ToListAsync();

            // Collect all related ProductCategory records
            var productCategories = products.SelectMany(p => p.ProductCategories).ToList();

            _unitOfWork.ProductCategories.DeleteRange(productCategories);

            // Delete products
            _unitOfWork.Products.DeleteRange(products);

            return await _unitOfWork.CommitTransactionAsync();
        }

        #endregion
    }
}
