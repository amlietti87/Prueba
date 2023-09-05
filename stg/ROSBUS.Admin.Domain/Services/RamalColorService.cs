using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class RamalColorService : ServiceBase<PlaRamalColor,Int64, IRamalColorRepository>, IRamalColorService
    { 
        public RamalColorService(IRamalColorRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public async Task<PagedResult<PlaRamalColor>> GetAllSinSentidos(Expression<Func<PlaRamalColor, bool>> predicate, List<Expression<Func<PlaRamalColor, Object>>> includeExpression = null)
        {
           return await this.repository.GetAllSinSentidos(predicate, includeExpression);
        }
    }
    
}
