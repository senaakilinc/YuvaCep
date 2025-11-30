using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Domain
{
    public class TeacherClass
    {
        public Guid TeacherId { get; set; }
        public Guid ClassId { get; set; }

        public Teacher Teacher { get; set; }
        public Class Class { get; set; }

    }
}
