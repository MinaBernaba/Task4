using BooksManagementSystem.Application.ResponseBase;
using MediatR;
using ProductProject.Application.Features.Products.Queries.Responses;

namespace ProductProject.Application.Features.Products.Queries.Models
{
    public class GetProductInfoQuery : IRequest<Response<ProductInfoResponse>>
    {
        public int ProductId { get; set; }
    }
}