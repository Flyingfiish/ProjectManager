using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using ProjectManager.Domain.Interfaces;
using ProjectManager.Domain.Specifications;
using ProjectManager.Infrastructure.EFCore;
using ProjectManager.Infrastructure.Extentions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Repositories
{
    public class Repository<T> : IRepository<T> where T : class, new()
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

        public async virtual Task Delete(Specification<T> spec)
        {
            List<T> entity = await _entities.Where(spec.ToExpression()).ToListAsync();
            _entities.RemoveRange(entity);
            await _context.SaveChangesAsync();
            _context.Entry(entity).State = EntityState.Detached;
        }

        public void Dispose()
        {
            _context.DisposeAsync();
        }

        public async virtual Task<T> ReadOne(Specification<T> spec)
        {
            IQueryable<T> query = _context.Set<T>();
            if (spec.Includes != null)
            {
                query = spec.Includes(query);
            }

            return await query
                .AsNoTracking()
                .FirstOrDefaultAsync(spec.ToExpression());
        }

        public async IAsyncEnumerable<T> ReadMany(Specification<T> spec)
        {
            IQueryable<T> query = _context.Set<T>();
            if (spec.Includes != null)
            {
                query = spec.Includes(query);
            }
            IAsyncEnumerator<T> enumerator = query
                .AsNoTracking()
                .Where(spec.ToExpression())
                .AsAsyncEnumerable()
                .GetAsyncEnumerator();
            while (await enumerator.MoveNextAsync())
            {
                yield return enumerator.Current;
            }
        }

        public async Task Update(Specification<T> spec, Action<T> func)
        {
            T entity = await _entities.FirstOrDefaultAsync(spec.ToExpression());
            func(entity);
            await _context.SaveChangesAsync();
            _context.Entry(entity).State = EntityState.Detached;
        }
    }
}
