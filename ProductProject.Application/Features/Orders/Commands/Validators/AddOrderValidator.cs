using FluentValidation;
using FluentValidation.Results;
using ProductProject.Application.Features.Orders.Commands.Models;
using ProductProject.Application.ServiceInterfaces;
using ProductProjrect.Data.Entities;

namespace ProductProject.Application.Features.Orders.Commands.Validators
{
    public class AddOrderValidator : AbstractValidator<AddOrderCommand>
    {
        private readonly IProductService _productService;

        public AddOrderValidator(IProductService productService)
        {
            _productService = productService;
            ApplyValidationRules();
        }

        public override async Task<ValidationResult> ValidateAsync(ValidationContext<AddOrderCommand> context, CancellationToken cancellation = default)
        {
            var command = context.InstanceToValidate;
            var productIds = command.OrderDetails.Select(orderDetail => orderDetail.ProductId).ToHashSet();
            var products = await _productService.GetByIdsAsync(productIds);
            context.RootContextData["Products"] = products;
            return await base.ValidateAsync(context, cancellation);
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.OrderDetails)
                .NotEmpty().WithMessage("At least one order detail is required!")
                .WithErrorCode("400")
                .DependentRules(() =>
                {
                    RuleForEach(x => x.OrderDetails).ChildRules(orderDetail =>
                    {
                        orderDetail.RuleFor(x => x.ProductId)
                            .GreaterThan(0).WithMessage("Product ID must be greater than zero.")
                            .WithErrorCode("400")
                            .DependentRules(() =>
                            {
                                orderDetail.RuleFor(x => x.ProductId)
                                    .Must((orderDetail, productId, context) =>
                                    {
                                        var products = context.RootContextData["Products"] as List<Product>;
                                        return products?.Any(p => p.ProductId == productId) ?? false;
                                    })
                                    .WithMessage(x => $"Product ID: {x.ProductId} does not exist.")
                                    .WithErrorCode("404")
                                    .DependentRules(() =>
                                    {
                                        // Quantity validation, dependent on ProductId existence
                                        orderDetail.RuleFor(x => x.Quantity)
                                            .GreaterThan(0).WithMessage("Quantity must be greater than zero.")
                                            .WithErrorCode("400")
                                            .Must((orderDetail, quantity, context) =>
                                            {
                                                var products = context.RootContextData["Products"] as List<Product>;
                                                var product = products!.First(p => p.ProductId == orderDetail.ProductId);
                                                return quantity <= product.StockQuantity;
                                            })
                                            .WithMessage(x => $"Product ID: {x.ProductId} does not have enough stock.")
                                            .WithErrorCode("409");
                                    });
                            });
                    });
                });
        }
    }

}