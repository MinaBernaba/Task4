using ProductProject.Application.Features.Products.Commands.Responses;
using ProductProject.Application.Features.Products.Queries.Responses;
using ProductProjrect.Data.Entities;

namespace ProductProject.Application.ServiceInterfaces
{
    public interface IProductService : IBaseService<Product>
    {
        Task<bool> AddAsync(Product product, HashSet<int> categoryIds);
        Task<bool> UpdateAsync(Product product, HashSet<int> categoryIds);
        Task<SetProductCategoriesResponse?> SetProductCategoriesAsync(int productId, HashSet<int> NewCategoryIds);
        Task<IEnumerable<ProductInfoResponse>> GetAllProductsInfoAsync();
        Task<ProductInfoResponse> GetProductInfoAsync(int productId);
    }
}
