using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class SubEstadosAppService : AppServiceBase<SinSubEstados, SubEstadosDto, int, ISubEstadosService>, ISubEstadosAppService
    {
        public SubEstadosAppService(ISubEstadosService serviceBase) 
            :base(serviceBase)
        {
         
        }

        public override Task<PagedResult<SubEstadosDto>> GetDtoAllAsync(Expression<Func<SinSubEstados, bool>> predicate, List<Expression<Func<SinSubEstados, object>>> includeExpression = null)
        {
            return base.GetDtoAllAsync(predicate, includeExpression);
        }

        public override Task<PagedResult<SubEstadosDto>> GetDtoPagedListAsync<TFilter>(TFilter filter)
        {
            return base.GetDtoPagedListAsync(filter);
        }

        public override Task<List<ItemDto<int>>> GetItemsAsync<TFilter>(TFilter filter)
        {
            return base.GetItemsAsync(filter);
        }

        public override PagedResult<SinSubEstados> GetPagedList<TFilter>(TFilter filter)
        {
            return base.GetPagedList(filter);
        }

        public override Task<PagedResult<SinSubEstados>> GetPagedListAsync<TFilter>(TFilter filter)
        {
            return base.GetPagedListAsync(filter);
        }
    }
}
