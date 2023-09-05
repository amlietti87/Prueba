using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class PlaMinutosPorSectorAppService : AppServiceBase<PlaMinutosPorSector, PlaMinutosPorSectorDto, long, IPlaMinutosPorSectorService>, IPlaMinutosPorSectorAppService
    {
        public PlaMinutosPorSectorAppService(IPlaMinutosPorSectorService serviceBase) 
            :base(serviceBase)
        {
         
        } 
    }
}
