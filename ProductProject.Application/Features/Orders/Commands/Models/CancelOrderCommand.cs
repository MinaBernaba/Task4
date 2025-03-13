using BooksManagementSystem.Application.ResponseBase;
using MediatR;

namespace ProductProject.Application.Features.Orders.Commands.Models
{
    public class CancelOrderCommand : IRequest<Response<string>>
    {
        public int OrderId { get; set; }
    }
}
