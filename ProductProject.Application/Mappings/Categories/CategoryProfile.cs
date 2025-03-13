using AutoMapper;

namespace ProductProject.Application.Mappings.Categories
{
    public partial class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            AddCategoryMapper();
            UpdateCategoryMapper();
            GetAllCategoriesMapper();
        }

    }
}