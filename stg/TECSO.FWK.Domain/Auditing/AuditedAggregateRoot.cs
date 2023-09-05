using System;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.Domain.Auditing
{
  [Serializable]
  public abstract class AuditedAggregateRoot : AuditedAggregateRoot<int>
  {
  }
    [Serializable]
    public abstract class AuditedAggregateRoot<TPrimaryKey> : CreationAuditedAggregateRoot<TPrimaryKey>, IAudited, ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime
    {
        public virtual DateTime? LastUpdatedDate { get; set; }

        public virtual int? LastUpdatedUserId { get; set; }
    }

    [Serializable]
    public abstract class AuditedAggregateRoot<TPrimaryKey, TUser> : AuditedAggregateRoot<TPrimaryKey>, IAudited<TUser>, IAudited, ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime, ICreationAudited<TUser>, IModificationAudited<TUser> 
        where TUser : IEntity<int>
    {
        [ForeignKey("CreatedUserId")]
        public virtual TUser CreatedUser { get; set; }

        [ForeignKey("LastUpdatedUserId")]
        public virtual TUser LastUpdatedUser { get; set; }
    }
}
