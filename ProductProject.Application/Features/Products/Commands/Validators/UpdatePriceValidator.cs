using FluentValidation;
using ProductProject.Application.Features.Products.Commands.Models;
using ProductProject.Application.ServiceInterfaces;
using System.Net;

namespace ProductProject.Application.Features.Products.Commands.Validators
{
    public class UpdatePriceValidator : AbstractValidator<UpdatePriceCommand>
    {
        private readonly IProductService _productService;

        public UpdatePriceValidator(IProductService productService, ICategoryService categoryService)
        {
            _productService = productService;
            ApplyValidationRules();
        }
        private void ApplyValidationRules()
        {
            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Price must be greater than 0!")
                .WithErrorCode(HttpStatusCode.BadRequest.ToString())
                .LessThanOrEqualTo(1_000_000).WithMessage("Price must not exceed 1,000,000!")
                .WithErrorCode("400");


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

        }
    }
}