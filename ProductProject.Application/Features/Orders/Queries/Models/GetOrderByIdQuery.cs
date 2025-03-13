using BooksManagementSystem.Application.ResponseBase;
using MediatR;
using ProductProject.Application.Features.Orders.Queries.Responses;

namespace ProductProject.Application.Features.Orders.Queries.Models
{
    public class GetOrderByIdQuery : IRequest<Response<GetOrderInfoResponse>>
    {
        public int OrderId { get; set; }
    }
}
