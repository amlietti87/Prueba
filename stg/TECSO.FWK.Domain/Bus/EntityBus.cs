using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Event;
using TECSO.FWK.Domain.UOW;

namespace TECSO.FWK.Domain.bus
{


    public interface IEventDataWithInheritableGenericArgument
    {
        object[] GetConstructorArgs();
    }

    [Serializable]
    public abstract class EventData : IEventData
    {
        public DateTime EventTime { get; set; }

        public object EventSource { get; set; }

        protected EventData()
        {
            this.EventTime = DateTime.Now;
        }
    }

    [Serializable]
    public class EntityEventData<TEntity> : EventData, IEventDataWithInheritableGenericArgument
    {
        public TEntity Entity { get; private set; }

        public EntityEventData(TEntity entity)
        {
            this.Entity = entity;
        }

        public virtual object[] GetConstructorArgs()
        {
            return new object[1] { (object)this.Entity };
        }
    }


    [Serializable]
    public class EntityChangedEventData<TEntity> : EntityEventData<TEntity>
    {
        public EntityChangedEventData(TEntity entity)
          : base(entity)
        {
        }
    }


    [Serializable]
    public class EntityDeletedEventData<TEntity> : EntityChangedEventData<TEntity>
    {
        public EntityDeletedEventData(TEntity entity)
          : base(entity)
        {
        }
    }

    [Serializable]
    public class EntityChangingEventData<TEntity> : EntityEventData<TEntity>
    {
        public EntityChangingEventData(TEntity entity)
          : base(entity)
        {
        }
    }

    [Serializable]
    public class EntityCreatingEventData<TEntity> : EntityChangingEventData<TEntity>
    {
        public EntityCreatingEventData(TEntity entity)
          : base(entity)
        {
        }
    }

    [Serializable]
    public class EntityUpdatedEventData<TEntity> : EntityChangedEventData<TEntity>
    {
        public EntityUpdatedEventData(TEntity entity)
          : base(entity)
        {
        }
    }

    [Serializable]
    public class EntityDeletingEventData<TEntity> : EntityChangingEventData<TEntity>
    {
        public EntityDeletingEventData(TEntity entity)
          : base(entity)
        {
        }
    }

    [Serializable]
    public class EntityUpdatingEventData<TEntity> : EntityChangingEventData<TEntity>
    {
        public EntityUpdatingEventData(TEntity entity)
          : base(entity)
        {
        }
    }


    public interface ITransientDependency
    {
    }


    public interface IEntityChangeEventHelper
    {
        void TriggerEvents(EntityChangeReport changeReport);

        Task TriggerEventsAsync(EntityChangeReport changeReport);

        void TriggerEntityCreatingEvent(object entity);

        void TriggerEntityCreatedEventOnUowCompleted(object entity);

        void TriggerEntityUpdatingEvent(object entity);

        void TriggerEntityUpdatedEventOnUowCompleted(object entity);

        void TriggerEntityDeletingEvent(object entity);

        void TriggerEntityDeletedEventOnUowCompleted(object entity);
    }

    public class NullEntityChangeEventHelper : IEntityChangeEventHelper
    {
        public static NullEntityChangeEventHelper Instance { get; } = new NullEntityChangeEventHelper();

        private NullEntityChangeEventHelper()
        {
        }

        public void TriggerEntityCreatingEvent(object entity)
        {
        }

        public void TriggerEntityCreatedEventOnUowCompleted(object entity)
        {
        }

        public void TriggerEntityUpdatingEvent(object entity)
        {
        }

        public void TriggerEntityUpdatedEventOnUowCompleted(object entity)
        {
        }

        public void TriggerEntityDeletingEvent(object entity)
        {
        }

        public void TriggerEntityDeletedEventOnUowCompleted(object entity)
        {
        }

        public void TriggerEvents(EntityChangeReport changeReport)
        {
        }

        public Task TriggerEventsAsync(EntityChangeReport changeReport)
        {
            return (Task)Task.FromResult<int>(0);
        }
    }


    [Serializable]
    public class EntityCreatedEventData<TEntity> : EntityChangedEventData<TEntity>
    {
        public EntityCreatedEventData(TEntity entity)
          : base(entity)
        {
        }
    }

    public class EntityChangeEventHelper : ITransientDependency, IEntityChangeEventHelper
    {
         private readonly IUnitOfWorkManager _unitOfWorkManager;

        public IEventBus EventBus { get; set; }

        public EntityChangeEventHelper(IUnitOfWorkManager unitOfWorkManager)
        {
            this._unitOfWorkManager = unitOfWorkManager;
            this.EventBus = (IEventBus)NullEventBus.Instance;
        }

        public virtual void TriggerEvents(EntityChangeReport changeReport)
        {
            this.TriggerEventsInternal(changeReport);
            if (changeReport.IsEmpty() || this._unitOfWorkManager.Current == null)
                return;
            this._unitOfWorkManager.Current.SaveChanges();
        }

        public Task TriggerEventsAsync(EntityChangeReport changeReport)
        {
            this.TriggerEventsInternal(changeReport);
            if (changeReport.IsEmpty() || this._unitOfWorkManager.Current == null)
                return (Task)Task.FromResult<int>(0);
            return this._unitOfWorkManager.Current.SaveChangesAsync(); 
        }



        public virtual void TriggerEntityCreatingEvent(object entity)
        {
            this.TriggerEventWithEntity(typeof(EntityCreatingEventData<>), entity, true);
        }

        public virtual void TriggerEntityCreatedEventOnUowCompleted(object entity)
        {
            this.TriggerEventWithEntity(typeof(EntityCreatedEventData<>), entity, false);
        }

        public virtual void TriggerEntityUpdatingEvent(object entity)
        {
            this.TriggerEventWithEntity(typeof(EntityUpdatingEventData<>), entity, true);
        }

        public virtual void TriggerEntityUpdatedEventOnUowCompleted(object entity)
        {
            this.TriggerEventWithEntity(typeof(EntityUpdatedEventData<>), entity, false);
        }

        public virtual void TriggerEntityDeletingEvent(object entity)
        {
            this.TriggerEventWithEntity(typeof(EntityDeletingEventData<>), entity, true);
        }

        public virtual void TriggerEntityDeletedEventOnUowCompleted(object entity)
        {
            this.TriggerEventWithEntity(typeof(EntityDeletedEventData<>), entity, false);
        }

        public virtual void TriggerEventsInternal(EntityChangeReport changeReport)
        {
            this.TriggerEntityChangeEvents(changeReport.ChangedEntities);
            this.TriggerDomainEvents(changeReport.DomainEvents);
        }

        protected virtual void TriggerEntityChangeEvents(List<EntityChangeEntry> changedEntities)
        {
            foreach (EntityChangeEntry changedEntity in changedEntities)
            {
                switch (changedEntity.ChangeType)
                {
                    case EntityChangeType.Created:
                        this.TriggerEntityCreatingEvent(changedEntity.Entity);
                        this.TriggerEntityCreatedEventOnUowCompleted(changedEntity.Entity);
                        continue;
                    case EntityChangeType.Updated:
                        this.TriggerEntityUpdatingEvent(changedEntity.Entity);
                        this.TriggerEntityUpdatedEventOnUowCompleted(changedEntity.Entity);
                        continue;
                    case EntityChangeType.Deleted:
                        this.TriggerEntityDeletingEvent(changedEntity.Entity);
                        this.TriggerEntityDeletedEventOnUowCompleted(changedEntity.Entity);
                        continue;
                    default:
                        throw new TecsoException("Unknown EntityChangeType: " + (object)changedEntity.ChangeType);
                }
            }
        }

        protected virtual void TriggerDomainEvents(List<DomainEventEntry> domainEvents)
        {
            foreach (DomainEventEntry domainEvent in domainEvents)
                this.EventBus.Trigger(domainEvent.EventData.GetType(), domainEvent.SourceEntity, domainEvent.EventData);
        }

        protected virtual void TriggerEventWithEntity(Type genericEventType, object entity, bool triggerInCurrentUnitOfWork)
        {
            Type type = entity.GetType();
            Type eventType = genericEventType.MakeGenericType(type);
            if (triggerInCurrentUnitOfWork || this._unitOfWorkManager.Current == null)
                this.EventBus.Trigger(eventType, (IEventData)Activator.CreateInstance(eventType, new object[1]
                {
          entity
                }));
            else
                this._unitOfWorkManager.Current.Completed += (EventHandler)((sender, args) => this.EventBus.Trigger(eventType, (IEventData)Activator.CreateInstance(eventType, new object[1]
               {
          entity
               })));
        }
    }

    public class EntityChangeReport
    {
        public List<EntityChangeEntry> ChangedEntities { get; }

        public List<DomainEventEntry> DomainEvents { get; }

        public EntityChangeReport()
        {
            this.ChangedEntities = new List<EntityChangeEntry>();
            this.DomainEvents = new List<DomainEventEntry>();
        }

        public bool IsEmpty()
        {
            if (this.ChangedEntities.Count <= 0)
                return this.DomainEvents.Count <= 0;
            return false;
        }

        public override string ToString()
        {
            return string.Format("[EntityChangeReport] ChangedEntities: {0}, DomainEvents: {1}", (object)this.ChangedEntities.Count, (object)this.DomainEvents.Count);
        }
    }

    [Serializable]
    public class EntityChangeEntry
    {
        public object Entity { get; set; }

        public EntityChangeType ChangeType { get; set; }

        public EntityChangeEntry(object entity, EntityChangeType changeType)
        {
            this.Entity = entity;
            this.ChangeType = changeType;
        }
    }

    public enum EntityChangeType
    {
        Created,
        Updated,
        Deleted,
    }

    [Serializable]
    public class DomainEventEntry
    {
        public object SourceEntity { get; }

        public IEventData EventData { get; }

        public DomainEventEntry(object sourceEntity, IEventData eventData)
        {
            this.SourceEntity = sourceEntity;
            this.EventData = eventData;
        }
    }
}
