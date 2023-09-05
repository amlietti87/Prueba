using System;
using System.Linq.Expressions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.Domain.Entities
{
    public interface IFilterPagedListBase<TModel, TPrimaryKey>
        where TModel : Entity<TPrimaryKey>
    {

        int? Page { get; set; }


        int? PageSize { get; set; }


        String Sort { get; set; }

        Expression<Func<TModel, bool>> GetFilterExpression();
    } 

}