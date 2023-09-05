using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.service.http;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory.Firmador
{
    public class AccionFirmadorBase : AccionBuilder<dynamic>
    {
        private readonly IFdFirmadorAppService _fdFirmadorAppService;
        private readonly IAdjuntosService _adjuntosService;

        public ISignalRHttpService _signalRHttpService { get; }

        private readonly IUserService _userService;
        private readonly IFirmadorHelper _firmadorHelper;

        public AccionFirmadorBase(IServiceProvider _serviceProvider, IFirmadorHelper firmadorHelper, IFdFirmadorAppService fdFirmadorAppService, IAdjuntosService adjuntosService, ISignalRHttpService signalRHttpService, IUserService userService)
            : base(_serviceProvider)
        {
            _firmadorHelper = firmadorHelper;
            _fdFirmadorAppService = fdFirmadorAppService;
            _adjuntosService = adjuntosService;
            _signalRHttpService = signalRHttpService;
            _userService = userService;
        }


        public override async Task ValidatePermission(AplicarAccioneDto accion)
        {
            if (!accion.EsFirmador)
            {
                await base.ValidatePermission(accion);
            }
        }

        protected override async Task<dynamic> ExecuteAccionInternal(AplicarAccioneDto dto)
        {
            if (!dto.EsFirmador)
            {
                try
                {
                    var Documentos = (await this.documentosprocesadosService.GetAllAsync(e => dto.Documentos.Contains(e.Id),
                                           new List<Expression<Func<FdDocumentosProcesados, object>>>() { e => e.TipoDocumento })).Items.ToList();

                    foreach (var doc in Documentos.OrderBy(e => e.Fecha))
                    {
                        await this.logger.LogInformation($"Evaluando el Documento {doc.Id}");

                        FdAcciones accion = await this.RecuperarAccionPorTipoDoc(doc.TipoDocumentoId, doc.EstadoId);

                        if (accion.MenorFechaPrimero == true)
                        {
                            await this.ValidarAccionMenorFecha(doc);
                        }
                    }
                    var id = Guid.NewGuid().ToString();

                    FdFirmadorDto firmadorDto = await _firmadorHelper.RecuperarJNLP(dto);

                    FileDto filedto = new FileDto();
                    filedto.ByteArray = firmadorDto.file;
                    filedto.FileDescription = "ArchivoFirmador";
                    filedto.FileName = "ArchivoFirmador_" + DateTime.Now.ToString("yyyyMMddhhmmss") + ".jnlp";
                    filedto.FileType = "application/x-java-jnlp-file";
                    filedto.ForceDownload = true;
                    filedto.Id = id;

                    AplicarAccioneResponseDto responseDto = new AplicarAccioneResponseDto();
                    responseDto.FileDto = filedto;
                    responseDto.FirmadorId = firmadorDto.Id;

                    return responseDto;
                }
                catch (Exception ex)
                {
                    await this.logger.LogInformation($"Error: {ex.Message}");
                    await this.logger.LogInformation($"StackTrace: {ex.StackTrace}");
                    throw ex;
                }
               
            }
            else
            {

                await this.RegistrarFirmaDocumento(dto);
                return null;
                

            }
        }


        protected async Task RegistrarFirmaDocumento(AplicarAccioneDto dto)
        {
           


            //El sistema inserta un registro en FD_DocumentosProcesadosHistorico

            var Documentos = (await this.documentosprocesadosService.GetAllAsync(e => dto.Documentos.Contains(e.Id),
                                new List<Expression<Func<FdDocumentosProcesados, object>>>() {
                e => e.TipoDocumento
            })).Items.ToList();

            foreach (var doc in Documentos.OrderBy(e => e.Fecha))
            {
                try
                {
                    await this.logger.LogInformation($"Evaluando el Documento {doc.Id}");
                    FdDocumentosProcesadosHistorico historico = this.CrearHistorico(doc);

                    FdAcciones accion = await this.RecuperarAccionPorTipoDoc(doc.TipoDocumentoId, doc.EstadoId);

                    if (accion==null)
                    {
                        throw new ValidationException($"No se pudo determinar la accion para el documento y el estado {doc.EstadoId}");
                    }

                    if (accion.MenorFechaPrimero == true)
                    {
                        await this.ValidarAccionMenorFecha(doc);
                    }

                    //El sistema actualiza el registro FD_DocumentosProcesados.
                    doc.ArchivoId = this.CrearArchivo(dto.Firmador.file, dto.Firmador.FileName);
                    doc.EstadoId = accion.EstadoNuevoId;
                    doc.NombreArchivo = dto.Firmador.FileName;

                    if (!dto.Empleador)
                        doc.EmpleadoConforme = dto.Firmador.EmpleadoConforme.GetValueOrDefault();

                    doc.Cerrado = accion.EsFin;
                    doc.LastUpdatedDate = DateTime.Now;
                    doc.LastUpdatedUserId = AuthUserService.GetCurretUserId();

                    var log = new FdFirmadorLogDto() { FechaHora = DateTime.Now, FirmadorId = dto.Firmador.Id };
                    log.DetalleLog = $"Subir documento enviado: Documento enviado: { doc.ArchivoId }";
                    dto.Firmador.FdFirmadorLog.Add(log);


                    //El sistema actualiza el registro de FD_FirmadorDetalle 
                    var fd = dto.Firmador.FdFirmadorDetalle.FirstOrDefault(e => e.DocumentoProcesadoId == doc.Id);
                    fd.ArchivoIdRecibido = doc.ArchivoId;
                    fd.Firmado = true;
                    fd.HasChange = true;
                    this.documentosprocesadosService.GuardarDocumento(doc, historico, dto.Firmador);



                    if (accion.GeneraNotificacion)
                    {
                        try
                        {
                            await this.RealizarNotificacion(accion);
                        }
                        catch (Exception ex)
                        {
                            log = new FdFirmadorLogDto() { FechaHora = DateTime.Now, FirmadorId = dto.Firmador.Id };
                            log.DetalleLog = $"Error al realizar la notificacion gral {ex.ToString()}";
                            dto.Firmador.FdFirmadorLog.Add(log);
                        }
                        
                    }

                    //El sistema verifica si tiene que enviar notificación por correo
                    //El sistema verifica si tienen que generar notificaciones al interactuar con el Firmador en función a la configuración del tipo de documento/acción
                    //El sistema inserta un registro en la tabla FD_FirmadorLog 
                    //Se tienen que generar notificaciones al interactuar con el Firmador
                    //El sistema genera las notificaciones al usuario que ejecutó la acción (sys_UsersAD.id = FD_Firmador.CreatedUserId tal que FD_Firmador.id obtenido) para mostrarlas en el navegador con el mensaje “El documento se firmó correctamente.”
                    if (accion.GeneraNotificacion)
                    {
                        var user = (await this._userService.GetAllAsync(e => e.EmpleadoId == doc.EmpleadoId)).Items.FirstOrDefault();
                        if (user!=null)
                        {
                            string title = $"Documentos pendientes firma empleado {doc.Cuilempleado}";
                            string content = $"Se encuentra pendiente de firma para el empleado {doc.Cuilempleado} el siguiente documento: {doc.NombreArchivo} ";

                            log = new FdFirmadorLogDto() { FechaHora = DateTime.Now, FirmadorId = dto.Firmador.Id };
                            log.DetalleLog = $"Subir Documento Se envia Mail a  {user.Mail}";
                            dto.Firmador.FdFirmadorLog.Add(log);

                            await this.emailSender.SendDefaultAsync(user.Mail, title, content);
                        }
                        else
                        {
                            log = new FdFirmadorLogDto() { FechaHora = DateTime.Now, FirmadorId = dto.Firmador.Id };
                            log.DetalleLog = $"Subir Documento No se puede enviar mail a EmpleadoId {doc.EmpleadoId}";
                            dto.Firmador.FdFirmadorLog.Add(log);
                        }

                    }


                }
                catch (Exception ex)
                {
                    await this.logger.LogInformation($"Error: {ex.Message}");
                    await this.logger.LogInformation($"StackTrace: {ex.StackTrace}");
                    if (dto.Firmador != null)
                    {
                        await this._fdFirmadorAppService.UpdateLogs(dto.Firmador);
                    }
                    throw ex;
                }
                finally
                {
                    string mje = dto.Firmador.Id.ToString(); 
                    await _signalRHttpService.SendMessage(mje, "Firmador_JNLPProgress");
                    if (dto.Firmador != null)
                    {
                        await this._fdFirmadorAppService.UpdateLogs(dto.Firmador);
                    }
                }

            }








        }

        private Guid CrearArchivo(byte[] file, string fileName)
        {
            Adjuntos adjuntos = new Adjuntos();

            adjuntos.Archivo = file;
            adjuntos.Nombre = fileName;

            return _adjuntosService.Add(adjuntos).Id;
        }
    }
}
