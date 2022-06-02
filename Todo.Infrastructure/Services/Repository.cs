using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Todo.Core.Common.Abstractions;
using Todo.Domain.Common;

namespace Todo.Infrastructure.Services
{
    public class Repository<T> : IRepository<T> where T : class, IEntity, new()
    {
        private readonly TodoContext _context;
        private readonly DbSet<T> _set;
        public virtual IQueryable<T> Items => _set;

        public bool AutoSaveChanges { get; set; } = false;

        public Repository(TodoContext context)
        {
            _context = context;
            _set = context.Set<T>() ?? throw new ArgumentNullException();
        }

        public async Task<T> GetAsync(long id, CancellationToken cancellationToken = default)
        {
            return await Items
                .AsNoTracking()
                .FirstOrDefaultAsync(item => item.Id == id, cancellationToken: cancellationToken);
        }

        public async Task<T> AddAsync(T item, CancellationToken Cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _context.Entry(item).State = EntityState.Added;
            if (AutoSaveChanges)
                await _context.SaveChangesAsync(Cancel).ConfigureAwait(false);
            return item;
        }

        public async Task<IEnumerable<T>> AddManyAsync(IEnumerable<T> items, CancellationToken Cancel = default)
        {
            foreach (var item in items)
            {
                if (item is null)
                    throw new ArgumentNullException(nameof(item));
                _context.Entry(item).State = EntityState.Added;
            }

            if (AutoSaveChanges)
                await _context.SaveChangesAsync(Cancel).ConfigureAwait(false);
            return items;
        }

        public async Task UpdateAsync(T item, CancellationToken Cancel = default)
        {
            if (item is null) throw new ArgumentNullException(nameof(item));
            _context.Entry(item).State = EntityState.Modified;
            if (AutoSaveChanges)
                await _context.SaveChangesAsync(Cancel).ConfigureAwait(false);
        }

        public async Task UpdateManyAsync(IEnumerable<T> items, CancellationToken Cancel = default)
        {
            foreach (var item in items)
            {
                if (item is null) throw new ArgumentNullException(nameof(item));
                _context.Entry(item).State = EntityState.Modified;
            }
            if (AutoSaveChanges)
                await _context.SaveChangesAsync(Cancel).ConfigureAwait(false);
        }

        public async Task RemoveAsync(long id, CancellationToken Cancel = default)
        {
            _context.Remove(new T { Id = id });
            if (AutoSaveChanges)
                await _context.SaveChangesAsync(Cancel).ConfigureAwait(false);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }



    }
}
