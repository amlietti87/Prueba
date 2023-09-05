using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    

    public abstract class TecsoUser<TUser> : UserBase, IFullAudited<TUser>, IAudited<TUser>, IAudited, ICreationAudited, IHasCreationTime, IModificationAudited, IHasModificationTime, ICreationAudited<TUser>, IModificationAudited<TUser>, IFullAudited, IDeletionAudited, IHasDeletionTime, ISoftDelete, IDeletionAudited<TUser> 
        where TUser : TecsoUser<TUser>
    { 
        public virtual TUser DeletedUser { get; set; }
        public virtual TUser CreatedUser { get; set; }
        public virtual TUser LastUpdatedUser { get; set; }
    }


    public abstract class UserBase : FullAuditedEntity<int>
    {
        protected UserBase()
        {

        }

         
        //public string UserName { get; set; }
           
        
    }

}
