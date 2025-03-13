using ProductProject.Application.Features.Categories.Queries.Responses;
using ProductProjrect.Data.Entities;

namespace ProductProject.Application.Mappings.Categories
{
    public partial class CategoryProfile
    {
        public void GetAllCategoriesMapper() => CreateMap<Category, GetAllCategoriesResponse>();
    }
}