using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Caching;

namespace ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory
{
    public class Accion_DescargarArchivo : AccionBuilder<List<Guid>>, IAccion_DescargarArchivo
    {


        public Accion_DescargarArchivo(IServiceProvider _serviceProvider) : base(_serviceProvider) {

        }



        protected override async Task<List<Guid>> ExecuteAccionInternal(AplicarAccioneDto dto)
        {
            var x = await this.documentosprocesadosService.GetAllAsync(e => dto.Documentos.Contains(e.Id));

            var archivos = x.Items.Select(e => e.ArchivoId).ToList();
            return archivos;
        }




    }

    public interface IAccion_DescargarArchivo: IAccionBuilder
    {
    }
}