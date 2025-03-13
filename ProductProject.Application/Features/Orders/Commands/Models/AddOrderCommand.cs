using BooksManagementSystem.Application.ResponseBase;
using MediatR;

namespace ProductProject.Application.Features.Orders.Commands.Models
{
    public class AddOrderCommand : IRequest<Response<string>>
    {
        public HashSet<AddOrderDetail> OrderDetails { get; set; } = new();
    }
    public class AddOrderDetail
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
