using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Domain
{
    public class Teacher : User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string TCIDNumber { get; set; }
        public string phoneNumber { get; set; }

        public ICollection<TeacherClass> teacherClasses { get; set; }
        public Teacher()
        {
            teacherClasses = new HashSet<TeacherClass>();
        }
    }
}
