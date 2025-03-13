using BooksManagementSystem.Application.ResponseBase;
using MediatR;

namespace ProductProject.Application.Features.Categories.Commands.Models
{
    public class DeleteCategoryCommand : IRequest<Response<string>>
    {
        public int CategoryId { get; set; }
    }
}