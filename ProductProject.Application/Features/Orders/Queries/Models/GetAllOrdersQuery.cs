using BooksManagementSystem.Application.ResponseBase;
using MediatR;
using ProductProject.Application.Features.Orders.Queries.Responses;

namespace ProductProject.Application.Features.Orders.Queries.Models
{
    public class GetAllOrdersQuery : IRequest<Response<IEnumerable<GetOrderInfoResponse>>>
    {
    }
}
