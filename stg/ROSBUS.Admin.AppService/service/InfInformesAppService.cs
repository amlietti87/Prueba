using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class InfInformesAppService : AppServiceBase<InfInformes, InfInformesDto, string, IInfInformesService>, IInfInformesAppService
    {
        private readonly IHServiciosAppService hServiciosAppService;
        private readonly IAuthService authService;
        public InfInformesAppService(IInfInformesService serviceBase, IHServiciosAppService _hServiciosAppService, IAuthService _authService)
            : base(serviceBase)
        {
            hServiciosAppService = _hServiciosAppService;
            authService = _authService;
        }       

        public Task<List<InformeConsulta>> ConsultaInformesUserDia()
        {
            return this._serviceBase.ConsultaInformesUserDia();
        }
    }
}
