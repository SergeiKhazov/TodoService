using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Todo.Core.Common.Abstractions
{
    public interface IRepository<T>
    {
        IQueryable<T> Items { get; }

        Task<T> GetAsync(long id, CancellationToken Cancel = default);

        Task<T> AddAsync(T item, CancellationToken Cancel = default);

        Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> items, CancellationToken Cancel = default);

        Task UpdateAsync(T item, CancellationToken Cancel = default);

        Task RemoveAsync(long id, CancellationToken Cancel = default);

        Task SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}