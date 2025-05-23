using Beyond.Todo.API.Models;
using Beyond.Todo.Application.Commands;
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
        public async Task<IActionResult> GetItems([FromQuery] int skip = 0, [FromQuery] int take = 10)
        {
            var items = await _mediator.Send(new GetTodosQuery(skip, take));
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var item = await _mediator.Send(new GetTodoByIdQuery(id));
            return Ok(item);
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

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateItemCommand command)
        {
            if (id != command.Id)
            {
                return BadRequest();
            }

            await _mediator.Send(command);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new RemoveItemCommand(id));
            return NoContent();
        }
    }
}
