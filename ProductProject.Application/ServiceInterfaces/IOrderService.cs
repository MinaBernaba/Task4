using ProductProject.Application.Features.Orders.Commands.Models;
using ProductProject.Application.Features.Orders.Queries.Responses;
using ProductProjrect.Data.Entities;

namespace ProductProject.Application.ServiceInterfaces
{
    public interface IOrderService : IBaseService<Order>
    {
        Task<bool> AddAsync(HashSet<AddOrderDetail> orderDetails);
        Task<bool> CancelOrderAsync(int orderId);
        Task<GetOrderInfoResponse> GetOrderInfo(int orderId);
        Task<IEnumerable<GetOrderInfoResponse>> GetAllOrdersInfo();
    }
}
