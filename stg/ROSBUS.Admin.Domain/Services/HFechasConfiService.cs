using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Admin.Domain.Model.Reportes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class HFechasConfiService : ServiceBase<HFechasConfi,int, IHFechasConfiRepository>, IHFechasConfiService
    { 
        public HFechasConfiService(IHFechasConfiRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public Task<ItemDto> CopiarHorario(int cod_hfecha, DateTime fec_desde, bool CopyConductores)
        {
            return this.repository.CopiarHorario(cod_hfecha, fec_desde, CopyConductores);
        }

        public Task<List<PlaHorarioFechaLineaListView>> GetLineasHorarias()
        {
            return this.repository.GetLineasHorarias();
        }

        public async Task<List<string>> ObtenerDestinatarios(decimal CodLin)
        {
            return await this.repository.ObtenerDestinatarios(CodLin);
        }

        public Task<bool> HorarioDiagramado(int CodHfecha, int? CodTdia, List<int> CodServicio)
        {
            return this.repository.HorarioDiagramado(CodHfecha, CodTdia, CodServicio);
        }

        public async Task<int> RemapearRecoridoBandera(HFechasConfiFilter filter)
        {
            return await this.repository.RemapearRecoridoBandera(filter);
        }

        public async Task<List<HKilometros>> GetKilometrosAsync(List<int> CodBan, List<int> CodSec)
        {
            return await this.repository.GetKilometrosAsync(CodBan, CodSec);
        }

        public Task<List<ReporteHorarioExcelModel>> GenerarExcelHorarios(ExportarExcelFilter filter)
        {
            return this.repository.GenerarExcelHorarios(filter);
        }

        public async Task<List<RelevoModel>> GetDatosReporteRelevos(ExportarExcelFilter filtro)
        {
            return await this.repository.GetDatosReporteRelevos(filtro);
        }

        public async Task<string> GetTitulo (ExportarExcelFilter filtro)
        {
            return await this.repository.GetTitulo(filtro);
        }

        public async Task<HBasec> UpdateHBasec(int CodBan, int CodHFecha, int? CodRec)
        {
            var rta = await this.repository.UpdateHBasec(CodBan, CodHFecha, CodRec);
            return rta;
        }

        public async Task GuardarBanderaPorSector(HFechasConfi data)
        {
            await this.repository.GuardarBanderaPorSector(data);
        }
    }
    
}
