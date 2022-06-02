using AutoMapper;
using Todo.Core.Mappings;
using Todo.Domain;

namespace Todo.Core.Models
{
    public class TodoItemDTO: IMapFrom<TodoItem>
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }

        void IMapFrom<TodoItem>.Mapping(Profile profile) => 
            profile.CreateMap<TodoItemDTO, TodoItem>()
            .ForMember(d => d.Id, opt => opt.Ignore())
            .ReverseMap()
            ;
    }
}
