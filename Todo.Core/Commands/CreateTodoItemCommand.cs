using MediatR;
using Todo.Core.Models;

namespace Todo.Core.Commands
{
    public class CreateTodoItemCommand: IRequest<TodoItemDTO>
    {
        public TodoItemDTO TodoItemDTO { get; set; }
    }
}
