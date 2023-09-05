using Microsoft.Extensions.Configuration;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService.service
{
    public class LineaHttpAppService : HttpAppServiceBase<Linea, LineaDto, decimal>, ILineaAppService
    {
        public override string EndPoint => "Linea";

      

        public LineaHttpAppService(IAuthService authService)
            : base(authService)
        {

        }

        public Task<SearchResultDto> Search(string filterText)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ItemDecimalDto>> GetLineasPorUsuario()
        {
            string action = "GetLineasPorUsuario";

            var pList = await this.httpClient.GetRequestResponseModel<List<ItemDecimalDto>>(action);

            return pList;
        }

        public async Task UpdateLineasAsociadas(LineaDto lineaDto)
        {
            string action = "UpdateLineasAsociadas";

            var pList = await this.httpClient.PostRequest<LineaDto>(action, lineaDto);
            
        }

        public async Task<LineaDto> RecuperarLineaConLineasAsociadas(decimal id)
        {
            string action = "RecuperarLineaConLineasAsociadas";

            //var pList = await this.httpClient.GetRequestResponseModel<LineaDto>(action, id);
            return null;
        }

        public Task<List<LineaRosBusDto>> GetAllRosBusLineasAsync()
        {
            throw new NotImplementedException();
        }
    }
}
