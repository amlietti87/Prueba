using Microsoft.Extensions.DependencyInjection;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory
{
    public class Accion_RevisarArchivo : AccionBuilder<AplicarAccioneResponseDto>, IAccion_RevisarArchivo
    {

        protected readonly IFdDocumentosErrorAppService documentosErrorService;
        public Accion_RevisarArchivo(IServiceProvider _serviceProvider) : base(_serviceProvider)
        {
            documentosErrorService = serviceProvider.GetRequiredService<IFdDocumentosErrorAppService>();
        }

        protected override async Task<AplicarAccioneResponseDto> ExecuteAccionInternal(AplicarAccioneDto dto)
        {
            AplicarAccioneResponseDto responseDto = new AplicarAccioneResponseDto();

            var Documentos = (await this.documentosErrorService.GetAllAsync(e => dto.Documentos.Contains(e.Id),
                    new List<Expression<Func<FdDocumentosError, object>>>() {
                e => e.TipoDocumento
})).Items.ToList();

            foreach (var item in Documentos)
            {
                DetalleResponse detalle = new DetalleResponse();
                detalle.IsValid = true;
                //fecha - desc tipo documento-apellido y nombre empleado
                detalle.Documento = String.Format("{0} - {1} - {2}",
                    item.Fecha.HasValue ? item.Fecha.Value.ToString("yyyy-MM-dd") : string.Empty,
                    item.TipoDocumento?.Descripcion,
                    item.NombreEmpleado.Trim());

                try
                {
                    await this.logger.LogInformation($"Evaluando el Documento {item.Id}");

                    var doc = await this.documentosErrorService.GetByIdAsync(item.Id);

                    if (doc.Revisado)
                    {
                        throw new ValidationException("No se puede pasar a Revisado = True cuando ya está en este valor.");
                    }

                    this.documentosErrorService.GuardarRevisado(doc);
                }


                catch (Exception ex)
                {
                    detalle.IsValid = false;

                    detalle.Error += $"Error al procesar el archivo. ";
                    detalle.Error += $"{ex.Message}";

                    await this.logger.LogInformation($"Error: {ex.Message}");
                    await this.logger.LogInformation($"StackTrace: {ex.StackTrace}");
                }
                finally
                {
                    responseDto.Detalles.Add(detalle);
                }
            }

            return responseDto;
        }

    }

    public interface IAccion_RevisarArchivo : IAccionBuilder
    {
    }
}