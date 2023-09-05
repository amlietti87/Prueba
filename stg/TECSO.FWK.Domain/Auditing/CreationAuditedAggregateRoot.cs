using System;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.Domain.Auditing
{
    [Serializable]
    public abstract class CreationAuditedAggregateRoot : CreationAuditedAggregateRoot<int>
    {
    }

    [Serializable]
    public abstract class CreationAuditedAggregateRoot<TPrimaryKey, TUser> : CreationAuditedAggregateRoot<TPrimaryKey>, ICreationAudited<TUser>, ICreationAudited, IHasCreationTime 
        where TUser : IEntity<int>
    {
        [ForeignKey("CreatedUserId")]
        public virtual TUser CreatedUser { get; set; }
    }

    [Serializable]
    public abstract class CreationAuditedAggregateRoot<TPrimaryKey> : AggregateRoot<TPrimaryKey>, ICreationAudited, IHasCreationTime
    {
        public virtual DateTime? CreatedDate { get; set; }

        public int? CreatedUserId { get; set; }

        public virtual int? GetCreatorUserId()
        {
            return CreatedUserId;
        }

        public virtual void SetCreatorUserId(int? value)
        {
            CreatedUserId = value;
        }

        protected CreationAuditedAggregateRoot()
        {
            this.CreatedDate = DateTime.Now;
        }
    }
}
