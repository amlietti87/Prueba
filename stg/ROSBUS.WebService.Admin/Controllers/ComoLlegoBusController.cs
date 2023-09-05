using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Entities.Filters.ComoLlegoBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.ApiServices.Filters;
using TECSO.FWK.ApiServices.Model;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using static ROSBUS.Admin.AppService.Model.ComoLlegoBusDto;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [ActionAuthorize]
    [ApiAuthorize]
    public class ComoLlegoBus : TECSO.FWK.ApiServices.ControllerBase, ISecurityController
    {
       
        ILineaAppService _lineaAppService;
        IBanderaAppService _banderaAppService;
        IPlaLineaAppService _plaLineaAppService;

        public IPermissionContainer PermissionContainer { get ; set ; }

        public ComoLlegoBus 
            (
            ILineaAppService lineaService,
            IBanderaAppService banderaService,
            IPlaLineaAppService plaLineaAppService
            )
        {
            _lineaAppService = lineaService;
            _banderaAppService = banderaService;
            _plaLineaAppService = plaLineaAppService;

            PermissionContainer = new ManagerPermissionContainerBase();
            PermissionContainer.AddPermission("GetAllRosBusRoutes", "ComoLlego", "ComoLlego", "RetornarRutas");
            PermissionContainer.AddPermission("GetRosBusRouteDetail", "ComoLlego", "ComoLlego", "RetornarDetalleRuta");
            PermissionContainer.AddPermission("GetHorariosRuta", "ComoLlego", "ComoLlego", "RetornarCuadroHorario");
            PermissionContainer.AddPermission("GetParadasRuta", "ComoLlego", "ComoLlego", "RetornarParadas");
            PermissionContainer.AddPermission("GetTarifasRuta", "ComoLlego", "ComoLlego", "RetornarTarifas");
            PermissionContainer.AddPermission("GetAllRosBusLineas", "ComoLlego", "ComoLlego", "RetornarLíneas");
            PermissionContainer.AddPermission("GetAllBanderasLinea", "ComoLlego", "ComoLlego", "RetornarBanderas");
            PermissionContainer.AddPermission("GetRecorridoBandera", "ComoLlego", "ComoLlego", "RetornarRecorrido");
        }

        //EndPoint Servicio 2
        [HttpPost]
        
        public async Task<IActionResult> GetAllRosBusRoutes([FromBody] ComoLlegoBusFilter filter)
        {
            try
            {
                var rosbusroutes = await this._plaLineaAppService.GetAllRosBusRoutes(filter);
                return ReturnData<List<Leg>>(rosbusroutes);
            }
            catch (Exception ex) 
            {

                return ReturnError<string>(ex);
            }
            
        }

        //EndPoint Servicio 3
        [HttpPost]
        public async Task<IActionResult> GetRosBusRouteDetail([FromBody] ComoLlegoBusFilter filter)
        {
            try
            {
                var rosbusroutedetail = await this._plaLineaAppService.GetRosBusRouteDetail(filter);
                return ReturnData<RosarioBusRutas>(rosbusroutedetail);
            }
            catch (Exception ex)
            {

                return ReturnError<string>(ex);
            }
        }


        // Empezar desde aca para abajo, el 2 y el 3 no se hacen.
        //EndPoint Servicio 4
        [HttpGet]
        public async Task<IActionResult> GetHorariosRuta(ComoLlegoBusFilter filter)
        {
            try
            {
                FileDto cuadroHorario = await this._plaLineaAppService.GetHorariosRuta(filter);
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(cuadroHorario.ByteArray, contentType);
            }
            catch (Exception ex)
            {
                return ReturnError<FileDto>(ex);
            }
            
        }

        //EndPoint Servicio 5
        [HttpGet]
        public async Task<IActionResult> GetParadasRuta(ComoLlegoBusFilter filter)
        {
            try
            {
                FileDto paradasruta = await this._plaLineaAppService.getParadasRuta(filter);
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(paradasruta.ByteArray, contentType);
            }
            catch (Exception ex)
            {
                return ReturnError<FileDto>(ex);
            }

        }

        //EndPoint Servicio 6
        [HttpGet]
        public async Task<IActionResult> GetTarifasRuta(ComoLlegoBusFilter filter)
        {
            try
            {
                FileDto CuadroTarifario = await this._plaLineaAppService.GetTarifasRuta(filter);
                string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                return File(CuadroTarifario.ByteArray, contentType);
            }
            catch (Exception ex)
            {
                return ReturnError<FileDto>(ex);
            }

        }

        //EndPoint Servicio 7
        [HttpGet]
        public async Task<IActionResult> GetAllRosBusLineas()
        {
            try
            {
                var RosBusLineas = await this._lineaAppService.GetAllRosBusLineasAsync();
                return ReturnData<List<LineaRosBusDto>>(RosBusLineas);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }


        }

        //EndPoint Servicio 8
        [HttpGet]
        public async Task<IActionResult> GetAllBanderasLinea(ComoLlegoBusFilter filter)
        {

            try
            {
                var BanderasxLinea = await this._banderaAppService.GetAllBanderasLineaAsync(filter);
                return ReturnData<List<BanderasLineasDto>>(BanderasxLinea);
            }
            catch (Exception ex)
            {

                return ReturnError<string>(ex);
            }
      
        }

        //EndPoint Servicio 9
        [HttpGet]
        public async Task<IActionResult> GetRecorridoBandera(ComoLlegoBusFilter filter)
        {

            try
            {
                var RutaxBandera = await this._plaLineaAppService.GetRutaBandera(filter.CodBan);
                return ReturnData<RutaWS9>(RutaxBandera);
            }
            catch (Exception ex)
            {

                return ReturnError<string>(ex);
            }

        }
    }


 

}
