using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TECSO.FWK.Domain
{
    [Serializable]
    public class EntityNotFoundException : TecsoException
    {
        public Type EntityType { get; set; }

        public object Id { get; set; }

        public EntityNotFoundException()
        {
        }

        public EntityNotFoundException(SerializationInfo serializationInfo, StreamingContext context)
          : base(serializationInfo, context)
        {
        }

        public EntityNotFoundException(Type entityType, object id)
          : this(entityType, id, (Exception)null)
        {
        }

        public EntityNotFoundException(Type entityType, object id, Exception innerException)
          : base(string.Format("There is no such an entity. Entity type: {0}, id: {1}", (object)entityType.FullName, id), innerException)
        {
            this.EntityType = entityType;
            this.Id = id;
        }

        public EntityNotFoundException(string message)
          : base(message)
        {
        }

        public EntityNotFoundException(string message, Exception innerException)
          : base(message, innerException)
        {
        }
    }
}
