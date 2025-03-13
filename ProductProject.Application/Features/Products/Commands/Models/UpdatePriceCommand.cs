using BooksManagementSystem.Application.ResponseBase;
using MediatR;

namespace ProductProject.Application.Features.Products.Commands.Models
{
    public class UpdatePriceCommand : IRequest<Response<string>>
    {
        public int ProductId { get; set; }
        public int Price { get; set; }
    }
}