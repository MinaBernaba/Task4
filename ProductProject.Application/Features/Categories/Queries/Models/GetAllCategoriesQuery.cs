using BooksManagementSystem.Application.ResponseBase;
using MediatR;
using ProductProject.Application.Features.Categories.Queries.Responses;

namespace ProductProject.Application.Features.Categories.Queries.Models
{
    public class GetAllCategoriesQuery : IRequest<Response<IEnumerable<GetAllCategoriesResponse>>>
    {

    }
}