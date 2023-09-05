using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using SharpKml.Engine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.Extensions;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class PuntosController : ManagerController<PlaPuntos, Guid, PuntosDto, PuntosFilter, IPuntosAppService>
    {

        public PuntosController(IPuntosAppService service)
            : base(service)
        {
        }

        [HttpGet]
        public async Task<IActionResult> RecuperarDatosIniciales(int? CodRec)
        {
            try
            {
                if (!CodRec.HasValue)
                {
                    return ReturnError<List<PuntosDto>>( new ArgumentException("Falta codigo de linea"));
                }
                var list = await this.Service.RecuperarDatosIniciales(CodRec.Value); 
                return ReturnData<List<PuntosDto>>(list);
            }
            catch (Exception ex)
            {
                return ReturnError<List<PuntosDto>>(ex);
            }
        }
         
        [HttpPost]
        public async Task<IActionResult> GetReporte([FromBody]PuntosFilter filter)
        {
            return ReturnData<FileDto>(await this.Service.GetReporte(filter));
        }

        [HttpPost]
        public async Task<IActionResult> GetKML([FromBody]PuntosFilter filter)
        {

            try
            {
                var kml = await this.Service.CreateKML(filter);
                return ReturnData<FileDto>(kml);
            }
            catch (Exception ex)
            {
                return ReturnError<FileDto> (ex);
            }
        }



        [HttpGet]
        public async Task<IActionResult> GetGeoJson(int? CodRec)
        {

            if (!CodRec.HasValue)
            {
                return ReturnError<List<PuntosDto>>(new ArgumentException("Falta codigo de linea"));
            }




            return ReturnData<FeatureCollection>(await this.Service.RecuperarRecorridoGeoJson(CodRec.GetValueOrDefault()));

        }

        [HttpGet]
        public async Task<IActionResult> GetGeoJsonDetaReco(int? CodRec)
        {

            if (!CodRec.HasValue)
            {
                return ReturnError<List<PuntosDto>>(new ArgumentException("Falta codigo de linea"));
            }

            return Ok(await this.Service.RecuperarDetaRecoGeoJson(CodRec.GetValueOrDefault()));

        }


    }




}
