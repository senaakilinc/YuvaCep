using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class RegisterRequest
    {
        public string ReferenceCode { get; set; } = string.Empty;

        public string TCNumber { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
