using FluentValidation;
using ProductProject.Application.Features.Products.Commands.Models;
using ProductProject.Application.ServiceInterfaces;
using System.Net;

namespace ProductProject.Application.Features.Products.Commands.Validators
{
    public class UpdateProductValidator : AbstractValidator<UpdateProductCommand>
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public UpdateProductValidator(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {

            RuleFor(x => x.ProductId)
                .GreaterThan(0).WithMessage("Product ID must be greater than 0.")
                .WithErrorCode("400")
                .DependentRules(() =>
                {
                    RuleFor(x => x.ProductId)
                    .MustAsync(async (productId, cancellationToken) =>
                    await _productService.IsExistAsync(productId))
                    .WithMessage(x => $"Product ID: {x.ProductId} does not exist.")
                    .WithErrorCode("404");
                });

            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required!")
                .WithErrorCode("400")
                .DependentRules(() =>
                {
                    RuleFor(x => x.ProductName)
                        .MustAsync(async (model, productName, cancellationToken) =>
                            !await _productService.IsExistAsync(p => p.ProductName.ToLower() == productName.ToLower() && p.ProductId != model.ProductId))
                        .WithMessage(x => $"Product name: '{x.ProductName}' already exists.")
                        .WithErrorCode("409");
                });

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0!")
                .WithErrorCode(HttpStatusCode.BadRequest.ToString())
                .LessThanOrEqualTo(1_000_000).WithMessage("Price must not exceed 1,000,000!")
                .WithErrorCode("400");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative!")
                .WithErrorCode("400");

            RuleFor(x => x.CategoryIds)
                .NotEmpty().WithMessage("At least one category is required.")
                .WithErrorCode("400")
                .DependentRules(() =>
                {
                    RuleFor(x => x.CategoryIds)
                        .MustAsync(async (categoryIds, cancellationToken) =>
                            await _categoryService.DoAllCategoryIdsExistAsync(categoryIds))
                        .WithMessage("One or more provided categories doesn't exist.")
                        .WithErrorCode("404");
                });
        }
    }
}