using Microsoft.Extensions.DependencyInjection;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Entities.Partials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory
{
    public class Accion_AprobarDocumento : AccionBuilder<AplicarAccioneResponseDto>, IAccion_AprobarDocumento
    {
        public Accion_AprobarDocumento(IServiceProvider _serviceProvider)
            : base(_serviceProvider)
        {

        }

        protected override async Task<AplicarAccioneResponseDto> ExecuteAccionInternal(AplicarAccioneDto dto)
        {

            AplicarAccioneResponseDto responseDto = new AplicarAccioneResponseDto();

            var Documentos = (await this.documentosprocesadosService.GetAllAsync(e => dto.Documentos.Contains(e.Id),
                                new List<Expression<Func<FdDocumentosProcesados, object>>>() {
                e => e.TipoDocumento
            })).Items.ToList();

            List<FdAcciones> accionesEjecutadas = new List<FdAcciones>();

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

                    doc.EstadoId = accion.EstadoNuevoId;

                    if (accion.EsFin)
                    {
                        doc.Cerrado = true;
                    }

                    this.documentosprocesadosService.GuardarDocumento(doc, historico,null);

                    if (accion.GeneraNotificacion && accion.NotificacionId.HasValue)
                    {
                        accionesEjecutadas.Add(accion);
                        
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

            //Mandar solo una notificacion por cada NotificacioID
            foreach (var item in accionesEjecutadas.Select(e=> e.NotificacionId).Distinct())
            {
                await this.RealizarNotificacion(accionesEjecutadas.FirstOrDefault(e=> e.NotificacionId == item));
            }

            return responseDto;
        }

        protected override async Task RealizarNotificacion(FdAcciones accion)
        {

            /*
             Se tiene que desarrollar la notificación a generar al Aprobar documento.

                Asunto: Documentos aprobados pendientes de firma
                Cuerpo: Existen documentos listos para ser firmados. Seleccione la acción "Firmar Empleador" en la BD-Empleador del sistema de gestión documental.

                La notificación se envía al usuario suscrito a la notificación configurada en FD_Acciones.notificacionId para todos los tipos de documento que cambió de estado, 
                siempre que FD_Acciones.generaNotificacion=1
             */


            if (accion != null && accion.NotificacionId.HasValue && accion.GeneraNotificacion)
            {

                //buscar destinatarios 
                List<Destinatario> destinatarios = await this.notificationAppService.GetDestinatariosNotificacionesMail(accion.NotificacionId.Value);

                string title = "Documentos aprobados pendientes de firma";
                string content = "Existen documentos listos para ser firmados. Seleccione la acción \"Firmar Empleador\" en la BD-Empleador del sistema de gestión documental.";

                foreach (var mail in destinatarios)
                {
                    await this.emailSender.SendDefaultAsync(mail.Email, title, content);
                }
            }
        }

    }

    public interface IAccion_AprobarDocumento : IAccionBuilder
    {
    }
}