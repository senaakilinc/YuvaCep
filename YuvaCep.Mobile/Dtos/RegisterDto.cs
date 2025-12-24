using System;
using System.Collections.Generic;
using System.Text;

namespace YuvaCep.Mobile.Dtos
{
    public class RegisterDto
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string TCIDNumber { get; set; } = string.Empty;
        public string PhoneNumber {  get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int Role { get; set; } // 1: Öğretmen, 2: Veli 
    }
}
