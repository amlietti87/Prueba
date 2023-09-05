using GeoJSON.Net.Geometry;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters.ComoLlegoBus;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using static ROSBUS.Admin.AppService.Model.ComoLlegoBusDto;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IPlaLineaAppService : IAppServiceBase<PlaLinea, PlaLineaDto, int>
    {
        bool TieneMapasEnBorrador(int id);
        Task<List<Leg>> GetAllRosBusRoutes(ComoLlegoBusFilter filter);
        Task<RosarioBusRutas> GetRosBusRouteDetail(ComoLlegoBusFilter filter);
        Task<FileDto> GetHorariosRuta(ComoLlegoBusFilter filter);
        Task<FileDto> getParadasRuta(ComoLlegoBusFilter filter);
        Task<FileDto> GetTarifasRuta(ComoLlegoBusFilter filter);
        Task<RutaWS9> GetRutaBandera(int? codBan);
    }
}
