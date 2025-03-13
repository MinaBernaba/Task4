using BooksManagementSystem.Application.ResponseBase;
using MediatR;

namespace ProductProject.Application.Features.Categories.Commands.Models
{
    public class UpdateCategoryCommand : IRequest<Response<string>>
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = null!;
        public int? ParentCategoryId { get; set; }
    }
}