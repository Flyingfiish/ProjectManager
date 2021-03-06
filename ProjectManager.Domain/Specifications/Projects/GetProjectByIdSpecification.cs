using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Specifications.Projects
{
    public class GetProjectByIdSpecification : Specification<Project>
    {
        public GetProjectByIdSpecification(Guid id)
        {
            Predicate = p => p.Id == id;
        }
    }
}
