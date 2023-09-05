using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.AppService.Model
{
    [Serializable]
    public abstract class AuditedEntityDto : AuditedEntityDto<int>
    {
    }

    [Serializable]
    public abstract class CreationAuditedEntityDto : CreationAuditedEntityDto<int>
    {
    }

    [Serializable]
    public abstract class AuditedEntityDto<TPrimaryKey> : CreationAuditedEntityDto<TPrimaryKey>, IAudited, ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime
    {
        public DateTime? LastUpdatedDate { get; set; }

        public int? LastUpdatedUserId { get; set; }
    }

    [Serializable]
    public abstract class CreationAuditedEntityDto<TPrimaryKey> : EntityDto<TPrimaryKey>, ICreationAudited, IHasCreationTime
    {
        public DateTime? CreatedDate { get; set; }

        public int? CreatedUserId { get; set; }

        public int? GetCreatorUserId()
        {
            return CreatedUserId;
        }

        public void SetCreatorUserId(int? value)
        {
            CreatedUserId = value;
        }

        protected CreationAuditedEntityDto()
        {
            this.CreatedDate = DateTime.Now;
        }
    }

    [Serializable]
    public abstract class FullAuditedEntityDto : FullAuditedEntityDto<int>
    {
    }

    [Serializable]
    public abstract class FullAuditedEntityDto<TPrimaryKey> : AuditedEntityDto<TPrimaryKey>, IFullAudited, IAudited, ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime, IDeletionAudited, IHasDeletionTime, ISoftDelete
    {
        public bool IsDeleted { get; set; }

        public int? DeletedUserId { get; set; }

        public DateTime? DeletedDate { get; set; }
    }

    public abstract class FullAuditedEntityDtoWithAnulado<TPrimaryKey> : FullAuditedEntityDto<TPrimaryKey>
    {
        public bool Anulado { get; set; }
    }
}
