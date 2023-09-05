using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IHMinxtipoAppService : IAppServiceBase<HMinxtipo, HMinxtipoDto, int>
    {
        Task<MinutosPorSectorDto> GetMinutosPorSector(HMinxtipoFilter filter);

        Task<List<HSectoresDto>> GetHSectores(HMinxtipoFilter filter);
        Task UpdateHMinxtipo(List<HMinxtipoDto> input);
        Task<ImportadorHMinxtipoResult> RecuperarPlanilla(HMinxtipoFilter filter);
        Task ImportarMinutos(HMinxtipoImporarInput input);
        Task<string> CopiarMinutosAsync(CopiarHMinxtipoInput input);
        Task<List<HSectoresDto>> SetHSectores(List<HSectoresDto> input);
        Task <List<FileDto>> GenerarExcelMinXSec(HMinxtipoFilter filter);
    }
}
