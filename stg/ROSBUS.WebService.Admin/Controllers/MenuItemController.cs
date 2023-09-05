using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;

using TECSO.FWK.ApiServices;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    [Authorize]
    public class MenuItemController : TECSO.FWK.ApiServices.ControllerBase
    {
        private IsucursalesAppService Service;

        public MenuItemController(IsucursalesAppService _Service)
        {
            this.Service = _Service;
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var entity = await this.Service.GetAllAsync(e => true);
                var administracionPlanificacion = new List<MenuItemDto>();
                var mapas = new List<MenuItemDto>();
                var horarios = new MenuItemDto();
                foreach (var item in entity.Items)
                {
                    administracionPlanificacion.Add(new MenuItemDto()
                    {
                        name = item.DscSucursal,
                        permissionName = "",
                        icon = "flaticon-app",
                        items = new List<MenuItemDto>() {new MenuItemDto("Líneas", "Planificacion.Linea.Administracion", "fa fa-bus", "/planificacion/linea/" + item.Id),
                                                         new MenuItemDto("Grupos de líneas", "Planificacion.Bandera.Administracion", "flaticon-layers", "/planificacion/grupolineas/" + item.Id),
                                                         new MenuItemDto("Banderas", "Planificacion.GrupoLinea.Administracion", "fa fa-flag-o", "/planificacion/bandera/" + item.Id),
                                                         new MenuItemDto("Talleres", "Planificacion.Bandera.Administracion", "flaticon-layers", "/planificacion/talleres/" + item.Id),

                        }
                    });



                    mapas.Add(new MenuItemDto()
                    {
                        name = item.DscSucursal,
                        permissionName = "",
                        icon = "flaticon-app",
                        items = new List<MenuItemDto>() {new MenuItemDto("Lineas", "Planificacion.Linea.Administracion", "fa fa-bus", "/planificacion/lineas/" + item.Id)
                                                         

                        }
                    });

                }

                horarios = new MenuItemDto()
                {
                    name = "Horarios",
                    permissionName = "",
                    icon = "la la-clock-o",
                    items = new List<MenuItemDto>()
                    {
                        new MenuItemDto("Fecha Horaria", "Horarios.FechaHorario.EntrarHorario", "flaticon-clock-2", "/planificacion/horariofecha"),
                        new MenuItemDto()
                        {
                            name = "Reportes",
                            permissionName = "",
                            icon = "fa fa-print",
                            items =new List<MenuItemDto>()
                            {
                                new MenuItemDto("Sabana", "Horarios.Sabana.Administracion", "flaticon-clock-2", "/planificacion/sabana"),
                                new MenuItemDto("Detalle Salida y Relevos", "Horarios.Sabana.detalleSalidasYRelevos", "fa fa-print", "/planificacion/detalleSalidasYRelevos"),
                                new MenuItemDto("Horarios Pasajeros", "Horarios.Sabana.horarioPasajeros", "fa fa-print", "/planificacion/horarioPasajeros"),
                                new MenuItemDto("Distribucion de Coches", "Horarios.Sabana.distribucionCoches", "fa fa-print", "/planificacion/distribucionCoches"),
                                new MenuItemDto("Paradas para Pasajeros", "Horarios.Sabana.ParadasPasajeros", "fa fa-print", "/planificacion/reportePasajeros"),
                            }
                        }
                        
                    }
                };


                



                var menudto = new AppMenu("Main", "Main");

                menudto.items.Add(new MenuItemDto("Administración Planificación", "", "flaticon-list-3", null, administracionPlanificacion));
                menudto.items.Add(horarios);
                menudto.items.Add(new MenuItemDto("Mapas", "", "flaticon-map-location", null, mapas));


                return ReturnData(menudto);
            }
            catch (Exception ex)
            {
                return ReturnError<AppMenu>(ex);
            }
        }

    }

 
}