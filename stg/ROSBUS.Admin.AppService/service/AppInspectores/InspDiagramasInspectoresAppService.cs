using OfficeOpenXml;
using OfficeOpenXml.Style;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Interface.AppInspectores;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Entities.Filters.AppInspectores;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Admin.Domain.ParametersHelper;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.DocumentsHelper.Excel;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService.service
{
    public class InspDiagramasInspectoresAppService : AppServiceBase <InspDiagramasInspectores, InspDiagramasInspectoresDto, int, IInspDiagramasInspectoresService>, IInspDiagramasInspectoresAppService
    {
        private readonly IInspDiagramasInspectoresTurnosAppService inspDiagramasInspectoresTurnosService;
        private readonly IParametersHelper parametersHelper;
        private readonly IAuthService authService;
        private readonly IUserService userService;
        private readonly IInspRangosHorarioAppService inspRangoshorarios;
        private readonly IInspEstadosDiagramaInspectoresAppService inspEstadosDiagramas;
        public InspDiagramasInspectoresAppService(IInspDiagramasInspectoresService serviceBase,IInspDiagramasInspectoresTurnosAppService _isnpDiagramasInspectoresTurnos, IParametersHelper _parametersHelper, 
            IAuthService _authService, IUserService _userService, IInspRangosHorarioAppService _inspRangoshorarios, IInspEstadosDiagramaInspectoresAppService _inspEstadosDiagramas)
             : base(serviceBase)
        {
            inspDiagramasInspectoresTurnosService = _isnpDiagramasInspectoresTurnos;
            parametersHelper = _parametersHelper;
            authService = _authService;
            userService = _userService;
            inspRangoshorarios = _inspRangoshorarios;
            inspEstadosDiagramas = _inspEstadosDiagramas;
        }

        public Task<DiagramaMesAnioDto> DiagramaMesAnioGrupo(int Id, List<int> turnoId, Boolean blockentity)
        {
            return this._serviceBase.DiagramaMesAnioGrupo(Id, turnoId, blockentity);
        }

        public Task<DiasMesDto> DiagramacionPorDia(DateTime Fecha)
        {
            return this._serviceBase.DiagramacionPorDia(Fecha);
        }

        
       

        public async Task<List<InspDiagramasInspectoresTurnosDto>> TurnosDeLaDiagramacion (int Id)
        {

            var turnos = await inspDiagramasInspectoresTurnosService.GetAllAsync(dt => dt.DiagramaInspectoresId == Id);

            return MapList<InspDiagramasInspectoresTurnos, InspDiagramasInspectoresTurnosDto>(turnos.Items.ToList()).ToList();
        }

        public Task<InspectorDiaDto> EliminarCelda(DiasMesDto model)
        {
            return this._serviceBase.EliminarCelda(model);
        }

        public async Task SaveDiagramacion (List<InspectorDiaDto> InspectorDiaDtos, int Id)
        {
            List<HFrancos> hFrancos = new List<HFrancos>();
            List<PersJornadasTrabajadas> jornadasTrabajadas  = new List<PersJornadasTrabajadas>();

            foreach(var insp in InspectorDiaDtos)
            {
                if((insp.EsFranco && !insp.EsFrancoTrabajado) || (insp.EsFranco && insp.FrancoNovedad))
                {
                    HFrancos hfranco = new HFrancos();
                    hfranco.Id = insp.CodEmpleado;
                    hfranco.Fecha = insp.diaMesFecha.Value;
                    hfranco.CodNov = inspRangoshorarios.GetAll(r => r.Id == insp.RangoHorarioId).Items.FirstOrDefault().NovedadId.Value;    
                    hfranco.Modificable = "S";
                    var logonName =  userService.GetAll(u => u.Id == authService.GetCurretUserId()).Items.FirstOrDefault().LogonName;
                    hfranco.Observacion = "Asignado " + DateTime.Now + " - Usuario " + logonName;
                    hfranco.PasadoSueldos = "N";
                    hfranco.RangoHorarioId = insp.RangoHorarioId;
                    hfranco.CodUsu = this.parametersHelper.GetParameter<string>("insp_diagrama_cod_usu");
                    hfranco.DiagramaInspectoresTurnoId = this.inspDiagramasInspectoresTurnosService.GetAll(dt => dt.TurnoId == insp.InspTurnoId && dt.DiagramaInspectoresId == Id).Items.FirstOrDefault().Id;

                    hFrancos.Add(hfranco);
                }

                PersJornadasTrabajadas persJornada = null;
                if (insp.EsJornada || (insp.EsFranco && insp.EsFrancoTrabajado))
                {
                    persJornada = new PersJornadasTrabajadas();

                    persJornada.Id = insp.CodJornada == 0 ? 0 : insp.CodJornada;
                    if (insp.CodJornada == 0)
                    {
                        persJornada.HoraDesde = insp.HoraDesde.Value;
                        persJornada.HoraHasta = insp.HoraHasta.Value;
                    }
                    persJornada.CodEmpleado =  insp.CodEmpleado;
                    persJornada.CodTurno = insp.InspTurnoId;
                    persJornada.Duracion = insp.HoraHastaModificada.Value.Subtract(insp.HoraDesdeModificada.Value).ToString().Substring(0,5);                    
                    persJornada.HoraDesdeModif = insp.HoraDesdeModificada.Value;                    
                    persJornada.HoraHastaModif = insp.HoraHastaModificada.Value;
                    persJornada.Fecha = insp.diaMesFecha.Value;
                    persJornada.TpoServicio = "N".Substring(0,1);
                    persJornada.PasadaSueldos = "N".Substring(0, 1);
                    persJornada.RangoHorarioId = insp.RangoHorarioId;
                    persJornada.ZonaId = insp.ZonaId;
                    if (insp.EsFrancoTrabajado)
                    {
                        persJornada.Pago = insp.Pago == 1 ? true : false;
                    }
                    else
                    {
                        persJornada.Pago = null;
                    }
                    #region Sys_Parametros

                    persJornada.CodGalpon =  this.parametersHelper.GetParameter<int>("insp_diagrama_cod_galpon");

                    persJornada.CodArea = this.parametersHelper.GetParameter<int>("insp_diagrama_cod_area");

                    #endregion

                    #region DiagramasInspectoresTurnosId

                    persJornada.DiagramaInspectoresTurnoId = this.inspDiagramasInspectoresTurnosService.GetAll(dt => dt.TurnoId == persJornada.CodTurno && dt.DiagramaInspectoresId == Id).Items.FirstOrDefault().Id;

                    #endregion

                    jornadasTrabajadas.Add(persJornada);

                    if (insp.EsFranco && insp.EsFrancoTrabajado)
                    {
                        HFrancos hfranco = new HFrancos();
                        hfranco.Id = insp.CodEmpleado;
                        hfranco.Fecha = insp.diaMesFecha.Value;
                        hfranco.JornadasTrabajadaId = persJornada.Id;
                        if(persJornada.Id == 0)
                            hfranco.JornadasTrabajada = persJornada;

                        hfranco.CodNov = inspRangoshorarios.GetAll(r => r.Id == insp.RangoHorarioId).Items.FirstOrDefault().NovedadId.Value;
                        hfranco.Modificable = "S";
                        var logonName = userService.GetAll(u => u.Id == authService.GetCurretUserId()).Items.FirstOrDefault().LogonName;
                        hfranco.Observacion = "Asignado " + DateTime.Now + " - Usuario " + logonName;
                        hfranco.PasadoSueldos = "N";
                        hfranco.RangoHorarioId = insp.RangoHorarioId;
                        hfranco.CodUsu = this.parametersHelper.GetParameter<string>("insp_diagrama_cod_usu");
                        hfranco.DiagramaInspectoresTurnoId = this.inspDiagramasInspectoresTurnosService.GetAll(dt => dt.TurnoId == insp.InspTurnoId && dt.DiagramaInspectoresId == Id).Items.FirstOrDefault().Id;
                        

                        hFrancos.Add(hfranco);
                    }
                }
            }

            await this._serviceBase.SaveDiagramacion(hFrancos, jornadasTrabajadas);
            await this._serviceBase.UnBlockEntity(Id);
        }    

        public async Task PublicarDiagramacion(InspDiagramasInspectores Diagramacion)
        {
            InspDiagramasInspectores diagramaInsp = new InspDiagramasInspectores();

            diagramaInsp = await this._serviceBase.GetByIdAsync(Diagramacion.Id);

            var estado = inspEstadosDiagramas.GetDtoByIdAsync(diagramaInsp.EstadoDiagramaId);            

            if (estado.Id != 2)
            {
                var turnos = await inspDiagramasInspectoresTurnosService.GetAllAsync(dt => dt.DiagramaInspectoresId == diagramaInsp.Id);
                List<int> turnosId = new List<int>();
                foreach(var tur in turnos.Items)
                {
                    turnosId.Add(tur.TurnoId);
                }

                var diagramacionDiasMes = await  this.DiagramaMesAnioGrupo(diagramaInsp.Id, turnosId, false);

                if (diagramacionDiasMes.DiasMes != null)
                {
                    foreach (var dia in diagramacionDiasMes.DiasMes)
                    {
                        foreach (var insp in dia.Inspectores)
                        {
                            if (!insp.EsFranco && !insp.EsFrancoTrabajado && !insp.EsJornada && !insp.EsNovedad)
                            {
                                throw new ValidationException("La diagramación no está completa. No se puede pasar a Publicado");
                            }
                        }
                    }

                    diagramaInsp = await this._serviceBase.GetDtoByAndBlockIdAsync(diagramaInsp.Id);

                    diagramaInsp.EstadoDiagramaId = 2;
                    
                    //diagramaInsp.BlockDate = diagramacionDiasMes.BlockDate;

                    await this._serviceBase.UpdateAsync(diagramaInsp);

                }


            }

        }

        public async Task<FileDto> ImprimirDiagrama(int Id, List<int> turnoId)
        {
            InspDiagramasInspectores diagramaInsp = new InspDiagramasInspectores();

            diagramaInsp = await this._serviceBase.GetByIdAsync(Id);
            string anio = "";
            string mes= "";
            string grupo= "";

            // Todo
            String tempFile = string.Format("{0}{1}.{2}", System.IO.Path.GetTempPath(), Guid.NewGuid().ToString(), "xlsx");
            var newFile = new FileInfo(tempFile);

            using (ExcelPackage package = new ExcelPackage(newFile))
            {
                var diagramacionDiasMes = await this.DiagramaMesAnioGrupo(diagramaInsp.Id, turnoId,false);
                if(diagramacionDiasMes.DiasMes == null)
                {
                    throw new ValidationException("No existen usuarios para el Grupo de Inspectores");
                }
                anio = diagramacionDiasMes.Anio.ToString();
                mes = diagramacionDiasMes.Mes.ToString();
                grupo = diagramacionDiasMes.GrupoInspectores;

                ExcelWorksheet ws = package.Workbook.Worksheets.Add("Diagramación");
                ws.PrinterSettings.PaperSize = ePaperSize.A4;
                ws.PrinterSettings.Orientation = eOrientation.Landscape;

                var RowFrom = 1;
                ws.Cells[string.Format("B{0}:G{0}", RowFrom)].Merge = true;
                ws.Cells[string.Format("B{0}", RowFrom)].Value = "Mes: " + diagramacionDiasMes.Mes + "  Año: " + diagramacionDiasMes.Anio + "  Grupo: " + diagramacionDiasMes.GrupoInspectores + "  Estado: " + diagramacionDiasMes.Estado;
                ws.Cells[string.Format("B{0}", RowFrom)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                ws.Cells[string.Format("B{0}", RowFrom)].Style.Font.Bold = true;
                ws.Cells[string.Format("B{0}", RowFrom)].Style.Font.Size = 12;
                RowFrom++;

                int columnIndex = 1; // 'A';                 
                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Value = "Fecha";
                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Font.Size = 9;

                columnIndex++;
                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Value = "Día";
                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Font.Size = 10;

                foreach (var ins in diagramacionDiasMes.DiasMes.FirstOrDefault().Inspectores)
                {
                    columnIndex++;
                    ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Value = ins.DescripcionInspector + " " + ins.Legajo + " " + ins.InspTurno;
                    ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                    ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.WrapText = true;
                    ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Font.Size = 8;
                }

                ws.DefaultColWidth = 10;
                ws.Cells[string.Format("A1:{0}{1}", GetColumn(columnIndex), diagramacionDiasMes.DiasMes.Count() + 2)].Style.Border.Top.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A1:{0}{1}", GetColumn(columnIndex), diagramacionDiasMes.DiasMes.Count() + 2)].Style.Border.Bottom.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A1:{0}{1}", GetColumn(columnIndex), diagramacionDiasMes.DiasMes.Count() + 2)].Style.Border.Left.Style = ExcelBorderStyle.Thin;
                ws.Cells[string.Format("A1:{0}{1}", GetColumn(columnIndex), diagramacionDiasMes.DiasMes.Count() + 2)].Style.Border.Right.Style = ExcelBorderStyle.Thin;

                foreach (var dia in diagramacionDiasMes.DiasMes)
                {
                    columnIndex = 1;
                    RowFrom++;
                    ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Value = dia.Fecha.ToString("dd/MM/yyyy");
                    ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Font.Size = 9;
                    ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    if (dia.Color != null)
                    {
                        Color colFromHex = System.Drawing.ColorTranslator.FromHtml(dia.Color);
                        ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Fill.BackgroundColor.SetColor(colFromHex);
                    }

                    columnIndex++;
                    ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Value = dia.NombreDia;
                    ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Font.Size = 9;
                    ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                    if (dia.Color != null)
                    {
                        Color colFromHex = System.Drawing.ColorTranslator.FromHtml(dia.Color);
                        ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                        ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Fill.BackgroundColor.SetColor(colFromHex);
                    }

                    foreach (var insp in dia.Inspectores)
                    {
                        columnIndex++;
                        if (dia.Color != null)
                        {
                            Color colFromHex = System.Drawing.ColorTranslator.FromHtml(dia.Color);
                            ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                            ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Fill.BackgroundColor.SetColor(colFromHex);
                        }

                        if (insp.EsJornada)
                        {
                            ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Value = insp.HoraDesdeModificada.Value.ToString("HH:mm") + " a " + insp.HoraHastaModificada.Value.ToString("HH:mm") + "   " + insp.NombreZona;
                            ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.WrapText = true;
                            ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Font.Size = 8;
                            if (insp.Color != null)
                            {
                                Color colFromHex = System.Drawing.ColorTranslator.FromHtml(insp.Color);
                                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Fill.BackgroundColor.SetColor(colFromHex);
                            }
                        }

                        if (insp.EsFranco)
                        {
                            if (insp.EsFrancoTrabajado)
                            {
                                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Value = insp.HoraDesdeModificada.Value.ToString("HH:mm") + " a " + insp.HoraHastaModificada.Value.ToString("HH:mm") + "   " + insp.NombreZona;
                                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.WrapText = true;
                                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Font.Size = 8;
                                if (insp.Color != null)
                                {
                                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml(insp.Color);
                                    ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Fill.BackgroundColor.SetColor(colFromHex);
                                }
                            }
                            else
                            {
                                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Value = insp.NombreRangoHorario;
                                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Font.Size = 8;
                                if (insp.Color != null)
                                {
                                    Color colFromHex = System.Drawing.ColorTranslator.FromHtml(insp.Color);
                                    ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                    ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Fill.BackgroundColor.SetColor(colFromHex);
                                }
                            }
                        }

                        if (insp.EsNovedad)
                        {
                            ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Value = insp.DescNovedad;
                            ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                            ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.VerticalAlignment = ExcelVerticalAlignment.Center;
                            ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Font.Size = 8;
                            if (insp.Color != null)
                            {
                                Color colFromHex = System.Drawing.ColorTranslator.FromHtml(insp.Color);
                                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Fill.PatternType = ExcelFillStyle.Solid;
                                ws.Cells[string.Format("{0}{1}", GetColumn(columnIndex), RowFrom)].Style.Fill.BackgroundColor.SetColor(colFromHex);
                            }
                        }
                    }
                }
                package.Save();

                var file = new FileDto();
                file.ByteArray = ExcelHelper.GenerateByteArray(package);
                file.ForceDownload = true;
                file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                file.FileName = string.Format("Diagramacion - {0}{1} - {2}.xlsx", anio, mes, grupo);
                file.FileDescription = "Diagramacion";

                return file;
            }

        }

        protected string GetColumn(int from)
        {
            string columnName = string.Empty;

            if (from > 26)
            {
                int cociente = from / 26;
                if (cociente != 0)
                {
                    columnName = ((char)(cociente + 64)).ToString();
                    from = from - (cociente * 26);
                }
            }
            if (from == 0)
            {
                columnName += "Z";
            }
            else
            {
                columnName += ((char)(from + 64)).ToString();
            }

            return columnName;
        }
    }
}
