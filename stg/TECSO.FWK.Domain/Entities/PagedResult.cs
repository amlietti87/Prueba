using System;
using System.Collections.Generic;
using System.Text;

namespace TECSO.FWK.Domain.Entities
{
    public class PagedResult<T>: ListResult<T>
    {

        public PagedResult()
        {

        }

        public PagedResult(int totalCount, IReadOnlyList<T> items)
            :base(items)
        {
            this.TotalCount = totalCount;
        }

        public int TotalCount { get; set; }
    }
    
}
