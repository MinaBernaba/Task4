using ProductProject.Application.Features.Categories.Commands.Models;
using ProductProjrect.Data.Entities;

namespace ProductProject.Application.Mappings.Categories
{
    public partial class CategoryProfile
    {
        public void UpdateCategoryMapper() => CreateMap<UpdateCategoryCommand, Category>();
    }
}