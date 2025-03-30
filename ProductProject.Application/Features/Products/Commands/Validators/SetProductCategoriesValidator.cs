using FluentValidation;
using ProductProject.Application.Features.Products.Commands.Models;
using ProductProject.Application.ServiceInterfaces;

namespace ProductProject.Application.Features.Products.Commands.Validators
{
    public class SetProductCategoriesValidator : AbstractValidator<SetProductCategoriesCommand>
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;

        public SetProductCategoriesValidator(IProductService productService, ICategoryService categoryService)
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