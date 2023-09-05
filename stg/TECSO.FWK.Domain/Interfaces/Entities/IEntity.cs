using System;
using System.Collections.Generic;
using System.Text;

namespace TECSO.FWK.Domain.Interfaces.Entities
{
    public interface IEntity<TPrimaryKey>
    {
        //
        // Summary:
        //     Unique identifier for this entity.
        TPrimaryKey Id { get; set; }

        bool IsTransient();
    }
    public interface IEntity : IEntity<int>
    {
    }

    public interface ISoftDelete
    {
        bool IsDeleted { get; set; }
    }


    public interface IViewEntity<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }

        string Description { get; }


    }


    [Serializable]
    public class ItemDto<TPrimaryKey> : IViewEntity<TPrimaryKey>
    {
        public string Description { get; set; }
        public bool IsSelected { get; set; }

        public TPrimaryKey Id { get; set; }

        public ItemDto()
        {
        }

        public ItemDto(TPrimaryKey value, string displayText)
        {
            this.Id = value;
            this.Description = displayText.Trim();
        }
    }

    [Serializable]
    public class ItemDto : ItemDto<int>
    {
        public ItemDto()
        {
        }

        public ItemDto(int value, string displayText) : base(value, displayText)
        {

        }
    }

    [Serializable]
    public class ItemLongDto : ItemDto<long>
    {
        public ItemLongDto()
        {
        }

        public ItemLongDto(long value, string displayText, bool IsSelected = false) : base(value, displayText)
        {
            this.IsSelected = IsSelected;
        }
    }

    [Serializable]
    public class ItemDecimalDto : ItemDto<decimal>
    {
        public ItemDecimalDto()
        {
        }

        public ItemDecimalDto(decimal value, string displayText, bool IsSelected = false) : base(value, displayText)
        {
            this.IsSelected = IsSelected;
        }
    }

}
