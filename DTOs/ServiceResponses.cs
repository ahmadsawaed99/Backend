using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Backend.Data;

namespace Backend.DTOs
{
    public class ServiceResponses
    {
        public record class RegisterResponse(bool Flag, string Message , AppUser? User = null);
        public record class LoginResponse(bool Flag, string Token, string Role,string Message,string UserId);

    }
}
