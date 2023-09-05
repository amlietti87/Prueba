using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class FdAccionesAppService : AppServiceBase<FdAcciones, FdAccionesDto, int, IFdAccionesService>, IFdAccionesAppService
    {
        private readonly IAccionesFactory _factory;

        public FdAccionesAppService(IFdAccionesService serviceBase, IAccionesFactory factory) 
            :base(serviceBase)
        {
            _factory = factory;
            //_factory.Inicialize();
        }

       

        public async Task<Object> AplicarAccion(AplicarAccioneDto dto)
        {
            var ac = _factory.GetAccionFromId(dto.AccionId.GetValueOrDefault());

            if (ac == null)
            {
                throw new ValidationException("Accion no encontrada");
            }

            await ac.EjecutarAccion(dto);
            return ac.ReturnValue();

        }
    }
}
