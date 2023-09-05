using GeoJSON.Net.Feature;
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
    public interface IPuntosAppService : IAppServiceBase<PlaPuntos, PuntosDto, Guid>
    { 
        Task<List<PuntosDto>> RecuperarDatosIniciales(int CodRec);
    
        Task<FileDto> GetReporte(PuntosFilter filter);
        Task<List<PuntosDto>> RecuperarPuntosRecorrido(int CodRec);

        Task<FeatureCollection> RecuperarRecorridoGeoJson(int CodRec);

        Task<FeatureCollection> RecuperarDetaRecoGeoJson(int CodRec);

        Task<List<PlaPuntos>> GetFilterPuntosInicioFin(PuntosFilter pf);

        Task<List<GpsDetaReco>> RecuperarDetaReco(int CodRec);

        Task<FileDto> CreateKML(PuntosFilter filtro);
    }
}
