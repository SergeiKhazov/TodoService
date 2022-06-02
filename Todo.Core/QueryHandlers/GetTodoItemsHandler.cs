using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Todo.Core.Common.Abstractions;
using Todo.Core.Models;
using Todo.Core.Queries;
using Todo.Domain;

namespace Todo.Core.QueryHandlers
{
    public class GetTodoItemsHandler : IRequestHandler<GetTodoItemsQuery, IEnumerable<TodoItemDTO>>
    {
        private readonly IRepository<TodoItem> _repository;
        private readonly IMapper _mapper;

        public GetTodoItemsHandler(
            IRepository<TodoItem> repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<TodoItemDTO>> Handle(GetTodoItemsQuery query, CancellationToken cancellationToken)
        {
            var result = _mapper.Map<IEnumerable<TodoItemDTO>>(await _repository.Items.AsNoTracking().ToListAsync());
            return result;
        }
    }
}
