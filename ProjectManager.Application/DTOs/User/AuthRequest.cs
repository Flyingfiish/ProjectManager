using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs.User
{
    public class AuthRequest
    {
        [JsonPropertyName("login")] public string Login { get; set; }

        [JsonPropertyName("password")] public string Password { get; set; }
    }
}
