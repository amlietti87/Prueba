using System;
using System.Collections.Generic;
using System.Text;

namespace TECSO.FWK.Domain.Entities
{
    public abstract class EntityDto<TPrimaryKey> : IEntityDto<TPrimaryKey>
    {
        public EntityDto()
        {

        }


        public TPrimaryKey Id { get; set; }
        public abstract string Description { get; }
    }

    public interface IEntityDto<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }

        string Description { get; }
    }
}
