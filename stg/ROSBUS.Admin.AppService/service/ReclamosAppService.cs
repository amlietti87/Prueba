using OfficeOpenXml;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Entities.Enums;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.ART.Domain.Entities.ART;
using ROSBUS.ART.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.DocumentsHelper.Excel;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;


namespace ROSBUS.Admin.AppService
{

    public class ReclamosAppService : AppServiceBase<SinReclamos, ReclamosDto, int, IReclamosService>, IReclamosAppService
    {
        private readonly IAdjuntosService _adjuntosService;
        private readonly IReclamosHistoricosAppService _reclamosHistoricosAppService;
        private readonly ITiposReclamoService tiposReclamoService;
        private readonly IEstadosService estadosService;
        private readonly ISubEstadosService subEstadosService;
        private readonly IsucursalesService sucursalesService;
        private readonly IEmpresaService empresaService;
        private readonly Domain.Interfaces.Services.ART.IDenunciasService denunciasService;
        private readonly ICausasReclamoService causasReclamoService;
        private readonly IAbogadosService abogadosService;
        private readonly ITiposAcuerdoService tiposAcuerdoService;
        private readonly IRubrosSalarialesService rubrosSalarialesService;
        private readonly IJuzgadosService juzgadosService;
        private readonly IEmpleadosService empleadosService;
        public ReclamosAppService(IReclamosService serviceBase,
                                  IAdjuntosService adjuntosService,
                                  IReclamosHistoricosAppService reclamosHistoricosAppService,
                                  ITiposReclamoService tiposReclamoService,
                                  IEstadosService estadosService,
                                  ISubEstadosService subEstadosService,
                                  IsucursalesService sucursalesService,
                                  IEmpresaService empresaService,
                                  Domain.Interfaces.Services.ART.IDenunciasService denunciasService,
                                  ICausasReclamoService causasReclamoService,
                                  IAbogadosService abogadosService,
                                  ITiposAcuerdoService tiposAcuerdoService,
                                  IRubrosSalarialesService rubrosSalarialesService,
                                  IJuzgadosService juzgadosService,
                                  IEmpleadosService empleadosService) : base(serviceBase)
        {

            _adjuntosService = adjuntosService;
            _reclamosHistoricosAppService = reclamosHistoricosAppService;
            this.tiposReclamoService = tiposReclamoService;
            this.estadosService = estadosService;
            this.subEstadosService = subEstadosService;
            this.sucursalesService = sucursalesService;
            this.empresaService = empresaService;
            this.denunciasService = denunciasService;
            this.causasReclamoService = causasReclamoService;
            this.abogadosService = abogadosService;
            this.tiposAcuerdoService = tiposAcuerdoService;
            this.rubrosSalarialesService = rubrosSalarialesService;
            this.juzgadosService = juzgadosService;
            this.empleadosService = empleadosService;
        }

        public async Task<ReclamosDto> CambioEstado(ReclamosDto reclamo, ReclamosHistoricosDto historico)
        {
            var reclamoentity = await this._serviceBase.GetByIdAsync(reclamo.Id);
            MapObject(reclamo, reclamoentity);

            var reclamohistoricoentity = new SinReclamosHistoricos();
            MapObject(historico, reclamohistoricoentity);

            var transaction = await this._serviceBase.CambioEstado(reclamoentity, reclamohistoricoentity);
            var result = new ReclamosDto();
            MapObject(transaction, result);
            return result;
        }

        public async Task<List<AdjuntosDto>> GetAdjuntos(int reclamoId)
        {
            List<ItemDto<Guid>> adjuntos = new List<ItemDto<Guid>>();

            List<SinReclamoAdjuntos> sinAdj = await this._serviceBase.GetAdjuntos(reclamoId);



            AdjuntosFilter filter = new AdjuntosFilter();
            filter.Ids = sinAdj.Select(e => e.AdjuntoId).ToList(); ;

            adjuntos = await _adjuntosService.GetAdjuntosItemDto(filter);

            return adjuntos.Select(e => new AdjuntosDto() { Id = e.Id, Nombre = e.Description }).ToList();
        }

        public async Task AgregarAdjuntos(int reclamoId, List<AdjuntosDto> result)
        {
            var allEntity = await this._serviceBase.GetAllAsync(e => e.Id == reclamoId);

            var entity = allEntity.Items.FirstOrDefault();
            if (entity != null)
            {
                foreach (var item in result)
                {
                    entity.SinReclamoAdjuntos.Add(new SinReclamoAdjuntos() { AdjuntoId = item.Id, ReclamoId = reclamoId });
                }

            }

            await this._serviceBase.UpdateAsync(entity);
        }
        public override async Task<ReclamosDto> AddAsync(ReclamosDto dto)
        {
            if (dto.ReclamoCuotas != null)
            {
                foreach (var item in dto.ReclamoCuotas.Where(w => w.Id < 0).ToList())
                {
                    item.Id = 0;
                }
            }

            var res = await base.AddAsync(dto);

            return res;
        }


        public async Task<Boolean> CheckNuevoReclamoNoNecesario(ReclamosFilter filter)
        {
            var estadonuevo = await this.estadosService.GetByIdAsync(filter.EstadoId.Value);

            if (!estadonuevo.OrdenCambioEstado.HasValue)
            {
                return true;
            }

            var result = await this._serviceBase.GetAllAsync(e => e.InvolucradoId == filter.InvolucradoId.Value && e.IsDeleted == false && e.Estado.OrdenCambioEstado.HasValue, new List<Expression<Func<SinReclamos, object>>> { f => f.Estado });

            if (result.Items.Count > 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public async Task<ReclamosDto> Anular(ReclamosDto reclamo)
        {

            var reclamoentity = await this._serviceBase.GetByIdAsync(reclamo.Id);
            MapObject(reclamo, reclamoentity);

            var transaction = await this._serviceBase.Anular(reclamoentity);

            var result = new ReclamosDto();

            MapObject(transaction, result);

            return result;
        }

        public Task DeleteFileById(Guid id)
        {
            return this._serviceBase.DeleteFileById(id);
        }

        public override async Task DeleteAsync(int id)
        {
            var reclamosHistoricos = await _reclamosHistoricosAppService.GetItemsAsync(new ReclamosHistoricosFilter() { ReclamoId = id });
            if (reclamosHistoricos.Count > 0)
            {
                throw new System.ComponentModel.DataAnnotations.ValidationException("No se puede eliminar porque posee historial de estados");
            }
            await base.DeleteAsync(id);
        }

        public async override Task<ReclamosDto> UpdateAsync(ReclamosDto dto)
        {
            var entity = await this.GetByIdAsync(dto.Id);


            MapObject(dto, entity);
            if (dto.ReclamoCuotas != null)
            {
                foreach (var item in entity.ReclamoCuotas.Where(w => w.Id < 0).ToList())
                {
                    item.Id = 0;
                }
            }

            await this.UpdateAsync(entity);


            return MapObject<SinReclamos, ReclamosDto>(entity);
        }

        public async Task<FileDto> GetReporteExcel(ReclamosFilter filter)
        {
            var file = new FileDto();
            var excelReclamosFilter = new ExcelReclamosFilter();
            TECSO.FWK.Domain.Entities.PagedResult<ReclamosDto> reclamos = await GetDtoAllFilterAsync(filter);
            var result = reclamos.Items.Select(x => x.Id).ToList();
            excelReclamosFilter.Ids = string.Join(",", result);
            List<ReporteReclamosExcel> items = await _serviceBase.GetReporteExcel(excelReclamosFilter);
            List<ReporteReclamosExcelGrouped> reporteReclamosExcelList = new List<ReporteReclamosExcelGrouped>();
            CreationListReportExcel(items, reporteReclamosExcelList);
            ExcelParameters<ReporteReclamosExcelGrouped> parameters = new ExcelParameters<ReporteReclamosExcelGrouped>();
            parameters.HeaderText = null;
            string sheetName = DateTime.Now.ToString("yyyyMMddhhmmss");
            parameters.SheetName = string.Format("Reclamos - {0}", sheetName);
            parameters.Values = reporteReclamosExcelList;
            CreatingParameter(parameters);
            file.ByteArray = ExcelHelper.GenerateByteArray(parameters);
            file.ForceDownload = true;
            file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            file.FileName = string.Format("Reclamos - {0}.xlsx", sheetName);
            file.FileDescription = "Reporte de Reclamos";
            return file;
        }

        private void CreationListReportExcel(List<ReporteReclamosExcel> items, List<ReporteReclamosExcelGrouped> reporteReclamosExcelList)
        {
            foreach (var item in items.GroupBy(x => x.Id).Select(grp => grp.ToList().ToList()))
            {
                if (item.Count() > 1)
                {
                    Type reporteRecExcelGroupType = typeof(ReporteReclamosExcelGrouped);
                    ReporteReclamosExcelGrouped reporteRecExcelGroup = Activator.CreateInstance(reporteRecExcelGroupType) as ReporteReclamosExcelGrouped;

                    foreach (var propertyInfo in reporteRecExcelGroupType.GetProperties())
                    {
                        PropertyInfo propertyInfoSource = item[0].GetType().GetProperty(propertyInfo.Name);
                        var value = item[0].GetType().GetProperty(propertyInfo.Name).GetValue(item[0]);

                        PropertyInfo propertyInfoTarget = reporteRecExcelGroupType.GetProperty(propertyInfoSource.Name);
                        propertyInfoTarget.SetValue(reporteRecExcelGroup, value);
                    }
                    reporteReclamosExcelList.Add(reporteRecExcelGroup);

                    int next = 1;
                    while (next < item.Count())
                    {

                        Type reporteRecExcelGroupType2 = typeof(ReporteReclamosExcelGrouped);
                        ReporteReclamosExcelGrouped reporteRecExcelGroup2 = Activator.CreateInstance(reporteRecExcelGroupType2) as ReporteReclamosExcelGrouped;


                        foreach (var propertyInfo in reporteRecExcelGroupType2.GetProperties())
                        {
                            if (propertyInfo.Name == "FechaCuota" || propertyInfo.Name == "MontoCuota" || propertyInfo.Name == "ConceptoCuota")
                            {
                                PropertyInfo propertyInfoSource = item[next].GetType().GetProperty(propertyInfo.Name);
                                var value = item[next].GetType().GetProperty(propertyInfo.Name).GetValue(item[next]);
                                PropertyInfo propertyInfoTarget = reporteRecExcelGroupType2.GetProperty(propertyInfoSource.Name);
                                propertyInfoTarget.SetValue(reporteRecExcelGroup2, value);
                            }
                        }
                        reporteReclamosExcelList.Add((ReporteReclamosExcelGrouped)reporteRecExcelGroup2);
                        next++;
                    }
                }
                else
                {
                    SimpleCreationListReportExcel(item, reporteReclamosExcelList);
                }
            }
        }

        private void SimpleCreationListReportExcel(List<ReporteReclamosExcel> items, List<ReporteReclamosExcelGrouped> reporteReclamosExcelList)
        {
            foreach (var item in items)
            {

                Type reporteRecExcelGroupType = typeof(ReporteReclamosExcelGrouped);
                ReporteReclamosExcelGrouped reporteRecExcelGroup = Activator.CreateInstance(reporteRecExcelGroupType) as ReporteReclamosExcelGrouped;
                foreach (var propertyInfo in reporteRecExcelGroupType.GetProperties())
                {
                    PropertyInfo propertyInfoSource = item.GetType().GetProperty(propertyInfo.Name);
                    var value = item.GetType().GetProperty(propertyInfo.Name).GetValue(item);

                    PropertyInfo propertyInfoTarget = reporteRecExcelGroupType.GetProperty(propertyInfoSource.Name);
                    propertyInfoTarget.SetValue(reporteRecExcelGroup, value);
                }
                reporteReclamosExcelList.Add((ReporteReclamosExcelGrouped)reporteRecExcelGroup);
            }
        }


        private static void CreatingParameter(ExcelParameters<ReporteReclamosExcelGrouped> parameters)
        {
            foreach (var prop in typeof(ReporteReclamosExcelGrouped).GetProperties())
            {
                string labelText = string.Empty;
                if (prop.CustomAttributes.Count() > 0)
                {
                    labelText = prop.CustomAttributes.FirstOrDefault().NamedArguments.FirstOrDefault().TypedValue.Value.ToString();
                    parameters.AddField(prop.Name, labelText);
                }
                else
                {
                    parameters.AddField(prop.Name);
                }
            }
        }

        public async Task<List<ImportadorExcelReclamos>> UploadExcel(byte[] excelFile)
        {

            //Se recupera la primera hoja del libro de Excel
            Stream stream = new MemoryStream(excelFile);
            ExcelPackage pck = new ExcelPackage(stream);
            var workSheet = pck.Workbook.Worksheets.FirstOrDefault();

            /********************************************SE RECUPERAN LOS DATOS REQUERIDOS POR SERVICIO*******************************************************/
            PagedResult<ART.Domain.Entities.ART.TiposReclamo> pagedResultTiposReclamo = await tiposReclamoService.GetAllAsync(e => true);
            IReadOnlyList<ART.Domain.Entities.ART.TiposReclamo> tiposReclamo = pagedResultTiposReclamo.Items;

            PagedResult<SinEstados> pagedResultEstados = await estadosService.GetAllAsync(e => true);
            IReadOnlyList<SinEstados> estados = pagedResultEstados.Items;

            PagedResult<SinSubEstados> pagedResultSubEstados = await subEstadosService.GetAllAsync(e => true);
            IReadOnlyList<SinSubEstados> subEstados = pagedResultSubEstados.Items;

            PagedResult<Sucursales> pagedResultSucursales = await sucursalesService.GetAllAsync(e => true);
            IReadOnlyList<Sucursales> sucursales = pagedResultSucursales.Items;

            PagedResult<Empresa> pagedResultEmpresas = await empresaService.GetAllAsync(e => true);
            IReadOnlyList<Empresa> empresas = pagedResultEmpresas.Items;

            PagedResult<ArtDenuncias> pagedResultDenuncias = await denunciasService.GetAllAsync(e => true);
            IReadOnlyList<ArtDenuncias> denuncias = pagedResultDenuncias.Items;

            PagedResult<Operaciones.Domain.Entities.Empleados> pagedResultEmpleados = await empleadosService.GetAllAsync(e => true, new List<Expression<Func<Operaciones.Domain.Entities.Empleados, object>>> {
                e=> e.LegajosEmpleado,
                e=> e.UnidadNegocio
            });
            IReadOnlyList<Operaciones.Domain.Entities.Empleados> empleados = pagedResultEmpleados.Items;

            PagedResult<CausasReclamo> pagedResultCausasReclamos = await causasReclamoService.GetAllAsync(e => true);
            IReadOnlyList<CausasReclamo> causasReclamos = pagedResultCausasReclamos.Items;

            PagedResult<SinAbogados> pagedResultAbogados = await abogadosService.GetAllAsync(e => true);
            IReadOnlyList<SinAbogados> sinAbogados = pagedResultAbogados.Items;

            PagedResult<TiposAcuerdo> pagedResultTiposAcuerdo = await tiposAcuerdoService.GetAllAsync(e => true);
            IReadOnlyList<TiposAcuerdo> tiposAcuerdos = pagedResultTiposAcuerdo.Items;

            PagedResult<RubrosSalariales> pagedResultRubrosSalariales = await rubrosSalarialesService.GetAllAsync(e => true);
            IReadOnlyList<RubrosSalariales> rubrosSalariales = pagedResultRubrosSalariales.Items;

            PagedResult<SinJuzgados> pagedResultSinJuzgados = await juzgadosService.GetAllAsync(e => true);
            IReadOnlyList<SinJuzgados> sinJuzgados = pagedResultSinJuzgados.Items;

            /*************************************************************************************************************************************/
            PropertyInfo[] propertyInfos = typeof(ImportadorExcelReclamos).GetProperties();

            //Se crea el la lista con los datos
            ICollection<ImportadorExcelReclamos> excelFileDTO = new List<ImportadorExcelReclamos>();
            for (int row = 2; row <= workSheet.Dimension.End.Row; row++)
            {
                ImportadorExcelReclamos importadorExcelReclamos = new ImportadorExcelReclamos();

                ManagementDataValidation<string, int> dataValidatedTipoReclamo = ValidarTipoReclamo(workSheet.Cells[row, 1].Text,
                                                                                        workSheet.Cells[1, 1].Text,
                                                                                        "TipoReclamo",
                                                                                        propertyInfos,
                                                                                         tiposReclamo);
                importadorExcelReclamos.TipoReclamo = workSheet.Cells[row, 1].Text;
                importadorExcelReclamos.TiposReclamoId = dataValidatedTipoReclamo.ValueMapping.FirstOrDefault().Value;
                importadorExcelReclamos.Errors.AddRange(dataValidatedTipoReclamo.Errors);

                ManagementDataValidation<string, int> dataValidatedEstado = ValidarEstado(workSheet.Cells[row, 2].Text,
                                                                               workSheet.Cells[1, 2].Text,
                                                                               "Estado",
                                                                               propertyInfos,
                                                                                estados);
                importadorExcelReclamos.Estado = workSheet.Cells[row, 2].Text;
                importadorExcelReclamos.EstadoId = dataValidatedEstado.ValueMapping.FirstOrDefault().Value;
                importadorExcelReclamos.Errors.AddRange(dataValidatedEstado.Errors);

                ManagementDataValidation<string, int> dataValidatedSubEstado = ValidarSubEstado(workSheet.Cells[row, 3].Text,
                                                                                  workSheet.Cells[1, 3].Text,
                                                                                  "SubEstado",
                                                                                  propertyInfos,
                                                                                   subEstados);
                importadorExcelReclamos.SubEstado = workSheet.Cells[row, 3].Text;
                importadorExcelReclamos.SubEstadoId = dataValidatedSubEstado.ValueMapping.FirstOrDefault().Value;
                importadorExcelReclamos.Errors.AddRange(dataValidatedSubEstado.Errors);

                ManagementDataValidation<string, int> dataValidatedUnidadNegocio = ValidarUnidadNegocio(workSheet.Cells[row, 4].Text,
                                                                                            workSheet.Cells[1, 4].Text,
                                                                                            "UnidadNegocio",
                                                                                            propertyInfos,
                                                                                             sucursales);
                importadorExcelReclamos.UnidadNegocio = workSheet.Cells[row, 4].Text;
                importadorExcelReclamos.SucursalId = dataValidatedUnidadNegocio.ValueMapping.FirstOrDefault().Value;
                importadorExcelReclamos.Errors.AddRange(dataValidatedUnidadNegocio.Errors);

                ManagementDataValidation<string, decimal> dataValidatedEmpresa = ValidarEmpresa(workSheet.Cells[row, 5].Text,
                                                                                           workSheet.Cells[1, 5].Text,
                                                                                           "Empresa",
                                                                                           propertyInfos,
                                                                                            empresas);
                importadorExcelReclamos.Empresa = workSheet.Cells[row, 5].Text;
                importadorExcelReclamos.EmpresaId = dataValidatedEmpresa.ValueMapping.FirstOrDefault().Value;
                importadorExcelReclamos.Errors.AddRange(dataValidatedEmpresa.Errors);

                ManagementDataValidation<string, DateTime?> dataValidatedFechaReclamo = ValidateTypeDate(workSheet.Cells[row, 6].Text,
                                                                                                workSheet.Cells[1, 6].Text,
                                                                                                "FechaReclamoExcel",
                                                                                                propertyInfos);
                importadorExcelReclamos.FechaReclamoExcel = workSheet.Cells[row, 6].Text;
                importadorExcelReclamos.FechaReclamo = (dataValidatedFechaReclamo.ValueMapping.FirstOrDefault(x => x.Key == "PropDateTime").Value != null) ? dataValidatedFechaReclamo.ValueMapping.FirstOrDefault(x => x.Key == "PropDateTime").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedFechaReclamo.Errors);

                ManagementDataValidation<string, int?> dataValidatedNroDenuncia = ValidarNroDenuncia(workSheet.Cells[row, 7].Text,
                                                                                            workSheet.Cells[1, 7].Text,
                                                                                            "NroDenuncia",
                                                                                            propertyInfos,
                                                                                             denuncias);
                importadorExcelReclamos.NroDenuncia = dataValidatedNroDenuncia.Value;
                importadorExcelReclamos.DenunciaId = (dataValidatedNroDenuncia.ValueMapping.FirstOrDefault(x => x.Key == "DenunciaId").Value != null) ? dataValidatedNroDenuncia.ValueMapping.FirstOrDefault(x => x.Key == "DenunciaId").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedNroDenuncia.Errors);

                ManagementDataValidation<string, object> dataValidatedEmpleado = ValidarEmpleadoDNI_CUIL(workSheet.Cells[row, 8].Text,
                                                                                                    workSheet.Cells[row, 9].Text,
                                                                                                    workSheet.Cells[1, 8].Text,
                                                                                                    workSheet.Cells[1, 9].Text,
                                                                                                    "EmpleadoDNI",
                                                                                                    "EmpleadoCUIL",
                                                                                                    propertyInfos,
                                                                                                     empleados);
                importadorExcelReclamos.EmpleadoDNI = workSheet.Cells[row, 8].Text;
                importadorExcelReclamos.EmpleadoCUIL = workSheet.Cells[row, 9].Text;
                importadorExcelReclamos.EmpleadoId = (dataValidatedEmpleado.ValueMapping.FirstOrDefault(x => x.Key == "EmpleadoId").Value != null) ? (int)dataValidatedEmpleado.ValueMapping.FirstOrDefault(x => x.Key == "EmpleadoId").Value : (int?)null;
                importadorExcelReclamos.EmpleadoFechaIngreso = (dataValidatedEmpleado.ValueMapping.FirstOrDefault(x => x.Key == "EmpleadoFechaIngreso").Value != null) ? (DateTime)dataValidatedEmpleado.ValueMapping.FirstOrDefault(x => x.Key == "EmpleadoFechaIngreso").Value : (DateTime?)null;
                importadorExcelReclamos.EmpleadoLegajo = (dataValidatedEmpleado.ValueMapping.FirstOrDefault(x => x.Key == "EmpleadoLegajo").Value != null) ? (string)dataValidatedEmpleado.ValueMapping.FirstOrDefault(x => x.Key == "EmpleadoLegajo").Value : string.Empty;
                importadorExcelReclamos.EmpleadoEmpresaId = (dataValidatedEmpleado.ValueMapping.FirstOrDefault(x => x.Key == "EmpleadoEmpresaId").Value != null) ? (int)dataValidatedEmpleado.ValueMapping.FirstOrDefault(x => x.Key == "EmpleadoEmpresaId").Value : (decimal?)null;
                importadorExcelReclamos.EmpleadoAntiguedad = (dataValidatedEmpleado.ValueMapping.FirstOrDefault(x => x.Key == "EmpleadoAntiguedad").Value != null) ? (DateTime)dataValidatedEmpleado.ValueMapping.FirstOrDefault(x => x.Key == "EmpleadoAntiguedad").Value : (DateTime?)null;
                importadorExcelReclamos.EmpleadoArea = (dataValidatedEmpleado.ValueMapping.FirstOrDefault(x => x.Key == "EmpleadoArea").Value != null) ? (string)dataValidatedEmpleado.ValueMapping.FirstOrDefault(x => x.Key == "EmpleadoArea").Value : string.Empty;
                importadorExcelReclamos.Errors.AddRange(dataValidatedEmpleado.Errors);


                ManagementDataValidation<string, decimal?> dataValidatedMontoDemandado = ValidateTypeDecimal(workSheet.Cells[row, 10].Text,
                                                                                                workSheet.Cells[1, 10].Text,
                                                                                                "MontoDemandadoExcel",
                                                                                                propertyInfos);
                importadorExcelReclamos.MontoDemandadoExcel = workSheet.Cells[row, 10].Text;
                importadorExcelReclamos.MontoDemandado = (dataValidatedMontoDemandado.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value != null) ? dataValidatedMontoDemandado.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedMontoDemandado.Errors);


                ManagementDataValidation<string, DateTime?> dataValidatedFechaOfrecimiento = ValidateTypeDate(workSheet.Cells[row, 11].Text,
                                                                                                    workSheet.Cells[1, 11].Text,
                                                                                                    "FechaOfrecimientoExcel",
                                                                                                    propertyInfos);
                importadorExcelReclamos.FechaOfrecimientoExcel = workSheet.Cells[row, 11].Text;
                importadorExcelReclamos.FechaOfrecimiento = (dataValidatedFechaOfrecimiento.ValueMapping.FirstOrDefault(x => x.Key == "PropDateTime").Value != null) ? dataValidatedFechaOfrecimiento.ValueMapping.FirstOrDefault(x => x.Key == "PropDateTime").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedFechaOfrecimiento.Errors);

                ManagementDataValidation<string, decimal?> dataValidatedMontoOfrecido = ValidateTypeDecimal(workSheet.Cells[row, 12].Text,
                                                                                                workSheet.Cells[1, 12].Text,
                                                                                                "MontoOfrecidoExcel",
                                                                                                propertyInfos);
                importadorExcelReclamos.MontoOfrecidoExcel = workSheet.Cells[row, 12].Text;
                importadorExcelReclamos.MontoOfrecido = (dataValidatedMontoOfrecido.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value != null) ? dataValidatedMontoOfrecido.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedMontoOfrecido.Errors);


                ManagementDataValidation<string, decimal?> dataValidatedMontoReconsideracion = ValidateTypeDecimal(workSheet.Cells[row, 13].Text,
                                                                                                        workSheet.Cells[1, 13].Text,
                                                                                                        "MontoReconsideracionExcel",
                                                                                                        propertyInfos);
                importadorExcelReclamos.MontoReconsideracionExcel = workSheet.Cells[row, 13].Text;
                importadorExcelReclamos.MontoReconsideracion = (dataValidatedMontoReconsideracion.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value != null) ? dataValidatedMontoReconsideracion.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedMontoReconsideracion.Errors);

                ManagementDataValidation<string, int?> dataValidatedCausaReclamo = ValidarCausaReclamo(workSheet.Cells[row, 14].Text,
                                                                                                workSheet.Cells[1, 14].Text,
                                                                                                "CausaReclamo",
                                                                                                propertyInfos,
                                                                                                 causasReclamos);
                importadorExcelReclamos.CausaReclamo = workSheet.Cells[row, 14].Text;
                importadorExcelReclamos.CausaId = (dataValidatedCausaReclamo.ValueMapping.FirstOrDefault(x => x.Key == "CausaId").Value != null) ? dataValidatedCausaReclamo.ValueMapping.FirstOrDefault(x => x.Key == "CausaId").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedCausaReclamo.Errors);

                ManagementDataValidation<string, string> dataValidatedHechos = ValidateTypeString(workSheet.Cells[row, 15].Text,
                                                                                        workSheet.Cells[1, 15].Text,
                                                                                        "Hechos",
                                                                                        propertyInfos);
                importadorExcelReclamos.Hechos = dataValidatedHechos.Value;
                importadorExcelReclamos.Errors.AddRange(dataValidatedHechos.Errors);

                ManagementDataValidation<string, DateTime?> dataValidatedFechaPago = ValidateTypeDate(workSheet.Cells[row, 16].Text,
                                                                                        workSheet.Cells[1, 16].Text,
                                                                                        "FechaPagoExcel",
                                                                                        propertyInfos);
                importadorExcelReclamos.FechaPagoExcel = workSheet.Cells[row, 16].Text;
                importadorExcelReclamos.FechaPago = (dataValidatedFechaPago.ValueMapping.FirstOrDefault(x => x.Key == "PropDateTime").Value != null) ? dataValidatedFechaPago.ValueMapping.FirstOrDefault(x => x.Key == "PropDateTime").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedFechaPago.Errors);

                ManagementDataValidation<string, decimal?> dataValidatedMontoPagado = ValidateTypeDecimal(workSheet.Cells[row, 17].Text,
                                                                                                workSheet.Cells[1, 17].Text,
                                                                                                "MontoPagadoExcel",
                                                                                                propertyInfos);
                importadorExcelReclamos.MontoPagadoExcel = workSheet.Cells[row, 17].Text;
                importadorExcelReclamos.MontoPagado = (dataValidatedMontoPagado.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value != null) ? dataValidatedMontoPagado.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedMontoPagado.Errors);


                ManagementDataValidation<string, decimal?> dataValidatedMontoFranquicia = ValidateTypeDecimal(workSheet.Cells[row, 18].Text,
                                                                                                workSheet.Cells[1, 18].Text,
                                                                                                "MontoFranquiciaExcel",
                                                                                                propertyInfos);
                importadorExcelReclamos.MontoFranquiciaExcel = workSheet.Cells[row, 18].Text;
                importadorExcelReclamos.MontoFranquicia = (dataValidatedMontoFranquicia.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value != null) ? dataValidatedMontoFranquicia.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedMontoFranquicia.Errors);


                ManagementDataValidation<string, int?> dataValidatedAbogado = ValidarAbogado(workSheet.Cells[row, 19].Text,
                                                                                    workSheet.Cells[1, 19].Text,
                                                                                    "Abogado",
                                                                                    propertyInfos,
                                                                                     sinAbogados);
                importadorExcelReclamos.Abogado = workSheet.Cells[row, 19].Text;
                importadorExcelReclamos.AbogadoId = (dataValidatedAbogado.ValueMapping.FirstOrDefault(x => x.Key == "AbogadoId").Value != null) ? dataValidatedAbogado.ValueMapping.FirstOrDefault(x => x.Key == "AbogadoId").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedAbogado.Errors);


                ManagementDataValidation<string, decimal?> dataValidatedHonorariosAbogadoActor = ValidateTypeDecimal(workSheet.Cells[row, 20].Text,
                                                                                                        workSheet.Cells[1, 20].Text,
                                                                                                        "HonorariosAbogadoActorExcel",
                                                                                                        propertyInfos);
                importadorExcelReclamos.HonorariosAbogadoActorExcel = workSheet.Cells[row, 20].Text;
                importadorExcelReclamos.HonorariosAbogadoActor = (dataValidatedHonorariosAbogadoActor.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value != null) ? dataValidatedHonorariosAbogadoActor.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedHonorariosAbogadoActor.Errors);

                ManagementDataValidation<string, decimal?> dataValidatedHonorariosMediador = ValidateTypeDecimal(workSheet.Cells[row, 21].Text,
                                                                                                        workSheet.Cells[1, 21].Text,
                                                                                                        "HonorariosMediadorExcel",
                                                                                                        propertyInfos);
                importadorExcelReclamos.HonorariosMediadorExcel = workSheet.Cells[row, 21].Text;
                importadorExcelReclamos.HonorariosMediador = (dataValidatedHonorariosMediador.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value != null) ? dataValidatedHonorariosMediador.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedHonorariosMediador.Errors);


                ManagementDataValidation<string, decimal?> dataValidatedHonorariosPerito = ValidateTypeDecimal(workSheet.Cells[row, 22].Text,
                                                                                                    workSheet.Cells[1, 22].Text,
                                                                                                    "HonorariosPeritoExcel",
                                                                                                    propertyInfos);
                importadorExcelReclamos.HonorariosPeritoExcel = workSheet.Cells[row, 22].Text;
                importadorExcelReclamos.HonorariosPerito = (dataValidatedHonorariosPerito.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value != null) ? dataValidatedHonorariosPerito.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedHonorariosPerito.Errors);

                ManagementDataValidation<string, bool?> dataValidatedJuntaMedica = ValidateTypeBoolean(workSheet.Cells[row, 23].Text,
                                                                                    workSheet.Cells[1, 23].Text,
                                                                                    "JuntaMedica",
                                                                                    propertyInfos);
                importadorExcelReclamos.JuntaMedicaExcel = workSheet.Cells[row, 23].Text;
                importadorExcelReclamos.JuntaMedica = (dataValidatedJuntaMedica.ValueMapping.FirstOrDefault(x => x.Key == "Boolean").Value != null) ? dataValidatedJuntaMedica.ValueMapping.FirstOrDefault(x => x.Key == "Boolean").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedJuntaMedica.Errors);


                ManagementDataValidation<string, decimal?> dataValidatedPorcIncapacidad = ValidateTypeDecimal(workSheet.Cells[row, 24].Text,
                                                                                    workSheet.Cells[1, 24].Text,
                                                                                    "PorcIncapacidad",
                                                                                    propertyInfos);
                importadorExcelReclamos.PorcIncapacidad = workSheet.Cells[row, 24].Text;
                importadorExcelReclamos.PorcentajeIncapacidad = dataValidatedPorcIncapacidad.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value;
                importadorExcelReclamos.Errors.AddRange(dataValidatedPorcIncapacidad.Errors);


                ManagementDataValidation<string, int?> dataValidatedTipoAcuerdo = ValidarTipoAcuerdo(workSheet.Cells[row, 25].Text,
                                                                                    workSheet.Cells[1, 25].Text,
                                                                                    "TipoAcuerdo",
                                                                                    propertyInfos,
                                                                                     tiposAcuerdos);
                importadorExcelReclamos.TipoAcuerdo = workSheet.Cells[row, 25].Text;
                importadorExcelReclamos.TipoAcuerdoId = (dataValidatedTipoAcuerdo.ValueMapping.FirstOrDefault(x => x.Key == "TipoAcuerdoId").Value != null) ? dataValidatedTipoAcuerdo.ValueMapping.FirstOrDefault(x => x.Key == "TipoAcuerdoId").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedTipoAcuerdo.Errors);

                ManagementDataValidation<string, int?> dataValidatedRubroSalarial = ValidarRubroSalarial(workSheet.Cells[row, 26].Text,
                                                                                    workSheet.Cells[1, 26].Text,
                                                                                    "RubroSalarial",
                                                                                    propertyInfos,
                                                                                     rubrosSalariales);
                importadorExcelReclamos.RubroSalarial = workSheet.Cells[row, 26].Text;
                importadorExcelReclamos.RubroSalarialId = (dataValidatedRubroSalarial.ValueMapping.FirstOrDefault(x => x.Key == "RubroSalarialId").Value != null) ? dataValidatedRubroSalarial.ValueMapping.FirstOrDefault(x => x.Key == "RubroSalarialId").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedRubroSalarial.Errors);

                ManagementDataValidation<string, decimal?> dataValidatedMontoTasasJudiciales = ValidateTypeDecimal(workSheet.Cells[row, 27].Text,
                                                                                    workSheet.Cells[1, 27].Text,
                                                                                    "MontoTasasJudicialesExcel",
                                                                                    propertyInfos);
                importadorExcelReclamos.MontoTasasJudicialesExcel = workSheet.Cells[row, 27].Text;
                importadorExcelReclamos.MontoTasasJudiciales = (dataValidatedMontoTasasJudiciales.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value != null) ? dataValidatedMontoTasasJudiciales.ValueMapping.FirstOrDefault(x => x.Key == "Decimal").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedMontoTasasJudiciales.Errors);

                ManagementDataValidation<string, string> dataValidatedObservaciones = ValidateTypeString(workSheet.Cells[row, 28].Text,
                                                                                    workSheet.Cells[1, 28].Text,
                                                                                    "Observaciones",
                                                                                    propertyInfos);
                importadorExcelReclamos.Observaciones = dataValidatedObservaciones.Value;
                importadorExcelReclamos.Errors.AddRange(dataValidatedObservaciones.Errors);

                ManagementDataValidation<string, string> dataValidatedObsConvExtrajudicial = ValidateTypeString(workSheet.Cells[row, 29].Text,
                                                                                    workSheet.Cells[1, 29].Text,
                                                                                    "ObsConvExtrajudicial",
                                                                                    propertyInfos);
                importadorExcelReclamos.ObsConvExtrajudicial = dataValidatedObsConvExtrajudicial.Value;
                importadorExcelReclamos.Errors.AddRange(dataValidatedObsConvExtrajudicial.Errors);

                ManagementDataValidation<string, string> dataValidatedAutos = ValidateTypeString(workSheet.Cells[row, 30].Text,
                                                                                    workSheet.Cells[1, 30].Text,
                                                                                    "Autos",
                                                                                    propertyInfos);
                importadorExcelReclamos.Autos = dataValidatedAutos.Value;
                importadorExcelReclamos.Errors.AddRange(dataValidatedAutos.Errors);

                ManagementDataValidation<string, string> dataValidatedNroExpediente = ValidateTypeString(workSheet.Cells[row, 31].Text,
                                                                                    workSheet.Cells[1, 31].Text,
                                                                                    "NroExpediente",
                                                                                    propertyInfos);
                importadorExcelReclamos.NroExpediente = dataValidatedNroExpediente.Value;
                importadorExcelReclamos.Errors.AddRange(dataValidatedNroExpediente.Errors);


                ManagementDataValidation<string, int?> dataValidatedJuzgado = ValidarJuzgado(workSheet.Cells[row, 32].Text,
                                                                                    workSheet.Cells[1, 32].Text,
                                                                                    "Juzgado",
                                                                                    propertyInfos,
                                                                                     sinJuzgados);
                importadorExcelReclamos.Juzgado = workSheet.Cells[row, 32].Text;
                importadorExcelReclamos.JuzgadoId = (dataValidatedJuzgado.ValueMapping.FirstOrDefault(x => x.Key == "JuzgadoId").Value != null) ? dataValidatedJuzgado.ValueMapping.FirstOrDefault(x => x.Key == "JuzgadoId").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedJuzgado.Errors);


                ManagementDataValidation<string, int?> dataValidatedAbogadoEmpresa = ValidarAbogado(workSheet.Cells[row, 33].Text,
                                                                                    workSheet.Cells[1, 33].Text,
                                                                                    "AbogadoEmpresa",
                                                                                    propertyInfos,
                                                                                     sinAbogados);
                importadorExcelReclamos.AbogadoEmpresa = workSheet.Cells[row, 33].Text;
                importadorExcelReclamos.AbogadoEmpresaId = (dataValidatedAbogadoEmpresa.ValueMapping.FirstOrDefault(x => x.Key == "AbogadoId").Value != null) ? dataValidatedAbogadoEmpresa.ValueMapping.FirstOrDefault(x => x.Key == "AbogadoId").Value : null;
                importadorExcelReclamos.Errors.AddRange(dataValidatedAbogadoEmpresa.Errors);


                importadorExcelReclamos.IsValid = (importadorExcelReclamos.Errors.Count() > 0) ? false : true;
                excelFileDTO.Add(importadorExcelReclamos);
            }
            return (List<ImportadorExcelReclamos>)excelFileDTO;
        }

        public ManagementDataValidation<string, int> ValidarTipoReclamo(string value, string column, string propertyName, PropertyInfo[] propertyInfos, IReadOnlyList<ART.Domain.Entities.ART.TiposReclamo> tiposReclamo)
        {
            ManagementDataValidation<string, int> dataValidated = new ManagementDataValidation<string, int>();

            if (value == "Siniestros")
            {
                dataValidated.IsValid = false;
                dataValidated.Errors.Add("No se permite la importación de registros para los tipos de reclamos Siniestros");
                return dataValidated;
            }

            var requiredAttribute = propertyInfos.Where(x => x.Name == propertyName).FirstOrDefault().GetCustomAttributes(false).Where(x => x.GetType() == typeof(RequiredAttribute));
            if (requiredAttribute.Count() == 0)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    GetValueMappingTipoReclamo(value, column, tiposReclamo, dataValidated);
                }
                return dataValidated;
            }

            if (requiredAttribute.Count() == 1 && !string.IsNullOrWhiteSpace(value))
            {
                GetValueMappingTipoReclamo(value, column, tiposReclamo, dataValidated);
            }
            else
            {
                dataValidated.Errors.Add(string.Format(((ValidationAttribute)requiredAttribute.FirstOrDefault()).ErrorMessage, column));
            }
            return dataValidated;
        }

        private static void GetValueMappingTipoReclamo(string value, string column, IReadOnlyList<ART.Domain.Entities.ART.TiposReclamo> tiposReclamo, ManagementDataValidation<string, int> dataValidated)
        {
            if (tiposReclamo.FirstOrDefault(x => x.Descripcion == value.Trim()) != null)
            {
                dataValidated.ValueMapping.Add("TiposReclamoId", tiposReclamo.FirstOrDefault(x => x.Descripcion == value).Id);
            }
            else
            {
                dataValidated.Errors.Add($"El dato {value} no existe");
            }
        }

        public ManagementDataValidation<string, int> ValidarEstado(string value, string column, string propertyName, PropertyInfo[] propertyInfos, IReadOnlyList<SinEstados> estados)
        {
            ManagementDataValidation<string, int> dataValidated = new ManagementDataValidation<string, int>();

            var requiredAttribute = propertyInfos.Where(x => x.Name == propertyName).FirstOrDefault().GetCustomAttributes(false).Where(x => x.GetType() == typeof(RequiredAttribute));
            if (requiredAttribute.Count() == 0)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    GetValueMappingEstado(value, column, estados, dataValidated);
                }
                return dataValidated;
            }

            if (requiredAttribute.Count() == 1 && !string.IsNullOrWhiteSpace(value))
            {
                GetValueMappingEstado(value, column, estados, dataValidated);
            }
            else
            {
                dataValidated.Errors.Add(string.Format(((ValidationAttribute)requiredAttribute.FirstOrDefault()).ErrorMessage, column));
            }
            return dataValidated;
        }

        private static void GetValueMappingEstado(string value, string column, IReadOnlyList<SinEstados> estados, ManagementDataValidation<string, int> dataValidated)
        {
            if (estados.FirstOrDefault(x => x.Descripcion.Trim() == value.Trim()) != null)
            {
                dataValidated.ValueMapping.Add("EstadoId", estados.FirstOrDefault(x => x.Descripcion.Trim() == value.Trim()).Id);
            }
            else
            {
                dataValidated.Errors.Add($"El dato {value} no existe");
            }
        }

        public ManagementDataValidation<string, int> ValidarSubEstado(string value, string column, string propertyName, PropertyInfo[] propertyInfos, IReadOnlyList<SinSubEstados> subEstados)
        {
            ManagementDataValidation<string, int> dataValidated = new ManagementDataValidation<string, int>();

            var requiredAttribute = propertyInfos.Where(x => x.Name == propertyName).FirstOrDefault().GetCustomAttributes(false).Where(x => x.GetType() == typeof(RequiredAttribute));
            if (requiredAttribute.Count() == 0)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    GetValueMappingSubEstado(value, column, subEstados, dataValidated);
                }
                return dataValidated;
            }

            if (requiredAttribute.Count() == 1 && !string.IsNullOrWhiteSpace(value))
            {
                GetValueMappingSubEstado(value, column, subEstados, dataValidated);
            }
            else
            {
                dataValidated.Errors.Add(string.Format(((ValidationAttribute)requiredAttribute.FirstOrDefault()).ErrorMessage, column));
            }
            return dataValidated;
        }

        private static void GetValueMappingSubEstado(string value, string column, IReadOnlyList<SinSubEstados> subEstados, ManagementDataValidation<string, int> dataValidated)
        {
            if (subEstados.FirstOrDefault(x => x.Descripcion.Trim() == value.Trim()) != null)
            {
                dataValidated.ValueMapping.Add("SubEstadoId", subEstados.FirstOrDefault(x => x.Descripcion.Trim() == value.Trim()).Id);
            }
            else
            {
                dataValidated.Errors.Add($"El dato {value} no existe");
            }
        }

        public ManagementDataValidation<string, int> ValidarUnidadNegocio(string value, string column, string propertyName, PropertyInfo[] propertyInfos, IReadOnlyList<Sucursales> sucursales)
        {
            ManagementDataValidation<string, int> dataValidated = new ManagementDataValidation<string, int>();

            var requiredAttribute = propertyInfos.Where(x => x.Name == propertyName).FirstOrDefault().GetCustomAttributes(false).Where(x => x.GetType() == typeof(RequiredAttribute));
            if (requiredAttribute.Count() == 0)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    GetValueMappingUnidadNegocio(value, column, sucursales, dataValidated);
                }
                return dataValidated;
            }

            if (requiredAttribute.Count() == 1 && !string.IsNullOrWhiteSpace(value))
            {
                GetValueMappingUnidadNegocio(value, column, sucursales, dataValidated);
            }
            else
            {
                dataValidated.Errors.Add(string.Format(((ValidationAttribute)requiredAttribute.FirstOrDefault()).ErrorMessage, column));
            }
            return dataValidated;
        }

        private static void GetValueMappingUnidadNegocio(string value, string column, IReadOnlyList<Sucursales> sucursales, ManagementDataValidation<string, int> dataValidated)
        {
            if (sucursales.FirstOrDefault(x => x.DscSucursal.Trim() == value.Trim()) != null)
            {
                dataValidated.ValueMapping.Add("SucursalId", sucursales.FirstOrDefault(x => x.DscSucursal.Trim() == value.Trim()).Id);
            }
            else
            {
                dataValidated.Errors.Add($"El dato {value} no existe");
            }
        }

        public ManagementDataValidation<string, decimal> ValidarEmpresa(string value, string column, string propertyName, PropertyInfo[] propertyInfos, IReadOnlyList<Empresa> empresas)
        {
            ManagementDataValidation<string, decimal> dataValidated = new ManagementDataValidation<string, decimal>();
            var requiredAttribute = propertyInfos.Where(x => x.Name == propertyName).FirstOrDefault().GetCustomAttributes(false).Where(x => x.GetType() == typeof(RequiredAttribute));
            if (requiredAttribute.Count() == 0)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    GetValueMappingEmpresa(value, column, empresas, dataValidated);
                }
                return dataValidated;
            }

            if (requiredAttribute.Count() == 1 && !string.IsNullOrWhiteSpace(value))
            {
                GetValueMappingEmpresa(value, column, empresas, dataValidated);
            }
            else
            {
                dataValidated.Errors.Add(string.Format(((ValidationAttribute)requiredAttribute.FirstOrDefault()).ErrorMessage, column));
            }
            return dataValidated;
        }

        private static void GetValueMappingEmpresa(string value, string column, IReadOnlyList<Empresa> empresas, ManagementDataValidation<string, decimal> dataValidated)
        {
            if (empresas.FirstOrDefault(x => x.DesEmpr.Trim() == value.Trim()) != null)
            {
                dataValidated.ValueMapping.Add("EmpresaId", empresas.FirstOrDefault(x => x.DesEmpr.Trim() == value.Trim()).Id);
            }
            else
            {
                dataValidated.Errors.Add($"El dato {value} no existe");
            }
        }

        public ManagementDataValidation<string, int?> ValidarNroDenuncia(string value, string column, string propertyName, PropertyInfo[] propertyInfos, IReadOnlyList<ArtDenuncias> denuncias)
        {
            ManagementDataValidation<string, int?> dataValidated = new ManagementDataValidation<string, int?>();
            var requiredAttribute = propertyInfos.Where(x => x.Name == propertyName).FirstOrDefault().GetCustomAttributes(false).Where(x => x.GetType() == typeof(RequiredAttribute));
            if (requiredAttribute.Count() == 0)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    GetValueMappingDenuncias(value, column, denuncias, dataValidated);
                }
                return dataValidated;
            }

            if (requiredAttribute.Count() == 1 && !string.IsNullOrWhiteSpace(value))
            {
                GetValueMappingDenuncias(value, column, denuncias, dataValidated);
            }
            else
            {
                dataValidated.Errors.Add(string.Format(((ValidationAttribute)requiredAttribute.FirstOrDefault()).ErrorMessage, column));
            }
            return dataValidated;
        }

        private static void GetValueMappingDenuncias(string value, string column, IReadOnlyList<ArtDenuncias> denuncias, ManagementDataValidation<string, int?> dataValidated)
        {
            if (denuncias.FirstOrDefault(x => x.NroDenuncia.Trim() == value.Trim()) != null)
            {
                dataValidated.ValueMapping.Add("DenunciaId", denuncias.FirstOrDefault(x => x.NroDenuncia.Trim() == value.Trim()).Id);
            }
            else
            {
                dataValidated.Errors.Add($"El dato {value} no existe");
            }
        }


        public ManagementDataValidation<string, object> ValidarEmpleadoDNI_CUIL(string valueDNI, string valueCUIL, string columnDNI, string columnCUIL, string propertyNamaDni, string propertyNameCuil, PropertyInfo[] propertyInfos, IReadOnlyList<Operaciones.Domain.Entities.Empleados> empleados)
        {
            ManagementDataValidation<string, object> dataValidated = new ManagementDataValidation<string, object>();

            if (string.IsNullOrWhiteSpace(valueDNI) && string.IsNullOrWhiteSpace(valueCUIL))
            {
                dataValidated.IsValid = false;
                dataValidated.Errors.Add("Falta informar DNI / CUIL empleado");
                return dataValidated;
            }

            //Se valida por DNI, si no se ha diligenciado el CUIL.
            if (!string.IsNullOrEmpty(valueDNI) && string.IsNullOrWhiteSpace(valueCUIL))
            {
                ValidateEmpleadoByDNI(dataValidated, empleados, valueDNI);
                if (!dataValidated.IsValid)
                {
                    dataValidated.Errors.Add("Empleado no encontrado");
                }
                return dataValidated;
            }

            //Se valida por CUIL, si no se ha diligenciado el DNI.
            if (!string.IsNullOrEmpty(valueCUIL) && string.IsNullOrWhiteSpace(valueDNI))
            {
                IsEmpleadoCUILNumeric(dataValidated, valueCUIL);
                if (dataValidated.IsValid)
                {
                    ValidateEmpleadoByCUIL(dataValidated, empleados, valueCUIL);
                    if (!dataValidated.IsValid)
                    {
                        dataValidated.Errors.Add("Empleado no encontrado");
                    }
                }
                return dataValidated;
            }

            //Se valida por DNI y luego por CUIL si ambas han sido diligenciado.
            if (!string.IsNullOrEmpty(valueCUIL) && !string.IsNullOrWhiteSpace(valueDNI))
            {
                ValidateEmpleadoByDNI(dataValidated, empleados, valueDNI);
                if (!dataValidated.IsValid)
                {
                    IsEmpleadoCUILNumeric(dataValidated, valueCUIL);
                    if (dataValidated.IsValid)
                    {
                        ValidateEmpleadoByCUIL(dataValidated, empleados, valueCUIL);
                        if (!dataValidated.IsValid)
                        {
                            dataValidated.Errors.Add("Empleado no encontrado");
                        }
                    }
                }
            }
            return dataValidated;
        }

        private void IsEmpleadoCUILNumeric(ManagementDataValidation<string, object> dataValidated, string value)
        {
            if (value.All(char.IsDigit))
            {
                dataValidated.IsValid = true;
            }
            else
            {
                dataValidated.IsValid = false;
                dataValidated.Errors.Add("El CUIL tiene que contener solo números");
            }
        }
        private void ValidateEmpleadoByDNI(ManagementDataValidation<string, object> dataValidated, IReadOnlyList<Operaciones.Domain.Entities.Empleados> empleados, string value)
        {
            if (empleados.Where(e => e.Dni.Trim() == value.Trim()).FirstOrDefault() != null)
            {
                var empleado = empleados.Where(e => e.Dni.Trim() == value.Trim()).FirstOrDefault();
                FillDataEmpleado(empleado, dataValidated);
                dataValidated.IsValid = true;
            }
            else
            {
                dataValidated.IsValid = false;
            }
        }

        private void ValidateEmpleadoByCUIL(ManagementDataValidation<string, object> dataValidated, IReadOnlyList<Operaciones.Domain.Entities.Empleados> empleados, string value)
        {
            if (empleados.Where(e => e.Cuil.Trim() == value.Trim()).FirstOrDefault() != null)
            {
                var empleado = empleados.Where(e => e.Cuil.Trim() == value.Trim()).FirstOrDefault();
                FillDataEmpleado(empleado, dataValidated);
                dataValidated.IsValid = true;
            }
            else
            {
                dataValidated.IsValid = false;
            }
        }

        private void FillDataEmpleado(Operaciones.Domain.Entities.Empleados empleado, ManagementDataValidation<string, object> dataValidated)
        {
            dataValidated.ValueMapping.Add("EmpleadoId", empleado.Id);
            dataValidated.ValueMapping.Add("EmpleadoAntiguedad", empleado.FecAntiguedad);
            dataValidated.ValueMapping.Add("EmpleadoArea", empleado.Area);
            if (empleado.LegajosEmpleado.Count > 0)
            {
                if (empleado.LegajosEmpleado.Where(e => e.FecBaja == null && e.Id == empleado.Id).Count() > 0)
                {
                    var legempleado = empleado.LegajosEmpleado.Where(e => e.FecBaja == null && e.Id == empleado.Id).FirstOrDefault();
                    dataValidated.ValueMapping.Add("EmpleadoEmpresaId", legempleado.CodEmpresa);
                    dataValidated.ValueMapping.Add("EmpleadoFechaIngreso", legempleado.FecIngreso);
                    dataValidated.ValueMapping.Add("EmpleadoLegajo", legempleado.LegajoSap);
                }
                else if (empleado.LegajosEmpleado.Where(e => e.Id == empleado.Id).Count() > 0)
                {
                    var legempleado = empleado.LegajosEmpleado.Where(e => e.Id == empleado.Id).OrderByDescending(e => e.FecIngreso).FirstOrDefault();
                    dataValidated.ValueMapping.Add("EmpleadoEmpresaId", legempleado.CodEmpresa);
                    dataValidated.ValueMapping.Add("EmpleadoFechaIngreso", legempleado.FecIngreso);
                    dataValidated.ValueMapping.Add("EmpleadoLegajo", legempleado.LegajoSap);
                }
            }
        }


        public ManagementDataValidation<string, DateTime?> ValidateTypeDate(string value, string column, string propertyName, PropertyInfo[] propertyInfos)
        {
            ManagementDataValidation<string, DateTime?> dataValidated = new ManagementDataValidation<string, DateTime?>();
            var requiredAttribute = propertyInfos.Where(x => x.Name == propertyName).FirstOrDefault().GetCustomAttributes(false).Where(x => x.GetType() == typeof(RequiredAttribute));
            if (requiredAttribute.Count() == 0)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    GetValueMappingTypeDate(value, column, dataValidated);
                }
                return dataValidated;
            }

            if (requiredAttribute.Count() == 1 && !string.IsNullOrWhiteSpace(value))
            {
                GetValueMappingTypeDate(value, column, dataValidated);
            }
            else
            {
                dataValidated.Errors.Add(string.Format(((ValidationAttribute)requiredAttribute.FirstOrDefault()).ErrorMessage, column));
            }
            return dataValidated;
        }

        private static void GetValueMappingTypeDate(string value, string column, ManagementDataValidation<string, DateTime?> dataValidated)
        {
            if (DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.NoCurrentDateDefault, out DateTime result))
            {
                dataValidated.ValueMapping.Add("PropDateTime", result);
            }
            else
            {
                dataValidated.Errors.Add($"La columna {column} no tiene el formato correcto de fecha");
            }
        }

        public ManagementDataValidation<string, string> ValidateTypeString(string value, string column, string propertyName, PropertyInfo[] propertyInfos)
        {
            ManagementDataValidation<string, string> dataValidated = new ManagementDataValidation<string, string>();
            var requiredAttribute = propertyInfos.Where(x => x.Name == propertyName).FirstOrDefault().GetCustomAttributes(false).Where(x => x.GetType() == typeof(RequiredAttribute));
            if (requiredAttribute.Count() == 0)
            {
                dataValidated.Value = value;
                return dataValidated;
            }

            if (requiredAttribute.Count() == 1 && !string.IsNullOrWhiteSpace(value))
            {
                dataValidated.Value = value;
            }
            else
            {
                dataValidated.IsValid = false;
                dataValidated.Errors.Add(string.Format(((ValidationAttribute)requiredAttribute.FirstOrDefault()).ErrorMessage, column));
            }
            return dataValidated;

        }

        public ManagementDataValidation<string, decimal?> ValidateTypeDecimal(string value, string column, string propertyName, PropertyInfo[] propertyInfos)
        {
            ManagementDataValidation<string, decimal?> dataValidated = new ManagementDataValidation<string, decimal?>();
            var requiredAttribute = propertyInfos.Where(x => x.Name == propertyName).FirstOrDefault().GetCustomAttributes(false).Where(x => x.GetType() == typeof(RequiredAttribute));
            if (requiredAttribute.Count() == 0)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    GetValueMappingTypeDecimal(value, column, dataValidated);
                }
                return dataValidated;
            }

            if (requiredAttribute.Count() == 1 && !string.IsNullOrWhiteSpace(value))
            {
                GetValueMappingTypeDecimal(value, column, dataValidated);
            }
            else
            {
                dataValidated.Errors.Add(string.Format(((ValidationAttribute)requiredAttribute.FirstOrDefault()).ErrorMessage, column));
            }
            return dataValidated;
        }

        private static void GetValueMappingTypeDecimal(string value, string column, ManagementDataValidation<string, decimal?> dataValidated)
        {
            if (decimal.TryParse(value, out decimal result))
            {
                dataValidated.ValueMapping.Add("Decimal", result);
            }
            else
            {
                dataValidated.Errors.Add($"La columna {column} no tiene el formato correcto de numero");
            }
        }

        public ManagementDataValidation<string, int?> ValidarCausaReclamo(string value, string column, string propertyName, PropertyInfo[] propertyInfos, IReadOnlyList<CausasReclamo> causasReclamos)
        {
            ManagementDataValidation<string, int?> dataValidated = new ManagementDataValidation<string, int?>();

            var requiredAttribute = propertyInfos.Where(x => x.Name == propertyName).FirstOrDefault().GetCustomAttributes(false).Where(x => x.GetType() == typeof(RequiredAttribute));
            if (requiredAttribute.Count() == 0)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    GetValueMappingCausaReclamo(value, column, causasReclamos, dataValidated);
                }
                return dataValidated;
            }

            if (requiredAttribute.Count() == 1 && !string.IsNullOrWhiteSpace(value))
            {
                GetValueMappingCausaReclamo(value, column, causasReclamos, dataValidated);
            }
            else
            {
                dataValidated.Errors.Add(string.Format(((ValidationAttribute)requiredAttribute.FirstOrDefault()).ErrorMessage, column));
            }
            return dataValidated;
        }

        private static void GetValueMappingCausaReclamo(string value, string column, IReadOnlyList<CausasReclamo> causasReclamos, ManagementDataValidation<string, int?> dataValidated)
        {
            if (causasReclamos.FirstOrDefault(x => x.Descripcion.Trim() == value.Trim()) != null)
            {
                dataValidated.ValueMapping.Add("CausaId", causasReclamos.FirstOrDefault(x => x.Descripcion.Trim() == value.Trim()).Id);
            }
            else
            {
                dataValidated.Errors.Add($"El dato {value} no existe");
            }
        }

        public ManagementDataValidation<string, int?> ValidarAbogado(string value, string column, string propertyName, PropertyInfo[] propertyInfos, IReadOnlyList<SinAbogados> sinAbogados)
        {
            ManagementDataValidation<string, int?> dataValidated = new ManagementDataValidation<string, int?>();

            var requiredAttribute = propertyInfos.Where(x => x.Name == propertyName).FirstOrDefault().GetCustomAttributes(false).Where(x => x.GetType() == typeof(RequiredAttribute));
            if (requiredAttribute.Count() == 0)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    GetValueMappingAbogado(value, column, sinAbogados, dataValidated);
                }
                return dataValidated;
            }

            if (requiredAttribute.Count() == 1 && !string.IsNullOrWhiteSpace(value))
            {
                GetValueMappingAbogado(value, column, sinAbogados, dataValidated);
            }
            else
            {
                dataValidated.Errors.Add(string.Format(((ValidationAttribute)requiredAttribute.FirstOrDefault()).ErrorMessage, column));
            }
            return dataValidated;
        }

        private static void GetValueMappingAbogado(string value, string column, IReadOnlyList<SinAbogados> sinAbogados, ManagementDataValidation<string, int?> dataValidated)
        {
            if (sinAbogados.FirstOrDefault(x => x.ApellidoNombre.Trim() == value.Trim()) != null)
            {
                dataValidated.ValueMapping.Add("AbogadoId", sinAbogados.FirstOrDefault(x => x.ApellidoNombre.Trim() == value.Trim()).Id);
            }
            else
            {
                dataValidated.Errors.Add($"El dato {value} no existe");
            }
        }

        public ManagementDataValidation<string, bool?> ValidateTypeBoolean(string value, string column, string propertyName, PropertyInfo[] propertyInfos)
        {
            List<string> options = new List<string> { "s", "si", "n", "no" };
            ManagementDataValidation<string, bool?> dataValidated = new ManagementDataValidation<string, bool?>();
            var requiredAttribute = propertyInfos.Where(x => x.Name == propertyName).FirstOrDefault().GetCustomAttributes(false).Where(x => x.GetType() == typeof(RequiredAttribute));
            if (requiredAttribute.Count() == 0)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    GetValueMappingTypeBoolean(value, column, options, dataValidated);
                }
                return dataValidated;
            }

            if (requiredAttribute.Count() == 1 && !string.IsNullOrWhiteSpace(value))
            {
                GetValueMappingTypeBoolean(value, column, options, dataValidated);
            }
            else
            {
                dataValidated.Errors.Add(string.Format(((ValidationAttribute)requiredAttribute.FirstOrDefault()).ErrorMessage, column));
            }
            return dataValidated;
        }

        private static void GetValueMappingTypeBoolean(string value, string column, List<string> options, ManagementDataValidation<string, bool?> dataValidated)
        {
            if (options.Exists(x => x == value.ToLower()))
            {
                var yes = options.Where(x => x == "s" || x == "si");
                var no = options.Where(x => x == "n" || x == "no");
                if (yes.Contains(value.ToLower()))
                {
                    dataValidated.ValueMapping.Add("Boolean", true);
                }

                if (no.Contains(value.ToLower()))
                {
                    dataValidated.ValueMapping.Add("Boolean", false);
                }
            }
            else
            {
                dataValidated.Errors.Add($"Los valores permitidos en la columna {column} son (s, n, si, no, S, N, SI, NO)");
            }
        }

        public ManagementDataValidation<string, int?> ValidarTipoAcuerdo(string value, string column, string propertyName, PropertyInfo[] propertyInfos, IReadOnlyList<TiposAcuerdo> tiposAcuerdos)
        {
            ManagementDataValidation<string, int?> dataValidated = new ManagementDataValidation<string, int?>();
            var requiredAttribute = propertyInfos.Where(x => x.Name == propertyName).FirstOrDefault().GetCustomAttributes(false).Where(x => x.GetType() == typeof(RequiredAttribute));
            if (requiredAttribute.Count() == 0)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    GetValueMappingTipoAcuerdo(value, column, tiposAcuerdos, dataValidated);
                }
                return dataValidated;
            }

            if (requiredAttribute.Count() == 1 && !string.IsNullOrWhiteSpace(value))
            {
                GetValueMappingTipoAcuerdo(value, column, tiposAcuerdos, dataValidated);
            }
            else
            {
                dataValidated.Errors.Add(string.Format(((ValidationAttribute)requiredAttribute.FirstOrDefault()).ErrorMessage, column));
            }
            return dataValidated;
        }

        private static void GetValueMappingTipoAcuerdo(string value, string column, IReadOnlyList<TiposAcuerdo> tiposAcuerdos, ManagementDataValidation<string, int?> dataValidated)
        {
            if (tiposAcuerdos.FirstOrDefault(x => x.Descripcion.Trim() == value.Trim()) != null)
            {
                dataValidated.ValueMapping.Add("TipoAcuerdoId", tiposAcuerdos.FirstOrDefault(x => x.Descripcion.Trim() == value.Trim()).Id);
            }
            else
            {
                dataValidated.Errors.Add($"El dato {value} no existe");
            }
        }

        public ManagementDataValidation<string, int?> ValidarRubroSalarial(string value, string column, string propertyName, PropertyInfo[] propertyInfos, IReadOnlyList<RubrosSalariales> rubrosSalariales)
        {
            ManagementDataValidation<string, int?> dataValidated = new ManagementDataValidation<string, int?>();
            var requiredAttribute = propertyInfos.Where(x => x.Name == propertyName).FirstOrDefault().GetCustomAttributes(false).Where(x => x.GetType() == typeof(RequiredAttribute));
            if (requiredAttribute.Count() == 0)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    GetValueMappingRubroSalarial(value, column, rubrosSalariales, dataValidated);
                }
                return dataValidated;
            }

            if (requiredAttribute.Count() == 1 && !string.IsNullOrWhiteSpace(value))
            {
                GetValueMappingRubroSalarial(value, column, rubrosSalariales, dataValidated);
            }
            else
            {
                dataValidated.Errors.Add(string.Format(((ValidationAttribute)requiredAttribute.FirstOrDefault()).ErrorMessage, column));
            }
            return dataValidated;
        }

        private static void GetValueMappingRubroSalarial(string value, string column, IReadOnlyList<RubrosSalariales> rubrosSalariales, ManagementDataValidation<string, int?> dataValidated)
        {
            if (rubrosSalariales.FirstOrDefault(x => x.Descripcion.Trim() == value.Trim()) != null)
            {
                dataValidated.ValueMapping.Add("RubroSalarialId", rubrosSalariales.FirstOrDefault(x => x.Descripcion.Trim() == value.Trim()).Id);
            }
            else
            {
                dataValidated.Errors.Add($"El dato {value} no existe");
            }
        }

        public ManagementDataValidation<string, int?> ValidarJuzgado(string value, string column, string propertyName, PropertyInfo[] propertyInfos, IReadOnlyList<SinJuzgados> sinJuzgados)
        {
            ManagementDataValidation<string, int?> dataValidated = new ManagementDataValidation<string, int?>();
            var requiredAttribute = propertyInfos.Where(x => x.Name == propertyName).FirstOrDefault().GetCustomAttributes(false).Where(x => x.GetType() == typeof(RequiredAttribute));
            if (requiredAttribute.Count() == 0)
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    GetValueMappingJuzgado(value, column, sinJuzgados, dataValidated);
                }
                return dataValidated;
            }

            if (requiredAttribute.Count() == 1 && !string.IsNullOrWhiteSpace(value))
            {
                GetValueMappingJuzgado(value, column, sinJuzgados, dataValidated);
            }
            else
            {
                dataValidated.Errors.Add(string.Format(((ValidationAttribute)requiredAttribute.FirstOrDefault()).ErrorMessage, column));
            }
            return dataValidated;
        }

        private static void GetValueMappingJuzgado(string value, string column, IReadOnlyList<SinJuzgados> sinJuzgados, ManagementDataValidation<string, int?> dataValidated)
        {
            if (sinJuzgados.FirstOrDefault(x => x.Descripcion.Trim() == value.Trim()) != null)
            {
                dataValidated.ValueMapping.Add("JuzgadoId", sinJuzgados.FirstOrDefault(x => x.Descripcion.Trim() == value.Trim()).Id);
            }
            else
            {
                dataValidated.Errors.Add($"El dato {value} no existe");
            }
        }

        public async Task<List<ImportadorExcelReclamos>> RecuperarPlanilla(ReclamoImportadorFileFilter filter)
        {
            return await this._serviceBase.RecuperarPlanilla(filter);
        }

        public async Task ImportarReclamos(ReclamoImportadorFileFilter input)
        {
            await this._serviceBase.ImportarReclamos(input);
        }

    }
    //Clase Helper para mapear los valores que se requieren para SinReclamos vs lo que viene desde Excel
    public class ManagementDataValidation<TValue, TValueMapping>
    {
        public ManagementDataValidation()
        {
            IsValid = true;
            Errors = new List<string>();
            ValueMapping = new Dictionary<TValue, TValueMapping>();
        }
        //Dato obtenido desde Excel
        public TValue Value { get; set; }
        //Clave foranea equivalente en la tabla sin_reclamos
        public Dictionary<TValue, TValueMapping> ValueMapping { get; set; }
        public List<string> Errors { get; set; }
        public bool IsValid { get; set; }
    }
}
