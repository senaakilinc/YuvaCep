using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Domain
{
    public class Parent : User
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string TCIDNumber { get; set; }
        public string PhoneNumber { get; set; }

       /* public ICollection <ParentChild> parentChildren { get; set; }

        public Parent()
        {
            parentChildren = new HashSet<ParentChild>();
        }*/
    }
}
