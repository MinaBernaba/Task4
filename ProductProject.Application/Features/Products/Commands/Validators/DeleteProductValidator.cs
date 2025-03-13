using FluentValidation;
using ProductProject.Application.Features.Products.Commands.Models;
using ProductProject.Application.ServiceInterfaces;

namespace ProductProject.Application.Features.Products.Commands.Validators
{
    public class DeleteProductValidator : AbstractValidator<DeleteProductCommand>
    {
        private readonly IProductService _productService;

        public DeleteProductValidator(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }
        private void ApplyValidationRules()
        {


        }

        private void ApplyCustomValidationRules()
        {
            RuleFor(x => x.ProductId)
                .MustAsync(async (productId, cancellationToken) =>
                await _productService.IsExistAsync(productId))
                .WithMessage(x => $"Product ID: {x.ProductId} does not exist.");
        }
    }
}