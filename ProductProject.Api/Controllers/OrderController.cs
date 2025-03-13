using MediatR;
using Microsoft.AspNetCore.Mvc;
using ProductProject.Application.Features.Orders.Commands.Models;
using ProductProject.Application.Features.Orders.Queries.Models;
using SchoolManagementSystem.api.Base;

namespace ProductProject.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController(IMediator _mediator) : AppControllerBase
    {
        [HttpGet("GetAllOrdersDetails")]
        public async Task<IActionResult> GetAllOrdersDetails()
            => NewResult(await _mediator.Send(new GetAllOrdersQuery()));

        [HttpGet("GetOrderById/{id}")]
        public async Task<IActionResult> GetOrderById(int id)
           => NewResult(await _mediator.Send(new GetOrderByIdQuery() { OrderId = id }));

        [HttpPost("AddOrder")]
        public async Task<IActionResult> AddOrder(AddOrderCommand addOrder)
            => NewResult(await _mediator.Send(addOrder));

        [HttpPatch("SetOrderComplete")]
        public async Task<IActionResult> SetOrderStatusComplete(CompleteOrderCommand setOrderStatusComplete)
            => NewResult(await _mediator.Send(setOrderStatusComplete));

        [HttpPatch("CancelOrder")]
        public async Task<IActionResult> SetOrderStatusCancelled(CancelOrderCommand setOrderStatusCancelled)
           => NewResult(await _mediator.Send(setOrderStatusCancelled));

    }
}
