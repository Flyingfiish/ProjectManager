using ProjectManager.Domain.Specifications;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Infrastructure.Repositories
{
    public interface IRepository<T> : IDisposable where T : class
    {
        public Task Create(T item);
        public Task<T> ReadOne(Specification<T> spec);
        public IAsyncEnumerable<T> ReadMany(Specification<T> spec);
        public Task Update(Specification<T> spec, Action<T> func);
        public Task Delete(Specification<T> spec);
    }
}
