using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory
{
    public class Accion_AbrirArchivo : AccionBuilder<List<VisorArchivos>>, IAccion_AbrirArchivo
    {
        public Accion_AbrirArchivo(IServiceProvider _serviceProvider) : base(_serviceProvider)
        {

        }

        protected override async Task<List<VisorArchivos>> ExecuteAccionInternal(AplicarAccioneDto dto)
        {
            var archivos = (await this.documentosprocesadosService.GetAllAsync(e => dto.Documentos.Contains(e.Id),
                new List<Expression<Func<FdDocumentosProcesados, object>>>() {
                e => e.TipoDocumento
            })).Items.Select(e => new VisorArchivos() { Fecha = e.Fecha.ToString("dd/MM/yyyy"), NombreEmpleado = e.NombreEmpleado, TipoDocumento = e.TipoDocumento.Descripcion, Archivo = e.ArchivoId }).ToList();

            return archivos;
        }

    }

    public interface IAccion_AbrirArchivo : IAccionBuilder
    {
    }

    public class VisorArchivos
    {
        public string NombreEmpleado { get; set; }
        public string Fecha { get; set; }
        public string TipoDocumento { get; set; }
        public Guid Archivo { get; set; }
    }
}