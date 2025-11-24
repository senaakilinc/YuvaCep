using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace YuvaCep.Domain
{
    public class ParentChild
    {
        public Guid ParentId { get; set; }
        public Guid ChildId { get; set; }

        //Date of parent's assignment to the child (for Security Audit)
        public DateTime DateAssignment { get; set; }

        public Parent Parent { get; set; }
        public Child Child { get; set; }

    }
}
