using Beyond.Todo.API.Models;
using Beyond.Todo.Application;
using Beyond.Todo.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Beyond.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TodoController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _mediator.Send(new GetCategoriesQuery());
            return Ok(categories);
        }

        [HttpGet]
        public async Task<IActionResult> GetItems()
        {
            var items = await _mediator.Send(new PrintItemsQuery());
            return Ok(items);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddTodoItemCommand command)
        {
            var id = await _mediator.Send(command);
            return Ok(id);
        }

        [HttpPost("{id}/progression")]
        public async Task<IActionResult> RegisterProgression(int id, [FromBody] RegisterProgressionDto body)
        {
            await _mediator.Send(new RegisterProgressionCommand(id, body.Date, body.Percent));
            return Ok();
        }
    }
}
