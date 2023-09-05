using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TECSO.FWK.Domain
{
    [Serializable]
    public class DomainValidationException : TecsoException
    {
        public DomainValidationException(string message)
         : base(message)
        {
        }
    }
 
}
