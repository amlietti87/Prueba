using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface ILineaAppService : IAppServiceBase<Linea, LineaDto, decimal>
    {
        Task<SearchResultDto> Search(string filterText);
        Task<List<ItemDecimalDto>> GetLineasPorUsuario();
        Task UpdateLineasAsociadas(LineaDto lineaDto);
        Task<LineaDto> RecuperarLineaConLineasAsociadas(decimal id);
        Task<List<LineaRosBusDto>> GetAllRosBusLineasAsync();
    }
}
