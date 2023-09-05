
// Type: TECSO.FWK.Domain.Auditing.FullAuditedAggregateRoot




using System;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.Domain.Auditing
{
    //[Serializable]
    //public abstract class FullAuditedAggregateRoot : FullAuditedAggregateRoot<int>
    //{
    //}

    //[Serializable]
    //public abstract class FullAuditedAggregateRoot<TPrimaryKey, TUser> : AuditedAggregateRoot<TPrimaryKey, TUser>, IFullAudited<TUser>, IAudited<TUser>, IAudited, ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime, ICreationAudited<TUser>, IModificationAudited<TUser>, IFullAudited, IDeletionAudited, IHasDeletionTime, ISoftDelete, IDeletionAudited<TUser> where TUser : IEntity<long>
    //{
    //    public virtual bool IsDeleted { get; set; }

    //    [ForeignKey("DeleterUserId")]
    //    public virtual TUser DeleterUser { get; set; }

    //    public virtual long? DeleterUserId { get; set; }
    
    //    public virtual DateTime? DeletionTime { get; set; }
    //}


    //[Serializable]
    //public abstract class FullAuditedAggregateRoot<TPrimaryKey> : AuditedAggregateRoot<TPrimaryKey>, IFullAudited, IAudited, ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime, IDeletionAudited, IHasDeletionTime, ISoftDelete
    //{
    //    public virtual bool IsDeleted { get; set; }

    //    public virtual long? DeleterUserId { get; set; }

    //    public virtual DateTime? DeletionTime { get; set; }
    //}
}
