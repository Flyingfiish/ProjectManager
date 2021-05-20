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

        public void Create(T item)
        {
            _entities.Add(item);
            _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;
        }

        public async Task CreateAsync(T item)
        {
            _entities.Add(item);
            await _context.SaveChangesAsync();
            _context.Entry(item).State = EntityState.Detached;
        }

        public virtual void Delete(T item)
        {
            _entities.Remove(item);
            _context.SaveChanges();
            _context.Entry(item).State = EntityState.Detached;
        }

        public async virtual Task DeleteAsync(T item)
        {
            _entities.Remove(item);
            await _context.SaveChangesAsync();
            _context.Entry(item).State = EntityState.Detached;
        }

        public void Dispose()
        {
            _context.DisposeAsync();
        }

        public List<T> ReadMany(Specification<T> spec)
        {
            List<T> result = _entities.Where(spec.ToExpression()).ToList();
            return result;
        }

        public async IAsyncEnumerable<T> ReadManyAsync(Specification<T> spec)
        {
            IAsyncEnumerator<T> enumerator = _entities.Where(spec.ToExpression()).AsAsyncEnumerable().GetAsyncEnumerator();
            while(await enumerator.MoveNextAsync())
            {
                yield return enumerator.Current;
            }
        }

        public virtual T ReadOne(Specification<T> spec)
        {
            return _entities.FirstOrDefault(spec.ToExpression());
        }

        public async virtual Task<T> ReadOneAsync(Specification<T> spec)
        {
            return await _entities.FirstOrDefaultAsync(spec.ToExpression());
        }

        public void Update(Guid id, Action<T> func)
        {
            T entity = _entities.FirstOrDefault(e => e.Id == id);
            func(entity);
            _context.SaveChanges();
            _context.Entry(entity).State = EntityState.Detached;
        }

        public async Task UpdateAsync(Guid id, Action<T> func)
        {
            T entity = await _entities.FirstOrDefaultAsync(e => e.Id == id);
            func(entity);
            await _context.SaveChangesAsync();
            _context.Entry(entity).State = EntityState.Detached;
        }
    }

    public static class ModifyExtention
    {
        public static void Modify<T>(this DbSet<T> set, Guid id, Action<T> func)
    where T : class, IGuidKey, new()
        {
            var target = new T
            {
                Id = id
            };
            var entry = set.Attach(target);
            func(target);
            foreach (var property in entry.Properties)
            {
                var original = property.OriginalValue;
                var current = property.CurrentValue;

                if (ReferenceEquals(original, current))
                {
                    continue;
                }

                if (original == null)
                {
                    property.IsModified = true;
                    continue;
                }

                var propertyIsModified = !original.Equals(current);
                property.IsModified = propertyIsModified;
            }
        }
    }
}
