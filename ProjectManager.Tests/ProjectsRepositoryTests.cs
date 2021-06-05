using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications.Projects;
using ProjectManager.Domain.Specifications.Users;
using ProjectManager.Infrastructure.EFCore;
using ProjectManager.Infrastructure.Repositories.Projects;
using ProjectManager.Infrastructure.Repositories.Teams;
using ProjectManager.Infrastructure.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectManager.Tests
{
    [TestClass]
    public class ProjectsRepositoryTests
    {
        UsersRepository _usersRepository;
        User _testUser = new User() { Login = "123", HashPassword = "123", FirstName = "123", SurName = "123", LastName = "123" };

        ProjectsRepository _projectsRepository;
        Project _testProject = new Project()
        {
            Title = "123",
            Description = "123",
            HexColor = "#FFF",
            CreatedAt = DateTime.Now,
            Statuses = new List<Status>()
            {
                new Status(){ Name = "ToDo"}
            },

        };

        TeamsRepository _teamsRepository;
        Team _testTeam = new Team();

        public ProjectsRepositoryTests(ApplicationContext context)
        {
            _usersRepository = new UsersRepository(context);
            _projectsRepository = new ProjectsRepository(context);
            _teamsRepository = new TeamsRepository(context);
        }

        [TestMethod]
        public void CreateReadAndDeleteWithoutUsersTest()
        {
            _usersRepository.Create(_testUser);
            User createdUser = _usersRepository.ReadOne(new GetUserByIdSpecification(_testUser.Id));

            _testProject.CreatedById = createdUser.Id;

            _projectsRepository.Create(_testProject);
            Project result = _projectsRepository.ReadOne(new GetProjectByIdSpecification(_testProject.Id));

            Assert.IsNotNull(result);

            _projectsRepository.Delete(result);
            _usersRepository.Delete(createdUser);
        }

        [TestMethod]
        public void CreateReadAndDeleteWithAddMemberProjectTest()
        {
            _usersRepository.Create(_testUser);
            User createdUser = _usersRepository.ReadOne(new GetUserByIdSpecification(_testUser.Id));

            _testProject.ManagerId = createdUser.Id;
            _testProject.CreatedById = createdUser.Id;

            _projectsRepository.Create(_testProject);


            _projectsRepository.AddMember(_testProject.Id, createdUser.Id, ParticipationType.Creator);
            Project result = _projectsRepository.ReadOne(new GetProjectByIdSpecification(_testProject.Id));

            Assert.IsTrue(result.Members.Count > 0);

            _projectsRepository.Delete(result);
            _usersRepository.Delete(createdUser);
        }

        [TestMethod]
        public void CreateReadAndDeleteWithAddTeamProjectTest()
        {
            _usersRepository.Create(_testUser);
            User createdUser = _usersRepository.ReadOne(new GetUserByIdSpecification(_testUser.Id));

            _testProject.ManagerId = createdUser.Id;
            _testProject.CreatedById = createdUser.Id;

            _projectsRepository.Create(_testProject);


            _projectsRepository.AddMember(_testProject.Id, createdUser.Id, ParticipationType.Creator);

            _teamsRepository.Create(_testTeam);
            _teamsRepository.AddMember(_testTeam.Id, createdUser.Id);
            _teamsRepository.UpdateTeamLeader(_testTeam.Id, createdUser.Id);

            _projectsRepository.AddTeam(_testProject.Id, _testTeam.Id);
            Project result = _projectsRepository.ReadOne(new GetProjectByIdSpecification(_testProject.Id));

            Assert.IsTrue(result.Members.Count > 0);

            //_projectsRepository.Delete(result);
            //_usersRepository.Delete(createdUser);
        }

        [TestMethod]
        public void DeleteProjectMembersTest()
        {
            _usersRepository.Create(_testUser);
            User createdUser = _usersRepository.ReadOne(new GetUserByIdSpecification(_testUser.Id));

            _testProject.ManagerId = createdUser.Id;
            _testProject.CreatedById = createdUser.Id;

            _projectsRepository.Create(_testProject);

            _projectsRepository.AddMember(_testProject.Id, createdUser.Id, ParticipationType.Creator);
            _projectsRepository.DeleteMember(_testProject.Id, createdUser.Id);
            Project result = _projectsRepository.ReadOne(new GetProjectByIdSpecification(_testProject.Id));

            Assert.IsTrue(result.Members.Count == 0);

            _projectsRepository.Delete(result);
            _usersRepository.Delete(createdUser);
        }

        [TestMethod]
        public void UpdateProjectInfoTest()
        {
            _usersRepository.Create(_testUser);
            User createdUser = _usersRepository.ReadOne(new GetUserByIdSpecification(_testUser.Id));

            _testProject.CreatedById = createdUser.Id;

            _projectsRepository.Create(_testProject);
            _projectsRepository.Update(_testProject.Id, p => { p.HexColor = "#AAA"; });

            Project result = _projectsRepository.ReadOne(new GetProjectByIdSpecification(_testProject.Id));

            Assert.AreEqual(result.HexColor, "#AAA");

            _projectsRepository.Delete(result);
            _usersRepository.Delete(createdUser);
        }

        [TestMethod]
        public void UpdateManagerTest()
        {
            _usersRepository.Create(_testUser);
            User createdUser = _usersRepository.ReadOne(new GetUserByIdSpecification(_testUser.Id));

            _testProject.CreatedById = createdUser.Id;

            _projectsRepository.Create(_testProject);
            _projectsRepository.AddMember(_testProject.Id, createdUser.Id, ParticipationType.Creator);
            _projectsRepository.UpdateManager(_testProject.Id, createdUser.Id);

            Project result = _projectsRepository.ReadOne(new GetProjectByIdSpecification(_testProject.Id));

            Assert.AreEqual(result.Manager.Id, _testUser.Id);

            _projectsRepository.Delete(result);
            _usersRepository.Delete(createdUser);
        }

        [TestMethod]
        public void CreateTaskTest()
        {
            _usersRepository.Create(_testUser);
            User createdUser = _usersRepository.ReadOne(new GetUserByIdSpecification(_testUser.Id));

            _testProject.CreatedById = createdUser.Id;

            var testStatus = new Status() { Name = "Backlog" };

            _testProject.Statuses.Add(testStatus);

            _projectsRepository.Create(_testProject);
            _projectsRepository.CreateTask(_testProject.Id, new Task() { Name = "123", CreatedById = createdUser.Id, StatusId = testStatus.Id });

            Project result = _projectsRepository.ReadOne(new GetProjectByIdSpecification(_testProject.Id));

            Assert.AreEqual(result.Tasks.FirstOrDefault().Name, "123");

            _projectsRepository.Delete(_testProject.Id);
            _usersRepository.Delete(createdUser);
        }
    }
}
