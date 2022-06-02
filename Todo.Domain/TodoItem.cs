
using Todo.Domain.Common;

namespace Todo.Domain
{
    public class TodoItem: IEntity
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public bool IsComplete { get; set; }
        public string Secret { get; set; }
    }
}