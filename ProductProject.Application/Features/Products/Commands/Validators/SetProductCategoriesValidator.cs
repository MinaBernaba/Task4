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

            RuleFor(x => x.CategoryIds)
                .MustAsync(async (categoryIds, cancellationToken) =>
                   await _categoryService.DoAllCategoryIdsExistAsync(categoryIds))
                .WithMessage("One or more provided categories do not exist.");
        }
    }
}