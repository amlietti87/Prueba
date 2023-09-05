using ROSBUS.ART.AppService.Interface;
using ROSBUS.ART.AppService.Model;
using ROSBUS.ART.Domain.Entities;
using ROSBUS.ART.Domain.Entities.ART;
using ROSBUS.ART.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.ART.AppService
{

    public class TiposAcuerdoAppService : AppServiceBase<TiposAcuerdo, TiposAcuerdoDto, int, ITiposAcuerdoService>, ITiposAcuerdoAppService
    {
        public TiposAcuerdoAppService(ITiposAcuerdoService serviceBase) 
            :base(serviceBase)
        {
         
        } 
    }
}
