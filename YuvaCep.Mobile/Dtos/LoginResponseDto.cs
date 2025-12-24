using System;
using System.Collections.Generic;
using System.Text;

namespace YuvaCep.Mobile.Dtos
{
    namespace YuvaCep.Mobile.Dtos
    {
        public class LoginResponse
        {
            public string Token { get; set; } = string.Empty;
            public string UserRole { get; set; } = string.Empty;
            public string Message { get; set; } = string.Empty;
            public bool IsSuccess { get; set; }
        }
    }
}
