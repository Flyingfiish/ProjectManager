using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Text.Json;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ProjectManager.Domain.Entities;
using ProjectManager.Application.Interfaces;
using ProjectManager.Application.DTOs.User;
using ProjectManager.Domain.Specifications.Users;
using ProjectManager.Infrastructure.JWT;
using ProjectManager.Application.Services;
using Microsoft.AspNetCore.Authorization;
using System.Threading;
using System.Net;
using Microsoft.AspNetCore.Identity;
using ProjectManager.Application;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ProjectManager.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUsersService _usersService;

        public UsersController(IUsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpPut]
        [Route("[action]")]
        public IActionResult Register(string login, string password, string firstName, string lastName, string surName)
        {
            RegisterRequest registerRequest = new RegisterRequest()
            {
                Login = login,
                Password = password,
                FirstName = firstName,
                LastName = lastName,
                SurName = surName
            };

            try
            {
                _usersService.Register(registerRequest);
                return Ok();
            }
            catch(Exception err)
            {
                return BadRequest(err.Message);
            }
        }

        [HttpGet]
        [Route("[action]")]
        public IActionResult Authenticate(string login, string password)
        {
            if(String.IsNullOrEmpty(login) || String.IsNullOrEmpty(password))
            {
                return BadRequest();
            }
            var identity = GetIdentity(login, password);
            if (identity == null)
            {
                return BadRequest(new { errorText = "Invalid username or password." });
            }

            var response = JWT.Create(identity);

            return Ok(response);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetCurrentUser()
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            var user = _usersService.GetUser(new GetUserByIdSpecification(new Guid(id)));
            return Ok(user);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetUserProjects()
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            var result = _usersService.GetUserProjects(new GetUserByIdSpecification(new Guid(id)));
            return Ok(result);
        }

        [Authorize]
        [HttpGet]
        [Route("[action]")]
        public IActionResult GetUserTasks()
        {
            var id = User.Claims.Where(c => c.Type == "Id").Select(c => c.Value).SingleOrDefault();
            if (String.IsNullOrEmpty(id))
            {
                return Unauthorized();
            }
            var result = _usersService.GetUserTasks(new GetUserByIdSpecification(new Guid(id)));
            return Ok(result);
        }

        private ClaimsIdentity GetIdentity(string login, string password)
        {
            UserShortDto person = _usersService.GetUser(new GetUserByLoginPassSpecification(login, PasswordHasher.GetHash(password)));
            if (person != null)
            {
                var claims = new List<Claim>
                {
                    new Claim(ClaimsIdentity.DefaultNameClaimType, person.Login),
                    new Claim(ClaimsIdentity.DefaultRoleClaimType, "User"),
                    new Claim("Id", person.Id.ToString())
                };
                ClaimsIdentity claimsIdentity =
                new ClaimsIdentity(claims, "Token", ClaimsIdentity.DefaultNameClaimType,
                    ClaimsIdentity.DefaultRoleClaimType);
                return claimsIdentity;
            }

            // если пользователя не найдено
            return null;
        }
    }
}
