using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class LoginRequest
    {
        //sisteme tc ve şifre ile girer veli
<<<<<<< Updated upstream
        public required string TCIDNumber { get; set; } 
        public required string Password { get; set; }
=======
        public string TCIDNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
>>>>>>> Stashed changes
    }
}
