using System;
using System.Runtime.Serialization;

namespace ROSBUS.Admin.AppService
{
    [Serializable]
    public class InvalidDocumentExeption : Exception
    {
        public InvalidDocumentExeption()
        {
        }

        public InvalidDocumentExeption(string message) : base(message)
        {
        }

        public InvalidDocumentExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected InvalidDocumentExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}