using FluentValidation;
using ProductProject.Application.Features.Orders.Commands.Models;
using ProductProject.Application.ServiceInterfaces;

namespace ProductProject.Application.Features.Orders.Commands.Validators
{
    public class AddOrderValidator : AbstractValidator<AddOrderCommand>
    {
        private readonly IProductService _productService;

        public AddOrderValidator(IProductService productService)
        {
            _productService = productService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.OrderDetails)
                .NotEmpty().WithMessage("At least one order detail is required!");

            RuleForEach(x => x.OrderDetails).ChildRules(orderDetail =>
            {
                orderDetail.RuleFor(x => x.ProductId)
                    .GreaterThan(0).WithMessage("Invalid Product ID!");

                orderDetail.RuleFor(x => x.Quantity)
                    .GreaterThan(0).WithMessage("Quantity must be greater than zero!");
            });
        }

        private void ApplyCustomValidationRules()
        {
            RuleForEach(x => x.OrderDetails).ChildRules(orderDetail =>
            {
                orderDetail.RuleFor(x => x.ProductId)
                    .MustAsync(async (productId, cancellationToken) =>
                        await _productService.IsExistAsync(productId))
                    .WithMessage(x => $"Product ID: {x.ProductId} does not exist.");

                orderDetail.RuleFor(x => x.Quantity)
                    .MustAsync(async (orderDetail, quantity, cancellationToken) =>
                    {
                        var product = await _productService.GetByIdAsync(orderDetail.ProductId);
                        return product != null && quantity <= product.StockQuantity;
                    })
                    .WithMessage(x => $"Product ID: {x.ProductId} does not have enough stock.");
            });
        }
    }

}
