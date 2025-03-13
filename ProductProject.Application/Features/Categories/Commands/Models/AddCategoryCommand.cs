using BooksManagementSystem.Application.ResponseBase;
using MediatR;

namespace ProductProject.Application.Features.Categories.Commands.Models
{
    public class AddCategoryCommand : IRequest<Response<string>>
    {
        public string CategoryName { get; set; } = null!;
        public int? ParentCategoryId { get; set; }
    }
}