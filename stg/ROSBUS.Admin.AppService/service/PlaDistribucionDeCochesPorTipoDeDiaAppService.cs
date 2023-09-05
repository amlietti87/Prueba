using Microsoft.AspNetCore.Http;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Caching;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class PlaDistribucionDeCochesPorTipoDeDiaAppService : AppServiceBase<PlaDistribucionDeCochesPorTipoDeDia, PlaDistribucionDeCochesPorTipoDeDiaDto, int, IPlaDistribucionDeCochesPorTipoDeDiaService>, IPlaDistribucionDeCochesPorTipoDeDiaAppService
    {
        private readonly IPlaTalleresIvuService _plaTalleresIvuService;
        private readonly ISubGalponService _subGalponService;
        public PlaDistribucionDeCochesPorTipoDeDiaAppService(IPlaDistribucionDeCochesPorTipoDeDiaService serviceBase
            , IBanderaService banderaService
            , IPlaTalleresIvuService plaTalleresIvuService
            , ISubGalponService subGalponService)
            : base(serviceBase)
        {

            _banderaService = banderaService;
            _plaTalleresIvuService = plaTalleresIvuService;
            _subGalponService = subGalponService;
        }
        private readonly IBanderaService _banderaService;
        public async Task<PlaDistribucionEstadoView> ExistenMediasVueltasIncompletas(PlaDistribucionDeCochesPorTipoDeDia input)
        {
            return await this._serviceBase.ExistenMediasVueltasIncompletas(input);
        }

        public async Task<Boolean> ValidateArchivos(IFormFileCollection files)
        {
            return true;
        }

        public async Task<List<ImportarHorariosDto>> Horarios(IFormFileCollection files)
        {
            List<ImportarHorariosDto> horarios = new List<ImportarHorariosDto>();

            string ViajesPath = IVUViajes.tableName + ".X10";
            string ViajesPorVehiculoPath = IVUViajesPorVehiculo.tableName + ".X10";
            string UbicacionesPath = IVUUbicaciones.tableName + ".X10";
            string BanderasPath = IVUBanderas.tableName + ".X10";
            string TurnosPath = IVUTurnos.tableName + ".X10";
            string ServiciosPath = IVUServicios.tableName + ".X10";
            string TiposDeDiaPath = IVUTiposDeDia.tableName + ".X10";
            string MinutosPorSectorPath = IVUMinutosPorSector.tableName + ".X10";
            string SectoresPorMediasVuetlasPath = IVUSectoresPorMediaVueltas.tableName + ".X10";
            string MetrosPorSectorPath = IVUMetrosPorSector.tableName + ".X10";

            DataSet ds = new DataSet();

            ds.Tables.Add(getTable(await this.ReadAsListAsync(files.Where(e => e.FileName == ViajesPath).FirstOrDefault())));
            ds.Tables.Add(getTable(await this.ReadAsListAsync(files.Where(e => e.FileName == ViajesPorVehiculoPath).FirstOrDefault())));
            ds.Tables.Add(getTable(await this.ReadAsListAsync(files.Where(e => e.FileName == UbicacionesPath).FirstOrDefault())));
            ds.Tables.Add(getTable(await this.ReadAsListAsync(files.Where(e => e.FileName == BanderasPath).FirstOrDefault())));
            ds.Tables.Add(getTable(await this.ReadAsListAsync(files.Where(e => e.FileName == TurnosPath).FirstOrDefault())));
            ds.Tables.Add(getTable(await this.ReadAsListAsync(files.Where(e => e.FileName == ServiciosPath).FirstOrDefault())));
            ds.Tables.Add(getTable(await this.ReadAsListAsync(files.Where(e => e.FileName == TiposDeDiaPath).FirstOrDefault())));
            ds.Tables.Add(getTable(await this.ReadAsListAsync(files.Where(e => e.FileName == MinutosPorSectorPath).FirstOrDefault())));
            ds.Tables.Add(getTable(await this.ReadAsListAsync(files.Where(e => e.FileName == SectoresPorMediasVuetlasPath).FirstOrDefault())));
            ds.Tables.Add(getTable(await this.ReadAsListAsync(files.Where(e => e.FileName == MetrosPorSectorPath).FirstOrDefault())));

            var banderasrepo = await _banderaService.GetAllBanderasWithRamal();
            var subgalpones = await _subGalponService.GetAllWithConfigu();
            var correspondencias = (await _plaTalleresIvuService.GetAllAsync(new PlaTalleresIvuFilter().GetFilterExpression())).Items;

            var query = (from row in ds.Tables[IVUViajes.tableName].Rows.Cast<DataRow>()

                         join servicios in ds.Tables[IVUServicios.tableName].Rows.Cast<DataRow>() on
                         new { Turno = row[IVUViajes.TurnoIdPropertyName], TipoDeDia = row[IVUViajes.TipoDeDiaIdPropertyName] } equals
                         new { Turno = servicios[IVUServicios.TurnoIdPropertyName], TipoDeDia = servicios[IVUServicios.TipoDeDiaIdPropertyName] } into sif
                         from servicios in sif.DefaultIfEmpty()
                             //from servicios in ds.Tables[IVUServicios.tableName].Rows.Cast<DataRow>().Where(serv => serv[IVUServicios.TurnoIdPropertyName] == row[IVUViajes.TurnoIdPropertyName] && serv[IVUServicios.TipoDeDiaIdPropertyName] == row[IVUViajes.TipoDeDiaIdPropertyName]).DefaultIfEmpty()
                             // let servicio = (from s in ds.Tables[IVUServicios.tableName].Rows.Cast<DataRow>() where s[IVUServicios.TurnoIdPropertyName] == row[IVUViajes.TurnoIdPropertyName] && s[IVUServicios.TipoDeDiaIdPropertyName] == row[IVUViajes.TipoDeDiaIdPropertyName]  select s[IVUServicios.ServicioIdPropertyName]).FirstOrDefault()
                         //join banderas in ds.Tables[IVUBanderas.tableName].Rows.Cast<DataRow>() on row[IVUViajes.BanderaIdPropertyName] equals banderas[IVUBanderas.BanderaIdPropertyName] into bif
                         //from banderas in bif.DefaultIfEmpty()

                         join viajesporvehiculo in ds.Tables[IVUViajesPorVehiculo.tableName].Rows.Cast<DataRow>() on row[IVUViajes.ViajeIdPropertyName] equals viajesporvehiculo[IVUViajesPorVehiculo.ViajeIdPropertyName] into vif
                         from viajesporvehiculo in vif.DefaultIfEmpty()

                         orderby Convert.ToInt32(servicios[IVUServicios.ServicioIdPropertyName]), Convert.ToInt32(row[IVUViajes.TurnoIdPropertyName])
                         where (Convert.ToInt32(row[IVUViajes.SalePropertyName]) != Convert.ToInt32(row[IVUViajes.LlegaPropertyName]))
                         select new
                         {
                             Servicio = servicios[IVUServicios.ServicioIdPropertyName].ToString(),
                             Sale = row[IVUViajes.SalePropertyName].ToString(),
                             Llega = row[IVUViajes.LlegaPropertyName].ToString(),
                             BanderaId = row[IVUViajes.BanderaIdPropertyName].ToString(),
                             TipoDeHora = viajesporvehiculo[IVUViajesPorVehiculo.TipoDeHoraIdPropertyName].ToString(),
                             UbicacionInicioId = Convert.ToInt32(row[IVUViajes.UbicacionInicioIdPropertyName].ToString())
                         }

                          ).Distinct().ToList();

            var result = new List<ImportarHorariosDto>();

            foreach (var item in query)
            {
                var mv = new ImportarHorariosDto();

                mv.Servicio = item.Servicio;
                mv.Sale = TimeSpan.FromSeconds(Convert.ToDouble(item.Sale)).ToString(@"hh\:mm");
                mv.Llega = TimeSpan.FromSeconds(Convert.ToDouble(item.Llega)).ToString(@"hh\:mm");

                Int32 banderaid = 0;

                if (int.TryParse(item.BanderaId.Replace("\"","").Trim(), out banderaid))
                {
                    var ban = banderasrepo.Where(e => e.Id == banderaid).FirstOrDefault();
                    mv.Bandera = ban?.AbrBan;
                    mv.dsc_TpoHora = item.TipoDeHora;
                    mv.des_subg = GetDescSubG(banderasrepo, subgalpones, correspondencias, banderaid, item.UbicacionInicioId);

                }
                else
                {

                }
                result.Add(mv);

            }




            return result.ToList();
        }

        public string GetDescSubG(List<HBanderas> banderas, List<SubGalpon> subgalpones, IReadOnlyList<PlaTalleresIvu> correspondencias, int IdBandera, int Ubicacion)
        {
            var bandera = banderas.Where(g => g.Id == Convert.ToInt32(IdBandera)).FirstOrDefault();
            var galpon = correspondencias.Where(h => h.CodGalIvu == Ubicacion).FirstOrDefault();
            if (bandera != null && galpon != null)
            {
                if (bandera.RamalColor != null)
                {
                    var subgalpon = subgalpones.Where(e => e.Configu.Any(f => f.CodLin == bandera.RamalColor.LineaId && f.CodGal == galpon.CodGal)).FirstOrDefault();
                    if (subgalpon != null)
                    {
                        return subgalpon.DesSubg;
                    }
                }
            }
            return null;

        }

        public async Task<SubGalpon> GetSubGalpon(int CodBan, int CodUbicacion)
        {
            SubGalpon subGalpon = new SubGalpon();
            var galpon = await _plaTalleresIvuService.GetGalponWithIvu(CodUbicacion);
            var linea = await _banderaService.GetLinea(CodBan);
            if (galpon != null && linea != null)
            {
                subGalpon = await _subGalponService.GetSubGalponWithLineaAndGalpon(galpon.Id, linea.Id);
                return subGalpon;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<string>> ReadAsListAsync(IFormFile file)
        {
            var result = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    result.Add(await reader.ReadLineAsync());
            }
            return result;
        }

        private static DataTable getTable(List<string> lines)
        {


            var info = getInfoLine(lines[7]);

            DataTable dataTable = new DataTable(info);

            var attributes = getInfoLine(lines[8]);
            var attributeTypes = getInfoLine(lines[9]);


            for (int i = 0; i < attributes.Split(';').Length; i++)
            {
                var attr = attributes.Split(';')[i];
                var attrType = attributeTypes.Split(';')[i];

                dataTable.Columns.Add(attr, getTypeByAttr(attrType));
            }

            var registros = lines.Where(e => e.StartsWith("rec"));
            foreach (var item in registros)
            {
                var infoLine = getInfoLine(item);

                var row = dataTable.LoadDataRow(infoLine.Split(';').Select(e => e.Trim()).ToArray(), true);
            }


            return dataTable;
        }

        private static Type getTypeByAttr(string attrType)
        {
            return typeof(string);
        }

        private static string getInfoLine(string line)
        {
            var lines = line.Split(';').ToList();
            lines.Remove(lines[0]);

            return string.Join(";", lines.Select(e => e.Trim()));
        }

        public async Task ImportarServicios(ImportarServiciosInput input)
        {
            await this._serviceBase.ImportarServicios(input);
        }

        public async Task RecrearSabanaSector(PlaDistribucionDeCochesPorTipoDeDia input)
        {
            await this._serviceBase.RecrearSabanaSector(input);
        }

        public async Task<List<HMediasVueltasImportadaDto>> RecuperarPlanilla(PlaDistribucionDeCochesPorTipoDeDiaFilter filter)
        {
            return await this._serviceBase.RecuperarPlanilla(filter);
        }

        public async Task<Boolean> TieneMinutosAsignados(PlaDistribucionDeCochesPorTipoDeDiaFilter filter)
        {
            return await this._serviceBase.TieneMinutosAsignados(filter);
        }
    }
}
