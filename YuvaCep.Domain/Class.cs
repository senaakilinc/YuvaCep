using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Domain
{
    public class Class
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public int MaxCapacity { get; set; }

        public ICollection<Child> Children { get; set; }
        public ICollection<TeacherClass> teacherClasses { get; set; }

        public Class() { 
          Id = Guid.NewGuid();
            Children = new HashSet<Child>();
            teacherClasses = new HashSet<TeacherClass>();
        }
    }
}
