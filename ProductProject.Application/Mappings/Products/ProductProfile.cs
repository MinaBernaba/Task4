using AutoMapper;

namespace ProductProject.Application.Mappings.Products
{
    public partial class ProductProfile : Profile
    {
        public ProductProfile()
        {
            AddProductMapper();
            UpdateProductMapper();
        }
    }
}