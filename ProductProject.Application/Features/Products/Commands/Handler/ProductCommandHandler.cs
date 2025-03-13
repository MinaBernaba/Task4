using AutoMapper;
using BooksManagementSystem.Application.ResponseBase;
using MediatR;
using ProductProject.Application.Features.Products.Commands.Models;
using ProductProject.Application.Features.Products.Commands.Responses;
using ProductProject.Application.ServiceInterfaces;
using ProductProjrect.Data.Entities;

namespace ProductProject.Application.Features.Products.Commands.Handler
{
    public class ProductCommandHandler(IProductService _productService, IMapper _mapper) : ResponseHandler,
        IRequestHandler<AddProductCommand, Response<string>>,
        IRequestHandler<UpdateProductCommand, Response<string>>,
        IRequestHandler<UpdatePriceCommand, Response<string>>,
        IRequestHandler<SetProductCategoriesCommand, Response<SetProductCategoriesResponse>>,
        IRequestHandler<DeleteProductCommand, Response<string>>
    {
        public async Task<Response<string>> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            var product = _mapper.Map<Product>(request);

            bool isAdded = await _productService.AddAsync(product, request.CategoryIds);
            if (!isAdded)
                return BadRequest<string>("Failed to add product. there was an error.");

            return Success($"Product added successfully with ID: {product.ProductId}.");
        }

        public async Task<Response<string>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(request.ProductId);

            _mapper.Map(request, product);

            bool isUpdated = await _productService.UpdateAsync(product, request.CategoryIds);

            if (!isUpdated)
                return BadRequest<string>("Failed to update product. there was an error.");

            return Success($"Product with ID: {product.ProductId} updated successfully.");

        }

        public async Task<Response<string>> Handle(UpdatePriceCommand request, CancellationToken cancellationToken)
        {
            var product = await _productService.GetByIdAsync(request.ProductId);

            product.Price = request.Price;

            bool isUpdated = await _productService.UpdateAsync(product);

            if (!isUpdated)
                return BadRequest<string>("Failed to update product. there was an error.");

            return Success($"The price of product with ID: {product.ProductId} updated to {product.Price} successfully.");
        }

        public async Task<Response<SetProductCategoriesResponse>> Handle(SetProductCategoriesCommand request, CancellationToken cancellationToken)
        {
            var response = await _productService.SetProductCategoriesAsync(request.ProductId, request.CategoryIds);

            if (response == null)
                return BadRequest<SetProductCategoriesResponse>($"Failed to update categories of the product. there was an error.");

            return Success(response, $"Categories of product ID: {request.ProductId} updated successfully.");

        }

        public async Task<Response<string>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            bool isDeleted = await _productService.DeleteAsync(request.ProductId);

            if (!isDeleted)
                return BadRequest<string>("Failed to delete product. there was an error.");

            return Success($"Product with ID: {request.ProductId} deleted successfully.");
        }
    }
}
