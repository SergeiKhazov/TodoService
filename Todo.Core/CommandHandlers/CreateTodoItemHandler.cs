using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Todo.Core.Commands;
using Todo.Core.Common.Abstractions;
using Todo.Core.Models;
using Todo.Domain;

namespace Todo.Core.CommandHandlers
{
    public class CreateTodoItemHandler: IRequestHandler<CreateTodoItemCommand, TodoItemDTO>
    {
        private readonly IRepository<TodoItem> _repository;
        private readonly IMapper _mapper;

        public CreateTodoItemHandler(
            IRepository<TodoItem> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }
        public async Task<TodoItemDTO> Handle(CreateTodoItemCommand command, CancellationToken cancellationToken)
        {
            var todoItem = _mapper.Map<TodoItem>(command.TodoItemDTO);
            await _repository.AddAsync(todoItem, cancellationToken);
            await _repository.SaveChangesAsync(cancellationToken);
            return _mapper.Map<TodoItemDTO>(todoItem);
        }
    }
}
