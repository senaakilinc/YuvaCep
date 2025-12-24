using System;
using System.Collections.Generic;
using System.Text;

namespace YuvaCep.Mobile.Dtos
{
    public class LoginDto
    {
        public string TCIDNumber { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
