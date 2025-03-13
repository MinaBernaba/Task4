using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductProject.Application.Features.Products.Commands.Models;
using ProductProject.Application.Features.Products.Queries.Models;
using SchoolManagementSystem.api.Base;

namespace ProductProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController(IMediator _mediator) : AppControllerBase
    {
        [HttpGet("GetAllProducts")]
        public async Task<IActionResult> GetAllProducts()
            => NewResult(await _mediator.Send(new GetAllProductsQuery()));

        [HttpGet("GetProductById/{id}")]
        public async Task<IActionResult> GetAllProducts(int id)
           => NewResult(await _mediator.Send(new GetProductInfoQuery() { ProductId = id }));


        [HttpPost("AddProduct")]
        public async Task<IActionResult> AddProduct(AddProductCommand addProduct)
            => NewResult(await _mediator.Send(addProduct));

        [HttpPatch("UpdateProductPrice")]
        public async Task<IActionResult> UpdateProductPrice(UpdatePriceCommand updatePrice)
            => NewResult(await _mediator.Send(updatePrice));

        [HttpPatch("SetProductCategories")]
        public async Task<IActionResult> SetProductCategories(SetProductCategoriesCommand setProductCategories)
            => NewResult(await _mediator.Send(setProductCategories));

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(UpdateProductCommand updateProduct)
            => NewResult(await _mediator.Send(updateProduct));

        [HttpDelete("DeleteProduct/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
            => NewResult(await _mediator.Send(new DeleteProductCommand() { ProductId = id }));

    }
}
