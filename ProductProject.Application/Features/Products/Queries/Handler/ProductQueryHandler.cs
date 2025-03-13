using BooksManagementSystem.Application.ResponseBase;
using MediatR;
using ProductProject.Application.Features.Products.Queries.Models;
using ProductProject.Application.Features.Products.Queries.Responses;
using ProductProject.Application.ServiceInterfaces;

namespace ProductProject.Application.Features.Products.Queries.Handler
{
    public class ProductQueryHandler(IProductService _productService) : ResponseHandler,
        IRequestHandler<GetAllProductsQuery, Response<IEnumerable<ProductInfoResponse>>>,
        IRequestHandler<GetProductInfoQuery, Response<ProductInfoResponse>>
    {
        public async Task<Response<IEnumerable<ProductInfoResponse>>> Handle(GetAllProductsQuery request, CancellationToken cancellationToken)
        {
            var response = await _productService.GetAllProductsInfoAsync();
            return Success(response);
        }

        public async Task<Response<ProductInfoResponse>> Handle(GetProductInfoQuery request, CancellationToken cancellationToken)
        {
            if (!await _productService.IsExistAsync(request.ProductId))
                return NotFound<ProductInfoResponse>($"No product with ID: {request.ProductId} exists.");

            var response = await _productService.GetProductInfoAsync(request.ProductId);
            return Success(response);
        }
    }
}
