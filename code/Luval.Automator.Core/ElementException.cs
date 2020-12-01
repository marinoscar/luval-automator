using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Automator.Core
{
    public class ElementException : Exception
    {
        public ElementException() { }

        public ElementException(string message) : base(message) { }

        public ElementException(string message, Exception innerException) : base(message, innerException) { }
    }
}
