using System;
using System.Collections.Generic;
using System.Text;

namespace TECSO.FWK.Domain.Entities
{
    [Flags]
    public enum LogLevel:int
    {
        Debug = 0,
        Information = 1,
        Warning = 2,
        Error = 4,
        Critical = 8,
    }
}
