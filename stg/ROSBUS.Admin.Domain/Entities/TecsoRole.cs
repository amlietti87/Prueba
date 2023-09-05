using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public abstract class TecsoRole<TUser> : RoleBase, IFullAudited<TUser>, IAudited<TUser>, IAudited, ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime, ICreationAudited<TUser>, IModificationAudited<TUser>, IFullAudited, IDeletionAudited, IHasDeletionTime, ISoftDelete, IDeletionAudited<TUser> 
        where TUser : TecsoUser<TUser>
    {

        public virtual TUser DeletedUser { get; set; }
        public virtual TUser CreatedUser { get; set; }
        public virtual TUser LastUpdatedUser { get; set; }

    }
     
    public abstract class RoleBase : FullAuditedEntity<int>
    {
        //
        // Summary:
        //     Unique name of this role.
        [Required]
        [StringLength(32)]
        public virtual string Name { get; set; }
        //
        // Summary:
        //     Display name of this role.
        [Required]
        [StringLength(64)]
        public virtual string DisplayName { get; set; }
        //
        // Summary:
        //     Is this a static role? Static roles can not be deleted, can not change their
        //     name. They can be used programmatically.
        public virtual bool IsStatic { get; set; }
        //
        // Summary:
        //     Is this role will be assigned to new users as default?
        public virtual bool IsDefault { get; set; }
        //
        // Summary:
        //     List of permissions of the role.
        //[ForeignKey("RoleId")]
        //public virtual ICollection<RolePermissionSetting> Permissions { get; set; }

    }
     
    

}
