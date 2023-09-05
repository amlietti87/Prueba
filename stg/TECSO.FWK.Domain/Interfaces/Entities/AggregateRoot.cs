using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Event;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.Domain.Entities
{
  public class AggregateRoot : AggregateRoot<int>, IAggregateRoot, IAggregateRoot<int>, IEntity<int>, IGeneratesDomainEvents, IEntity
  {
  }

    public class AggregateRoot<TPrimaryKey> : Entity<TPrimaryKey>, IAggregateRoot<TPrimaryKey>, IEntity<TPrimaryKey>, IGeneratesDomainEvents
    {
        [NotMapped]
        public virtual ICollection<IEventData> DomainEvents { get; }

        public AggregateRoot()
        {
            this.DomainEvents = (ICollection<IEventData>)new Collection<IEventData>();
        }
    }

    public interface IAggregateRoot<TPrimaryKey> : IEntity<TPrimaryKey>, IGeneratesDomainEvents
    {
    }
    public interface IAggregateRoot : IAggregateRoot<int>, IEntity<int>, IGeneratesDomainEvents, IEntity
    {
    }

    public interface IGeneratesDomainEvents
    {
        ICollection<IEventData> DomainEvents { get; }
    }
}

