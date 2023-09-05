using Microsoft.Extensions.Configuration;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.Filters.ComoLlegoBus;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Report;
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
    public class BanderaHttpAppService : HttpAppServiceBase<HBanderas, BanderaDto, int>, IBanderaAppService
    {
        public override string EndPoint => "Bandera";
               
        

        public BanderaHttpAppService(IAuthService authService)
            : base(authService)
        {
  
        }

        public Task<string> RecuperarCartel(int idBandera)
        {
            throw new NotImplementedException();
        }

        public async Task<List<ItemDto<int>>> RecuperarBanderasRelacionadasPorSector(BanderaFilter filtro)
        {
            string action = "RecuperarBanderasRelacionadasPorSector";

            var pList = await this.httpClient.PostRequest<List<ItemDto<int>>>(action, Newtonsoft.Json.JsonConvert.SerializeObject(filtro));

            return pList;
        }

        public async Task<HorariosPorSectorDto> RecuperarHorariosSectorPorSector(BanderaFilter filtro)
        {
            string action = "RecuperarHorariosSectorPorSector";

            var pList = await this.httpClient.PostRequest<HorariosPorSectorDto>(action, Newtonsoft.Json.JsonConvert.SerializeObject(filtro));

            return pList;
        }

        public async Task<List<ItemDto>> RecuperarLineasActivasPorFecha(BanderaFilter filtro)
        {
            string action = "RecuperarLineasActivasPorFecha";

            var  pList = await this.httpClient.GetRequest<List<ItemDto>>(action);

            return pList;
        }

        public async Task<List<ItemDto>> RecuperarBanderasPorServicio(BanderaFilter filtro)
        {
            string action = "RecuperarBanderasPorServicio";

            var pList = await this.httpClient.GetRequest<List<ItemDto>>(action);

            return pList;
        }

        public Task<FileDto> GetReporteCambiosDeSector(BanderaFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> OrigenPredictivo(BanderaFilter filtro)
        {
            throw new NotImplementedException();
        }

        public Task<List<string>> DestinoPredictivo(BanderaFilter filtro)
        {
            throw new NotImplementedException();
        }

        public Task<FileDto> GetReporteSabanaSinMinutos(HorariosPorSectorDto horarios)
        {
            throw new NotImplementedException();
        }

        public Task<ReportModel> GetDatosReportePuntaPunta(BanderaFilter filtro)
        {
            throw new NotImplementedException();
        }

        public Task<FileDto> GetReporteSabanaConMinutos(HorariosPorSectorDto horarios)
        {
            throw new NotImplementedException();
        }

        public Task<List<BanderasLineasDto>> GetAllBanderasLineaAsync(ComoLlegoBusFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
