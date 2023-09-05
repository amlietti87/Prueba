using Microsoft.AspNetCore.Http;
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
    public interface IPlaDistribucionDeCochesPorTipoDeDiaAppService : IAppServiceBase<PlaDistribucionDeCochesPorTipoDeDia, PlaDistribucionDeCochesPorTipoDeDiaDto, int>
    {
        Task<List<HMediasVueltasImportadaDto>> RecuperarPlanilla(PlaDistribucionDeCochesPorTipoDeDiaFilter filter);
        Task ImportarServicios(ImportarServiciosInput input);

        Task<PlaDistribucionEstadoView> ExistenMediasVueltasIncompletas(PlaDistribucionDeCochesPorTipoDeDia input);

        Task RecrearSabanaSector(PlaDistribucionDeCochesPorTipoDeDia input);
        Task<Boolean> TieneMinutosAsignados(PlaDistribucionDeCochesPorTipoDeDiaFilter filter);
        Task<Boolean> ValidateArchivos(IFormFileCollection files);
        Task<List<ImportarHorariosDto>> Horarios(IFormFileCollection files);
        Task<List<string>> ReadAsListAsync(IFormFile file);
    }
}
