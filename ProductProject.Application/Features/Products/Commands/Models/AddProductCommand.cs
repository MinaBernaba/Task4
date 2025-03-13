using BooksManagementSystem.Application.ResponseBase;
using MediatR;

namespace ProductProject.Application.Features.Products.Commands.Models
{
    public class AddProductCommand : IRequest<Response<string>>
    {
        public string ProductName { get; set; } = null!;
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }
        public HashSet<int> CategoryIds { get; set; } = new();
    }
}