using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace Todo.Core.Commands
{
    public class DeleteTodoItemCommand : IRequest<Unit>
    {
        public long Id { get; set; }
    }
}
