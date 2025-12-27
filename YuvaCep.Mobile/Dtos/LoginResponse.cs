using System;

namespace YuvaCep.Mobile.Dtos
{
    public class LoginResponse
    {
        public string Token { get; set; }      
        public string UserRole { get; set; }   
        public string Message { get; set; }    
        public bool IsSuccess { get; set; }    
    }
}