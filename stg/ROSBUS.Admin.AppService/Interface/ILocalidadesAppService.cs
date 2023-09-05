using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface ILocalidadesAppService : IAppServiceBase<Localidades, LocalidadesDto, int>
    {

        Task<PagedResult<Localidades>> GetAllLocalidades();
    }
}
