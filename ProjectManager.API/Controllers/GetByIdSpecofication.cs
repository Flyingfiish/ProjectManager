using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications;
using System;

namespace ProjectManager.API.Controllers
{
    internal class GetByIdSpecofication : Specification<Status>
    {
        private Guid projectId;

        public GetByIdSpecofication(Guid projectId)
        {
            this.projectId = projectId;
        }
    }
}