using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Todo.Core.Commands;
using Todo.Core.Common.Abstractions;
using Todo.Core.Common.Exceptions;
using Todo.Domain;

namespace Todo.Core.CommandHandlers
{
    public class DeleteTodoItemHandler: IRequestHandler<DeleteTodoItemCommand, Unit>
    {
        private readonly IRepository<TodoItem> _repository;

        public DeleteTodoItemHandler(
            IRepository<TodoItem> repository)
        {
            _repository = repository;
        }
        public async Task<Unit> Handle(DeleteTodoItemCommand command, CancellationToken cancellationToken)
        {
            var todoItem = await _repository.GetAsync(command.Id);

            if (todoItem == null)
            {
                throw new NotFoundException();
            }
            await _repository.RemoveAsync(command.Id);
            await _repository.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
