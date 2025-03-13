using ProductProject.Application.Features.Products.Commands.Models;
using ProductProjrect.Data.Entities;

namespace ProductProject.Application.Mappings.Products
{
    public partial class ProductProfile
    {
        public void UpdateProductMapper() => CreateMap<UpdateProductCommand, Product>();
    }
}