using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IRamalColorService : IServiceBase<PlaRamalColor, Int64>
    {
        Task<PagedResult<PlaRamalColor>> GetAllSinSentidos(Expression<Func<PlaRamalColor, bool>> predicate, List<Expression<Func<PlaRamalColor, Object>>> includeExpression = null);
    }
}
