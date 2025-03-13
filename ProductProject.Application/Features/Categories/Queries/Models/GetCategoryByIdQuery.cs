using BooksManagementSystem.Application.ResponseBase;
using MediatR;
using ProductProject.Application.Features.Categories.Queries.Responses;

namespace ProductProject.Application.Features.Categories.Queries.Models
{
    public class GetCategoryByIdQuery : IRequest<Response<GetCategoryDetailedInfoResponse>>
    {
        public int CategoryId { get; set; }
    }
}