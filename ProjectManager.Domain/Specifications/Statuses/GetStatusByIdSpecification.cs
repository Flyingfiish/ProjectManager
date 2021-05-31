using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Specifications.Statuses
{
    public class GetStatusByIdSpecification : Specification<Status>
    {
        public GetStatusByIdSpecification(Guid id)
        {
            Predicate = p => p.Id == id;
        }
    }
}
