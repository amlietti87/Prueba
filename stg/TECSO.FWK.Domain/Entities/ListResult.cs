using System;
using System.Collections.Generic;
using System.Text;

namespace TECSO.FWK.Domain.Entities
{
    public class ListResult<T>
    {
        private IReadOnlyList<T> _items;

        public ListResult()
        {

        }

        public ListResult(IReadOnlyList<T> items)
        {
            this.Items = items;
        }

        public IReadOnlyList<T> Items
        {
            get
            {
                return this._items ?? (this._items = (IReadOnlyList<T>)new List<T>());
            }
            set
            {
                this._items = value;
            }
        }
    }
}
