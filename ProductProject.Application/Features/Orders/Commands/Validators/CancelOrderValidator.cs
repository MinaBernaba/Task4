using FluentValidation;
using ProductProject.Application.Features.Orders.Commands.Models;
using ProductProject.Application.ServiceInterfaces;
using ProductProjrect.Data.Helper;

namespace ProductProject.Application.Features.Orders.Commands.Validators
{
    public class CancelOrderValidator : AbstractValidator<CancelOrderCommand>
    {
        private readonly IOrderService _orderService;

        public CancelOrderValidator(IOrderService orderService)
        {
            _orderService = orderService;
            ApplyValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("Order ID must be greater than zero.")
                .WithErrorCode("400")
                .DependentRules(() =>
                {
                    RuleFor(x => x.OrderId)
                        .MustAsync(async (orderId, cancellationToken) =>
                            await _orderService.IsExistAsync(orderId))
                        .WithMessage(x => $"Order ID: {x.OrderId} does not exist.")
                        .WithErrorCode("404")
                        .DependentRules(() =>
                        {
                            RuleFor(x => x.OrderId)
                                .MustAsync(async (orderId, cancellationToken) =>
                                {
                                    var order = await _orderService.GetByIdAsync(orderId);
                                    return order.Status == enOrderStatus.Pending;
                                })
                                .WithMessage(x => $"Order ID: {x.OrderId} can't be cancelled as it is either completed or already cancelled.")
                                .WithErrorCode("409");
                        });
                });
        }
    }

}
