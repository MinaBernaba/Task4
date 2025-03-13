using ProductProject.Application.Features.Categories.Queries.Responses;
using ProductProjrect.Data.Entities;

namespace ProductProject.Application.ServiceInterfaces
{
    public interface ICategoryService : IBaseService<Category>
    {
        Task<IEnumerable<GetAllCategoriesResponse>> GetAllCategoriesMainInfoAsync();
        Task<GetCategoryDetailedInfoResponse> GetCategoryDetailedInfoAsync(int id);
        Task<bool> DoAllCategoryIdsExistAsync(HashSet<int> categoryIds);
    }
}
