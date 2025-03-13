using Microsoft.EntityFrameworkCore;
using ProductProject.Application.Features.Categories.Queries.Responses;
using ProductProject.Application.ServiceInterfaces;
using ProductProject.Infrastructure.Interfaces;
using ProductProjrect.Data.Entities;
using System.Linq.Expressions;
using System.Text;

namespace ProductProject.Application.Services
{
    public class CategoryService(IUnitOfWork _unitOfWork) : ICategoryService
    {
        public IQueryable<Category> GetAll()
            => _unitOfWork.Categories.GetAllAsNoTracking();

        public async Task<IEnumerable<Category>> GetAllAsync()
            => await _unitOfWork.Categories.GetAllAsync();

        public async Task<IEnumerable<GetAllCategoriesResponse>> GetAllCategoriesMainInfoAsync()
        {
            var categories = await GetAll()
                .Select(category => new GetAllCategoriesResponse()
                {
                    CategoryId = category.CategoryId,
                    CategoryName = category.CategoryName,
                }).ToListAsync();

            return categories;
        }

        public async Task<GetCategoryDetailedInfoResponse> GetCategoryDetailedInfoAsync(int categoryId)
        {
            var category = await GetByIdAsync(categoryId);


            var stringBuilder = new StringBuilder();

            // Recursively get the full category path by StringBuilder
            await BuildCategoryPathRecursivelyAsync(category.CategoryName, category.ParentCategoryId, stringBuilder);

            string categoryHierarchy = stringBuilder.ToString();

            return new GetCategoryDetailedInfoResponse()
            {
                CategoryHierarchy = categoryHierarchy,
                CategoryName = category.CategoryName,
                CategoryId = category.CategoryId
            };
        }

        private async Task BuildCategoryPathRecursivelyAsync(string categoryName, int? parentCategoryId, StringBuilder stringBuilder)
        {
            if (parentCategoryId.HasValue)
            {
                var parentCategory = await GetAll()
                    .Where(c => c.CategoryId == parentCategoryId)
                    .Select(c => new { c.CategoryName, c.ParentCategoryId })
                    .FirstOrDefaultAsync();

                if (parentCategory != null)
                {
                    await BuildCategoryPathRecursivelyAsync(parentCategory.CategoryName, parentCategory.ParentCategoryId, stringBuilder);
                    stringBuilder.Append(" > ");
                }
            }

            stringBuilder.Append(categoryName);
        }

        public async Task<Category> GetByIdAsync(int categoryId)
            => await _unitOfWork.Categories.GetByIdAsync(categoryId);

        public async Task<bool> DoAllCategoryIdsExistAsync(HashSet<int> categoryIds)
        {
            int existingCount = await GetAll()
                       .Where(c => categoryIds.Contains(c.CategoryId))
                       .CountAsync();
            return existingCount == categoryIds.Count;
        }

        public async Task<bool> IsExistAsync(int categoryId)
           => await _unitOfWork.Categories.IsExistAsync(x => x.CategoryId == categoryId);

        public async Task<bool> IsExistAsync(params Expression<Func<Category, bool>>[] conditions)
            => await _unitOfWork.Categories.IsExistAsync(conditions);

        #region crud
        public async Task<bool> AddAsync(Category category)
        {
            await _unitOfWork.Categories.AddAsync(category);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> AddRangeAsync(IEnumerable<Category> categories)
        {
            await _unitOfWork.Categories.AddRangeAsync(categories);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateAsync(Category category)
        {
            _unitOfWork.Categories.Update(category);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateRangeAsync(IEnumerable<Category> categories)
        {
            _unitOfWork.Categories.UpdateRange(categories);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteAsync(int categoryId)
        {
            var entity = await GetByIdAsync(categoryId);
            _unitOfWork.Categories.Delete(entity);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRangeAsync(HashSet<int> categoryIds)
        {
            var categories = await GetAll()
                .Where(c => categoryIds.Contains(c.CategoryId))
                .ToListAsync();

            _unitOfWork.Categories.DeleteRange(categories);
            return await _unitOfWork.SaveChangesAsync() > 0;
        }
        #endregion
    }
}
