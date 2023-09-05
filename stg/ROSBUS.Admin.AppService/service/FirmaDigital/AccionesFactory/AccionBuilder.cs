using Microsoft.Extensions.DependencyInjection;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TECSO.FWK.Caching;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Domain.Mail;

namespace ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory
{
    public abstract class AccionBuilder<T> : IAccionBuilder
        where T : class
    {
        protected readonly IPermissionProvider permissionProvider;
        protected readonly IFdAccionesPermitidasAppService fdAccionesPermitidasService;

        protected readonly IFdDocumentosProcesadosAppService documentosprocesadosService;
        protected readonly ICacheManager cacheManager;
        protected readonly IAuthService AuthUserService;
        protected readonly ILogger logger;
        protected readonly INotificationAppService notificationAppService;
        protected readonly IServiceProvider serviceProvider;
        protected readonly IDefaultEmailer emailSender;

        protected T Value { get; set; }

        public AccionBuilder(IServiceProvider _serviceProvider)
        {
            try
            {

                serviceProvider = _serviceProvider;

                permissionProvider = serviceProvider.GetRequiredService<IPermissionProvider>();

                fdAccionesPermitidasService = serviceProvider.GetRequiredService<IFdAccionesPermitidasAppService>();

                documentosprocesadosService = serviceProvider.GetRequiredService<IFdDocumentosProcesadosAppService>();

                cacheManager = serviceProvider.GetRequiredService<ICacheManager>();

                AuthUserService = serviceProvider.GetRequiredService<IAuthService>();

                logger = serviceProvider.GetRequiredService<ILogger>();

                emailSender = serviceProvider.GetRequiredService<IDefaultEmailer>();

                this.notificationAppService = serviceProvider.GetRequiredService<INotificationAppService>(); ;
            }
            catch (Exception)
            {

            }
        }

        public async Task EjecutarAccion(AplicarAccioneDto dto)
        {
            await ValidatePermission(dto);
            await ValidateActions(dto);
            this.Value = await ExecuteAccionInternal(dto);
        }

        public Object ReturnValue()
        {
            return this.Value;
        }

        protected virtual async Task RealizarNotificacion(FdAcciones accion)
        {
            if (accion != null && accion.NotificacionId.HasValue)
            {
                
                //buscar destinatarios 
                List<Destinatario> destinatarios = await this.notificationAppService.GetDestinatariosNotificacionesMail(accion.NotificacionId.Value);
                
                string title = "Documentos procesados";
                string content = "Documentos procesados para la accion" + accion.AccionPermitida?.DisplayName; 

                foreach (var mail in destinatarios)
                {
                    await this.emailSender.SendDefaultAsync(mail.Email,title,content);
                }
            }
        }

        protected virtual FdDocumentosProcesadosHistorico CrearHistorico(FdDocumentosProcesados doc)
        {
            FdDocumentosProcesadosHistorico historico = new FdDocumentosProcesadosHistorico();
            historico.ArchivoId = doc.ArchivoId;
            historico.NombreArchivo = doc.NombreArchivo;
            historico.DocumentoProcesadoId = doc.Id;
            historico.Rechazado = doc.Rechazado;
            historico.EstadoId = doc.EstadoId;
            return historico;
        }

        protected virtual async Task<FdAcciones> RecuperarAccionPorTipoDoc(int tipoDocumentoId, int idEstadoActual)
        {
            var fdAccionesAppService = this.serviceProvider.GetRequiredService<IFdAccionesAppService>();

            var acciones = await fdAccionesAppService.GetAllAsync(e => e.TipoDocumentoId == tipoDocumentoId && e.EstadoActualId == idEstadoActual);

            if (acciones.Items.Count() != 1)
            {
                throw new ValidationException("No se pudo determinar la accion para el documento.");
            }

            return acciones.Items.FirstOrDefault();
        }

        protected virtual async Task ValidarAccionMenorFecha(FdDocumentosProcesados documento)
        {

            List<FdDocumentosProcesados> docs = new List<FdDocumentosProcesados>();
            
            if (documento.EstadoId == 3)
            {
                docs = (await this.documentosprocesadosService.GetAllAsync(e => e.TipoDocumentoId == documento.TipoDocumentoId && e.EstadoId == documento.EstadoId && e.Fecha < documento.Fecha && e.EmpleadoId == documento.EmpleadoId)).Items.ToList();
            }
            else 
            {
                docs = (await this.documentosprocesadosService.GetAllAsync(e => e.TipoDocumentoId == documento.TipoDocumentoId && e.EstadoId == documento.EstadoId && e.Fecha < documento.Fecha)).Items.ToList();
            }
            

            if (docs.Count > 0)
            {
                throw new ValidationException("Debe marcar el documento con menor fecha para ejecutar la acción.");
            }
        }

        protected virtual async Task<T> ExecuteAccionInternal(AplicarAccioneDto dto)
        {
            return null;
        }

        public virtual async Task ValidatePermission(AplicarAccioneDto accion)
        {
            //TODO: Validar Permiso
            if (!accion.AccionId.HasValue)
            {
                throw new ValidationException("Necesito una acción");
            }

            String[] usuarioPermisosToken = await this.permissionProvider.GetPermissionForCurrentUser();
            string token;
            if (accion.AccionId.Value > 0)
            {
                token = (await this.fdAccionesPermitidasService.GetDtoByIdAsync(accion.AccionId.Value)).TokenPermission;
            }
            else
            {
                token = GetTokenAccionesGenerales(accion);
            }

            if (token != null & !usuarioPermisosToken.Contains(token))
            {
                throw new ValidationException("No tiene permisos para realizar esta acción");
            };



        }

        public virtual async Task ValidateActions(AplicarAccioneDto dto) 
        {
            var Documentos = (await this.documentosprocesadosService.GetAllAsync(e => dto.Documentos.Contains(e.Id),
                       new List<Expression<Func<FdDocumentosProcesados, object>>>() {
                        e => e.TipoDocumento
                           })).Items.ToList();

            foreach (var doc in Documentos)
            {
                FdAcciones accion = new FdAcciones();
                if (dto.AccionId == 1 || dto.AccionId == 2 || dto.AccionId == 3 || dto.AccionId == -3)
                {
                    accion = await this.RecuperarAccionPorTipoDoc(doc.TipoDocumentoId, doc.EstadoId);
                }
 

                if (dto.AccionId == 1 && dto.AccionId != accion.AccionPermitidaId)
                {
                    throw new ValidationException("El documento " + doc.TipoDocumento.Descripcion + " con fecha " + doc.Fecha.ToString("dd/MM/yyyy") + " no puede ser aprobado");
                }

                if (dto.AccionId == 2 && dto.AccionId != accion.AccionPermitidaId)
                {
                    throw new ValidationException("El documento " + doc.TipoDocumento.Descripcion + " con fecha " + doc.Fecha.ToString("dd/MM/yyyy") + " no puede ser firmado");
                }

                if (dto.AccionId == 3 && dto.AccionId != accion.AccionPermitidaId)
                {
                    throw new ValidationException("El documento " + doc.TipoDocumento.Descripcion + " con fecha " + doc.Fecha.ToString("dd/MM/yyyy") + " no puede ser firmado");
                }

                if (dto.AccionId == -3)
                {
                    if (doc.Rechazado || (doc.Estado != null && doc.Estado.PermiteRechazo == false))
                    {
                        throw new ValidationException("El documento " + doc.TipoDocumento.Descripcion + " con fecha " + doc.Fecha.ToString("dd/MM/yyyy") + " no permite rechazo");
                    }
                }
            }


        }
        public string GetTokenAccionesGenerales(AplicarAccioneDto accion)
        {
            if (accion.AccionId == -1)
            {
                if (accion.Empleador)
                {
                    return "FirmaDigital.BD-Empleador.Abrir";
                }
                else
                {
                    return "FirmaDigital.BD-Empleado.Abrir";
                }
            }
            else if (accion.AccionId == -2)
            {
                if (accion.Empleador)
                {
                    return "FirmaDigital.BD-Empleador.Descargar";
                }
                else
                {
                    return "FirmaDigital.BD-Empleado.Descargar";
                }
            }
            else if (accion.AccionId == -3)
            {
                return "FirmaDigital.BD-Empleador.Rechazar";
            }
            else if (accion.AccionId == -4)
            {
                return "FirmaDigital.BD-Empleador.Ver";
            }
            else if (accion.AccionId == -6)
            {
                return "FirmaDigital.Archivos.RevisarErrores";
            }
            else if (accion.AccionId == -7)
            {
                return null;
            }
            else
            {
                throw new ValidationException("Acción invalida");
            }
        }



    }

    public interface IAccionBuilder
    {
        Task EjecutarAccion(AplicarAccioneDto dto);
        object ReturnValue();
    }
}