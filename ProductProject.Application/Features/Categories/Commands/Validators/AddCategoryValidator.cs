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
            _categoryService = categoryService;
            ApplyValidationRules();
        }

        public void ApplyValidationRules()
        {
            RuleFor(x => x.CategoryName)
                .NotEmpty().WithMessage("Category name is required!")
                .WithErrorCode("400")
                .DependentRules(() =>
                {
                    RuleFor(x => x.CategoryName)
                       .MustAsync(async (categoryName, cancellationToken) =>
                       !await _categoryService.IsExistAsync(x => x.CategoryName.ToLower() == categoryName.ToLower()))
                       .WithMessage(x => $"Category name: {x.CategoryName} already exists.")
                       .WithErrorCode("409");
                });

            RuleFor(x => x.ParentCategoryId)
               .MustAsync(async (parentCategoryId, cancellationToken) =>
               !parentCategoryId.HasValue || await _categoryService.IsExistAsync(c => c.CategoryId == parentCategoryId.Value))
               .WithMessage(x => $"Parent category ID: {x.ParentCategoryId} does not exist.")
               .WithErrorCode("404");
        }
    }
}