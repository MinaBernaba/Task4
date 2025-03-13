using BooksManagementSystem.Application.ResponseBase;
using MediatR;
using ProductProject.Application.Features.Products.Commands.Responses;

namespace ProductProject.Application.Features.Products.Commands.Models
{
    public class SetProductCategoriesCommand : IRequest<Response<SetProductCategoriesResponse>>
    {
        public int ProductId { get; set; }
        public HashSet<int> CategoryIds { get; set; } = new();
    }
}