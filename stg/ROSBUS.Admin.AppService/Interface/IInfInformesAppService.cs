using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IInfInformesAppService : IAppServiceBase<InfInformes, InfInformesDto, string>
    {
        Task<List<InformeConsulta>> ConsultaInformesUserDia();
    }
}
