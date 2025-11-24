using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using YuvaCep.Domain.Enums;

namespace YuvaCep.Domain
{
    public abstract class User
    {
        public Guid Id { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }
        public UserRole Role { get; set; }

        public User() { 
           Id = Guid.NewGuid();
        }
    }
}
