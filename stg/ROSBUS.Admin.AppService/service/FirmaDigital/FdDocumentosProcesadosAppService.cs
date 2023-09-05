using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Entities.Filters;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.IO;
using ROSBUS.Admin.Domain.Entities.Partials;
using TECSO.FWK.Domain.Mail;
using ROSBUS.Admin.Domain;
using TECSO.FWK.Domain.Entities;
using System.Linq.Expressions;
using TECSO.FWK.DocumentsHelper.Excel;
using ROSBUS.Admin.Domain.Model;
using Microsoft.AspNetCore.Http;
using TECSO.FWK.Domain;
using ROSBUS.Admin.AppService.service.http;

namespace ROSBUS.Admin.AppService
{

    public class FdDocumentosProcesadosAppService : AppServiceBase<FdDocumentosProcesados, FdDocumentosProcesadosDto, long, IFdDocumentosProcesadosService>, IFdDocumentosProcesadosAppService
    {
        private static Boolean IsRunningTask = false;
        private readonly ISysParametersService _parametersService;
        private readonly ILogger _logger;
        private readonly IFdTiposDocumentosService _fdTiposDocumentosService;
        private readonly IAdjuntosService _adjuntosService;
        private readonly IDefaultEmailer _emailSender;
        private readonly IEmpleadosService _empleadosService;
        private readonly IFdEstadosService _estadosService;
        private readonly IPermissionService _permissionService;
        private readonly IUserService _userService;
        private readonly IAuthService _authService;
        private readonly IEmpresaService _empresaService;
        private readonly IFdFirmadorAppService _fdFirmadorAppService;
        private readonly IFdCertificadosAppService _fdCertificadosAppService;
        private readonly IFdEstadosService _fdEstadosService;
        private IFdAccionesAppService _fdAccionesAppService;
        private readonly ISignalRHttpService _signalRHttpService;
        private const string CarpetaDestinoCorrectos = "FD_Procesados";
        private const string CarpetaDestinoError = "FD_Procesados_Erroneamente";

        public FdDocumentosProcesadosAppService(IFdDocumentosProcesadosService serviceBase,
            ISysParametersService parametersService,
            ILogger logger,
            IFdTiposDocumentosService fdTiposDocumentosService,
            IEmpleadosService empleadosService,
            IAdjuntosService adjuntosService,
            IDefaultEmailer emailSender,
            IPermissionService permissionService,
            IFdEstadosService estadosService,
            IUserService userService,
            IAuthService authService,
            IEmpresaService empresaService,
            IFdFirmadorAppService fdFirmadorAppService,
            IFdCertificadosAppService fdCertificadosAppService,
            IFdAccionesAppService fdAccionesAppService,
            ISignalRHttpService signalRHttpService,
            IFdEstadosService fdEstadosService
            )
            : base(serviceBase)
        {
            _parametersService = parametersService;
            _logger = logger;
            _fdTiposDocumentosService = fdTiposDocumentosService;
            _empleadosService = empleadosService;
            _adjuntosService = adjuntosService;
            _estadosService = estadosService;
            _emailSender = emailSender;
            _permissionService = permissionService;
            _userService = userService;
            _authService = authService;
            _empresaService = empresaService;
            _fdFirmadorAppService = fdFirmadorAppService;
            _fdCertificadosAppService = fdCertificadosAppService;
            _fdEstadosService = fdEstadosService;
            //creado de esta manera porque crea referencia circular
            //_fdAccionesAppService = ServiceProviderResolver.ServiceProvider.GetService<IFdAccionesAppService>();
            _fdAccionesAppService = fdAccionesAppService;
            _signalRHttpService = signalRHttpService;

        }

        private void LogError(Exception ex, FdFirmadorDto dto)
        {
            if (dto != null)
            {
                dto.FdFirmadorLog.Add(new FdFirmadorLogDto()
                {
                    DetalleLog = ex.Message,
                    FechaHora = DateTime.Now
                });

            }
        }
        private async Task<FdFirmadorLogDto> getLogFromRequest(HttpRequest request)
        {
            FdFirmadorLogDto log = new FdFirmadorLogDto();
            var body = await this.GetBodyContentAsStringAsync(request);
            var qs = request.QueryString.ToString();

            if (!string.IsNullOrEmpty(body))
            {
                log.DetalleLog = string.Format("Body {0}", body);
            }

            if (!string.IsNullOrEmpty(qs))
            {
                log.DetalleLog += string.Format("QueryString {0}", qs);
            }


            
            if (request.HasFormContentType)
            {
                foreach (var item in request.Form)
                {
                    log.DetalleLog += string.Format("Form {0}: {1}", item.Key, item.Value);
                }

                var file = request.Form.Files.FirstOrDefault();
                if (file != null)
                {
                    log.DetalleLog += string.Format(Char.ConvertFromUtf32(13) + "FIle ContentDisposition: {0}", file.ContentDisposition);
                    log.DetalleLog += string.Format(Char.ConvertFromUtf32(13) + "FIle ContentType: {0}", file.ContentType);
                    log.DetalleLog += string.Format(Char.ConvertFromUtf32(13) + "FIle FileName: {0}", file.FileName);
                    log.DetalleLog += string.Format(Char.ConvertFromUtf32(13) + "FIle Name: {0}", file.Name);
                }
            }

            



            log.FechaHora = DateTime.Now;
            return log;
        }
        private async Task<string> GetBodyContentAsStringAsync(Microsoft.AspNetCore.Http.HttpRequest request)
        {
            using (StreamReader stream = new StreamReader(request.Body))
            {
                string body = await stream.ReadToEndAsync();
                return body;
            }
        }

        public async Task<List<ArchivosTotalesPorEstado>> HistorialArchivosPorEstado(FdDocumentosProcesadosFilter documentosProcesadosFilter)
        {
            return await this._serviceBase.HistorialArchivosPorEstado(documentosProcesadosFilter);
        }

        public async Task<string> GetEmailDefault()
        {
            int id = this._authService.GetCurretUserId();

            return (await this._userService.GetByIdAsync(id)).Mail;
        }

        public async Task<FileDto> ExportarExcel(FdDocumentosProcesadosFilter filter)
        {
            var file = new FileDto();

            var docsEntities = (await this._serviceBase.ExportarExcel(filter));

            string permiso = string.Empty;

            if (filter.EsEmpleador.HasValue && filter.EsEmpleador.Value)
            {
                permiso = "FirmaDigital.BD-Empleador.Exportar";
            }
            else
            {
                permiso = "FirmaDigital.BD-Empleado.Exportar";
            }

            String[] usuarioPermisosToken = await this._permissionService.GetPermissionForCurrentUser();

            if (permiso != null & !usuarioPermisosToken.Contains(permiso))
            {
                throw new ValidationException("No tiene permisos para realizar esta acción");
            };


            List<ExportarExcelFirmaDigital> items = new List<ExportarExcelFirmaDigital>();

            foreach (var item in docsEntities)
            {
                var exc = new ExportarExcelFirmaDigital();
                exc.TipoDocumento = item.TipoDocumento.Descripcion;
                exc.Fecha = item.Fecha.ToString("dd/MM/yyyy");
                exc.FechaProcesado = item.FechaProcesado.ToString("dd/MM/yyyy H:mm");
                exc.Empleado = item.NombreEmpleado.Trim();
                exc.Legajo = item.LegajoEmpleado;
                exc.Cuil = item.Cuilempleado;
                exc.UnidadNegocio = item.Sucursal.DscSucursal;
                exc.Empresa = item.Empresa.DesEmpr;
                exc.Estado = item.Estado.Descripcion;
                // exc.EstadoConformidad = item.MotivoRechazo;
                exc.Rechazado = item.Rechazado ? "Si" : "No";
                exc.MotivoRechazo = item.MotivoRechazo;
                exc.Cerrado = item.Cerrado ? "Si" : "No";
                exc.NombreArchivo = (await _adjuntosService.GetByIdAsync(item.ArchivoId)).Nombre;
                items.Add(exc);
            }

            ExcelParameters<ExportarExcelFirmaDigital> parametros = new ExcelParameters<ExportarExcelFirmaDigital>();

            parametros.AddField("TipoDocumento", "Tipo de documento");
            parametros.AddField("Fecha", "Fecha");
            parametros.AddField("FechaProcesado", "Fecha procesado");
            parametros.AddField("Empleado", "Empleado");
            parametros.AddField("Legajo", "Legajo");
            parametros.AddField("Cuil", "CUIL");
            parametros.AddField("UnidadNegocio", "U. de Negocio");
            parametros.AddField("Empresa", "Empresa");
            parametros.AddField("Estado", "Estado");
            // parametros.AddField("Estado Conformidad", "EstadoConformidad");
            parametros.AddField("Rechazado", "Rechazado");
            parametros.AddField("MotivoRechazo", "Motivo rechazo");
            parametros.AddField("Cerrado", "Cerrado");
            parametros.AddField("NombreArchivo", "Nombre del archivo");

            parametros.HeaderText = null;
            string sheetName = DateTime.Now.ToString("yyyyMMddHHmmss");
            parametros.SheetName = string.Format("Documentos Procesados - {0}", sheetName);

            parametros.Values = items;


            file.ByteArray = ExcelHelper.GenerateByteArray(parametros);
            file.ForceDownload = true;
            file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            file.FileName = string.Format("Documentos Procesados - {0}", sheetName);
            file.FileDescription = string.Format("Documentos Procesados - {0}", sheetName);

            return file;
        }

        public async override Task<PagedResult<FdDocumentosProcesadosDto>> GetDtoPagedListAsync<TFilter>(TFilter filter)
        {
            FdDocumentosProcesadosFilter fil = filter as FdDocumentosProcesadosFilter;
            var User = this._userService.GetById(this._authService.GetCurretUserId());

            if (fil.EsEmpleador == false)
            {
                fil.EmpleadoId = new TECSO.FWK.Domain.Interfaces.Entities.ItemDto(User.EmpleadoId.GetValueOrDefault(), "");
            }


            var list = await _serviceBase.GetPagedListAsync(filter);
            var listDto = this.MapList<FdDocumentosProcesados, FdDocumentosProcesadosDto>(list.Items);

            //var Empleado = this._empleadosService.GetAll(e => e.Id == User.EmpleadoId);
            var estadosAMostrar = new List<int>();

            if (fil.EsEmpleador.HasValue && fil.EsEmpleador.Value)
            {


                foreach (var itemEmpleador in listDto)
                {
                    estadosAMostrar = itemEmpleador.TipoDocumento.FdAcciones.Where(e => e.MostrarBdempleador == true).Select(e => e.EstadoActualId).ToList();
                    estadosAMostrar.AddRange(itemEmpleador.TipoDocumento.FdAcciones.Where(e => e.MostrarBdempleador == true && e.EsFin).Select(e => e.EstadoNuevoId).ToList());


                    if (itemEmpleador.Rechazado == false)
                    {
                        itemEmpleador.AccionesDisponibles = itemEmpleador.TipoDocumento.FdAcciones.Where(e => e.EstadoActualId == itemEmpleador.EstadoId && e.AccionBdempleador == true && e.MostrarBdempleador == true).ToList();
                        itemEmpleador.AccionesDisponibles.Add(new FdAccionesDto() { AccionPermitidaId = FdAccionesPermitidas.AbrirArchivo, PermiteMarcarLote = true, AccionPermitida= new FdAccionesPermitidasDto() { Id = FdAccionesPermitidas.AbrirArchivo , TokenPermission= "FirmaDigital.BD-Empleador.Abrir" }});
                        itemEmpleador.AccionesDisponibles.Add(new FdAccionesDto() { AccionPermitidaId = FdAccionesPermitidas.DescargarArchivo, PermiteMarcarLote = true, AccionPermitida = new FdAccionesPermitidasDto() { Id = FdAccionesPermitidas.DescargarArchivo, TokenPermission = "FirmaDigital.BD-Empleador.Descargar" } });
                        //itemEmpleador.AccionesDisponibles.Add(new FdAccionesDto() { AccionPermitidaId = FdAccionesPermitidas.EnviarCorreo, AccionPermitida = new FdAccionesPermitidasDto() { Id = FdAccionesPermitidas.EnviarCorreo, TokenPermission = "FirmaDigital.BD-Empleado.EnviarCorreo" } });
                        //itemEmpleador.AccionesDisponibles.Add(new FdAccionesDto() { AccionPermitidaId = FdAccionesPermitidas.ExportarExcel, AccionPermitida = new FdAccionesPermitidasDto() { Id = FdAccionesPermitidas.ExportarExcel, TokenPermission = "FirmaDigital.BD-Empleador.Abrir" } });
                        if (itemEmpleador.PermiteRechazo)
                        {
                            itemEmpleador.AccionesDisponibles.Add(new FdAccionesDto() { AccionPermitidaId = FdAccionesPermitidas.RechazarDocumento, PermiteMarcarLote = true, AccionPermitida = new FdAccionesPermitidasDto() { Id = FdAccionesPermitidas.RechazarDocumento, TokenPermission = "FirmaDigital.BD-Empleador.Rechazar" } });
                        }
                        //itemEmpleador.AccionesDisponibles.Add(new FdAccionesDto() { AccionPermitidaId = FdAccionesPermitidas.RevisarArchivo }); ver que es
                        itemEmpleador.AccionesDisponibles.Add(new FdAccionesDto() { AccionPermitidaId = FdAccionesPermitidas.VerDetalleDocumento, PermiteMarcarLote = true, AccionPermitida = new FdAccionesPermitidasDto() { Id = FdAccionesPermitidas.VerDetalleDocumento, TokenPermission = "FirmaDigital.BD-Empleador.Ver" } });
                    }

                    itemEmpleador.EstadosConHistorico = new List<FdEstadosDto>();
                    if (estadosAMostrar.Any(e => e == itemEmpleador.Estado.Id))
                    {
                        itemEmpleador.EstadosConHistorico.Add(itemEmpleador.Estado);
                    }

                    if (itemEmpleador.FdDocumentosProcesadosHistorico != null)
                    {
                        if (itemEmpleador.Rechazado)
                        {
                            var laststate = itemEmpleador.FdDocumentosProcesadosHistorico.OrderBy(e => e.Id).LastOrDefault();
                            if (laststate != null)
                            {
                                itemEmpleador.FdDocumentosProcesadosHistorico.Remove(laststate);
                            }
                        }
                        foreach (var historicoEmpleador in itemEmpleador.FdDocumentosProcesadosHistorico.OrderBy(e => e.Id))
                        {

                            if (estadosAMostrar.Any(e => e == historicoEmpleador.EstadoId))
                            {
                                itemEmpleador.EstadosConHistorico.Add(historicoEmpleador.Estado);
                            }


                        }
                    }
                }
            }

            if (fil.EsEmpleador.HasValue && fil.EsEmpleador.Value == false)
            {
                foreach (var itemEmpleado in listDto)
                {
                    estadosAMostrar = itemEmpleado.TipoDocumento.FdAcciones.Where(e => e.MostrarBdempleado == true).Select(e => e.EstadoActualId).ToList();
                    estadosAMostrar.AddRange(itemEmpleado.TipoDocumento.FdAcciones.Where(e => e.MostrarBdempleado == true && e.EsFin).Select(e => e.EstadoNuevoId).ToList());

                    if (itemEmpleado.Rechazado == false)
                    {
                        itemEmpleado.AccionesDisponibles = itemEmpleado.TipoDocumento.FdAcciones.Where(e => e.EstadoActualId == itemEmpleado.EstadoId && e.AccionBdempleador == false && e.MostrarBdempleado == true).ToList();
                        itemEmpleado.AccionesDisponibles.Add(new FdAccionesDto() { AccionPermitidaId = FdAccionesPermitidas.AbrirArchivo, PermiteMarcarLote = true, AccionPermitida = new FdAccionesPermitidasDto() { Id = FdAccionesPermitidas.AbrirArchivo, TokenPermission = "FirmaDigital.BD-Empleado.Abrir" }});
                        itemEmpleado.AccionesDisponibles.Add(new FdAccionesDto() { AccionPermitidaId = FdAccionesPermitidas.DescargarArchivo, PermiteMarcarLote = true, AccionPermitida = new FdAccionesPermitidasDto() { Id = FdAccionesPermitidas.DescargarArchivo, TokenPermission = "FirmaDigital.BD-Empleado.Descargar"}});
                        itemEmpleado.AccionesDisponibles.Add(new FdAccionesDto() { AccionPermitidaId = FdAccionesPermitidas.EnviarCorreo, PermiteMarcarLote = true, AccionPermitida = new FdAccionesPermitidasDto() { Id = FdAccionesPermitidas.EnviarCorreo, TokenPermission = "FirmaDigital.BD-Empleado.EnviarCorreo" }});

                    }
           
                    itemEmpleado.EstadosConHistorico = new List<FdEstadosDto>();
                    if (estadosAMostrar.Any(e => e == itemEmpleado.EstadoId))
                    {
                        itemEmpleado.EstadosConHistorico.Add(itemEmpleado.Estado);
                    }




                    if (itemEmpleado.FdDocumentosProcesadosHistorico != null)
                    {
                        if (itemEmpleado.Rechazado)
                        {
                            var laststate = itemEmpleado.FdDocumentosProcesadosHistorico.OrderBy(e => e.Id).LastOrDefault();
                            if (laststate != null)
                            {
                                itemEmpleado.FdDocumentosProcesadosHistorico.Remove(laststate);
                            }
                        }
                        foreach (var historicoEmpleado in itemEmpleado.FdDocumentosProcesadosHistorico.OrderBy(e => e.Id))
                        {

                            if (estadosAMostrar.Any(e => e == historicoEmpleado.EstadoId))
                            {
                                itemEmpleado.EstadosConHistorico.Add(historicoEmpleado.Estado);
                            }


                        }
                    }

                    //if (Empleado.Items.Where(e => e.Id == itemEmpleado.EmpleadoId).Count() == 0)
                    //{
                    //    listDto = listDto.Where(e => e.Id != itemEmpleado.Id);
                    //    list.TotalCount--;
                    //}
                    //{
                    //    foreach (var historicoEmpleado in itemEmpleado.FdDocumentosProcesadosHistorico.OrderBy(e => e.Id))
                    //    {
                    //        var estado = itemEmpleado.EstadosConHistorico.Where(e => e.Id == historicoEmpleado.EstadoId).FirstOrDefault();
                    //        if (estado == null)
                    //        {
                    //            foreach (var AccionesDisponiblesEmpleado in itemEmpleado.AccionesDisponibles)
                    //            {
                    //                if (historicoEmpleado.EstadoId == AccionesDisponiblesEmpleado.EstadoActualId || historicoEmpleado.EstadoId == AccionesDisponiblesEmpleado.EstadoNuevoId)
                    //                {
                    //                    itemEmpleado.EstadosConHistorico.Add(itemEmpleado.Estado);
                    //                }
                    //            }

                    //        }
                    //    }
                    //}
                }

            }

            PagedResult<FdDocumentosProcesadosDto> pList = new PagedResult<FdDocumentosProcesadosDto>(list.TotalCount, listDto.ToList());
            return pList;
        }



        public async Task<Boolean> ImportarDocumentos()
        {
            if (IsRunningTask)
            {
                throw new ValidationException("Solo puede correr una sola task");
            }

            try
            {

                IsRunningTask = true;
                var estadoInicial= (await _fdEstadosService.GetAllAsync(e => e.ImportarDocumentoOk)).Items.FirstOrDefault();

                if (estadoInicial==null)
                {
                    await this._logger.LogInformation("No existe el Estdo con ImportarDocumentoOk=1");
                    throw new ValidationException("No existe el Estdo con ImportarDocumentoOk=1");
                }

                string path = (await this._parametersService.GetAllAsync(e => e.Token == "ImportadorDeDocumentosPath")).Items.FirstOrDefault().Value;
                await this._logger.LogInformation("Directorio encontrado correctamente");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                var files = Directory.GetFiles(path, "*.*", SearchOption.TopDirectoryOnly);

                await this._logger.LogInformation(String.Format("{0} archivos encontrados en el directorio", files.Length));

                List<ImportadorDocumentosModel> coleccion = new List<ImportadorDocumentosModel>();

                coleccion.AddRange(await this.UploadDocuments(files));

                List<FdDocumentosProcesados> procesadosCorrectos = new List<FdDocumentosProcesados>();
                List<FdDocumentosError> procesadosError = new List<FdDocumentosError>();

                foreach (var item in coleccion.Where(e => e.IsValid))
                {
                    FdDocumentosProcesados dp = new FdDocumentosProcesados();
                    var file = System.IO.Path.Combine(path, item.File);

                    dp.TipoDocumentoId = item.TipoDocumento.Id;
                    dp.EmpleadoId = item.Empleado.Id;
                    dp.SucursalEmpleadoId = item.Empleado.UnidadNegocio.cod_sucursal.Value;
                    dp.EmpresaEmpleadoId = item.LegajoEmpleado.CodEmpresa.GetValueOrDefault();
                    dp.LegajoEmpleado = item.LegajoEmpleado.LegajoSap;
                    dp.Cuilempleado = item.Empleado.Cuil;
                    dp.Fecha = item.Fecha.Value;
                    dp.FechaProcesado = DateTime.Now;
                    dp.ArchivoId = this.CrearArchivo(file);
                    dp.NombreArchivo = new System.IO.FileInfo(file).Name;
                    dp.EstadoId = estadoInicial.Id;
                    dp.File = item.File;
                    procesadosCorrectos.Add(dp);
                }

                foreach (var item in coleccion.Where(e => !e.IsValid))
                {
                    FdDocumentosError de = new FdDocumentosError();
                    de.Cuilempleado = item?.Empleado?.Cuil;
                    de.AddErrors(item.GetError());
                    de.EmpleadoId = item?.Empleado?.Id;
                    de.EmpresaEmpleadoId = item?.LegajoEmpleado?.CodEmpresa;
                    de.Fecha = item.Fecha;
                    de.FechaProcesado = DateTime.Now;
                    de.LegajoEmpleado = item?.LegajoEmpleado?.LegajoSap;
                    de.NombreArchivo = item.File;
                    de.SucursalEmpleadoId = item?.Empleado?.UnidadNegocio?.cod_sucursal;
                    de.TipoDocumentoId = item?.TipoDocumento?.Id;
                    de.File = item.File;

                    procesadosError.Add(de);
                }


                await this._logger.LogInformation($"Se leyeron {coleccion.Count} documentos de los cuales se encuentran {procesadosCorrectos.Count} correctos. se envian a guardar todos.");

                this._serviceBase.SaveDocumentosImportados(procesadosCorrectos, procesadosError);

                await this._logger.LogInformation($"Se guardaron los archivos. se procede a mover los archivos.");

                foreach (var item in procesadosCorrectos)
                {
                    var origen = System.IO.Path.Combine(path, item.File);
                    var destino = System.IO.Path.Combine(path, CarpetaDestinoCorrectos, DateTime.Now.ToString("ddMMyyyyhhmm_") + item.File);
                    var fileInfo = new FileInfo(destino);
                    if (!Directory.Exists(fileInfo.DirectoryName))
                    {
                        Directory.CreateDirectory(fileInfo.DirectoryName);
                    }
                    File.Move(origen, destino);
                }

                await this._logger.LogInformation($"Se movieron los correctos");

                foreach (var item in procesadosError)
                {
                    var origen = System.IO.Path.Combine(path, item.File);
                    var destino = System.IO.Path.Combine(path, CarpetaDestinoError, DateTime.Now.ToString("ddMMyyyyhhmm_") + item.File);

                    var fileInfo = new FileInfo(destino);
                    if (!Directory.Exists(fileInfo.DirectoryName))
                    {
                        Directory.CreateDirectory(fileInfo.DirectoryName);
                    }
                    File.Move(origen, destino);
                }

                await this._logger.LogInformation($"Se movieron los incorrectos. Se procede a enviar la notificacion");

                await this.Notificar(procesadosError);

                await this._logger.LogInformation($"Final. ");

                return true;
            }
            catch (Exception ex)
            {
                await this._logger.LogError(ex.Message);
                await this._logger.LogError(ex.StackTrace);
                if (ex.InnerException != null)
                {
                    await this._logger.LogError(ex.InnerException?.Message);
                    await this._logger.LogError(ex.InnerException?.StackTrace);
                }

                return false;
            }
            finally
            {
                IsRunningTask = false;

            }

        }

        private async Task Notificar(List<FdDocumentosError> procesadosError)
        {
            if (!procesadosError.Any())
            {
                return;
            }

            StringBuilder arrayforerrors = new StringBuilder("<b>Lista de errores</b>");
            arrayforerrors.AppendLine();
            //arrayforerrors.AppendFormat("Excel {0}", file);
            //arrayforerrors.AppendLine();
            arrayforerrors.AppendLine(@"<table style='border-spacing:0px;'>
                                                               <thead>
                                                                <tr>
                                                                    <th style='border:solid 1px'>Archivo</th>
                                                                    <th style='border:solid 1px'>Error</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>");


            foreach (var item in procesadosError)
            {

                foreach (var error in item.GetErrors())
                {
                    arrayforerrors.AppendFormat(@"<tr>
                                                    <td style='border:solid 1px'>{0}</td>
                                                    <td style='border:solid 1px'>{1}</td>
                                                </tr>", item.File, error);
                }

            }
            arrayforerrors.AppendLine(@"</tbody>
                                        </table>");

            List<Destinatario> destinatarios = await this._serviceBase.GetNotificacionesMail("ImportadorDocumentos");
            if (destinatarios != null && destinatarios.Count >= 1)
            {
                await this._logger.LogInformation($"Vamos a enviar los mails a {string.Join(";", destinatarios.Select(d => d.Email))}. ");

                foreach (var mail in destinatarios)
                {
                    await this._emailSender.SendDefaultAsync(mail.Email, "Error al importar los Documentos vía tarea programada", arrayforerrors.ToString());
                }
            }
        }

        private Guid CrearArchivo(string file)
        {
            Adjuntos adjuntos = new Adjuntos();
            adjuntos.Archivo = File.ReadAllBytes(file);
            adjuntos.Nombre = new System.IO.FileInfo(file).Name;

            return _adjuntosService.Add(adjuntos).Id;
        }

        private async Task<List<ImportadorDocumentosModel>> UploadDocuments(string[] files)
        {
            List<FileInfo> fileInfos = new List<FileInfo>();

            foreach (var file in files)
            {
                fileInfos.Add(new FileInfo(file));
            }

            List<ImportadorDocumentosModel> importadorDocumentos = new List<ImportadorDocumentosModel>();
            var cuils = fileInfos.Select(e => e.Name.Substring(3, e.Name.Length > 15 ? 11 : e.Name.Length - 1)).ToList();

            await this._logger.LogInformation(String.Format("Recuperando empleados  con cuils {0} ", string.Join(",", cuils)));

            List<System.Linq.Expressions.Expression<Func<Operaciones.Domain.Entities.Empleados, object>>> include;
            include = new List<System.Linq.Expressions.Expression<Func<Operaciones.Domain.Entities.Empleados, object>>>()
            {
                e => e.UnidadNegocio,
                e=> e.LegajosEmpleado
            };

            var empleados = await _empleadosService.GetAllAsync(e => cuils.Contains(e.Cuil), include);
            var tdocs = await this._fdTiposDocumentosService.GetAllAsync(e => true);

            List<int?> empleadosIds = new List<int?>();

            empleados.Items.Select(e => e.Id).ToList().ForEach(e => empleadosIds.Add(e));

            var users = (await this._userService.GetAllAsync(e => empleadosIds.Contains(e.EmpleadoId))).Items;


            foreach (var file in fileInfos)
            {


                var model = new ImportadorDocumentosModel();
                model.File = file.Name;
                model.IsValid = true;
                //var empCuil = file.Name.Substring(3, 11);
                //model.Empleado = empleados.Items.FirstOrDefault(e => e.Cuil == empCuil);
                //model.LegajoEmpleado = model.Empleado.LegajosEmpleado.FirstOrDefault();
                try
                {



                    var prefijo = file.Name.Substring(0, file.Name.Length >= 3 ? 3 : file.Name.Length - 1);

                    model.TipoDocumento = tdocs.Items.FirstOrDefault(e => e.Prefijo?.ToLower() == prefijo?.ToLower());

                    if (file.Extension?.ToLower() != ".pdf")
                    {
                        model.AddError($"No tiene la extension PDF {file.Name}");
                    }
                    else if (file.Name.ToLower().Replace(".pdf", "").Length != 22)
                    {
                        model.AddError($"No tiene 22 caracteres en el nombre del archivo: {file.Name}");
                    }




                    if (model.IsValid)
                    {
                        var empCuil = file.Name.Substring(3, 11);
                        model.Empleado = empleados.Items.FirstOrDefault(e => e.Cuil == empCuil);

                        if (model.Empleado != null)
                        {
                            model.LegajoEmpleado = model.Empleado.LegajosEmpleado.FirstOrDefault();
                        }
                        else
                        {
                            model.AddError($"No existe empleado con Cuil {empCuil}");
                        }

                        if (model.TipoDocumento == null)
                        {
                            model.AddError($"No existe Tipo de Documento: {prefijo}");
                        }
                        else if (model.TipoDocumento.Anulado)
                        {
                            model.AddError($"El Tipo de Documento esta anulado: {prefijo}");
                            model.TipoDocumento = null;
                        }

                        var cuil = file.Name.Substring(3, 11);

                        var fechaString = file.Name.Substring(14, 8);
                        Boolean hasError_Fecha = false;
                        try
                        {
                            var anio = int.Parse(fechaString.Substring(0, 4));
                            var mes = int.Parse(fechaString.Substring(4, 2));
                            var dia = int.Parse(fechaString.Substring(6, 2));
                            model.Fecha = new DateTime(anio, mes, dia);
                        }
                        catch
                        {
                            model.AddError($"La fecha tiene formato incorrecto: {fechaString}");
                            hasError_Fecha = true;
                        }

                        model.Empleado = empleados.Items.FirstOrDefault(e => e.Cuil == cuil);
                        model.LegajoEmpleado = model.Empleado?.LegajosEmpleado.FirstOrDefault();

                        if (model.Empleado == null)
                        {
                            model.AddError($"No existe Empleado: {cuil}");
                        }
                        else
                        {

                            if (!users.Any(e=> e.EmpleadoId == model.Empleado.Id))
                            {
                                model.AddError($"El empleado no está asociado a ningún usuario");
                            }

                            if (model.Empleado.UnidadNegocio == null || !model.Empleado.UnidadNegocio.cod_sucursal.HasValue)
                            {
                                model.AddError($"El empleado no tiene Sucursal: {cuil}");
                            }

                            if (model.IsValid)
                            {
                                model.LegajoEmpleado = model.Empleado.LegajosEmpleado.OrderBy(e => e.FecIngreso).LastOrDefault();
                                if (model.LegajoEmpleado == null || !model.LegajoEmpleado.CodEmpresa.HasValue)
                                {
                                    model.AddError($"El empleado no tiene Empresa: {cuil}");
                                }
                                else
                                {
                                    var empresa = (await this._empresaService.GetAllAsync(e => e.Id == model.LegajoEmpleado.CodEmpresa))?.Items?.FirstOrDefault();
                                    if (empresa == null)
                                    {
                                        model.LegajoEmpleado.CodEmpresa = null;
                                        model.AddError($"El empleado tiene una empresa inválida: {cuil}");
                                    }
                                }

                            }
                        }

                        if (!hasError_Fecha && model.Fecha.GetValueOrDefault().ToString("yyyyMMdd") != fechaString)
                        {
                            model.Fecha = null;
                            model.AddError($"Fecha Invalida: {fechaString}");
                        }
                    }


                    if (model.IsValid &&
                        this._serviceBase.ExistExpression(e => e.TipoDocumentoId == model.TipoDocumento.Id
                                                                                && e.EmpleadoId == model.Empleado.Id
                                                                                && e.Fecha == model.Fecha
                                                                                && e.Rechazado == false))
                    {
                        model.AddError($"El archivo ya fue procesado y no esta rechazado: {model.File}");
                    }



                }
                catch (Exception ex)
                {
                    await this._logger.LogError($"A ocurrido un error inesperado: {ex.ToString()}");
                    model.AddError($"A ocurrido un error inesperado: {ex.Message}");
                }
                finally
                {
                    importadorDocumentos.Add(model);
                }
            }

            return importadorDocumentos;


        }

        public void GuardarDocumento(FdDocumentosProcesados doc, FdDocumentosProcesadosHistorico historico, FdFirmadorDto fdFirmadorDto)
        {

            FdFirmador fdFirmadorEntity = null;

            List<FdFirmadorDetalle> listWithChanges = new List<FdFirmadorDetalle>();

            if (fdFirmadorDto!=null)
            {
                fdFirmadorEntity = this._fdFirmadorAppService.GetById(fdFirmadorDto.Id);
                foreach (var itemDto in fdFirmadorDto.FdFirmadorDetalle)
                {
                    if (itemDto.HasChange)
                    {
                        var item = fdFirmadorEntity.FdFirmadorDetalle.FirstOrDefault(e => e.Id == itemDto.Id);
                        this.MapObject<FdFirmadorDetalleDto, FdFirmadorDetalle>(itemDto, item);
                        listWithChanges.Add(item);
                    }
                }
                fdFirmadorEntity.FdFirmadorDetalle = listWithChanges;
            }


            this._serviceBase.GuardarDocumento(doc, historico, fdFirmadorEntity);
        }

        public async Task<FdFirmadorDto> GetMetadatos(string token, string idUsuario, HttpRequest request)
        {
            FdFirmadorDto firmador = null;
            try
            {
                firmador = (await this._fdFirmadorAppService.GetDtoAllAsync(e => e.SessionId == token, new List<Expression<Func<FdFirmador, object>>>() { e => e.FdFirmadorDetalle })).Items.FirstOrDefault();

                if (firmador == null)
                {
                    throw new ValidationException("No existe registro de firmador ");
                }


                FdFirmadorLogDto log = await this.getLogFromRequest(request);

                log.DetalleLog = "Metadatos recibido: " + log.DetalleLog;
                firmador.FdFirmadorLog.Add(log);

                if (firmador != null && firmador.UsuarioId != idUsuario)
                {
                    var str = $"Metadatos enviado El CUIL recibido en idUsuario ({idUsuario}) es diferente al guardado en FD_Firmador.Usuario_id para el token recibido";
                    firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = str, FechaHora = DateTime.Now });
                    throw new ValidationException("Problema al obtener datos de usuario");
                }

                firmador.Metadatos = new MetadatosDto();

                firmador.Metadatos.pathGetDescarga = firmador.PathGetDescarga;
                firmador.Metadatos.pathPostSubida = firmador.PathPostSubida;
                firmador.Metadatos.recibos = firmador.FdFirmadorDetalle.Select(e => e.DocumentoProcesadoId).ToArray();
                firmador.Metadatos.usuario = new MetadatosUsuarioDto()
                {
                    id = firmador.UsuarioId,
                    rol = firmador.UsuarioRol,
                    username = firmador.UsuarioUserName,
                    nombre = firmador.UsuarioNombre,
                    apellido = firmador.UsuarioApellido
                };

                try
                {
                    firmador.Metadatos.coordenadas_empleado = firmador.CoordenadasEmpleado?.Split(",").Select(e => int.Parse(e)).ToArray();
                    firmador.Metadatos.coordenadas_empleador = firmador.CoordenadasEmpleador?.Split(",").Select(e => int.Parse(e)).ToArray();
                }
                catch (Exception ex)
                {
                    throw new ValidationException("Error en la conversion de las coordenadas");
                }
                

                var strMet = $"Metadatos enviado {Newtonsoft.Json.JsonConvert.SerializeObject(firmador.Metadatos)}";
                firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = strMet, FechaHora = DateTime.Now });


                return firmador;

            }
            catch (Exception ex)
            {
                LogError(ex, firmador);
                throw ex;
            }
            finally
            {
                if (firmador != null)
                {
                    await this._fdFirmadorAppService.UpdateLogs(firmador);
                }
            }


        }

        public async Task<FdFirmadorDto> GetCertificado(string token, string idUsuario, HttpRequest request)
        {
            FdFirmadorDto firmador = null;

            try
            {
                firmador = (await this._fdFirmadorAppService.GetDtoAllAsync(e => e.SessionId == token)).Items.FirstOrDefault();

                if (firmador == null)
                {
                    throw new ValidationException("No existe registro de firmador ");
                }

                FdFirmadorLogDto log = await this.getLogFromRequest(request);
                log.DetalleLog = "Descargar certificado recibido: " + log.DetalleLog;
                firmador.FdFirmadorLog.Add(log);

                if (firmador.UsuarioId != idUsuario)
                {
                    var str = $"Descargar certificado enviado El CUIL recibido en idUsuario ({idUsuario}) es diferente al guardado en FD_Firmador.Usuario_id para el token recibido";
                    firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = str, FechaHora = DateTime.Now });
                    throw new ValidationException("Problema al obtener datos de usuario");
                }

                var user = (await this._userService.GetAllAsync(e => e.Id == firmador.CreatedUserId)).Items.FirstOrDefault();

                if (user == null)
                {
                    var str = $"Descargar certificado enviado No se pudo recuperar user con el CreatedUserId ({firmador.CreatedUserId}) ";
                    firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = str, FechaHora = DateTime.Now });
                    throw new ValidationException("Problema al obtener datos de usuario");
                }

                var empleado = (await _empleadosService.GetAllAsync(e => e.Id == user.EmpleadoId && e.Cuil == firmador.UsuarioId)).Items.FirstOrDefault();

                if (empleado == null)
                {
                    var str = $"Descargar certificado enviado No se pudo recuperar empleado con Id = user.EmpleadoId ({user.EmpleadoId}) y  cuil = firmador.UsuarioId ({firmador.UsuarioId})";
                    firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = str, FechaHora = DateTime.Now });
                    throw new ValidationException("Problema al obtener datos de usuario");
                }

                var cert = (await this._fdCertificadosAppService.GetAllAsync(e => e.UsuarioId == user.Id && e.Activo == true)).Items.FirstOrDefault();
                if (cert == null)
                {
                    var str = $"Descargar certificado enviado El CUIL recibido en idUsuario  ({ idUsuario }) no tiene ningún certificado activo.";
                    firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = str, FechaHora = DateTime.Now });
                    throw new FileNotFoundException("No existe certificado activo asociado al usuario");
                }


                var archivo = (await this._adjuntosService.GetAllAsync(e => e.Id == cert.ArchivoId)).Items.FirstOrDefault();
                if (archivo == null)
                {
                    var str = $"Descargar certificado enviado No se encontro el Archivo  ({ cert.ArchivoId }).";
                    firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = str, FechaHora = DateTime.Now });
                    throw new FileNotFoundException("No existe certificado activo asociado al usuario");
                }

                log = new FdFirmadorLogDto() { FechaHora = DateTime.Now, FirmadorId = firmador.Id };
                log.DetalleLog = $"Descargar certificado enviado: Certificado enviado = { archivo.Id }";
                firmador.FdFirmadorLog.Add(log);


                firmador.file = archivo.Archivo;

                firmador.FileName = archivo.Nombre;

                return firmador;
            }
            catch (Exception ex)
            {
                LogError(ex, firmador);
                throw ex;
            }
            finally
            {
                if (firmador != null)
                {
                    await this._fdFirmadorAppService.UpdateLogs(firmador);
                }
            }



        }


        public async Task<FdFirmadorDto> GetDocumento(string token, int idRecibo, HttpRequest request)
        {
            FdFirmadorDto firmador = null;

            try
            {
                firmador = (await this._fdFirmadorAppService.GetFirmadorByToken(token, idRecibo));


                if (firmador == null)
                {
                    throw new ValidationException("No existe registro de firmador ");
                }


                FdFirmadorLogDto log = await this.getLogFromRequest(request);
                log.DetalleLog = "Descargar documento recibido: " + log.DetalleLog;
                firmador.FdFirmadorLog.Add(log);


                var detFirmador = firmador.FdFirmadorDetalle.Where(e => e.DocumentoProcesadoId == idRecibo).FirstOrDefault();

                if (detFirmador == null)
                {

                    var str = $"Descargar documento enviado. El idRecibo recibido ({idRecibo}) no existe en FD_DetalleFirmador.";
                    firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = str, FechaHora = DateTime.Now });
                    throw new FileNotFoundException("El documento recibido no existe.");
                }


                var doc = (await this._serviceBase.GetAllAsync(e => e.Id == idRecibo)).Items.FirstOrDefault();

                if (doc == null)
                {
                    var str = $"Descargar documento enviado. El idRecibo recibido ({idRecibo}) no existe en FD_DetalleFirmador.";
                    firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = str, FechaHora = DateTime.Now });
                    throw new FileNotFoundException("El documento recibido no existe.");
                }


                if (doc.EstadoId != detFirmador.EstadoId || doc.Rechazado)
                {
                    var str = $"Descargar documento enviado. El idRecibo recibido {idRecibo} cambió el estado o fue rechazado.";
                    firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = str, FechaHora = DateTime.Now });
                    throw new InvalidDocumentExeption("El documento recibido cambió el estado o fue rechazado.");
                }


                var archivo = (await this._adjuntosService.GetAllAsync(e => e.Id == detFirmador.ArchivoIdEnviado)).Items.FirstOrDefault();
                if (archivo == null)
                {
                    var str = $"Descargar documento enviado. El idRecibo recibido ({ detFirmador.ArchivoIdEnviado }) no existe en FD_DetalleFirmador.";
                    firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = str, FechaHora = DateTime.Now });
                    throw new FileNotFoundException("El documento recibido no existe.");
                }

                log = new FdFirmadorLogDto() { FechaHora = DateTime.Now, FirmadorId = firmador.Id };
                log.DetalleLog = $"Descargar documento enviado: Documento enviado = { archivo.Id }";
                firmador.FdFirmadorLog.Add(log);

                firmador.file = archivo.Archivo;
                firmador.FileName = doc.NombreArchivo;

                return firmador;
            }
            catch (Exception ex)
            {
                LogError(ex, firmador);
                throw ex;
            }
            finally
            {
                if (firmador != null)
                {
                    await this._fdFirmadorAppService.UpdateLogs(firmador);
                }
            }



        }

        public async Task<FdFirmadorDto> RecibirDocumento(string token, int idRecibo, Boolean? conforme, HttpRequest request)
        {
            FdFirmadorDto firmador = null;

            try
            {



                /*

                 Ejecutar la Accion
                 Buscar FD_Firmador y validar que exista
                 Agregar log del Request

                 Buscar el fdDirmadoDetalle y validar que exista 
                 Validar que el Doc no este rechazado y que este en el mismo estado
                 Validar que el documento ya no este firmado


                 Insertar un registro en historico
                 Actualiza el FD_DocumentoProcesado
                 Actualiza el FD_FirmadorDetalle
                 Notificacion por correo
                 Genera las notificaciones al Usuario que ejecuto la accion "para mostrarlas en el navegador con el mensaje “El documento se firmó correctamente.”"


                 Insertar un log "Subir Documento enviado Documento recibido = valor de FD_DocumentoProcesado.ArchivoId Respuesta generada en el paso anterior"
             

                 Devuelve un 200
             */


                //Buscar FD_Firmador y validar que exista
                firmador = (await this._fdFirmadorAppService.GetFirmadorByToken(token, idRecibo));


                if (firmador == null)
                {
                    throw new ValidationException("No existe registro de firmador ");
                }

                

                //Agregar log del Request
                FdFirmadorLogDto log = await this.getLogFromRequest(request);
                log.DetalleLog = "Subir  documento recibido: " + log.DetalleLog;
                firmador.FdFirmadorLog.Add(log);


                //Buscar el fdDirmadoDetalle y validar que exista 
                var detFirmador = firmador.FdFirmadorDetalle.Where(e => e.DocumentoProcesadoId == idRecibo).FirstOrDefault();

                if (detFirmador == null)
                {
                    var str = $"Subir  documento enviado. El idRecibo recibido ({idRecibo}) no existe en FD_DetalleFirmador.";
                    firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = str, FechaHora = DateTime.Now });
                    throw new FileNotFoundException("El documento recibido no existe.");
                }


                //Validar que el Doc no este rechazado y que este en el mismo estado
                var doc = (await this._serviceBase.GetAllAsync(e => e.Id == idRecibo)).Items.FirstOrDefault();
                if (doc == null)
                {
                    var str = $"Subir  documento enviado. El idRecibo recibido ({idRecibo}) no existe en FD_DetalleFirmador.";
                    firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = str, FechaHora = DateTime.Now });
                    throw new FileNotFoundException("El documento recibido no existe.");
                }
                if (doc.EstadoId != detFirmador.EstadoId || doc.Rechazado)
                {
                    var str = $"Subir documento enviado. El idRecibo recibido {idRecibo} cambió el estado o fue rechazado.";
                    firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = str, FechaHora = DateTime.Now });
                    throw new InvalidDocumentExeption("El documento recibido cambió el estado o fue rechazado.");
                }




                //Validar que el documento ya no este firmado
                if (detFirmador.Firmado)
                {
                    var str = $"Subir  documento enviado. El idRecibo recibido {idRecibo} ya esta firmado.";
                    firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = str, FechaHora = DateTime.Now });
                    throw new AccessViolationException("El documento ya fue firmado.");
                }


                var archivo = request.Form.Files.FirstOrDefault();

                firmador.file = null;
                firmador.FileName = null;

                if (archivo != null && archivo.Length > 0)
                {
                    using (var ms = new MemoryStream())
                    {
                        archivo.CopyTo(ms);
                        var fileBytes = ms.ToArray();
                        firmador.file = fileBytes;
                        firmador.FileName = archivo.FileName;
                    }
                }

                if (archivo == null || firmador.file == null)
                {
                    var str = $"Subir  documento recibido. El archivo del documento idRecibo recibido ({ idRecibo }) no llego en el request.";
                    firmador.FdFirmadorLog.Add(new FdFirmadorLogDto() { DetalleLog = str, FechaHora = DateTime.Now });
                    throw new FileNotFoundException("El documento recibido no existe.");
                }

                firmador.EmpleadoConforme = conforme;

                AplicarAccioneDto aplicarAccioneDto = new AplicarAccioneDto();
                aplicarAccioneDto.AccionId = doc.EstadoId;
                aplicarAccioneDto.Documentos = new List<long>() { doc.Id };
                aplicarAccioneDto.Empleador = firmador.UsuarioRol == "empleado" ? false: true;
                aplicarAccioneDto.EsFirmador = true;
                aplicarAccioneDto.Firmador = firmador;

                

                await this._fdAccionesAppService.AplicarAccion(aplicarAccioneDto);


                return firmador;
            }
            catch (Exception ex)
            {
                LogError(ex, firmador);
                if (firmador != null)
                {
                    await this._fdFirmadorAppService.UpdateLogs(firmador);
                }
                throw ex;
            }
            
        }
    }
}
