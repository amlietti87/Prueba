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
    public class Accion_RechazarDocumento : AccionBuilder<AplicarAccioneResponseDto>, IAccion_RechazarDocumento
    {
        public Accion_RechazarDocumento(IServiceProvider _serviceProvider) : base(_serviceProvider)
        {

        }

        protected override async Task<AplicarAccioneResponseDto> ExecuteAccionInternal(AplicarAccioneDto dto)
        {
            AplicarAccioneResponseDto responseDto = new AplicarAccioneResponseDto();

            var Documentos = (await this.documentosprocesadosService.GetAllAsync(e => dto.Documentos.Contains(e.Id),
                                new List<Expression<Func<FdDocumentosProcesados, object>>>() {
                e => e.TipoDocumento
            })).Items.ToList();

            foreach (var doc in Documentos.OrderBy(e => e.Fecha))
            {

                DetalleResponse detalle = new DetalleResponse();
                detalle.IsValid = true;
                //fecha - desc tipo documento-apellido y nombre empleado
                detalle.Documento = String.Format("{0} - {1} - {2}", 
                    doc.Fecha.ToString("yyyy-MM-dd"),
                    doc.TipoDocumento?.Descripcion,
                    doc.NombreEmpleado.Trim());

                try
                {
                    await this.logger.LogInformation($"Evaluando el Documento {doc.Id}");

                    FdAcciones accion = await this.RecuperarAccionPorTipoDoc(doc.TipoDocumentoId, doc.EstadoId);

                    if (accion.MenorFechaPrimero == true)
                    {
                        await this.ValidarAccionMenorFecha(doc);
                    }

                    FdDocumentosProcesadosHistorico historico = this.CrearHistorico(doc);

                    doc.MotivoRechazo = dto.Motivo;
                    doc.Rechazado = true;

                    this.documentosprocesadosService.GuardarDocumento(doc, historico, null);

                    if (accion.GeneraNotificacion)
                    {
                        await this.RealizarNotificacion(accion);
                    }

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

    public interface IAccion_RechazarDocumento : IAccionBuilder
    {
    }
}