using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs.User
{
    public class RegisterRequest
    {
        [JsonPropertyName("login")] public string Login { get; set; }
        [JsonPropertyName("firstname")] public string FirstName { get; set; }
        [JsonPropertyName("lastname")] public string LastName { get; set; }
        [JsonPropertyName("surname")] public string SurName { get; set; }
        [JsonPropertyName("password")] public string Password { get; set; }
    }
}
