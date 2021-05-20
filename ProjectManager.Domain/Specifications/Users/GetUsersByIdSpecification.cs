using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Specifications.Users
{
    public class GetUsersByIdSpecification : Specification<User>
    {
        public GetUsersByIdSpecification(List<Guid> ids)
        {
            Predicate = u => ids.Contains(u.Id);
        }
    }
}
