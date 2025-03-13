using BooksManagementSystem.Application.ResponseBase;
using MediatR;
using ProductProject.Application.Features.Orders.Queries.Models;
using ProductProject.Application.Features.Orders.Queries.Responses;
using ProductProject.Application.ServiceInterfaces;

namespace ProductProject.Application.Features.Orders.Queries.Handler
{
    public class OrderQueryHandler(IOrderService _orderService) : ResponseHandler,
        IRequestHandler<GetAllOrdersQuery, Response<IEnumerable<GetOrderInfoResponse>>>,
        IRequestHandler<GetOrderByIdQuery, Response<GetOrderInfoResponse>>
    {
        public async Task<Response<IEnumerable<GetOrderInfoResponse>>> Handle(GetAllOrdersQuery request, CancellationToken cancellationToken)
        {
            var response = await _orderService.GetAllOrdersInfo();
            return Success(response);
        }

        public async Task<Response<GetOrderInfoResponse>> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
        {
            if (!await _orderService.IsExistAsync(request.OrderId))
                return NotFound<GetOrderInfoResponse>($"No order with ID: {request.OrderId} exist.");


            var response = await _orderService.GetOrderInfo(request.OrderId);
            return Success(response);
        }
    }
}
