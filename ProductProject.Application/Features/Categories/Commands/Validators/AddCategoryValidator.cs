using FluentValidation;
using ProductProject.Application.Features.Categories.Commands.Models;
using ProductProject.Application.ServiceInterfaces;

namespace ProductProject.Application.Features.Categories.Commands.Validators
{
    public class AddCategoryValidator : AbstractValidator<AddCategoryCommand>
    {
        private readonly ICategoryService _categoryService;

        public AddCategoryValidator(ICategoryService categoryService)
        {
            ApplyValidationRules();
            ApplyCustomValidationRules();
            _categoryService = categoryService;
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required!")
                .NotNull().WithMessage("Category name is required!");

        }
        public void ApplyCustomValidationRules()
        {
            RuleFor(x => x.CategoryName)
               .MustAsync(async (categoryName, cancellationToken) =>
               !await _categoryService.IsExistAsync(x => x.CategoryName.ToLower() == categoryName.ToLower()))
               .WithMessage(x => $"Category name: {x.CategoryName} already exists.");

            RuleFor(x => x.ParentCategoryId)
                .MustAsync(async (parentCategoryId, cancellationToken) =>
                !parentCategoryId.HasValue || await _categoryService.IsExistAsync(c => c.CategoryId == parentCategoryId.Value))
                .WithMessage("Parent category ID must exist if provided.");

        }
    }
}