using Microsoft.EntityFrameworkCore;
using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Specifications.Users
{
    public class GetUserByIdSpecification : Specification<User>
    {
        public GetUserByIdSpecification(Guid id)
        {
            Predicate = u => u.Id == id;
        }
    }
}
