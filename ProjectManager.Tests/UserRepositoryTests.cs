using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProjectManager.Domain.Entities;
using ProjectManager.Domain.Specifications.Users;
using ProjectManager.Infrastructure.EFCore;
using ProjectManager.Infrastructure.Repositories.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectManager.Tests
{
    [TestClass]
    public class UserRepositoryTests
    {
        UsersRepository _usersRepository;
        User _testUser = new User() { Login = "123", HashPassword = "123", FirstName = "123", SurName = "123", LastName = "123" };

        public UserRepositoryTests(ApplicationContext context)
        {
            _usersRepository = new UsersRepository(context);
        }

        [TestMethod]
        public void CreateReadAndDeleteUserTest()
        {
            _usersRepository.Create(_testUser);
            User result = _usersRepository.ReadOne(new GetUserByIdSpecification(_testUser.Id));
            Assert.IsNotNull(result);
            _usersRepository.Delete(result);
        }

        [TestMethod]
        public void ReadManyUsersTest()
        {
            _usersRepository.Create(_testUser);
            var ids = new List<Guid>();
            ids.Add(_testUser.Id);
            List<User> result = _usersRepository.ReadMany(new GetUsersByIdSpecification(ids));
            Assert.IsNotNull(result);
            User user = _usersRepository.ReadOne(new GetUserByIdSpecification(_testUser.Id));
            _usersRepository.Delete(user);
        }

        [TestMethod]
        public void UpdatePasswordUserTest()
        {
            _usersRepository.Create(_testUser);
            _usersRepository.Update(_testUser.Id, x => { x.HashPassword = "321"; });
            User result = _usersRepository.ReadOne(new GetUserByIdSpecification(_testUser.Id));
            Assert.AreEqual("321", result.HashPassword);
            _usersRepository.Delete(result);
        }

        [TestMethod]
        public void UpdateAvatarURLTest()
        {
            _usersRepository.Create(_testUser);
            _usersRepository.Update(_testUser.Id, x => { x.HexColor = "321"; });
            User result = _usersRepository.ReadOne(new GetUserByIdSpecification(_testUser.Id));
            Assert.AreEqual("321", result.HexColor);
            _usersRepository.Delete(result);
        }

        [TestMethod]
        public void UpdateLoginTest()
        {
            _usersRepository.Create(_testUser);
            _usersRepository.Update(_testUser.Id, x => { x.Login = "321"; });
            User result = _usersRepository.ReadOne(new GetUserByIdSpecification(_testUser.Id));
            Assert.AreEqual("321", result.Login);
            _usersRepository.Delete(result);
        }

        [TestMethod]
        public void UpdateBIOTest()
        {
            _usersRepository.Create(_testUser);
            _usersRepository.Update(_testUser.Id, x => { x.FirstName = "321"; });
            User result = _usersRepository.ReadOne(new GetUserByIdSpecification(_testUser.Id));
            bool isSuccess = false;
            if (result.FirstName == "321" && result.SurName == "123")
                isSuccess = true;
            Assert.IsTrue(isSuccess);
            _usersRepository.Delete(result);
        }
    }
}
