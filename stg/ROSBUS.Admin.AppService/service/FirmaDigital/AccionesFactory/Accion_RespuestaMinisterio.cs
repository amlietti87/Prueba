using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using System;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory
{
    public class Accion_RespuestaMinisterio : AccionBuilder<dynamic>, IAccion_RespuestaMinisterio
    {
        public Accion_RespuestaMinisterio(IServiceProvider _serviceProvider) : base(_serviceProvider)
        {

        }
        protected override async Task<dynamic> ExecuteAccionInternal(AplicarAccioneDto dto)
        {
            return base.ExecuteAccionInternal(dto);
        }

    }

    public interface IAccion_RespuestaMinisterio : IAccionBuilder
    {
    }
}