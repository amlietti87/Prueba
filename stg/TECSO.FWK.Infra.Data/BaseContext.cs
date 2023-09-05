using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.bus;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Event;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Domain.UOW;
using Microsoft.Extensions.DependencyInjection;
using System.Data.SqlClient;
using Snickler.EFCore;
using System.ComponentModel.DataAnnotations;

namespace TECSO.FWK.Infra.Data
{


    public class BaseContext : DbContext, ITransientDependency
    {

        public BaseContext(DbContextOptions options)
            : base(options)
        {
            this.InitializeDbContext();
            authService = ServiceProviderResolver.ServiceProvider.GetService<IAuthService>();

        }


        public override void Dispose()
        {
            base.Dispose();
        }

        private void InitializeDbContext()
        {
            this.SetNullsForInjectedProperties();
        }

        private void SetNullsForInjectedProperties()
        {
            //this.EntityChangeEventHelper = (IEntityChangeEventHelper)NullEntityChangeEventHelper.Instance;
            //this.EventBus = (IEventBus)NullEventBus.Instance;
        }
        //public IEventBus EventBus { get; set; }

        //public IEntityChangeEventHelper EntityChangeEventHelper { get; set; }

        protected IAuthService authService { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            //optionsBuilder.UseSqlServer(ConfigurationManager.ConnectionStrings["BloggingDatabase"].ConnectionString);
            base.OnConfiguring(optionsBuilder);
        }


        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            EntityChangeReport entityChangeReport = this.ApplyAbpConcepts();
            this.CheckConcurrency(entityChangeReport);
            return base.SaveChangesAsync(cancellationToken);
        }

        public override int SaveChanges()
        {

            try
            {
                EntityChangeReport entityChangeReport = this.ApplyAbpConcepts();
                this.CheckConcurrency(entityChangeReport);
                int num = base.SaveChanges();
                //todo falta ver los eventos
                //this.EntityChangeEventHelper.TriggerEvents(entityChangeReport);
                return num;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                throw new TecsoConcurrencyException(ex.Message, (Exception)ex);
            }
        }

        protected virtual EntityChangeReport ApplyAbpConcepts()
        {
            EntityChangeReport changeReport = new EntityChangeReport();
            int? auditUserId = this.GetAuditUserId();
            foreach (EntityEntry entry in this.ChangeTracker.Entries().ToList<EntityEntry>())
            {
                this.ApplyAbpConcepts(entry, auditUserId, changeReport);
            }
            
            return changeReport;
        }

        protected void CheckConcurrency(EntityChangeReport changeReport)
        {
            foreach (var item in changeReport.ChangedEntities)
            {
                IConcurrencyEntity concurrencyEntity = item.Entity as IConcurrencyEntity;
                if (concurrencyEntity != null)
                {
                    if (item.ChangeType== EntityChangeType.Deleted || item.ChangeType == EntityChangeType.Updated)
                    {
                        this.ValidateCocurrencySave(concurrencyEntity.GetEnityId(),concurrencyEntity.GetType().FullName,this.authService.GetCurretUserId(), concurrencyEntity.BlockDate.GetValueOrDefault());
                    }
                }
            }

            
        }

        protected virtual void ApplyAbpConcepts(EntityEntry entry, int? userId, EntityChangeReport changeReport)
        {
            switch (entry.State)
            {
                case EntityState.Deleted:
                    this.ApplyAbpConceptsForDeletedEntity(entry, userId, changeReport);
                    break;
                case EntityState.Modified:
                    this.ApplyAbpConceptsForModifiedEntity(entry, userId, changeReport);
                    break;
                case EntityState.Added:
                    this.ApplyAbpConceptsForAddedEntity(entry, userId, changeReport);
                    break;
            }
            this.AddDomainEvents(changeReport.DomainEvents, entry.Entity);
        }

        protected virtual void AddDomainEvents(List<DomainEventEntry> domainEvents, object entityAsObj)
        {
            IGeneratesDomainEvents igeneratesDomainEvents = entityAsObj as IGeneratesDomainEvents;
            if (igeneratesDomainEvents == null || TECSO.FWK.Extensions.CollectionExtensions.IsNullOrEmpty<IEventData>((ICollection<IEventData>)igeneratesDomainEvents.DomainEvents))
                return;
            domainEvents.AddRange(((IEnumerable<IEventData>)igeneratesDomainEvents.DomainEvents).Select<IEventData, DomainEventEntry>((Func<IEventData, DomainEventEntry>)(eventData => new DomainEventEntry(entityAsObj, eventData))));
            igeneratesDomainEvents.DomainEvents.Clear();
        }



        protected virtual void SetCreationAuditProperties(object entityAsObj, int? userId)
        {
            EntityAuditingHelper.SetCreationAuditProperties(entityAsObj, userId);
        }

        protected virtual void SetModificationAuditProperties(object entityAsObj, int? userId)
        {
            EntityAuditingHelper.SetModificationAuditProperties(entityAsObj, userId);
        }

        protected virtual void SetDeletionAuditProperties(object entityAsObj, int? userId)
        {
            EntityAuditingHelper.SetDeletionAuditProperties(entityAsObj, userId);
        }


        protected virtual void ApplyAbpConceptsForAddedEntity(EntityEntry entry, int? userId, EntityChangeReport changeReport)
        {
            this.CheckAndSetId(entry);

            this.SetCreationAuditProperties(entry.Entity, userId);
            changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, (EntityChangeType)0));
        }

        protected virtual void ApplyAbpConceptsForModifiedEntity(EntityEntry entry, int? userId, EntityChangeReport changeReport)
        {
            this.SetModificationAuditProperties(entry.Entity, userId);
            if (entry.Entity is ISoftDelete && ((ISoftDelete)ObjectExtensions.As<ISoftDelete>(entry.Entity)).IsDeleted)
            {
                this.SetDeletionAuditProperties(entry.Entity, userId);
                changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, (EntityChangeType)2));
            }
            else
                changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, (EntityChangeType)1));
        }

        protected virtual void ApplyAbpConceptsForDeletedEntity(EntityEntry entry, int? userId, EntityChangeReport changeReport)
        {
            this.CancelDeletionForSoftDelete(entry);
            this.SetDeletionAuditProperties(entry.Entity, userId);
            changeReport.ChangedEntities.Add(new EntityChangeEntry(entry.Entity, (EntityChangeType)2));
        }


        protected virtual void CancelDeletionForSoftDelete(EntityEntry entry)
        {
            if (!(entry.Entity is ISoftDelete))
                return;
            entry.Reload();
            entry.State = EntityState.Modified;
            ((ISoftDelete)ObjectExtensions.As<ISoftDelete>(entry.Entity)).IsDeleted = true;
        }


        protected virtual void CheckAndSetId(EntityEntry entry)
        {
            //IEntity<Guid> entity = entry.Entity as IEntity<Guid>;
            //if (entity == null || !(entity.Id == Guid.Empty))
            //    return;
            //DatabaseGeneratedAttribute attributeOrDefault = (DatabaseGeneratedAttribute)ReflectionHelper.GetSingleAttributeOrDefault<DatabaseGeneratedAttribute>((MemberInfo)entry.Property("Id").Metadata.PropertyInfo, (M0)null, true);
            //if (attributeOrDefault != null && attributeOrDefault.DatabaseGeneratedOption != DatabaseGeneratedOption.None)
            //    return;
            //entity.set_Id(this.GuidGenerator.Create());
        }

        protected int? AuditUserId { get; set; }

        public virtual int? GetAuditUserId()
        {
            //sacar id de la sesscion?
            return AuditUserId ?? (AuditUserId = authService.GetCurretUserId());
        }

    }




    public static class ConcurrencyExtensions
    {

        public static DateTime? BlockEntity(this DbContext context, Object entity, int userId)
        {
            IConcurrencyEntity concurrencyEntity = entity as IConcurrencyEntity;

            DateTime? resp = null;
            if (concurrencyEntity != null)
            {

                var sp = context.LoadStoredProc("dbo.sys_Concurrencia_BloquearEntidad")
                               .WithSqlParam("EntityName", new SqlParameter("EntityName", entity.GetType().FullName))
                               .WithSqlParam("UserId", new SqlParameter("UserId", userId))
                               .WithSqlParam("EnityId", new SqlParameter("EnityId", concurrencyEntity.GetEnityId()));

                sp.ExecuteStoredProc((handler) =>
                {
                    concurrencyEntity.BlockDate = resp = handler.ReadToValue<DateTime>();
                });
                
            }

            return resp;
        }


        public static void ValidateBlockEntity(this DbContext context, Object entity, int userId)
        {
            IConcurrencyEntity concurrencyEntity = entity as IConcurrencyEntity;
            IList<ItemDto> users = new List<ItemDto>();

            if (concurrencyEntity != null)
            {
                var sp = context.LoadStoredProc("dbo.sys_Concurrencia_UsuarioBloqueo")
                                .WithSqlParam("EntityName", new SqlParameter("EntityName", entity.GetType().FullName))
                                .WithSqlParam("EnityId", new SqlParameter("EnityId", concurrencyEntity.GetEnityId()));

                sp.ExecuteStoredProc((handler) =>
                {
                    users = handler.ReadToList<ItemDto>();
                });

                if (users.Count() > 0)
                {
                    var msj = $"{string.Join(",", users.Select(e => e.Description).Distinct())}";
                    int code = users.Any(e => e.Id.Equals(userId)) ? ConcurrencyException.ConcurrencyException_CurrentUser : ConcurrencyException.ConcurrencyException_OtherUser;
                    throw new ConcurrencyException(msj, code);
                }
            }
        }

        public static void UnBlockEntity(this DbContext context, Object entity, int userId)
        {
            IConcurrencyEntity concurrencyEntity = entity as IConcurrencyEntity;

            if (concurrencyEntity != null)
            {
                context.UnBlockEntityFull(concurrencyEntity.GetEnityId(), entity.GetType().FullName, userId);
            }
        }

        public static void UnBlockEntityFull(this DbContext context, string enityId, string entityName, int userId)
        {
            context.Database.ExecuteSqlCommand("exec dbo.sys_Concurrencia_DesbloquearEntidad @EntityName, @EnityId, @UserId ",
                    new SqlParameter("EntityName", entityName),
                    new SqlParameter("EnityId", enityId),
                    new SqlParameter("UserId", userId)
                    );
        }


        public static void ValidateCocurrencySave(this DbContext context, string enityId, string entityName, int userId, DateTime blockDate)
        {
            var sp = context.LoadStoredProc("dbo.sys_Concurrencia_ValidateCocurrencySave")
                               .WithSqlParam("EntityName", new SqlParameter("EntityName", entityName))
                               .WithSqlParam("EnityId", new SqlParameter("EnityId", enityId))
                               .WithSqlParam("UserId", new SqlParameter("UserId", userId))
                               .WithSqlParam("blockDate", new SqlParameter("blockDate", blockDate))
                               ;

            IList<ItemLongDto> items = new List<ItemLongDto>();

            sp.ExecuteStoredProc((handler) =>
            {
                items = handler.ReadToList<ItemLongDto>();
            });
            if (items.Count == 0)
            {
                throw new ValidationException("No es posible realizar los cambios ya que la entidad fue alterada por otro usuario.");
            }
        }
    }
}
