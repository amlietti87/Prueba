using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class CroElemenetoAppService : AppServiceBase<CroElemeneto, CroElemenetoDto, Guid, ICroElemenetoService>, ICroElemenetoAppService
    {
        private readonly IAdjuntosService _adjuntosService;
        public CroElemenetoAppService(ICroElemenetoService serviceBase, IAdjuntosService adjuntosService) 
            :base(serviceBase)
        {
            _adjuntosService = adjuntosService;
        }

        public async Task<AdjuntosDto> GetAdjunto(Guid Id)
        {
           var  adjunto = await _adjuntosService.GetAdjuntoItemDto(Id);

            AdjuntosDto adj = new AdjuntosDto();
            adj.Id = adjunto.Id;
            adj.Nombre = adjunto.Description;
            return adj;
        }



    }
}
