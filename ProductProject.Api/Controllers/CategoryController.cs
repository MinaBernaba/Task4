using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductProject.Application.Features.Categories.Commands.Models;
using ProductProject.Application.Features.Categories.Queries.Models;
using SchoolManagementSystem.api.Base;

namespace ProductProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController(IMediator _mediator) : AppControllerBase()
    {
        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
           => NewResult(await _mediator.Send(new GetAllCategoriesQuery()));

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(int id)
            => NewResult(await _mediator.Send(new GetCategoryByIdQuery() { CategoryId = id }));


        [HttpPost("AddCategory")]
        public async Task<IActionResult> AddCategory(AddCategoryCommand addCategory)
            => NewResult(await _mediator.Send(addCategory));

        [HttpPut("UpdateCategory")]
        public async Task<IActionResult> UpdateCategory(UpdateCategoryCommand updateCategory)
           => NewResult(await _mediator.Send(updateCategory));

        [HttpDelete("DeleteeCategory/{id}")]
        public async Task<IActionResult> DeleteeCategory(int id)
           => NewResult(await _mediator.Send(new DeleteCategoryCommand() { CategoryId = id }));

    }
}
