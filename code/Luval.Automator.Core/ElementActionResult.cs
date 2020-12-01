using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Automator.Core
{
    public class ElementActionResult
    {
        public bool Success { get; set; }
        public ElementException Exception { get; set; }
        public string Result { get; set; }

        public static ElementActionResult CreateSuccess()
        {
            return CreateSuccess(null);
        }

        public static ElementActionResult CreateSuccess(string result)
        {
            return new ElementActionResult() { Success = true, Result = result };
        }

        public static ElementActionResult CreateError(Exception ex, string message, params object[] args)
        {
            return new ElementActionResult() { Success = false, Exception = new ElementException(string.Format(message, args), ex) };
        }

        public static ElementActionResult CreateError(string message, params object[] args)
        {
            return CreateError(null, message, args);
        }

        public static ElementActionResult CreateError(string message)
        {
            return CreateError(null, message, null);
        }
    }
}
