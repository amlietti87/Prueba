using OfficeOpenXml;
using ROSBUS.Admin.AppService.Interface.ART;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Entities.Enums;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.Filters.ART;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Interfaces.Services.ART;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Admin.Domain.Report;
using ROSBUS.Admin.Domain.Report.Report.Model;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.DocumentsHelper.Excel;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Extensions;
using IEstadosService = ROSBUS.Admin.Domain.Interfaces.Services.ART.IEstadosService;

namespace ROSBUS.Admin.AppService.service.ART
{
    

    public class DenunciasAppService : AppServiceBase<ArtDenuncias, ArtDenunciasDto, int, IDenunciasService>, IDenunciasAppService
    {

        private static Boolean IsRunningTask = false;


        private readonly IAdjuntosService _adjuntosService;
        private readonly IServiceProvider _serviceProvider;
        private readonly IReclamosService _reclamosService;
        private readonly IDefaultEmailer _emailSender;
        private readonly ISysParametersService _parametersService;
        private readonly ILogger _logger;
        public DenunciasAppService(IDenunciasService serviceBase,
            IAdjuntosService adjuntosService,
            IReclamosService reclamosService,
            IDefaultEmailer emailSender,
            ILogger logger,
            ISysParametersService parametersService,
            IServiceProvider serviceProvider) : base(serviceBase)
        {
            _adjuntosService = adjuntosService;
            _serviceProvider = serviceProvider;
            _reclamosService = reclamosService;
            _emailSender = emailSender;
            _parametersService = parametersService;
            _logger = logger;
        }

        public async override Task<ArtDenunciasDto> AddAsync(ArtDenunciasDto dto)
        {
            var mismonrodenuncia = await this._serviceBase.GetAllAsync(e => e.NroDenuncia.Trim() == dto.NroDenuncia.Trim() && e.PrestadorMedicoId == dto.PrestadorMedicoId);
            if (mismonrodenuncia.TotalCount > 0)
            {
                throw new DomainValidationException("Existe un mismo número de denuncia para este prestador");
            }

            var entity = MapObject<ArtDenunciasDto, ArtDenuncias>(dto);

            foreach (var item in entity.ArtDenunciasNotificaciones.Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            }

            var result = await this.AddAsync(entity);
            return MapObject<ArtDenuncias, ArtDenunciasDto>(entity);
        }

        public async override Task<PagedResult<ArtDenunciasDto>> GetDtoPagedListAsync<TFilter>(TFilter filter)
        {

            var list = await _serviceBase.GetPagedListAsync(filter);
            var listDto = this.MapList<ArtDenuncias, ArtDenunciasDto>(list.Items);
            DenunciasFilter fil = filter as DenunciasFilter;

            if (fil.selectEmpleados != null)
            {
                int i = 0;
                var colors = Enum.GetValues(typeof(Colors)).Cast<Colors>().ToList();
                foreach (var key in listDto.GroupBy(e => e.FechaOcurrencia).Where(g => g.Count() > 1))
                {
                    foreach (var item in key)
                    {
                        item.Color = colors[i].ToString();
                    }

                    i = i + 1;
                }
            }

            PagedResult<ArtDenunciasDto> pList = new PagedResult<ArtDenunciasDto>(list.TotalCount, listDto.ToList());
            return pList;
        }

        public async Task<List<ImportadorExcelDenuncias>> RecuperarPlanilla(DenunciaImportadorFileFilter filter)
        {
            return await this._serviceBase.RecuperarPlanilla(filter);
        }

        public async override Task<List<ItemDto<int>>> GetItemsAsync<TFilter>(TFilter filter)
        {
            var list = await this.GetPagedListAsync(filter);
            var r = list.Items.Select(filter.GetItmDTO()).ToList();
            return r;
        }

        public async Task<ArtDenunciasDto> Anular(ArtDenunciasDto denuncia)
        {
            var reclamos = await this._reclamosService.GetAllAsync(e => e.DenunciaId == denuncia.Id);
            if (reclamos.TotalCount > 0)
            {
                throw new DomainValidationException("No se puede anular la denuncia si está asociada a un reclamo");
            }

            var denunciaentity = await this._serviceBase.GetByIdAsync(denuncia.Id);
            MapObject(denuncia, denunciaentity);

            var transaction = await this._serviceBase.Anular(denunciaentity);

            var result = new ArtDenunciasDto();

            MapObject(transaction, result);

            return result;
        }

        public async override Task<ArtDenunciasDto> UpdateAsync(ArtDenunciasDto dto)
        {
            var entity = await this.GetByIdAsync(dto.Id);
            MapObject(dto, entity);

            foreach (var item in entity.ArtDenunciasNotificaciones.Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            }

            await this.UpdateAsync(entity);


            return MapObject<ArtDenuncias, ArtDenunciasDto>(entity);
        }

        public async Task<List<HistorialDenuncias>> HistorialDenunciaPorPrestador(int EmpleadoId)
        {
            return await this._serviceBase.HistorialDenunciaPorPrestador(EmpleadoId);
        }

        public async Task<List<HistorialReclamosEmpleado>> HistorialReclamosEmpleado(int EmpleadoId)
        {
            return await this._serviceBase.HistorialReclamosEmpleado(EmpleadoId);
        }

        public async Task<List<HistorialDenunciasPorEstado>> HistorialDenunciasPorEstado(int EmpleadoId)
        {
            return await this._serviceBase.HistorialDenunciasPorEstado(EmpleadoId);
        }

        public async Task<List<AdjuntosDto>> GetAdjuntosDenuncias(int DenunciaId)
        {
            List<ItemDto<Guid>> adjuntos = new List<ItemDto<Guid>>();

            var sinAdj = await this._serviceBase.GetAdjuntosDenuncias(DenunciaId);



            AdjuntosFilter filter = new AdjuntosFilter();
            filter.Ids = sinAdj.Select(e => e.AdjuntoId).ToList(); ;

            adjuntos = await _adjuntosService.GetAdjuntosItemDto(filter);

            return adjuntos.Select(e => new AdjuntosDto() { Id = e.Id, Nombre = e.Description }).ToList();
        }

        public async Task AgregarAdjuntos(int DenunciaId, List<AdjuntosDto> result)
        {
            var allEntity = await this._serviceBase.GetAllAsync(e => e.Id == DenunciaId);

            var entity = allEntity.Items.FirstOrDefault();
            if (entity != null)
            {
                foreach (var item in result)
                {
                    entity.ArtDenunciaAdjuntos.Add(new ArtDenunciaAdjuntos() { AdjuntoId = item.Id, DenunciaId = DenunciaId });
                }

            }

            await this._serviceBase.UpdateAsync(entity);

        }

        public Task DeleteFileById(Guid id)
        {
            return this._serviceBase.DeleteFileById(id);
        }

        public async Task<FileDto> GetReporteExcel(ExcelDenunciasFilter filter)
        {
            var file = new FileDto();

            //var puntos = this._serviceBase.GetAll(filter.GetFilterExpression());
            List<ReporteDenunciasExcel> items = await this._serviceBase.GetReporteExcel(filter);
            ExcelParameters<ReporteDenunciasExcel> parametros = new ExcelParameters<ReporteDenunciasExcel>();

            parametros.HeaderText = null;
            string sheetName = DateTime.Now.ToString("yyyyMMddHHmmss");
            parametros.SheetName = string.Format("Denuncias - {0}", sheetName);

            LimpiarEncabezados(items);

            parametros.Values = items;

            parametros.AddField("NroDenuncia", "NroDenuncia");
            parametros.AddField("Sucursal", "U. de Negocio");
            parametros.AddField("Estado", "Estado");
            parametros.AddField("Empresa", "Empresa");
            parametros.AddField("FechaOcurrencia", "FechaOcurrencia");
            parametros.AddField("FechaRecepcionDenuncia", "FechaRecepcionDenuncia");
            parametros.AddField("PrestadorMedico", "PrestadorMedico");
            parametros.AddField("NombreEmpleado", "EmpleadoNombre");
            parametros.AddField("EmpleadoDNI", "EmpleadoDNI");
            parametros.AddField("EmpleadoCUIL", "EmpleadoCUIL");
            parametros.AddField("EmpleadoFechaIngreso", "EmpleadoFechaIngreso");
            parametros.AddField("EmpleadoLegajo", "EmpleadoLegajo");
            parametros.AddField("EmpleadoEmpresa", "EmpleadoEmpresa");
            parametros.AddField("EmpleadoAntiguedad", "EmpleadoAntiguedad");
            parametros.AddField("EmpleadoArea", "EmpleadoArea");
            parametros.AddField("EmpleadoFechaEgreso", "EmpleadoFechaEgreso");
            parametros.AddField("AltaMedica", "AltaMedica");
            parametros.AddField("FechaAltaMedica", "FechaAltaMedica");
            parametros.AddField("AltaLaboral", "AltaLaboral");
            parametros.AddField("FechaAltaLaboral", "FechaAltaLaboral");
            parametros.AddField("FechaProbableAlta", "FechaProbableAlta");
            parametros.AddField("CantidadDiasBaja", "CantidadDiasBaja");
            parametros.AddField("Reingreso", "TieneReingreso");
            parametros.AddField("DenunciaOrigen", "DenunciaOrigen");
            parametros.AddField("Contingencia", "Contingencia");
            parametros.AddField("Tratamiento", "Tratamiento");
            parametros.AddField("Patologia", "Patologia");
            parametros.AddField("NroSiniestro", "NroSiniestro");
            parametros.AddField("FechaAudienciaMedica", "FechaAudienciaMedica");
            parametros.AddField("MotivoAudiencia", "MotivoAudiencia");
            parametros.AddField("PorcentajeIncapacidad", "PorcentajeIncapacidad");
            parametros.AddField("BajaServicio", "BajaServicio");
            parametros.AddField("FechaBajaServicio", "FechaBajaServicio");
            parametros.AddField("FechaUltimoControl", "FechaUltimoControl");
            parametros.AddField("FechaProximaConsulta", "FechaProximaConsulta");
            parametros.AddField("Juicio", "Juicio");
            parametros.AddField("Diagnostico", "Diagnostico");
            parametros.AddField("Observaciones", "Observaciones");
            parametros.AddField("MotivoAnulado", "MotivoAnulado");
            parametros.AddField("Anulado", "Anulado");
            parametros.AddField("DeletedDate", "DeletedDate");
            parametros.AddField("FechaNotificacion", "FechaNotificacion");
            parametros.AddField("MotivoNotificacion", "MotivoNotificacion");
            parametros.AddField("ObservacionesNotificacion", "ObservacionesNotificacion");


            file.ByteArray = ExcelHelper.GenerateByteArray(parametros);
            file.ForceDownload = true;
            file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            file.FileName = string.Format("Denuncias - {0}.xlsx", sheetName);
            file.FileDescription = "Reporte de Denuncias";

            return file;
        }

        private void LimpiarEncabezados(List<ReporteDenunciasExcel> items)
        {
            string NroDenuncia = "";
            foreach (var fila in items)
            {
                if (fila.NroDenuncia == NroDenuncia)
                {
                    NroDenuncia = fila.NroDenuncia;
                    fila.NroDenuncia = null;
                    fila.Sucursal = null;
                    fila.Estado = null;
                    fila.Empresa = null;
                    fila.Empleado = null;
                    fila.EmpleadoFechaIngreso = null;
                    fila.EmpleadoLegajo = null;
                    fila.EmpleadoEmpresa = null;
                    fila.EmpleadoAntiguedad = null;
                    fila.EmpleadoArea = null;
                    fila.FechaOcurrencia = null;
                    fila.FechaRecepcionDenuncia = null;
                    fila.Contingencia = null;
                    fila.Diagnostico = null;
                    fila.Patologia = null;
                    fila.PrestadorMedico = null;
                    fila.BajaServicio = null;
                    fila.FechaBajaServicio = null;
                    fila.Tratamiento = null;
                    fila.FechaUltimoControl = null;
                    fila.FechaProximaConsulta = null;
                    fila.FechaAudienciaMedica = null;
                    fila.MotivoAudiencia = null;
                    fila.PorcentajeIncapacidad = null;
                    fila.AltaMedica = null;
                    fila.FechaAltaMedica = null;
                    fila.AltaLaboral = null;
                    fila.FechaAltaLaboral = null;
                    fila.Reingreso = string.Empty;
                    fila.NroSiniestro = null;
                    fila.CantidadDiasBaja = null;
                    fila.Juicio = null;
                    fila.Observaciones = null;
                    fila.NombreEmpleado = null;
                    fila.MotivoAnulado = null;
                    fila.Anulado = null;
                    fila.FechaAnulado = null;

                    fila.DenunciaOrigen = null;
                    fila.EmpleadoDNI = null;
                    fila.EmpleadoCUIL = null;
                    fila.EmpleadoFechaEgreso = null;
                    fila.FechaProbableAlta = null;

                }
                if (fila.NroDenuncia != null)
                {
                    NroDenuncia = fila.NroDenuncia;
                }
            }
        }

        private Boolean ValidarCabecera(ExcelWorksheet workSheet)
        {
            const int PrestadorMedico = 1;
            const int NroDenuncia = 2;
            const int Estado = 3;
            const int Empresa = 4;
            const int EmpleadoDNI = 5;
            const int EmpleadoCUIL = 6;
            const int FechaOcurrencia = 7;
            const int FechaRecepcionDenuncia = 8;
            const int Contingencia = 9;
            const int Diagnostico = 10;
            const int Patologia = 11;
            const int FechaBajaServicio = 12;
            const int Tratamiento = 13;
            const int FechaUltimoControl = 14;
            const int FechaProximaConsulta = 15;
            const int FechaAudienciaMedica = 16;
            const int MotivoAudiencia = 17;
            const int PorcentajeIncapacidad = 18;
            const int FechaAltaMedica = 19;
            const int FechaAltaLaboral = 20;
            const int FechaProbableAlta = 21;
            const int NroDenunciaOrigen = 22;
            const int NroSiniestro = 23;
            const int Observaciones = 24;
            const int MotivoNotificacion = 25;
            const int FechaNotificacion = 26;
            const int ObservacionesNotificacion = 27;

            if (workSheet.Cells[1, PrestadorMedico].Text.ToLower().TrimOrNull() != "prestadormedico")
            {
                return false;
            }
            if (workSheet.Cells[1, NroDenuncia].Text.ToLower().TrimOrNull() != "nrodenuncia")
            {
                return false;
            }
            if (workSheet.Cells[1, Estado].Text.ToLower().TrimOrNull() != "estado")
            {
                return false;
            }
            if (workSheet.Cells[1, Empresa].Text.ToLower().TrimOrNull() != "empresa")
            {
                return false;
            }
            if (workSheet.Cells[1, EmpleadoDNI].Text.ToLower().TrimOrNull() != "empleadodni")
            {
                return false;
            }
            if (workSheet.Cells[1, EmpleadoCUIL].Text.ToLower().TrimOrNull() != "empleadocuil")
            {
                return false;
            }
            if (workSheet.Cells[1, FechaOcurrencia].Text.ToLower().TrimOrNull() != "fechaocurrencia")
            {
                return false;
            }
            if (workSheet.Cells[1, FechaRecepcionDenuncia].Text.ToLower().TrimOrNull() != "fecharecepciondenuncia")
            {
                return false;
            }
            if (workSheet.Cells[1, Contingencia].Text.ToLower().TrimOrNull() != "contingencia")
            {
                return false;
            }
            if (workSheet.Cells[1, Diagnostico].Text.ToLower().TrimOrNull() != "diagnostico")
            {
                return false;
            }
            if (workSheet.Cells[1, Patologia].Text.ToLower().TrimOrNull() != "patologia")
            {
                return false;
            }
            if (workSheet.Cells[1, FechaBajaServicio].Text.ToLower().TrimOrNull() != "fechabajaservicio")
            {
                return false;
            }
            if (workSheet.Cells[1, Tratamiento].Text.ToLower().TrimOrNull() != "tratamiento")
            {
                return false;
            }
            if (workSheet.Cells[1, FechaUltimoControl].Text.ToLower().TrimOrNull() != "fechaultimocontrol")
            {
                return false;
            }
            if (workSheet.Cells[1, FechaProximaConsulta].Text.ToLower().TrimOrNull() != "fechaproximaconsulta")
            {
                return false;
            }
            if (workSheet.Cells[1, FechaAudienciaMedica].Text.ToLower().TrimOrNull() != "fechaaudienciamedica")
            {
                return false;
            }
            if (workSheet.Cells[1, MotivoAudiencia].Text.ToLower().TrimOrNull() != "motivoaudiencia")
            {
                return false;
            }
            if (workSheet.Cells[1, PorcentajeIncapacidad].Text.ToLower().TrimOrNull() != "porcentajeincapacidad")
            {
                return false;
            }
            if (workSheet.Cells[1, FechaAltaMedica].Text.ToLower().TrimOrNull() != "fechaaltamedica")
            {
                return false;
            }
            if (workSheet.Cells[1, FechaAltaLaboral].Text.ToLower().TrimOrNull() != "fechaaltalaboral")
            {
                return false;
            }
            if (workSheet.Cells[1, FechaProbableAlta].Text.ToLower().TrimOrNull() != "fechaprobablealta")
            {
                return false;
            }
            if (workSheet.Cells[1, NroDenunciaOrigen].Text.ToLower().TrimOrNull() != "nrodenunciaorigen")
            {
                return false;
            }
            if (workSheet.Cells[1, NroSiniestro].Text.ToLower().TrimOrNull() != "nrosiniestro")
            {
                return false;
            }
            if (workSheet.Cells[1, Observaciones].Text.ToLower().TrimOrNull() != "observaciones")
            {
                return false;
            }
            if (workSheet.Cells[1, MotivoNotificacion].Text.ToLower().TrimOrNull() != "motivonotificacion")
            {
                return false;
            }
            if (workSheet.Cells[1, FechaNotificacion].Text.ToLower().TrimOrNull() != "fechanotificacion")
            {
                return false;
            }
            if (workSheet.Cells[1, ObservacionesNotificacion].Text.ToLower().TrimOrNull() != "observacionesnotificacion")
            {
                return false;
            }
            return true;
        }

        public async Task<Boolean> ImportWithTask()
        {
            if (IsRunningTask)
            {
                throw new ValidationException("Solo puede correr una sola task");
            }

            try
            {
                
                IsRunningTask = true;
                string path = (await this._parametersService.GetAllAsync(e => e.Token == "ImportadorDenuncias")).Items.FirstOrDefault().Value;
                await this._logger.LogInformation("Directorio encontrado correctamente");

                var files = Directory.GetFiles(path, "Denuncias*.xlsx", SearchOption.TopDirectoryOnly);

                await this._logger.LogInformation(String.Format("{0} archivos encontrados en el directorio", files.Length));


                foreach (var file in files)
                {

                    List<ImportadorExcelDenuncias> coleccion = new List<ImportadorExcelDenuncias>();

                    //using (var fileStream = new FileStream(file, FileMode.Open, FileAccess.Read))
                    //{
                    //    await this._logger.LogInformation(String.Format("Se procede a leer el excel {0}", file));

                    //    coleccion = await this.UploadExcel(fileStream.GetAllBytes());
                    //}

                    await this._logger.LogInformation(String.Format("Se procede a leer el excel {0}", file));

                    coleccion = await this.UploadExcel(File.ReadAllBytes(file));

                    await this._logger.LogInformation(String.Format("Excel {0} leído", file));



                    if (coleccion.Where(e => e.IsValid == false).Count() > 0)
                    {
                        StringBuilder arrayforerrors = new StringBuilder("<b>Lista de errores</b>");
                        arrayforerrors.AppendLine();
                        //arrayforerrors.AppendFormat("Excel {0}", file);
                        //arrayforerrors.AppendLine();
                        arrayforerrors.AppendLine(@"<table style='border-spacing:0px;'>
                                                               <thead>
                                                                <tr>
                                                                    <th style='border:solid 1px'>Archivo</th>
                                                                    <th style='border:solid 1px'>Nro Fila</th>
                                                                    <th style='border:solid 1px'>Error</th>
                                                                </tr>
                                                            </thead>
                                                            <tbody>");

                        int rowNumber = 2;
                        foreach (var item in coleccion)
                        {

                            if (!item.IsValid)
                            {

                                foreach (var error in item.Errors)
                                {
                                    arrayforerrors.AppendFormat(@"<tr>
                                                                            <td style='border:solid 1px'>{0}</td>
                                                                            <td style='border:solid 1px;     text-align: center;'>{1}</td>
                                                                            <td style='border:solid 1px'>{2}</td>
                                                                        </tr>", file, rowNumber, error);
                                }

                            }

                            rowNumber++;
                        }

                        arrayforerrors.AppendLine(@"</tbody>
                                                                </table>");

                        await this._logger.LogInformation("Se procede a recuperar lista de emails");
                        var destinatarios = await this._serviceBase.GetNotificacionesMail("ImportadorDenuncias");
                        if (destinatarios != null && destinatarios.Count >= 1)
                        {
                            foreach (var mail in destinatarios)
                            {
                                await this._logger.LogInformation(String.Format("Enviar mail a {0} informando errores", mail.Email));
                                await this._emailSender.SendDefaultAsync(mail.Email, "Error al importar denuncias vía tarea programada", arrayforerrors.ToString());
                            }
                        }



                        var onlyfilename = System.IO.Path.GetFileName(file);
                        onlyfilename = String.Format("{0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HHmm"), onlyfilename);
                        var pathprocesadoserroneamente = System.IO.Path.Combine(path, "Procesados Erroneamente");


                        bool exists = System.IO.Directory.Exists(pathprocesadoserroneamente);

                        if (!exists)
                        {
                            System.IO.Directory.CreateDirectory(pathprocesadoserroneamente);
                        }

                        var archivodestino = System.IO.Path.Combine(pathprocesadoserroneamente, onlyfilename);
                        await this._logger.LogInformation(String.Format("Se procede a mover el archivo a {0}", archivodestino));
                        File.Move(file, archivodestino);

                    }
                    else
                    {
                        await this._logger.LogInformation("No se encontraron errores en el excel, se pasa a impactar en base de datos");

                        await this._serviceBase.ImportarDenunciasFromTask(coleccion, file);

                    }

                }


                return true;
            }
            catch (Exception ex)
            {
                await this._logger.LogError(ex.Message);
                return false;
            }
            finally
            {
                IsRunningTask = false;
            }

            
        }


        public async Task<List<ImportadorExcelDenuncias>> UploadExcel(byte[] excelFile)
        {
            Stream stream = new MemoryStream(excelFile);

            ExcelPackage pck = new ExcelPackage(stream);
            var workSheet = pck.Workbook.Worksheets.FirstOrDefault();

            var start = workSheet.Dimension.Start;
            var end = workSheet.Dimension.End;

            var startRow = start.Row + 1;

            if (!this.ValidarCabecera(workSheet))
            {
                throw new DomainValidationException("La cabecera no coincide con el formato establecido");
            }

            ICollection<ImportadorExcelDenuncias> excelFileDTO = new List<ImportadorExcelDenuncias>();



            for (int rowNumber = 2; rowNumber < end.Row + 1; rowNumber++)
            {


                var row = new ImportadorExcelDenuncias();
                row.IsValid = true;
                row.Errors = new List<string>();
                row.PrestadorMedico = workSheet.Cells[rowNumber, 1].Text;
                row.NroDenuncia = workSheet.Cells[rowNumber, 2].Text;
                row.Estado = workSheet.Cells[rowNumber, 3].Text;
                row.Empresa = workSheet.Cells[rowNumber, 4].Text;
                row.EmpleadoDNI = workSheet.Cells[rowNumber, 5].Text;
                row.EmpleadoCUIL = workSheet.Cells[rowNumber, 6].Text;

                this.FormatCell(workSheet.Cells[rowNumber, 7]);

                if (workSheet.Cells[rowNumber, 7].Value != null && workSheet.Cells[rowNumber, 7].Value.GetType() == typeof(System.DateTime))
                {
                    row.FechaOcurrencia = ((System.DateTime)workSheet.Cells[rowNumber, 7].Value).Date;
                }
                else
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(workSheet.Cells[rowNumber, 7].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        row.FechaOcurrencia = DateTime.ParseExact(workSheet.Cells[rowNumber, 7].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(workSheet.Cells[rowNumber, 7].Text))
                        {
                            row.Errors.Add("La columna Fecha Ocurrencia no tiene el formato de fecha correcto");
                            row.IsValid = false;
                        }
                        row.FechaOcurrencia = null;
                    }
                }
                this.FormatCell(workSheet.Cells[rowNumber, 8]);

                if (workSheet.Cells[rowNumber, 8].Value != null && workSheet.Cells[rowNumber, 8].Value.GetType() == typeof(System.DateTime))
                {
                    row.FechaRecepcionDenuncia = ((System.DateTime)workSheet.Cells[rowNumber, 8].Value).Date;
                }
                else
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(workSheet.Cells[rowNumber, 8].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        row.FechaRecepcionDenuncia = DateTime.ParseExact(workSheet.Cells[rowNumber, 8].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(workSheet.Cells[rowNumber, 8].Text))
                        {
                            row.Errors.Add("La columna FechaRecepcionDenuncia no tiene el formato de fecha correcto");
                            row.IsValid = false;
                        }
                        row.FechaRecepcionDenuncia = null;
                    }
                }

                row.Contingencia = workSheet.Cells[rowNumber, 9].Text;
                row.Diagnostico = workSheet.Cells[rowNumber, 10].Text;
                row.Patologia = workSheet.Cells[rowNumber, 11].Text;

                this.FormatCell(workSheet.Cells[rowNumber, 12]);

                if (workSheet.Cells[rowNumber, 12].Value != null && workSheet.Cells[rowNumber, 12].Value.GetType() == typeof(System.DateTime))
                {
                    row.FechaBajaServicio = ((System.DateTime)workSheet.Cells[rowNumber, 12].Value).Date;
                }
                else
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(workSheet.Cells[rowNumber, 12].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        row.FechaBajaServicio = DateTime.ParseExact(workSheet.Cells[rowNumber, 12].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(workSheet.Cells[rowNumber, 12].Text))
                        {
                            row.Errors.Add("La columna FechaBajaServicio no tiene el formato de fecha correcto");
                            row.IsValid = false;
                        }
                        row.FechaBajaServicio = null;
                    }
                }

                row.Tratamiento = workSheet.Cells[rowNumber, 13].Text;


                this.FormatCell(workSheet.Cells[rowNumber, 14]);
                if (workSheet.Cells[rowNumber, 14].Value != null && workSheet.Cells[rowNumber, 14].Value.GetType() == typeof(System.DateTime))
                {
                    row.FechaUltimoControl = ((System.DateTime)workSheet.Cells[rowNumber, 14].Value).Date;
                }
                else
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(workSheet.Cells[rowNumber, 14].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        row.FechaUltimoControl = DateTime.ParseExact(workSheet.Cells[rowNumber, 14].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(workSheet.Cells[rowNumber, 14].Text))
                        {
                            row.Errors.Add("La columna FechaUltimoControl no tiene el formato de fecha correcto");
                            row.IsValid = false;
                        }
                        row.FechaUltimoControl = null;
                    }
                }

                this.FormatCell(workSheet.Cells[rowNumber, 15]);
                if (workSheet.Cells[rowNumber, 15].Value != null && workSheet.Cells[rowNumber, 15].Value.GetType() == typeof(System.DateTime))
                {
                    row.FechaProximaConsulta = ((System.DateTime)workSheet.Cells[rowNumber, 15].Value).Date;
                }
                else
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(workSheet.Cells[rowNumber, 15].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        row.FechaProximaConsulta = DateTime.ParseExact(workSheet.Cells[rowNumber, 15].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(workSheet.Cells[rowNumber, 15].Text))
                        {
                            row.Errors.Add("La columna FechaProximaConsulta no tiene el formato de fecha correcto");
                            row.IsValid = false;
                        }
                        row.FechaProximaConsulta = null;
                    }
                }

                this.FormatCell(workSheet.Cells[rowNumber, 16]);
                if (workSheet.Cells[rowNumber, 16].Value != null && workSheet.Cells[rowNumber, 16].Value.GetType() == typeof(System.DateTime))
                {
                    row.FechaAudienciaMedica = ((System.DateTime)workSheet.Cells[rowNumber, 16].Value).Date;
                }
                else
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(workSheet.Cells[rowNumber, 16].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        row.FechaAudienciaMedica = DateTime.ParseExact(workSheet.Cells[rowNumber, 16].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(workSheet.Cells[rowNumber, 16].Text))
                        {
                            row.Errors.Add("La columna FechaAudienciaMedica no tiene el formato de fecha correcto");
                            row.IsValid = false;
                        }
                        row.FechaAudienciaMedica = null;
                    }
                }

                row.MotivoAudiencia = workSheet.Cells[rowNumber, 17].Text;
                row.PorcentajeIncapacidad = workSheet.Cells[rowNumber, 18].Text;

                this.FormatCell(workSheet.Cells[rowNumber, 19]);
                if (workSheet.Cells[rowNumber, 19].Value != null && workSheet.Cells[rowNumber, 19].Value.GetType() == typeof(System.DateTime))
                {
                    row.FechaAltaMedica = ((System.DateTime)workSheet.Cells[rowNumber, 19].Value).Date;
                }
                else
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(workSheet.Cells[rowNumber, 19].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        row.FechaAltaMedica = DateTime.ParseExact(workSheet.Cells[rowNumber, 19].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(workSheet.Cells[rowNumber, 19].Text))
                        {
                            row.Errors.Add("La columna FechaAltaMedica no tiene el formato de fecha correcto");
                            row.IsValid = false;
                        }
                        row.FechaAltaMedica = null;
                    }
                }

                this.FormatCell(workSheet.Cells[rowNumber, 20]);
                if (workSheet.Cells[rowNumber, 20].Value != null && workSheet.Cells[rowNumber, 20].Value.GetType() == typeof(System.DateTime))
                {
                    row.FechaAltaLaboral = ((System.DateTime)workSheet.Cells[rowNumber, 20].Value).Date;
                }
                else
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(workSheet.Cells[rowNumber, 20].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        row.FechaAltaLaboral = DateTime.ParseExact(workSheet.Cells[rowNumber, 20].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(workSheet.Cells[rowNumber, 20].Text))
                        {
                            row.Errors.Add("La columna FechaAltaLaboral no tiene el formato de fecha correcto");
                            row.IsValid = false;
                        }
                        row.FechaAltaLaboral = null;
                    }
                }

                this.FormatCell(workSheet.Cells[rowNumber, 21]);
                if (workSheet.Cells[rowNumber, 21].Value != null && workSheet.Cells[rowNumber, 21].Value.GetType() == typeof(System.DateTime))
                {
                    row.FechaProbableAlta = ((System.DateTime)workSheet.Cells[rowNumber, 21].Value).Date;
                }
                else
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(workSheet.Cells[rowNumber, 21].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        row.FechaProbableAlta = DateTime.ParseExact(workSheet.Cells[rowNumber, 21].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(workSheet.Cells[rowNumber, 21].Text))
                        {
                            row.Errors.Add("La columna FechaProbableAlta no tiene el formato de fecha correcto");
                            row.IsValid = false;
                        }
                        row.FechaProbableAlta = null;
                    }
                }

                row.NroDenunciaOrigen = workSheet.Cells[rowNumber, 22].Text;
                row.NroSiniestro = workSheet.Cells[rowNumber, 23].Text;
                row.Observaciones = workSheet.Cells[rowNumber, 24].Text;
                row.MotivoNotificacion = workSheet.Cells[rowNumber, 25].Text;

                this.FormatCell(workSheet.Cells[rowNumber, 26]);
                if (workSheet.Cells[rowNumber, 26].Value != null && workSheet.Cells[rowNumber, 26].Value.GetType() == typeof(System.DateTime))
                {
                    row.FechaNotificacion = ((System.DateTime)workSheet.Cells[rowNumber, 26].Value).Date;
                }
                else
                {
                    DateTime dt;
                    if (DateTime.TryParseExact(workSheet.Cells[rowNumber, 26].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                    {
                        row.FechaNotificacion = DateTime.ParseExact(workSheet.Cells[rowNumber, 26].Text, "dd/MM/yyyy", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        if (!String.IsNullOrWhiteSpace(workSheet.Cells[rowNumber, 26].Text))
                        {
                            row.Errors.Add("La columna FechaNotificacion no tiene el formato de fecha correcto");
                            row.IsValid = false;
                        }
                        row.FechaNotificacion = null;
                    }
                }
                row.ObservacionesNotificacion = workSheet.Cells[rowNumber, 27].Text;

                excelFileDTO.Add(row);
            }

            // Needed services
            var estadosService = (IEstadosService)_serviceProvider.GetService(typeof(IEstadosService));
            var empresasService = (IEmpresaService)_serviceProvider.GetService(typeof(IEmpresaService));
            var empleadosService = (IEmpleadosService)_serviceProvider.GetService(typeof(IEmpleadosService));
            var contingenciasService = (IContingenciasService)_serviceProvider.GetService(typeof(IContingenciasService));
            var patologiasService = (IPatologiasService)_serviceProvider.GetService(typeof(IPatologiasService));
            var prestadoresMedicosService = (IPrestadoresMedicosService)_serviceProvider.GetService(typeof(IPrestadoresMedicosService));
            var tratamientosService = (ITratamientosService)_serviceProvider.GetService(typeof(ITratamientosService));
            var motivosAudienciasService = (IMotivosAudienciasService)_serviceProvider.GetService(typeof(IMotivosAudienciasService));
            var motivosNotificacionesService = (IMotivosNotificacionesService)_serviceProvider.GetService(typeof(IMotivosNotificacionesService));
            var siniestrosService = (ISiniestrosService)_serviceProvider.GetService(typeof(ISiniestrosService));
            /* 
             * 1) Get Estados
             * 2) Get Empresas
             * 3) Get Empleados
             * 4) Get Contingencias
             * 5) Get Patologias
             * 6) Get Prestadores Medicos
             * 7) Get Tratamientos
             * 8) Get MotivoAudiencias
             * 9) Get MotivosNotificacion
             * */
            PagedResult<ArtDenuncias> DenunciasPagedResult = await this._serviceBase.GetAllAsync(e => true);
            IReadOnlyList<ArtDenuncias> Denuncias = DenunciasPagedResult.Items;

            PagedResult<ArtEstados> EstadosPagedResult = await estadosService.GetAllAsync(e => true);
            IReadOnlyList<ArtEstados> Estados = EstadosPagedResult.Items;


            PagedResult<Empresa> EmpresasPagedResult = await empresasService.GetAllAsync(e => true);
            IReadOnlyList<Empresa> Empresas = EmpresasPagedResult.Items;

            PagedResult<Empleados> EmpleadosPagedResult = await empleadosService.GetAllAsync(e => true, new List<Expression<Func<Empleados, object>>> {
                e=> e.LegajosEmpleado,
                e=> e.UnidadNegocio
            });
            IReadOnlyList<Empleados> Empleados = EmpleadosPagedResult.Items;

            PagedResult<ArtContingencias> ContingenciasPagedResult = await contingenciasService.GetAllAsync(e => true);
            IReadOnlyList<ArtContingencias> Contingencias = ContingenciasPagedResult.Items;

            PagedResult<ArtPatologias> PatologiasPagedResult = await patologiasService.GetAllAsync(e => true);
            IReadOnlyList<ArtPatologias> Patologias = PatologiasPagedResult.Items;

            PagedResult<ArtPrestadoresMedicos> PrestadoresMedicosPagedResult = await prestadoresMedicosService.GetAllAsync(e => true);
            IReadOnlyList<ArtPrestadoresMedicos> PrestadoresMedicos = PrestadoresMedicosPagedResult.Items;

            PagedResult<ArtTratamientos> TratamientosPagedResult = await tratamientosService.GetAllAsync(e => true);
            IReadOnlyList<ArtTratamientos> Tratamientos = TratamientosPagedResult.Items;

            PagedResult<ArtMotivosAudiencias> MotivosAudienciasPagedResult = await motivosAudienciasService.GetAllAsync(e => true);
            IReadOnlyList<ArtMotivosAudiencias> MotivosAudiencias = MotivosAudienciasPagedResult.Items;

            PagedResult<ArtMotivosNotificaciones> MotivosNotificacionesPagedResult = await motivosNotificacionesService.GetAllAsync(e => true);
            IReadOnlyList<ArtMotivosNotificaciones> MotivosNotificaciones = MotivosNotificacionesPagedResult.Items;

            PagedResult<SinSiniestros> SiniestrosPagedResult = await siniestrosService.GetAllAsync(e => true);
            IReadOnlyList<SinSiniestros> Siniestros = SiniestrosPagedResult.Items;

            //10) Filter each populating Errors Field If not found
            foreach (var row in excelFileDTO)
            {

                // PrestadoresMedicos
                if (PrestadoresMedicos.Where(e => e.Descripcion.ToLower().Trim() == row.PrestadorMedico.ToLower().Trim() && e.Anulado == false).ToList().Count < 1)
                {
                    if (String.IsNullOrWhiteSpace(row.PrestadorMedico))
                    {
                        row.Errors.Add("Prestador médico es requerido.");
                        row.IsValid = false;
                    }
                    else if (PrestadoresMedicos.Where(e => e.Descripcion.ToLower().Trim() == row.PrestadorMedico.ToLower().Trim()).ToList().Count < 1)
                    {
                        row.Errors.Add("No existe prestador médico");
                        row.IsValid = false;
                    }
                    else
                    {
                        row.Errors.Add("Prestador médico anulado");
                        row.IsValid = false;
                    }
                }
                else
                {
                    row.PrestadorMedicoId = PrestadoresMedicos.Where(e => e.Descripcion.ToLower().Trim() == row.PrestadorMedico.ToLower().Trim() && e.Anulado == false).FirstOrDefault().Id;

                    if (String.IsNullOrWhiteSpace(row.NroDenuncia))
                    {
                        row.Errors.Add("Nro. denuncia es requerido");
                        row.IsValid = false;
                    }
                    else
                    {
                        if (Denuncias.Where(e => e.NroDenuncia == row.NroDenuncia && e.PrestadorMedicoId == row.PrestadorMedicoId && e.Anulado == true).ToList().Count >= 1)
                        {
                            row.Errors.Add("La denuncia existe y está anulada");
                            row.IsValid = false;
                        }
                        if (excelFileDTO.Where(e => e.NroDenuncia == row.NroDenuncia && e.PrestadorMedicoId == row.PrestadorMedicoId).ToList().Count >= 2)
                        {
                            row.Errors.Add("Existe una denuncia coincidente con el Nro Denuncia y Prestador Médico en el mismo excel");
                            row.IsValid = false;
                        }
                    }
                }

                if (!String.IsNullOrWhiteSpace(row.PorcentajeIncapacidad))
                {
                    decimal output;
                    if (!(decimal.TryParse(row.PorcentajeIncapacidad, out output)))
                    {
                        row.Errors.Add("Porcentaje de incapacidad incorrecto.");
                        row.IsValid = false;
                    }
                    else
                    {
                        if (decimal.Parse(row.PorcentajeIncapacidad) < 0 || decimal.Parse(row.PorcentajeIncapacidad) > 100)
                        {
                            row.Errors.Add("Porc. incapacidad debe estar entre 0 y 100.");
                            row.IsValid = false;
                        }
                    }
                }
                if (!String.IsNullOrWhiteSpace(row.Estado))
                {
                    if (Estados.Where(e => e.Descripcion.ToLower().Trim() == row.Estado.ToLower().Trim()).ToList().Count < 1)
                    {
                        row.Errors.Add("No existe estado denuncia.");
                        row.IsValid = false;
                    }
                    else if (Estados.Where(e => e.Descripcion.ToLower().Trim() == row.Estado.ToLower().Trim() && e.Anulado == false).ToList().Count < 1)
                    {
                        row.Errors.Add("Estado denuncia anulado.");
                        row.IsValid = false;
                    }
                    else
                    {
                        row.EstadoId = Estados.Where(e => e.Descripcion.ToLower() == row.Estado.ToLower().TrimOrNull() && e.Anulado == false).FirstOrDefault().Id;
                    }
                }
                else
                {

                    if (Estados.Where(e => e.Predeterminado == true).ToList().Count < 1)
                    {
                        row.Errors.Add("No ingresó estado y no existe predeterminado.");
                        row.IsValid = false;
                    }
                    else
                    {
                        row.EstadoId = Estados.Where(e => e.Predeterminado == true).FirstOrDefault().Id;
                    }
                }

                // Empresas
                if (String.IsNullOrWhiteSpace(row.Empresa))
                {
                    row.Errors.Add("Empresa es requerido.");
                    row.IsValid = false;
                }
                else
                {
                    if (Empresas.Where(e => e.DesEmpr.ToLower().Trim() == row.Empresa.ToLower().Trim() && e.FecBaja == null).ToList().Count < 1)
                    {
                        row.Errors.Add("No existe empresa.");
                        row.IsValid = false;
                    }
                    else
                    {
                        row.EmpresaId = Empresas.Where(e => e.DesEmpr.ToLower().Trim() == row.Empresa.ToLower().Trim() && e.FecBaja == null).FirstOrDefault().Id;
                    }
                }
                // Empleados
                if (String.IsNullOrWhiteSpace(row.EmpleadoCUIL) && String.IsNullOrWhiteSpace(row.EmpleadoDNI))
                {
                    row.Errors.Add("Falta informar DNI / CUIL empleado.");
                    row.IsValid = false;
                }
                else
                {
                    if (!row.EmpleadoCUIL.All(char.IsDigit))
                    {
                        row.Errors.Add("El CUIL tiene que contener solo números");
                        row.IsValid = false;
                    }

                    if (Empleados.Where(e => e.Dni.Trim().TrimStart(new Char[] { '0' }) == row.EmpleadoDNI.Trim()).ToList().Count < 1 && Empleados.Where(e => e.Cuil.Trim() == row.EmpleadoCUIL.Trim()).ToList().Count < 1)
                    {
                        row.Errors.Add("Empleado no encontrado");
                        row.IsValid = false;
                    }
                    else
                    {
                        var empleado = Empleados.Where(e => e.Dni.Trim().TrimStart(new Char[] { '0' }) == row.EmpleadoDNI.Trim()).FirstOrDefault();
                        if (empleado == null)
                        {
                            empleado = Empleados.Where(e => e.Cuil.Trim() == row.EmpleadoCUIL.Trim()).FirstOrDefault();
                        }

                        row.EmpleadoId = empleado.Id;
                        row.SucursalId = empleado.UnidadNegocio.cod_sucursal.Value;
                        row.EmpleadoAntiguedad = empleado.FecAntiguedad;
                        row.EmpleadoArea = empleado.Area;

                        if (empleado.LegajosEmpleado.Count >= 1)
                        {
                            if (empleado.LegajosEmpleado.Where(e => e.FecBaja == null && e.Id == empleado.Id).Count() >= 1)
                            {
                                var legempleado = empleado.LegajosEmpleado.Where(e => e.FecBaja == null && e.Id == empleado.Id).FirstOrDefault();
                                row.EmpleadoEmpresaId = legempleado.CodEmpresa;
                                row.EmpleadoFechaIngreso = legempleado.FecIngreso;
                                row.EmpleadoLegajo = legempleado.LegajoSap;
                            }
                            else if (empleado.LegajosEmpleado.Where(e => e.Id == empleado.Id).Count() >= 1)
                            {
                                var legempleado = empleado.LegajosEmpleado.Where(e => e.Id == empleado.Id).OrderByDescending(e => e.FecIngreso).FirstOrDefault();
                                row.EmpleadoEmpresaId = legempleado.CodEmpresa;
                                row.EmpleadoFechaIngreso = legempleado.FecIngreso;
                                row.EmpleadoLegajo = legempleado.LegajoSap;
                            }
                        }
                    }
                }


                if (row.FechaOcurrencia == null)
                {
                    row.Errors.Add("Fecha de ocurrencia es requerida.");
                    row.IsValid = false;
                }
                else
                {
                    if (row.FechaAltaLaboral.HasValue)
                    {
                        if (row.FechaOcurrencia.Value.Date < row.FechaAltaLaboral.Value.Date)
                        {
                            row.CantidadDiasBaja = Convert.ToInt32((row.FechaAltaLaboral.Value.Date - row.FechaOcurrencia.Value.Date).TotalDays);
                        }
                        else
                        {
                            row.CantidadDiasBaja = 0;
                        }
                    }
                    else
                    {
                        if (row.FechaOcurrencia.Value.Date <= DateTime.Now.Date)
                        {
                            row.CantidadDiasBaja = Convert.ToInt32((DateTime.Now.Date - row.FechaOcurrencia.Value.Date).TotalDays);
                        }
                        else
                        {
                            row.CantidadDiasBaja = 0;
                        }
                    }
                }
                if (row.FechaRecepcionDenuncia == null)
                {
                    row.Errors.Add("Fecha de recepción de denuncia es requerida.");
                    row.IsValid = false;
                }

                // Contingencias
                if (!String.IsNullOrWhiteSpace(row.Contingencia))
                {
                    if (Contingencias.Where(e => e.Descripcion.ToLower().Trim() == row.Contingencia.ToLower().Trim()).ToList().Count < 1)
                    {
                        row.Errors.Add("No existe contingencia.");
                        row.IsValid = false;
                    }
                    else if (Contingencias.Where(e => e.Descripcion.ToLower().Trim() == row.Contingencia.ToLower().Trim() && e.Anulado == false).ToList().Count < 1)
                    {
                        row.Errors.Add("Contingencia anulada.");
                        row.IsValid = false;
                    }
                    else
                    {
                        row.ContingenciaId = Contingencias.Where(e => e.Descripcion.ToLower().Trim() == row.Contingencia.ToLower().Trim() && e.Anulado == false).FirstOrDefault().Id;
                    }
                }
                else
                {
                    row.ContingenciaId = null;
                }

                // Patologias
                if (!String.IsNullOrWhiteSpace(row.Patologia))
                {
                    if (Patologias.Where(e => e.Descripcion.ToLower().Trim() == row.Patologia.ToLower().Trim()).ToList().Count < 1)
                    {
                        row.Errors.Add("No existe patología.");
                        row.IsValid = false;
                    }
                    else if (Patologias.Where(e => e.Descripcion.ToLower().Trim() == row.Patologia.ToLower().Trim() && e.Anulado == false).ToList().Count < 1)
                    {
                        row.Errors.Add("Patología anulada.");
                        row.IsValid = false;
                    }
                    else
                    {
                        row.PatologiaId = Patologias.Where(e => e.Descripcion.ToLower().Trim() == row.Patologia.ToLower().Trim() && e.Anulado == false).FirstOrDefault().Id;
                    }
                }
                else
                {
                    row.PatologiaId = null;
                }

                // Tratamientos
                if (!String.IsNullOrWhiteSpace(row.Tratamiento))
                {
                    if (Tratamientos.Where(e => e.Descripcion.ToLower().Trim() == row.Tratamiento.ToLower().Trim()).ToList().Count < 1)
                    {
                        row.Errors.Add("No existe tratamiento.");
                        row.IsValid = false;
                    }
                    else if (Tratamientos.Where(e => e.Descripcion.ToLower().Trim() == row.Tratamiento.ToLower().Trim() && e.Anulado == false).ToList().Count < 1)
                    {
                        row.Errors.Add("Tratamiento anulado.");
                        row.IsValid = false;
                    }
                    else
                    {
                        row.TratamientoId = Tratamientos.Where(e => e.Descripcion.ToLower().Trim() == row.Tratamiento.ToLower().Trim() && e.Anulado == false).FirstOrDefault().Id;
                    }
                }
                else
                {
                    row.TratamientoId = null;
                }

                // MotivoAudiencias
                if (!String.IsNullOrWhiteSpace(row.MotivoAudiencia))
                {
                    if (MotivosAudiencias.Where(e => e.Descripcion.ToLower().Trim() == row.MotivoAudiencia.ToLower().Trim()).ToList().Count < 1)
                    {
                        row.Errors.Add("No existe motivo de audiencia médica");
                        row.IsValid = false;
                    }
                    else if (MotivosAudiencias.Where(e => e.Descripcion.ToLower().Trim() == row.MotivoAudiencia.ToLower().Trim() && e.Anulado == false).ToList().Count < 1)
                    {
                        row.Errors.Add("Audiencia médica anulada");
                        row.IsValid = false;
                    }
                    else
                    {
                        row.MotivoAudienciaId = MotivosAudiencias.Where(e => e.Descripcion.ToLower().Trim() == row.MotivoAudiencia.ToLower().Trim() && e.Anulado == false).FirstOrDefault().Id;
                    }
                }
                else
                {
                    row.MotivoAudienciaId = null;
                }


                // MotivosNotificacion
                if (!String.IsNullOrWhiteSpace(row.MotivoNotificacion))
                {
                    if (MotivosNotificaciones.Where(e => e.Descripcion.ToLower().Trim() == row.MotivoNotificacion.ToLower().Trim()).ToList().Count < 1)
                    {
                        row.Errors.Add("No existe motivo notificación.");
                        row.IsValid = false;
                    }
                    else if (MotivosNotificaciones.Where(e => e.Descripcion.ToLower().Trim() == row.MotivoNotificacion.ToLower().Trim() && e.Anulado == false).ToList().Count < 1)
                    {
                        row.Errors.Add("Motivo notificación anulado.");
                        row.IsValid = false;
                    }
                    else
                    {
                        row.MotivoNotificacionId = MotivosNotificaciones.Where(e => e.Descripcion.ToLower().Trim() == row.MotivoNotificacion.ToLower().Trim() && e.Anulado == false).FirstOrDefault().Id;
                    }
                }
                else
                {
                    row.MotivoNotificacionId = null;
                }

                // DenunciaOrigen
                if (!String.IsNullOrWhiteSpace(row.NroDenunciaOrigen))
                {
                    if (Denuncias.Where(e => e.NroDenuncia.Trim() == row.NroDenunciaOrigen.Trim() && e.PrestadorMedicoId == row.PrestadorMedicoId && e.EmpleadoId == row.EmpleadoId && e.Anulado == false && e.IsDeleted == false).ToList().Count < 1)
                    {
                        row.Errors.Add("No existe denuncia origen.");
                        row.IsValid = false;
                    }
                    else
                    {
                        row.DenunciaOrigenId = Denuncias.Where(e => e.NroDenuncia.Trim() == row.NroDenunciaOrigen.Trim() && e.PrestadorMedicoId == row.PrestadorMedicoId && e.EmpleadoId == row.EmpleadoId && e.Anulado == false && e.IsDeleted == false).FirstOrDefault().Id;
                    }
                }
                else
                {
                    row.DenunciaOrigenId = null;
                }

                //Siniestro
                if (!String.IsNullOrWhiteSpace(row.NroSiniestro))
                {
                    if (Siniestros.Where(e => e.NroSiniestro.Trim() == row.NroSiniestro.Trim() && e.ConductorId == row.EmpleadoId && e.Anulado == false && e.IsDeleted == false).ToList().Count < 1)
                    {
                        row.Errors.Add("No existe nro. siniestro.");
                        row.IsValid = false;
                    }
                    else
                    {
                        row.SiniestroId = Siniestros.Where(e => e.NroSiniestro.Trim() == row.NroSiniestro.Trim() && e.ConductorId == row.EmpleadoId && e.Anulado == false && e.IsDeleted == false).FirstOrDefault().Id;
                    }
                }
                else
                {
                    row.SiniestroId = null;
                }

                if (row.MotivoNotificacionId.HasValue || (!String.IsNullOrWhiteSpace(row.ObservacionesNotificacion)) || row.FechaNotificacion.HasValue)
                {
                    if (!(row.MotivoNotificacionId.HasValue && row.FechaNotificacion.HasValue))
                    {
                        if (!row.MotivoNotificacionId.HasValue)
                        {
                            row.Errors.Add("Motivo de notificación requerido.");
                            row.IsValid = false;
                        }
                        if (!row.FechaNotificacion.HasValue)
                        {
                            row.Errors.Add("Fecha de notificación requerida.");
                            row.IsValid = false;
                        }
                    }
                }


            }

            return (List<ImportadorExcelDenuncias>)excelFileDTO;
        }

        private void FormatCell(ExcelRange cel, string format = "dd/MM/yyyy")
        {
            if (cel.Style.Numberformat.Format != format)
            {
                cel.Style.Numberformat.Format = format;
            }
        }
        public async Task ImportarDenuncias(DenunciaImportadorFileFilter input)
        {
            await this._serviceBase.ImportarDenuncias(input);
        }


        //TODO: Get Data According the Task 1007
        public async Task<ReportModel> GetDatosReporte(ArtDenunciasDto dto)
        {

            // Needed services
            var estadosService = (IEstadosService)_serviceProvider.GetService(typeof(IEstadosService));
            var empleadosService = (IEmpleadosService)_serviceProvider.GetService(typeof(IEmpleadosService));
            var localidadesService = (ILocalidadesService)_serviceProvider.GetService(typeof(ILocalidadesService));

            ArtEstados estado = await estadosService.GetByIdAsync(dto.EstadoId);
            Empleados empleado = await empleadosService.GetByIdAsync(dto.EmpleadoId);

            string domicilioLocalidad = await GetDomicilioLocalidad(empleado, localidadesService);

            DenunciaReportModel denunciaReportModel = new DenunciaReportModel();
            denunciaReportModel.NroDenuncia = dto.NroDenuncia;
            denunciaReportModel.Estado = estado.Descripcion;
            denunciaReportModel.Empresa = dto.EmpresaGrilla;
            denunciaReportModel.Empleado = $"{empleado.Cuil}  {dto.NombreEmpleado}";
            denunciaReportModel.Domicilio = domicilioLocalidad;
            denunciaReportModel.Contingencia = (dto.Contingencia != null) ? dto.Contingencia.Descripcion : string.Empty;
            denunciaReportModel.Prestador = (dto.PrestadorMedico != null) ? dto.PrestadorMedico.Descripcion : string.Empty;
            denunciaReportModel.Tratamiento = (dto.Tratamiento != null) ? dto.Tratamiento.Descripcion : string.Empty;
            denunciaReportModel.AltaMedica = dto.AltaMedica.HasValue ? Convert.ToBoolean(dto.AltaMedica) ? "SI" : "NO" : "-";
            denunciaReportModel.Diagnostico = dto.Diagnostico;
            denunciaReportModel.Patologia = (dto.Patologia != null) ? dto.Patologia.Descripcion : string.Empty;
            denunciaReportModel.FechaOcurrencia = dto.FechaOcurrencia.ToString("dd/MM/yyyy");
            denunciaReportModel.FechaBajaServicio = dto.FechaBajaServicio.HasValue ? dto.FechaBajaServicio.Value.ToString("dd/MM/yyyy") : "-";
            denunciaReportModel.FechaAudienciaMedica = dto.FechaAudienciaMedica.HasValue ? dto.FechaAudienciaMedica.Value.ToString("dd/MM/yyyy") : "-";
            denunciaReportModel.FechaUltimoControl = dto.FechaUltimoControl.HasValue ? dto.FechaUltimoControl.Value.ToString("dd/MM/yyyy") : "-";
            denunciaReportModel.FechaRecepcionDenuncia = dto.FechaRecepcionDenuncia.ToString("dd/MM/yyyy");
            denunciaReportModel.FechaProximaConsultaTurno = dto.FechaProximaConsulta.HasValue ? dto.FechaProximaConsulta.Value.ToString("dd/MM/yyyy") : "-";
            denunciaReportModel.FechaProbableAltaMedica = dto.FechaAltaMedica.HasValue ? dto.FechaAltaMedica.Value.ToString("dd/MM/yyyy") : "-";

            ReportModel reportModel = new ReportModel
            {
                ReportName = ReportName.DenunciaReportNamespace
            };
            reportModel.AddDataSources("DataSetDenuncia", new List<DenunciaReportModel> { denunciaReportModel });
            return reportModel;
        }
        //TODO: Preguntar si ya existe una funcionalidad disponible para domicilio y localidades. Sino, se debe refactorizar con extension para su reutilización
        private async Task<string> GetDomicilioLocalidad(Empleados empleado, ILocalidadesService localidadesService)
        {
            int codLocalidad = (empleado.CodLocalidad.HasValue ? Convert.ToInt32(empleado.CodLocalidad) : 0);

            var localidades = await localidadesService.GetAllLocalidades();
            Localidades localidad = localidades.Items.Where(e=> e.Id == codLocalidad).FirstOrDefault();

            StringBuilder domicilioLocalidad = new StringBuilder();
            domicilioLocalidad.Append($"{empleado.CalleDomicilio?.Trim()} {empleado.NroDomicilio?.Trim()}");
            if (!string.IsNullOrEmpty(empleado.PisoDomicilio))
                domicilioLocalidad.Append($" {empleado.PisoDomicilio.Trim()}");
            if (!string.IsNullOrEmpty(empleado.DeptoDomicilio))
                domicilioLocalidad.Append($" {empleado.DeptoDomicilio.Trim()}");
            if (localidad != null)
                domicilioLocalidad.Append($" {localidad.GetDescription().Trim()}");
            return domicilioLocalidad.ToString();

        }
    }
}
