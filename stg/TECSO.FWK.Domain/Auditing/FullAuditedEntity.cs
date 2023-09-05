
using System;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Interfaces;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.Domain.Auditing
{
    [Serializable]
    public abstract class FullAuditedEntity : FullAuditedEntity<int>, IEntity, IEntity<int>
    {
    }

    [Serializable]
    public abstract class FullAuditedEntity<TPrimaryKey, TUser> : AuditedEntity<TPrimaryKey, TUser>, IFullAudited<TUser>, IAudited<TUser>, IAudited, ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime, ICreationAudited<TUser>, IModificationAudited<TUser>, IFullAudited, IDeletionAudited, IHasDeletionTime, ISoftDelete, IDeletionAudited<TUser> 
        where TUser : IEntity<int>
    {
        public virtual bool IsDeleted { get; set; }

        [ForeignKey("DeletedUserId")]
        public virtual TUser DeletedUser { get; set; }

        public virtual int? DeletedUserId { get; set; }

        public virtual DateTime? DeletedDate { get; set; }
    }

    [Serializable]
    public abstract class FullAuditedEntity<TPrimaryKey> : AuditedEntity<TPrimaryKey>, IFullAudited, IAudited, ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime, IDeletionAudited, IHasDeletionTime, ISoftDelete
    {
        public virtual bool IsDeleted { get; set; }

        public virtual int? DeletedUserId { get; set; }

        public virtual DateTime? DeletedDate { get; set; }
    }
}
