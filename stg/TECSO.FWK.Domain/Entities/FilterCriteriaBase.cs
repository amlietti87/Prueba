using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq.Expressions;
using System.Text;

namespace TECSO.FWK.Domain.Entities
{
    public class FilterCriteriaBase<TPrimaryKey>
    {
        
        public TPrimaryKey Id { get; set; }


        
    }
}
