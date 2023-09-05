using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TECSO.FWK.Domain
{
    [Serializable]
    public class ConcurrencyException : Exception
    {
        public const int ConcurrencyException_CurrentUser = 1;
        public const int ConcurrencyException_OtherUser = 2;

        public ConcurrencyException(string message, int code)
         : base(message)
        {
            this.Code = code;
        }

        public int Code { get; }
    }


}
