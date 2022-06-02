using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Todo.Core.Commands;
using Todo.Core.Common.Abstractions;
using Todo.Core.Common.Exceptions;
using Todo.Domain;

namespace Todo.Core.CommandHandlers
{
    public class UpdateTodoItemHandler: IRequestHandler<UpdateTodoItemCommand, Unit>
    {
        private readonly IRepository<TodoItem> _repository;
        private readonly IMapper _mapper;

        public UpdateTodoItemHandler(
            IRepository<TodoItem> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<Unit> Handle(UpdateTodoItemCommand command, CancellationToken cancellationToken)
        {
            if (command.Id != command.TodoItemDTO.Id)
            {
                throw new BadRequestException();
            }

            var todoItem = await _repository.GetAsync(command.Id);

            if (todoItem == null)
            {
                throw new NotFoundException();
            }

            _mapper.Map(command.TodoItemDTO, todoItem);
            await _repository.UpdateAsync(todoItem);
            todoItem.Id = command.TodoItemDTO.Id;
            try
            {
                await _repository.SaveChangesAsync(cancellationToken);
            }
            catch (DbUpdateConcurrencyException ex) when (!_repository.Items.Any(x => x.Id == command.Id))
            {
                throw new NotFoundException("Can't update db", innerException: ex);
            }

            return Unit.Value;
        }
    }
}
