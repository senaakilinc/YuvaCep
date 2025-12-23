using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Application.Dtos
{
    public class NotificationResultDto
    {
        public bool Success { get; set; }
        public int RecipientCount { get; set; }
        public string Message { get; set; }
    }
}
