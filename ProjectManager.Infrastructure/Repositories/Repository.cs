using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Domain.Specifications;
using ProjectManager.Infrastructure.EFCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, IGuidKey, new()
    {
        protected ApplicationContext _context;
        protected DbSet<T> _entities;

        public Repository(ApplicationContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public async Task Create(T item)
        {
            _entities.Add(item);
            await _context.SaveChangesAsync();
            _context.Entry(item).State = EntityState.Detached;
        }

        public async virtual Task Delete(T item)
        {
            _entities.Remove(item);
            await _context.SaveChangesAsync();
            _context.Entry(item).State = EntityState.Detached;
        }

        public void Dispose()
        {
            _context.DisposeAsync();
        }

        public async IAsyncEnumerable<T> ReadMany(Specification<T> spec)
        {
            IAsyncEnumerator<T> enumerator = _entities.Where(spec.ToExpression()).AsAsyncEnumerable().GetAsyncEnumerator();
            while(await enumerator.MoveNextAsync())
            {
                yield return enumerator.Current;
            }
        }

        public async virtual Task<T> ReadOne(Specification<T> spec)
        {
            return await _entities.FirstOrDefaultAsync(spec.ToExpression());
        }

        public async Task Update(Guid id, Action<T> func)
        {
            T entity = await _entities.FirstOrDefaultAsync(e => e.Id == id);
            func(entity);
            await _context.SaveChangesAsync();
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}
