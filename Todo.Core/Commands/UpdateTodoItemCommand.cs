using MediatR;
using Todo.Core.Models;

namespace Todo.Core.Commands
{
    public class UpdateTodoItemCommand: IRequest<Unit>
    {
        public long Id { get; set; }
        public TodoItemDTO TodoItemDTO { get; set; }
    }
}
