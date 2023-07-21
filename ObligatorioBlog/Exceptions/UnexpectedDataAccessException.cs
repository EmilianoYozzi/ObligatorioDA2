using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Exceptions
{
    public class UnexpectedDataAccessException : Exception
    {
        public UnexpectedDataAccessException(Exception inner) : base(("Unknown error in data base " + inner.Message),inner) {}
    }
}
