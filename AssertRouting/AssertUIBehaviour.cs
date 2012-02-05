using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AssertRouting
{
    [Flags]
    public enum AssertUIBehaviour
    {
        Normal = 0,

        DisableUI = 1,
        ThrowExceptions = 2, 
        
        DisableUIAndThrowExceptions = DisableUI + ThrowExceptions,
    }
}
