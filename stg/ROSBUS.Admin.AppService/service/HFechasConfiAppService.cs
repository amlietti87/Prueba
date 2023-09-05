using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Admin.Domain.Model.Reportes;
using ROSBUS.Admin.Domain.Report;
using ROSBUS.Admin.Domain.Report.Report.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.DocumentsHelper.Excel;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class HFechasConfiAppService : AppServiceBase<HFechasConfi, HFechasConfiDto, int, IHFechasConfiService>, IHFechasConfiAppService
    {

        private readonly ILineaService lineaService;
        private readonly IPlaDistribucionDeCochesPorTipoDeDiaService disService;
        private readonly IDefaultEmailer emailSender;
        private readonly IAuthService authservice;
        private readonly IUserService user;
        
        public HFechasConfiAppService(IHFechasConfiService serviceBase, IUserService _user, ILineaService _lineaService, IPlaDistribucionDeCochesPorTipoDeDiaService _disService, IDefaultEmailer _emailSender, IAuthService _authservice)
            : base(serviceBase)
        {
            this.lineaService = _lineaService;
            this.disService = _disService;
            this.emailSender = _emailSender;
            this.authservice = _authservice;
            this.user = _user;


        }



        public async override Task<HFechasConfiDto> UpdateAsync(HFechasConfiDto dto)
        {
            foreach (var hbasec in dto.HBasec)
            {
                if (hbasec.Kmr == null)
                {
                    hbasec.Kmr = 0;
                }
                if (hbasec.Km == null)
                {
                    hbasec.Km = 0;
                }
            }
            ToDateHFechasConfiDto(dto);

            var entity = await this.GetByIdAsync(dto.Id);
            var estadoOriginal = entity.PlaEstadoHorarioFechaId;


            //retorna de aprobado a borrador
            if (estadoOriginal == PlaEstadoHorarioFecha.Aprobado
                //&& dto.PlaEstadoHorarioFechaId == PlaEstadoHorarioFecha.Borrador
                )
            {

                var horarioDiagramado = await this._serviceBase.HorarioDiagramado(dto.Id, null, null);

                if (horarioDiagramado)
                {
                    throw new ValidationException("El horario ya fue diagramado");
                }

            }



            MapObject(dto, entity);

            var PasoAprobado = false;
            //pasa de borrador a aprobado
            if (entity.PlaEstadoHorarioFechaId == PlaEstadoHorarioFecha.Aprobado && estadoOriginal == PlaEstadoHorarioFecha.Borrador)
            {

                PasoAprobado = true;

                if (!entity.BeforeMigration.HasValue || entity.BeforeMigration.Value == true)
                {
                    throw new ValidationException("No se puede aprobar el horario porque proviene del viejo sistema");
                }

                //foreach (var hbasec in entity.HBasec)
                //{

                //    if (hbasec.CodRecNavigation.Puntos.Any(e => e.PlaCoordenada.Anulado == true))
                //    {
                //        throw new ValidationException("No se puede aprobar el horario cuando uno de los mapas asociados posee una coordenada anulada");
                //    }

                //}


                foreach (var item in entity.PlaDistribucionDeCochesPorTipoDeDia)
                {
                    var existenMediasVueltasIncompletas = await this.disService.ExistenMediasVueltasIncompletas(item);

                    if (existenMediasVueltasIncompletas.Estado != PlaDistribucionEstadoEnum.Valido)
                    {
                        var tddto = dto.DistribucionDeCochesPorTipoDeDia.FirstOrDefault(e => e.Id == item.Id);
                        throw new ValidationException(string.Format("Existen Minutos Incompletos para el tipo de dia {0}", tddto?.TipoDediaDescripcion));
                    }

                    var existenDuracionesIncompletas = await this.disService.ExistenDuracionesIncompletas(item);

                    if (existenDuracionesIncompletas.Estado == PlaDuracionesEstadoEnum.Incompleta)
                    {
                        var tddto = dto.DistribucionDeCochesPorTipoDeDia.FirstOrDefault(e => e.Id == item.Id);
                        throw new ValidationException(string.Format("Existen servicios con duraciones incompletas para el tipo de dia {0}", tddto?.TipoDediaDescripcion));
                    }
                }

                //
            }
            entity.Activo = "S";

            await this.UpdateAsync(entity);

            if (PasoAprobado)
            {
                //TODO:  mandar mail 811
                SysUsersAd user = this.user.GetById(this.authservice.GetCurretUserId());
                var lin = await lineaService.GetByIdAsync(entity.CodLin);
                string deslin = lin?.DesLin;
                string fechadesde = entity.FecDesde.ToShortDateString();
                string fechahasta = string.Empty;
                if (entity.FecHasta.HasValue)
                {
                    fechahasta = entity.FecHasta.Value.ToShortDateString();
                }

                var destinatarios = await this.ObtenerDestinatarios(entity.CodLin);

                foreach (var mail in destinatarios)
                {
                    await this.emailSender.SendDefaultAsync(mail, "Linea Horaria Aprobada", string.Format("La Linea Horaria Nro: {0} se encuentra aprobada <br /> Fecha Desde: {1} <br /> Fecha Hasta: {2} <br />", deslin, fechadesde, fechahasta));
                }

            }

            return MapObject<HFechasConfi, HFechasConfiDto>(entity);
        }

        public override Task<HFechasConfiDto> AddAsync(HFechasConfiDto dto)
        {
            ToDateHFechasConfiDto(dto);

            return base.AddAsync(dto);
        }

        private static void ToDateHFechasConfiDto(HFechasConfiDto dto)
        {
            dto.FechaDesde = dto.FechaDesde.Date;
            if (dto.FechaHasta.HasValue)
            {
                dto.FechaHasta = dto.FechaHasta.Value.Date;
            }
        }

        public async Task<List<string>> ObtenerDestinatarios(decimal CodLin)
        {
            return await this._serviceBase.ObtenerDestinatarios(CodLin);
        }

        public async Task<List<PlaHorarioFechaLineaListView>> GetLineasHorarias()
        {
            return await this._serviceBase.GetLineasHorarias();
        }


        public override async Task<PagedResult<HFechasConfiDto>> GetDtoAllAsync(Expression<Func<HFechasConfi, bool>> predicate, List<Expression<Func<HFechasConfi, object>>> includeExpression = null)
        {
            var p = await base.GetDtoAllAsync(predicate, includeExpression);

            return new PagedResult<HFechasConfiDto>(p.TotalCount, p.Items.OrderByDescending(e => e.FechaDesde).ToList());
        }

        public async Task<ItemDto> CopiarHorario(int cod_hfecha, DateTime fec_desde, bool CopyConductores)
        {
            return await this._serviceBase.CopiarHorario(cod_hfecha, fec_desde, CopyConductores);

        }


        public async override Task<HFechasConfiDto> GetDtoByIdAsync(int id)
        {
            var dto = await base.GetDtoByIdAsync(id);
            dto.HBasec = dto.HBasec.OrderBy(e => e.DescripcionAbreviatura).ToList();
            List<HKilometros> kmlist = await this._serviceBase.GetKilometrosAsync(
                dto.HBasec.Select(e => e.CodBan).Distinct().ToList(),
                dto.HBasec.Select(e => e.CodSec).Distinct().ToList());

            foreach (var hBasec in dto.HBasec)
            {
                var km = kmlist.FirstOrDefault(e => e.CodBan == hBasec.CodBan && e.CodSec == hBasec.CodSec);
                if (km != null)
                { 
                    hBasec.Kmr = km.Kmr;
                    hBasec.Km = km.Km;
                    hBasec.CodBanderaColor = km.CodBanderaColor;
                    hBasec.CodBanderaTup = km.CodBanderaTup;
                }
            }

            return dto;
        }

        public async Task<bool> HorarioDiagramado(int CodHfecha, int? CodTdia, List<int> CodServicio)
        {
            return await this._serviceBase.HorarioDiagramado(CodHfecha, CodTdia, CodServicio);
        }

        public async Task<int> RemapearRecoridoBandera(HFechasConfiFilter filter)
        {
            return await this._serviceBase.RemapearRecoridoBandera(filter);
        }

        public override async Task DeleteAsync(int id)
        {
            //return base.DeleteAsync(id);

            if (await this._serviceBase.HorarioDiagramado(id, null, null))
            {
                throw new ValidationException("No se permite Eliminar la Linea Horario .");
            }

            await base.DeleteAsync(id);

        }

        public async Task<FileDto> GenerarExcelHorarios(ExportarExcelFilter filter)
        {
            var file = new FileDto();
            //var puntos = this._serviceBase.GetAll(filter.GetFilterExpression());
            List<ReporteHorarioExcelModel> items = await this._serviceBase.GenerarExcelHorarios(filter);
            ExcelParameters<ReporteHorarioExcelModel> parametros = new ExcelParameters<ReporteHorarioExcelModel>();

            parametros.HeaderText = null;
            parametros.SheetName = "H - " + items.FirstOrDefault().Linea?.TrimEnd() ;
            parametros.Values = items.OrderBy(e => e.CodTipoDia).ThenBy(e => e.Servicio).ThenBy(e => e.Sale).ToList();

            if (filter.CodTdia.HasValue)
            {
                parametros.SheetName += " - " + items.FirstOrDefault().TipoDia?.TrimEnd();
                //parametros.AddField("TipoDia", "Tipo de dia");
            }
            else
            {
                parametros.AddField("TipoDia", "Tipo de dia");
            }
            parametros.SheetName += " - " + items.FirstOrDefault().FechaHorario.ToString("yyyy.MM.dd");



            parametros.AddField("ServicioFormat", "Servicio");
            parametros.AddField("SaleFormat", "Sale MV", formatCell: "[$-F400]h:mm:ss\\ AM/PM");
            parametros.AddField("LlegaFormat", "Llega MV",  formatCell: "[$-F400]h:mm:ss\\ AM/PM");
            parametros.AddField("Bandera", "Bandera");
            parametros.AddField("TipoDeHora", "Tipo de Hora");
            parametros.AddField("Duracion", "Duración");
            parametros.AddField("SubGalpon", "Sub Galpon");

            file.ByteArray = ExcelHelper.GenerateByteArray(parametros);
            file.ForceDownload = true;
            file.FileType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            file.FileName = parametros.SheetName + ".xlsx";
            file.FileDescription = "Reporte de Horarios";

            return file;
        }


        public async Task<ReportModel> GetDatosReporteRelevos(ExportarExcelFilter filtro)
        {
            List<RelevoModel> datos = await this._serviceBase.GetDatosReporteRelevos(filtro);
            if (!datos.Any())
            {
                throw new ValidationException("No se encontraron registros de duraciones para el Galpon y tipo de dia seleccionados.");
            }

            foreach (var item in datos)
            {
                if (!item.SaleTitular.HasValue && item.SaleRelevo.HasValue && item.SaleAux.HasValue)
                {
                    item.LlegaRelevo = null;
                    item.SaleRelevo = null;
                    item.Bandera = "";
                    item.DescripcionSector = "";
                    item.Horario = null;
                }
            }
            List<RelevoReportModel> reportModel = datos.Select(e => new RelevoReportModel()
            {
                Bandera = e.Bandera,
                DescripcionSector = e.DescripcionSector,
                Empresa = e.Empresa,
                FechaHorario = e.FechaHorario,
                Horario = e.Horario,
                Linea = e.Linea,
                LlegaAux = e.LlegaAux,
                LlegaRelevo = e.LlegaRelevo,
                
                num_ser = e.num_ser,
                SaleAux = e.SaleAux,
                SaleRelevo = e.SaleRelevo,
                
                SubGalpon = e.SubGalpon,
                TipoDia = e.TipoDia,
                BanderaAux=e.BanderaAux,
                DescripcionSectorAux=e.DescripcionSectorAux,
                HorarioAux=e.HorarioAux,

            }).ToList();

            ReportModel rp = new ReportModel();
            rp.ReportName = ReportName.RelevoReportNamespace;
            rp.AddDataSources("DataSetRelevos", reportModel);
            return rp;
        }

        public async Task<string> GetTitulo (ExportarExcelFilter filtro)
        {
            string titulo = await this._serviceBase.GetTitulo(filtro);
            return titulo;
        }

        public async Task<HBasecDto> UpdateHBasec(HBasecDto hbasec)
        {
            var rta = await this._serviceBase.UpdateHBasec(hbasec.CodBan, hbasec.CodHfecha, hbasec.CodRec);
            var hbasecnew = MapObject<HBasec, HBasecDto>(rta);
            return hbasecnew;
        }

        public async Task GuardarBanderaPorSector(HFechasConfiDto data)
        {
            var entity = AutoMapper.Mapper.Map<HFechasConfi>(data);
            await this._serviceBase.GuardarBanderaPorSector(entity);
        }
    }
}
