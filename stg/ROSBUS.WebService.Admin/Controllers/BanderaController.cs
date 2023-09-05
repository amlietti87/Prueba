using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.WebService.Admin.Controllers
{

    [Route("[controller]/[action]")]
    //[PermissionsByActionAuthorize(typeof(BanderaPermissionContainer))]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class BanderaController : ManagerSecurityController<HBanderas, int, BanderaDto, BanderaFilter, IBanderaAppService>
    {

        //public class BanderaPermissionContainer : PermissionContainerBase
        //{
        //    public BanderaPermissionContainer()
        //        :base("Planificacion", "Bandera")
        //    {
        //        //Ejemplo para agregar un permiso
        //        //this.permissions.Add(this.GetPermissionType());

        //    }

        //}

        public BanderaController(IBanderaAppService service)
            : base(service)
        {

        }


        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Planificacion", "Bandera");
        }


        [HttpPost]
        public virtual async Task<IActionResult> RecuperarCartel([FromBody]int idBandera)
        {

            try
            {
                string cartel = await this.Service.RecuperarCartel(idBandera);

                return ReturnData<string>(cartel);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }


        
        [HttpGet]
        public virtual async Task<IActionResult> RecuperarLineasActivasPorFecha([FromQuery]BanderaFilter Filtro)
        {
            try
            {

                if (Filtro == null)
                {
                    Filtro = new BanderaFilter();
                }

                List<ItemDto> items = await this.Service.RecuperarLineasActivasPorFecha(Filtro);

                return ReturnData<List<ItemDto>>(items);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        [HttpGet]
        public virtual async Task<IActionResult> OrigenPredictivo([FromQuery]BanderaFilter Filtro)
        {
            try
            {

                if (Filtro == null)
                {
                    Filtro = new BanderaFilter();
                }

                List<string> items = await this.Service.OrigenPredictivo(Filtro);

                return ReturnData<List<string>>(items);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        [HttpGet]
        public virtual async Task<IActionResult> DestinoPredictivo([FromQuery]BanderaFilter Filtro)
        {
            try
            {

                if (Filtro == null)
                {
                    Filtro = new BanderaFilter();
                }

                List<string> items = await this.Service.DestinoPredictivo(Filtro);

                return ReturnData<List<string>>(items);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        [HttpGet]
        public virtual async Task<IActionResult> RecuperarBanderasRelacionadasPorSector([FromQuery]BanderaFilter Filtro)
        {

            try
            {

                if (Filtro == null)
                {
                    Filtro = new BanderaFilter();
                }

                var items = await this.Service.RecuperarBanderasRelacionadasPorSector(Filtro);

                return ReturnData<List<ItemDto<int>>>(items);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> GetReporteSabanaSinMinutos([FromBody]HorariosPorSectorDto horarios)
        {
            FileDto ReporteSabana = new FileDto();

            if (horarios.TipoInforme == 1)
            {
                ReporteSabana =  await this.Service.GetReporteSabanaSinMinutos(horarios);
            }
            else if (horarios.TipoInforme == 2)
            {
                ReporteSabana = await this.Service.GetReporteSabanaConMinutos(horarios);
            }

            return ReturnData<FileDto>(ReporteSabana);
            
        }


        [HttpPost]
        public async Task<IActionResult> GetReporteCambiosDeSector([FromBody]BanderaFilter filter)
        {
            return ReturnData<FileDto>(await this.Service.GetReporteCambiosDeSector(filter));
        }




        [HttpPost]
        public virtual async Task<IActionResult> RecuperarHorariosSectorPorSector([FromBody]BanderaFilter Filtro)
        {
            try
            {
                if (Filtro == null)
                {
                    Filtro = new BanderaFilter();
                }

                var items = await this.Service.RecuperarHorariosSectorPorSector(Filtro);

                return ReturnData<HorariosPorSectorDto>(items);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }
        }

        [HttpGet]
        public virtual async Task<IActionResult> RecuperarBanderasPorServicio([FromQuery] BanderaFilter Filtro)
        {
            try
            {
                if (Filtro == null)
                {
                    Filtro = new BanderaFilter();
                }

                List<ItemDto> items = await this.Service.RecuperarBanderasPorServicio(Filtro);

                return ReturnData<List<ItemDto>>(items);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
                
            }
        }

 
    }


}