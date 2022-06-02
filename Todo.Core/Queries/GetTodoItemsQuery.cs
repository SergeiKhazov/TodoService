using MediatR;
using System.Collections.Generic;
using Todo.Core.Models;

namespace Todo.Core.Queries
{
    public class GetTodoItemsQuery : IRequest<IEnumerable<TodoItemDTO>>
    {
    }
}
