using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectManager.Domain.Specifications.Tasks
{
    public class GetTaskByProjectIdSpecification : Specification<Task>
    {
        public GetTaskByProjectIdSpecification(Guid projectId)
        {
            Predicate = p => p.ProjectId == projectId;
        }
    }
}
