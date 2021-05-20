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
        public void Create(T item);
        public T ReadOne(Specification<T> spec);
        public List<T> ReadMany(Specification<T> spec);
        public void Update(Guid id, Action<T> func);
        public void Delete(T item);
    }
}
