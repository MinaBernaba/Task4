using FluentValidation;
using ProductProject.Application.Features.Categories.Commands.Models;
using ProductProject.Application.ServiceInterfaces;

namespace ProductProject.Application.Features.Categories.Commands.Validators
{
    public class UpdateCategoryValidator : AbstractValidator<UpdateCategoryCommand>
    {
        private readonly ICategoryService _categoryService;

        public UpdateCategoryValidator(ICategoryService categoryService)
        {
            _categoryService = categoryService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID must be greater than zero.")
                .NotNull().WithMessage("Category ID is required!")
                .NotEmpty().WithMessage("Category ID is required!");

            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required!")
                .NotNull().WithMessage("Category name is required!");
        }

        public void ApplyCustomValidationRules()
        {
            RuleFor(x => x.CategoryId)
                .MustAsync(async (categoryId, cancellationToken) =>
                    await _categoryService.IsExistAsync(c => c.CategoryId == categoryId))
                .WithMessage("Category ID does not exist!");

            RuleFor(x => x.CategoryName)
                .MustAsync(async (model, categoryName, cancellationToken) =>
                    !await _categoryService.IsExistAsync(c => c.CategoryName.ToLower() == categoryName.ToLower() && c.CategoryId != model.CategoryId))
                .WithMessage(x => $"Category name: {x.CategoryName} already exists.");

            RuleFor(x => x.ParentCategoryId)
                .MustAsync(async (parentCategoryId, cancellationToken) =>
                    !parentCategoryId.HasValue || await _categoryService.IsExistAsync(c => c.CategoryId == parentCategoryId.Value))
                .WithMessage("Parent category ID must exist if provided.");
        }
    }
}
