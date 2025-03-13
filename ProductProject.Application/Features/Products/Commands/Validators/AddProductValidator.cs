using FluentValidation;
using ProductProject.Application.Features.Products.Commands.Models;
using ProductProject.Application.ServiceInterfaces;

namespace ProductProject.Application.Features.Products.Commands.Validators
{
    public class AddProductValidator : AbstractValidator<AddProductCommand>
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public AddProductValidator(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            _categoryService = categoryService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.ProductName)
                .NotEmpty().WithMessage("Product name is required!")
                .NotNull().WithMessage("Product name is required!");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0!")
                .LessThanOrEqualTo(1_000_000).WithMessage("Price must not exceed 1,000,000!");

            RuleFor(x => x.StockQuantity)
                .GreaterThanOrEqualTo(0).WithMessage("Stock quantity cannot be negative!");

            RuleFor(x => x.CategoryIds)
          .NotEmpty().WithMessage("At least one category is required!");
        }

        private void ApplyCustomValidationRules()
        {
            RuleFor(x => x.ProductName)
                .MustAsync(async (productName, cancellationToken) =>
                    !await _productService.IsExistAsync(p => p.ProductName.ToLower() == productName.ToLower()))
                .WithMessage(x => $"Product name: '{x.ProductName}' already exists.");

            RuleFor(x => x.CategoryIds)
                .MustAsync(async (categoryIds, cancellationToken) =>
                   await _categoryService.DoAllCategoryIdsExistAsync(categoryIds))
                .WithMessage("One or more provided categories do not exist.");

        }
    }

}