using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ProjectManager.Application.DTOs.User
{
    public class AuthResponse
    {
        [JsonPropertyName("id")] public string Id { get; set; }

        [JsonPropertyName("login")] public string Login { get; set; }

        [JsonPropertyName("token")] public string JWToken { get; set; }
    }
}
