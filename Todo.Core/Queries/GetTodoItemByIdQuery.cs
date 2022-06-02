using MediatR;
using Todo.Core.Models;

namespace Todo.Core.Queries
{
    public class GetTodoItemByIdQuery: IRequest<TodoItemDTO>
    {
        public long Id { get; set; }
    }
}
