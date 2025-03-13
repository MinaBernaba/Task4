using BooksManagementSystem.Application.ResponseBase;
using MediatR;

namespace ProductProject.Application.Features.Orders.Commands.Models
{
    public class CompleteOrderCommand : IRequest<Response<string>>
    {
        public int OrderId { get; set; }
    }
}
