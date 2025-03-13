using FluentValidation;
using ProductProject.Application.Features.Orders.Commands.Models;
using ProductProject.Application.ServiceInterfaces;
using ProductProjrect.Data.Helper;

namespace ProductProject.Application.Features.Orders.Commands.Validators
{
    public class CompleteOrderValidator : AbstractValidator<CompleteOrderCommand>
    {
        private readonly IOrderService _orderService;

        public CompleteOrderValidator(IOrderService orderService)
        {
            _orderService = orderService;
            ApplyValidationRules();
            ApplyCustomValidationRules();
        }

        private void ApplyValidationRules()
        {
            RuleFor(x => x.OrderId)
                .GreaterThan(0).WithMessage("Invalid Order ID!");
        }

        private void ApplyCustomValidationRules()
        {
            RuleFor(x => x.OrderId)
                .MustAsync(async (orderId, cancellationToken) =>
                    await _orderService.IsExistAsync(orderId))
                .WithMessage(x => $"Order ID: {x.OrderId} does not exist.");

            RuleFor(x => x.OrderId)
                .MustAsync(async (orderId, cancellationToken) =>
                {
                    var order = await _orderService.GetByIdAsync(orderId);
                    return order != null && order.Status == enOrderStatus.Pending;
                })
                .WithMessage(x => $"Order ID: {x.OrderId} cannot be completed as it is either cancelled or already completed.");
        }
    }

}
