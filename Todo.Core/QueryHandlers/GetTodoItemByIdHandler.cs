using AutoMapper;
using MediatR;
using System.Threading;
using System.Threading.Tasks;
using Todo.Core.Common.Abstractions;
using Todo.Core.Common.Exceptions;
using Todo.Core.Models;
using Todo.Core.Queries;
using Todo.Domain;

namespace Todo.Core.QueryHandlers
{
    public class GetTodoItemByIdHandler: IRequestHandler<GetTodoItemByIdQuery, TodoItemDTO>
    {
        private readonly IRepository<TodoItem> _repository;
        private readonly IMapper _mapper;

        public GetTodoItemByIdHandler(
            IRepository<TodoItem> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<TodoItemDTO> Handle(GetTodoItemByIdQuery request, CancellationToken cancellationToken)
        {
            var todoItem = await _repository.GetAsync(request.Id);

            if(todoItem is null)
            {
                throw new NotFoundException();
            }
            var todoItemDTO = _mapper.Map<TodoItemDTO>(todoItem);
            return todoItemDTO;
        }
    }
}
