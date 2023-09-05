using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class HTposHorasAppService : AppServiceBase<HTposHoras, HTposHorasDto, string, IHTposHorasService>, IHTposHorasAppService
    {
        public HTposHorasAppService(IHTposHorasService serviceBase) 
            :base(serviceBase)
        {
         
        }

        public async override Task<PagedResult<HTposHorasDto>> GetDtoAllAsync(Expression<Func<HTposHoras, bool>> predicate, List<Expression<Func<HTposHoras, object>>> includeExpression = null)
        {
            var list = await this._serviceBase.GetAllAsync(predicate, includeExpression);

            var listDto = this.MapList<HTposHoras, HTposHorasDto>(list.Items.OrderBy(e => e.Orden));

            PagedResult<HTposHorasDto> pList = new PagedResult<HTposHorasDto>(list.TotalCount, listDto.ToList());

            return pList;
        }

    }
}
