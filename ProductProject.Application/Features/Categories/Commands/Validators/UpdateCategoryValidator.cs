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
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.CategoryId)
                .GreaterThan(0).WithMessage("Category ID must be greater than zero.")
                .WithErrorCode("400")
                .DependentRules(() =>
                {
                    RuleFor(x => x.CategoryId)
                        .MustAsync(async (categoryId, cancellationToken) =>
                            await _categoryService.IsExistAsync(c => c.CategoryId == categoryId))
                        .WithMessage("Category ID does not exist.")
                        .WithErrorCode("404");
                });


            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required.")
                .WithErrorCode("400")
                .DependentRules(() =>
                {
                    RuleFor(x => x.CategoryName)
                        .MustAsync(async (model, categoryName, cancellationToken) =>
                        !await _categoryService.IsExistAsync(c => c.CategoryName.ToLower() == categoryName.ToLower() && c.CategoryId != model.CategoryId))
                        .WithMessage(x => $"Category name '{x.CategoryName}' already exists.")
                        .WithErrorCode("409");
                });


            RuleFor(x => x.ParentCategoryId)
                 .MustAsync(async (parentCategoryId, cancellationToken) =>
                 !parentCategoryId.HasValue || await _categoryService.IsExistAsync(c => c.CategoryId == parentCategoryId.Value))
                 .WithMessage("Parent category ID does not exist.")
                 .WithErrorCode("404");
        }

    }
}
