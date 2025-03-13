using BooksManagementSystem.Application.ResponseBase;
using MediatR;
using ProductProject.Application.Features.Orders.Commands.Models;
using ProductProject.Application.ServiceInterfaces;
using ProductProjrect.Data.Helper;

namespace ProductProject.Application.Features.Orders.Commands.Handler
{
    public class OrderCommandHandler(IOrderService _orderService) : ResponseHandler,
        IRequestHandler<AddOrderCommand, Response<string>>,
        IRequestHandler<CompleteOrderCommand, Response<string>>,
        IRequestHandler<CancelOrderCommand, Response<string>>

    {
        public async Task<Response<string>> Handle(AddOrderCommand request, CancellationToken cancellationToken)
        {
            bool isCreated = await _orderService.AddAsync(request.OrderDetails);

            if (!isCreated)
                return BadRequest<string>("Failed to add order, there is an error.");

            return Success("Your order added successfully.");
        }

        public async Task<Response<string>> Handle(CompleteOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderService.GetByIdAsync(request.OrderId);

            order.Status = enOrderStatus.Completed;
            await _orderService.UpdateAsync(order);

            return Success($"Order with ID: {request.OrderId} completed successfully.");
        }

        public async Task<Response<string>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            bool isCancelled = await _orderService.CancelOrderAsync(request.OrderId);
            if (!isCancelled)
                return BadRequest<string>("Failed to cancel order, there is an error.");

            return Success($"Order with ID: {request.OrderId} cancelled successfully.");
        }
    }
}