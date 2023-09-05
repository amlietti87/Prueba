using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class TipoLineaAppService : AppServiceBase<PlaTipoLinea, TipoLineaDto, int, ITipoLineaService>, ITipoLineaAppService
    {
        public TipoLineaAppService(ITipoLineaService serviceBase) 
            :base(serviceBase)
        {
         
        } 

        public async Task<List<PlaTipoLinea>> RecuperarTipoLineaPorSector(HDesignarFilter Filtro)
        {
            var tipoLineas = await _serviceBase.RecuperarTipoLineaPorSector(Filtro);
            return tipoLineas;

        }
    }
}
