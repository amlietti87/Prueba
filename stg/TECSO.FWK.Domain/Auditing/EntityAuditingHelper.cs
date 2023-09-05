using System;

namespace TECSO.FWK.Domain.Auditing
{
    public static class EntityAuditingHelper
    {
        public static void SetCreationAuditProperties(object entityAsObj, int? userId)
        {
            IHasCreationTime hasCreationTime = entityAsObj as IHasCreationTime;
            if (hasCreationTime == null)
                return;
            if (hasCreationTime.CreatedDate == new DateTime())
                hasCreationTime.CreatedDate = DateTime.Now;
            if (!(entityAsObj is ICreationAudited) || !userId.HasValue)
                return;
            ICreationAudited creationAudited = entityAsObj as ICreationAudited;
            if (creationAudited.GetCreatorUserId().HasValue)
                return;
            creationAudited.SetCreatorUserId(userId);
        }

        public static void SetModificationAuditProperties(object entityAsObj, int? userId)
        {
            if (entityAsObj is IHasModificationTime)
                entityAsObj.As<IHasModificationTime>().LastUpdatedDate = new DateTime?(DateTime.Now);
            if (!(entityAsObj is IModificationAudited))
                return;
            IModificationAudited modificationAudited = entityAsObj.As<IModificationAudited>();
            if (!userId.HasValue)
            {
                modificationAudited.LastUpdatedUserId = new int?();
            }
            else
            {
                modificationAudited.LastUpdatedUserId = userId;
            }
        }


        public static void SetDeletionAuditProperties(object entityAsObj, int? userId)
        {
            if (entityAsObj is IHasDeletionTime)
            {
                IHasDeletionTime ihasDeletionTime = (IHasDeletionTime)ObjectExtensions.As<IHasDeletionTime>(entityAsObj);
                if (!ihasDeletionTime.DeletedDate.HasValue)
                    ihasDeletionTime.DeletedDate = (new DateTime?(DateTime.Now));
            }
            if (!(entityAsObj is IDeletionAudited))
                return;
            IDeletionAudited ideletionAudited = (IDeletionAudited)ObjectExtensions.As<IDeletionAudited>(entityAsObj);
            if (ideletionAudited.DeletedUserId.HasValue)
                return;
            if (!userId.HasValue)
                ideletionAudited.DeletedUserId = (new int?());
            else
                ideletionAudited.DeletedUserId = (userId);
        }


    }
}
