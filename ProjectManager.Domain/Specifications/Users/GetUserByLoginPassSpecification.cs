using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Specifications.Users
{
    public class GetUserByLoginPassSpecification : Specification<User>
    {
        public GetUserByLoginPassSpecification(string login, string passwrod)
        {
            Predicate = u => u.Login == login && u.HashPassword == passwrod;
        }
    }
}
