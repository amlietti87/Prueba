using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Report;
using ROSBUS.Admin.Domain.Report.Report.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class HMediasVueltasAppService : AppServiceBase<HMediasVueltas, HMediasVueltasDto, int, IHMediasVueltasService>, IHMediasVueltasAppService
    {
        private readonly ILineaService _lineaService;
        private readonly ITiposDeDiasService _tiposDeDias;
        private readonly ISubGalponService _subGalpon;
        private readonly IEmpresaService _empresa;
        private readonly IHFechasConfiService _hFechas;
        private readonly IHMediasVueltasService _mediasVueltas;
        public HMediasVueltasAppService(IHMediasVueltasService serviceBase, ILineaService lineaService, ITiposDeDiasService tiposDeDias, ISubGalponService subGalpon, IEmpresaService empresa, IHFechasConfiService hFechas, IHMediasVueltasService hMediasVueltas)
            : base(serviceBase)
        {
            this._lineaService = lineaService;
            this._tiposDeDias = tiposDeDias;
            this._subGalpon = subGalpon;
            this._empresa = empresa;
            this._hFechas = hFechas;
            this._mediasVueltas = hMediasVueltas;
        }

        public async Task<List<HMediasVueltasView>> LeerMediasVueltasIncompletas(HMediasVueltasFilter Filtro)
        {
            return await this._serviceBase.LeerMediasVueltasIncompletas(Filtro);
        }

        public async Task<ReportModel> GetDatosReportePuntaPunta(HMediasVueltasFilter filtro)
        {

            var lineaentity = await this._lineaService.GetByIdAsync(filtro.CodLinea.Value);
            if (lineaentity == null)
            {
                throw new DomainValidationException("No encuentra linea");
            }
            var tipodiaentity = await this._tiposDeDias.GetByIdAsync(filtro.CodTdia.Value);
            if (tipodiaentity == null)
            {
                throw new DomainValidationException("No encuentra tipo de día");
            }
            var subgalponentity = await this._subGalpon.GetByIdAsync(filtro.CodSubg.Value);
            if (subgalponentity == null)
            {
                throw new DomainValidationException("No encuentra Sub-Galpon");
            }

            var empresaentity = await this._mediasVueltas.GetCodigoEmpresa(lineaentity.Id);

            var fechahorario = (await _hFechas.GetByIdAsync(filtro.CodHfecha.Value)).FecDesde;
            string FechaEmision = DateTime.Now.Date.ToString("dd/MM/yyyy");

            List<HMediasVueltas> horarioslist = new List<HMediasVueltas>();

            foreach (var item in filtro.Servicios)
            {
                var mediavueltaxservicio = (await GetAllAsync(e => e.CodServicio == item.Id)).Items.OrderBy(e=> e.Sale);
                horarioslist.AddRange(mediavueltaxservicio);
            }

            List<PuntaPuntaReportModel> list = new List<PuntaPuntaReportModel>();
            foreach (var item in horarioslist)
            {

                PuntaPuntaReportModel row = new PuntaPuntaReportModel();
                row.Empresa = empresaentity.DesEmpr;
                row.Bandera = item.CodBanNavigation.AbrBan;
                row.FechaEmision = FechaEmision;
                row.FechaHorario = fechahorario.ToString("dd/MM/yyyy");
                row.Linea = lineaentity.DesLin;
                row.Llega = item.Llega.ToString("HH:mm");
                row.Minutos = item.DifMin.ToString();
                row.Sale = item.Sale.ToString("HH:mm");
                row.Servicio = filtro.Servicios.Find(e => e.Id == item.CodServicio).NumSer;
                row.SubGalpon = subgalponentity.DesSubg;
                row.TipoDia = tipodiaentity.DesTdia;
                list.Add(row);

            }


            //ROW COLOR 1 = ÚLTIMA ROW. ROW COLOR 2 = ESPACIOS EN BLANCO SIN BORDES
            foreach (var grupos in list.GroupBy(e => e.SubGalpon))
            {
                int rowservicio = 1;
                foreach (var porservicio in grupos.GroupBy(e => e.Servicio))
                {
                    porservicio.LastOrDefault().RowColor = 1;
                    foreach (var item in porservicio)
                    {
                        item.NumeroColumna = rowservicio;
                    }
                    rowservicio++;
                    if (rowservicio == 5)
                    {
                        rowservicio = 1;
                    }

                }

            }
            var listclone = list;
            //foreach (var grupos in list.GroupBy(e => e.SubGalpon))
            //{
            //    int numerocolumna = 1;
            //    int maxrows = 0;
            //    var agrupadosporservicio = grupos.GroupBy(e => e.Servicio);
            //    var conteo = new List<KeyValuePair<int, int>>();
            //    foreach (var porservicio in agrupadosporservicio)
            //    {
            //        if (numerocolumna == 5 || porservicio == agrupadosporservicio.LastOrDefault())
            //        {
            //            foreach (var item in conteo)
            //            {
            //                var diferencia = maxrows - item.Value;
            //                for (int i = 0; i < diferencia; i++)
            //                {
            //                    listclone.Add(CreateRowVacia(list.FirstOrDefault(), porservicio.Key, item.Key));
            //                }
            //            }
            //            numerocolumna = 1;
            //            maxrows = 0;
            //            conteo = new List<KeyValuePair<int, int>>();
            //        }
            //        if (maxrows <= porservicio.Count())
            //        {
            //            maxrows = porservicio.Count();
            //        }

            //        conteo.Add(new KeyValuePair<int, int>(numerocolumna, porservicio.Count()));
            //        numerocolumna++;
            //    }
            //}

            ReportModel rp = new ReportModel();
            rp.ReportName = ReportName.PuntaPuntaReportNamespace;
            rp.AddDataSources("Horarios", listclone);
            return rp;
        }

        private PuntaPuntaReportModel CreateRowVacia(PuntaPuntaReportModel predecesor, string Servicio, int NumeroColumna)
        {
            PuntaPuntaReportModel nuevo = new PuntaPuntaReportModel();
            nuevo.Bandera = null;
            nuevo.Empresa = predecesor.Empresa;
            nuevo.FechaEmision = predecesor.FechaEmision;
            nuevo.FechaHorario = predecesor.FechaHorario;
            nuevo.Linea = predecesor.Linea;
            nuevo.Llega = null;
            nuevo.Minutos = null;
            nuevo.NumeroColumna = NumeroColumna;
            nuevo.RowColor = 2;
            nuevo.Sale = null;
            nuevo.Servicio = Servicio;
            nuevo.SubGalpon = predecesor.SubGalpon;
            nuevo.TipoDia = predecesor.TipoDia;
            return nuevo;
        }
    }
}
