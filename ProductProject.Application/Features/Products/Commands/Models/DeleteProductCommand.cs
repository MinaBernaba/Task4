using BooksManagementSystem.Application.ResponseBase;
using MediatR;

namespace ProductProject.Application.Features.Products.Commands.Models
{
    public class DeleteProductCommand : IRequest<Response<string>>
    {
        public int ProductId { get; set; }
    }
}