using ProjectManager.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Specifications.Common
{
    public class GetByIdSpecification<T> : Specification<T> where T : class, IGuidKey, new()
    {
        public GetByIdSpecification(Guid id) 
        {
            Predicate = p => p.Id == id;
        }
    }
}
