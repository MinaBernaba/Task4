using AutoMapper;
using BooksManagementSystem.Application.ResponseBase;
using MediatR;
using ProductProject.Application.Features.Categories.Commands.Models;
using ProductProject.Application.ServiceInterfaces;
using ProductProjrect.Data.Entities;

namespace ProductProject.Application.Features.Categories.Commands.Handler
{
    public class CategoryCommandHandler(ICategoryService _categoryService, IMapper _mapper) : ResponseHandler,
        IRequestHandler<AddCategoryCommand, Response<string>>,
        IRequestHandler<UpdateCategoryCommand, Response<string>>,
        IRequestHandler<DeleteCategoryCommand, Response<string>>
    {
        public async Task<Response<string>> Handle(AddCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = _mapper.Map<Category>(request);

            bool isAdded = await _categoryService.AddAsync(category);

            if (!isAdded)
                return BadRequest<string>("Failed to add category. there was an error.");

            return Success($"Category added successfully with ID: {category.CategoryId}.");
        }

        public async Task<Response<string>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            //var category = _mapper.Map<Category>(request);

            var category = await _categoryService.GetByIdAsync(request.CategoryId);

            _mapper.Map(request, category);

            bool isUpdated = await _categoryService.UpdateAsync(category);

            if (!isUpdated)
                return BadRequest<string>("Failed to update category. there was an error.");

            return Success($"Category with ID: {category.CategoryId} updated successfully.");

        }

        public async Task<Response<string>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            if (!await _categoryService.IsExistAsync(request.CategoryId))
                return BadRequest<string>($"No Category with ID: {request.CategoryId}");

            bool isDeleted = await _categoryService.DeleteAsync(request.CategoryId);

            if (!isDeleted)
                return BadRequest<string>("Failed to delete category. there was an error.");

            return Success($"Category with ID: {request.CategoryId} deleted successfully.");


        }
    }
}
