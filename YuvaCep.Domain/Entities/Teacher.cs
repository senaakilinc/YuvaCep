using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using YuvaCep.Domain.Enums;

namespace YuvaCep.Domain.Entities
{
    public class Teacher : User
    {
        public string PhoneNumber { get; set; } = string.Empty;
    }
}