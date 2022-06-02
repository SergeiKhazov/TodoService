using MediatR;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Todo.Core.Commands;
using Todo.Core.Models;
using Todo.Core.Queries;

namespace Todo.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public TodoItemsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<TodoItemDTO>>> GetTodoItems() => (await _mediator.Send(new GetTodoItemsQuery())).ToList();

        [HttpGet("{id}")]
        public async Task<ActionResult<TodoItemDTO>> GetTodoItem(long id) => await _mediator.Send(new GetTodoItemByIdQuery { Id = id });

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTodoItem(long id, TodoItemDTO todoItemDTO)
        {
            await _mediator.Send(new UpdateTodoItemCommand { Id = id, TodoItemDTO = todoItemDTO });
            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult<TodoItemDTO>> CreateTodoItem(TodoItemDTO todoItemDTO)
        {
            var todoItem = await _mediator.Send(new CreateTodoItemCommand { TodoItemDTO = todoItemDTO });
            return CreatedAtAction(nameof(GetTodoItem), new { id = todoItem.Id }, todoItem);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodoItem(long id)
        {
            await _mediator.Send(new DeleteTodoItemCommand { Id = id });
            return NoContent();
        }
    }
}
