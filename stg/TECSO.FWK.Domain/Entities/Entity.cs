using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using System.Text;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.Domain.Entities
{ 

    [Serializable]
    public abstract class Entity : Entity<int>, IEntity, IEntity<int>
    {
    }


    [Serializable]
    public abstract class Entity<TPrimaryKey> : IEntity<TPrimaryKey>
    {
        public virtual TPrimaryKey Id { get; set; }

        public virtual bool IsTransient()
        {
            if (EqualityComparer<TPrimaryKey>.Default.Equals(this.Id, default(TPrimaryKey)))
                return true;
            if (typeof(TPrimaryKey) == typeof(int))
                return Convert.ToInt32((object)this.Id) <= 0;
            if (typeof(TPrimaryKey) == typeof(long))
                return Convert.ToInt64((object)this.Id) <= 0L;
            return false;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || (object)(obj as Entity<TPrimaryKey>) == null)
                return false;
            if ((object)this == obj)
                return true;
            Entity<TPrimaryKey> entity = (Entity<TPrimaryKey>)obj;
            if (this.IsTransient() && entity.IsTransient())
                return false;
            Type type1 = this.GetType();
            Type type2 = entity.GetType();
            if (!type1.GetTypeInfo().IsAssignableFrom(type2) && !type2.GetTypeInfo().IsAssignableFrom(type1))
                return false;
          
            return this.Id.Equals((object)entity.Id);
        }

        public override int GetHashCode()
        {
            return this.Id.GetHashCode();
        }

        public static bool operator ==(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            if (object.Equals((object)left, (object)null))
                return object.Equals((object)right, (object)null);
            return left.Equals((object)right);
        }

        public static bool operator !=(Entity<TPrimaryKey> left, Entity<TPrimaryKey> right)
        {
            return !(left == right);
        }

        public override string ToString()
        {
            return string.Format("[{0} {1}]", (object)this.GetType().Name, (object)this.Id);
        }
    }

}
