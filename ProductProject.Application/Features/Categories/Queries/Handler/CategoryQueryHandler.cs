using BooksManagementSystem.Application.ResponseBase;
using MediatR;
using ProductProject.Application.Features.Categories.Queries.Models;
using ProductProject.Application.Features.Categories.Queries.Responses;
using ProductProject.Application.ServiceInterfaces;

namespace ProductProject.Application.Features.Categories.Queries.Handler
{
    public class CategoryQueryHandler(ICategoryService _categoryService) : ResponseHandler,
        IRequestHandler<GetAllCategoriesQuery, Response<IEnumerable<GetAllCategoriesResponse>>>,
        IRequestHandler<GetCategoryByIdQuery, Response<GetCategoryDetailedInfoResponse>>
    {
        public async Task<Response<IEnumerable<GetAllCategoriesResponse>>> Handle(GetAllCategoriesQuery request, CancellationToken cancellationToken)
        {
            var Categories = await _categoryService.GetAllCategoriesMainInfoAsync();

            return Success(Categories);

        }

        public async Task<Response<GetCategoryDetailedInfoResponse>> Handle(GetCategoryByIdQuery request, CancellationToken cancellationToken)
        {
            if (!await _categoryService.IsExistAsync(request.CategoryId))
                return BadRequest<GetCategoryDetailedInfoResponse>($"No Category with ID: {request.CategoryId}");


            var detailedCategory = await _categoryService.GetCategoryDetailedInfoAsync(request.CategoryId);

            return Success(detailedCategory);
        }
    }
}
