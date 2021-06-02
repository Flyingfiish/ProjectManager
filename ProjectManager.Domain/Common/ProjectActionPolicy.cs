using ProjectManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Domain.Common
{
    public class ProjectActionPolicy
    {
        private static readonly Exception exception = new Exception("You have no rights");

        public static bool TaskCRUD(ParticipationType participationType)
        {
            if (participationType == ParticipationType.Creator ||
                participationType == ParticipationType.ProjectManager ||
                participationType == ParticipationType.Expert ||
                participationType == ParticipationType.TeamLeader)
                return true;
            else
                throw exception;
        }

        public static bool TaskMooving(ParticipationType participationType)
        {
            return true;
        }

        public static bool UserManagement(ParticipationType participationType)
        {
            if (participationType == ParticipationType.Creator ||
                participationType == ParticipationType.ProjectManager)
                return true;
            else
                throw exception;
        }

        public static bool StatusCRUD(ParticipationType participationType)
        {
            if (participationType == ParticipationType.Creator ||
                participationType == ParticipationType.ProjectManager)
                return true;
            else
                throw exception;
        }

        public static bool PMManagement(ParticipationType participationType)
        {
            if (participationType == ParticipationType.Creator ||
                participationType == ParticipationType.ProjectManager)
                return true;
            else
                throw exception;
        }
    }
}
