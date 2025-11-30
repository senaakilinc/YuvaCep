using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Domain
{
    public class Child
    {
        Guid Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string HealthNotes { get; set; }

        //Parent pairing key: 6-digit unique code
        public string ReferenceCode { get; set; } 
        public Guid ClassId { get; set; }
        
        public Class Class { get; set; }
        public ICollection<ParentChild> ParentChildren { get; set; }

        public Child()
        {
            Id = Guid.NewGuid();
            ParentChildren = new HashSet<ParentChild>();
        }
    }
}
