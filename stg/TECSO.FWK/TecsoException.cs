using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TECSO.FWK
{
    [Serializable]
    public class TecsoException : Exception
    {
        public TecsoException()
        {
        }

        public TecsoException(SerializationInfo serializationInfo, StreamingContext context)
          : base(serializationInfo, context)
        {
        }

        public TecsoException(string message)
          : base(message)
        {
        }

        public TecsoException(string message, Exception innerException)
          : base(message, innerException)
        {
        }
    }
}
