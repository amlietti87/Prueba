using System;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.Domain.Auditing
{

    [Serializable]
    public abstract class AuditedEntity : AuditedEntity<int>, IEntity, IEntity<int>
    {
    }

    [Serializable]
    public abstract class AuditedEntity<TPrimaryKey> : CreationAuditedEntity<TPrimaryKey>, IAudited, ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime
    {
        public virtual DateTime? LastUpdatedDate { get; set; }

        public virtual int? LastUpdatedUserId { get; set; }
    }

    [Serializable]
    public abstract class AuditedEntity<TPrimaryKey, TUser> : AuditedEntity<TPrimaryKey>, IAudited<TUser>, IAudited, ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime, ICreationAudited<TUser>, IModificationAudited<TUser> 
        where TUser : IEntity<int>
    {
        [ForeignKey("CreatedUserId")]
        public virtual TUser CreatedUser { get; set; }

        [ForeignKey("LastUpdatedUserId")]
        public virtual TUser LastUpdatedUser { get; set; }
    }
}
