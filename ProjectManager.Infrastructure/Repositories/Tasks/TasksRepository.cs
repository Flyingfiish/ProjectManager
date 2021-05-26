using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Infrastructure.EFCore;

namespace ProjectManager.Infrastructure.Repositories.Tasks
{
    public class TasksRepository : Repository<Domain.Entities.Task>, ITasksRepository
    {
        public TasksRepository(ApplicationContext context) : base(context)
        {

        }
    }
}
