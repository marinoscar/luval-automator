using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Luval.Automator.Core
{
    public interface IElement
    {
        string Id { get; set; }
        string Framework { get; set; }

    }
}
