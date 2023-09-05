using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Operaciones.AppService.Interface;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;

namespace ROSBUS.WebService.Siniestros.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class CCochesController : ManagerController<CCoches, string, CCochesDto, CCochesFilter, ICCochesAppService>
    {
        public CCochesController(ICCochesAppService service)
            : base(service)
        {

        }

        [HttpGet]
        public async Task<IActionResult> GetLineaPorDefecto(int CodEmpleado, DateTime FechaSiniestro, TimeSpan HoraSiniestro)
        {
            try
            {
                DateTime fechayhora = new DateTime(FechaSiniestro.Year, FechaSiniestro.Month, FechaSiniestro.Day, HoraSiniestro.Hours, HoraSiniestro.Minutes, HoraSiniestro.Seconds);
                
                var data = await Service.GetLineaPorDefecto(CodEmpleado, fechayhora);

                if (data.Count > 1 || data.Count == 0)
                {
                    return ReturnData<int>(0);
                }
                else
                {
                    return ReturnData<int>(data.FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                return ReturnError<int>(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetCocheById(string id, DateTime FechaSiniestro)
        {
            try
            {

                var data = await Service.GetCocheById(id, FechaSiniestro);
                var dto = this.MapObject<CCoches, CCochesDto>(data);
                return ReturnData<CCochesDto>(dto);
            }
            catch (Exception ex)
            {
                return ReturnError<CCochesDto>(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> ExisteCoche(string id)
        {
            try
            {

                var data = await Service.ExisteCoche(id);
                return ReturnData<bool>(data);
            }
            catch (Exception ex)
            {
                return ReturnError<bool>(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCoches(string FechaSiniestro, string Filter)
        {
            try
            {

                var data = await Service.GetAllCoches(DateTime.ParseExact(FechaSiniestro, "dd/MM/yyyy hh:mm", System.Globalization.CultureInfo.InvariantCulture), Filter);
                var dto = this.MapObject<List<CCoches>, List<CCochesDto>>(data);
                return ReturnData<List<CCochesDto>>(dto);
            }
            catch (Exception ex)
            {
                return ReturnError<List<CCochesDto>>(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> RecuperarCCochesPorFechaServicioLinea (DateTime Fecha, int? Cod_Servicio, int Cod_Linea)
        {
            try
            {                
                var coches = await Service.RecuperarCCochesPorFechaServicioLinea(Fecha, Cod_Servicio, Cod_Linea);
                return ReturnData<List<CCochesDto>>(coches);
            }
            catch(Exception ex)
            {
                return ReturnError<List<CCochesDto>>(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> RecuperarCCoches (string FilterText)
        {
            try
            {
                var coches = await Service.RecuperarCCoches(FilterText);
                return ReturnData<List<CCochesDto>>(coches);
            }
            catch (Exception ex)
            {
                return ReturnError<List<CCochesDto>>(ex);
            }
        }

    }



}
