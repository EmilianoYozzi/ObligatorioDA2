using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException() { }
        public ResourceNotFoundException(string message)
            : base(message) { }
    }
}
