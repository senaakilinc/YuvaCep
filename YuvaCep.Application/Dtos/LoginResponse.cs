using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class LoginResponse
    {
        public required string Token { get; set; }
        public required string UserRole { get; set; }
        public required string Message { get; set; } 
    }
}
