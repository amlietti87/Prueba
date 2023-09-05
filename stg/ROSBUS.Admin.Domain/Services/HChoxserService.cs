using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Caching;
using TECSO.FWK.Domain.Services;
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.Domain.Services
{
    public class HChoxserService : ServiceBase<HChoxser,int, IHChoxserRepository>, IHChoxserService
    {

        public ICacheManager cacheManager;
        private readonly IHServiciosService hServiciosService;
        private readonly ITiposDeDiasService tiposDeDiasService;
        public HChoxserService(IHChoxserRepository produtoRepository, ICacheManager _cacheManager, IHServiciosService _hServiciosService, ITiposDeDiasService _tiposDeDiasService)
            : base(produtoRepository)
        {
            cacheManager = _cacheManager;
            hServiciosService = _hServiciosService;
            tiposDeDiasService = _tiposDeDiasService;
        }

        public async Task<ImportadorHChoxserResult> RecuperarPlanilla(HChoxserFilter filter)
        {
            var mvCache = await cacheManager.GetCache<string, ImportadorHChoxserResult>("ItemImportadorHChoxserResult")
                                         .GetAsync(filter.PlanillaId, e => TransformarDatos(filter));

            return mvCache;
        }

        private async Task<ImportadorHChoxserResult> TransformarDatos(HChoxserFilter filtro)
        {
            ImportadorHChoxserResult result = new ImportadorHChoxserResult();
            result.filtro = filtro;

            var Importados = await cacheManager.GetCache<string, ImportadorHChoxser>("ItemImportadorHChoxser").GetAsync(filtro.PlanillaId, e => null);

            HServiciosFilter hServiciosFilter = new HServiciosFilter() { CodHfecha = filtro.CodHfecha, CodTdia = filtro.CodTdia };

            var servicios = (await hServiciosService.GetAllAsync(hServiciosFilter.GetFilterExpression(), new List<System.Linq.Expressions.Expression<Func<HServicios, object>>>() { e=> e.HMediasVueltas })).Items;


            foreach (var item in servicios)
            {
                if (!Importados.List.Any(e=> item.NumSer.TrimStart('0') == e.servicio))
                {
                    throw new ValidationException(String.Format("Falta cargar el servicio {0} al Excel para el tipo de dia seleccionado", item.NumSer));
                }
            }

            var tiposdedias = Importados.List.Select(e => e.tipoDeDia).Distinct().Count();

            if (tiposdedias > 1)
            {
                throw new ValidationException("Tiene cargado más de un tipo de día");
            }
            else if (tiposdedias == 1)
            {
                string tipodiastr = Importados.List.Select(e => e.tipoDeDia).FirstOrDefault();

                if (String.IsNullOrWhiteSpace(tipodiastr))
                {
                    throw new ValidationException("Falta cargar la descripción del Tipo de Día en la última columna del Excel");
                }

                var findtipodedia = (await tiposDeDiasService.GetAllAsync(e=> e.DesTdia.ToLower().Trim() == tipodiastr.ToLower().Trim())).Items.FirstOrDefault();

                if (findtipodedia == null)
                {
                    throw new ValidationException("No se encuentra el tipo de día");
                }
                else
                {
                    if (findtipodedia.Id != filtro.CodTdia)
                    {
                        throw new ValidationException("El tipo de día de la pantalla no es igual al del Excel");
                    }
                }

            }


            foreach (var item in Importados.List)
            {

                //item.IsValid = true;
                if (item.IsValid)
                {
                    try
                    {

                        


                        var serv = servicios.FirstOrDefault(e => e.NumSer.TrimStart('0') == item.servicio);
                        if (serv == null)
                        {
                            item.IsValid = false;
                            item.Errors.Add("No se encontro el servicio " + item.servicio);
                        }
                        else
                        {
                            item.servicioId = serv.Id;
                            if (!serv.HMediasVueltas.Any())
                            {
                                item.IsValid = false;
                                item.Errors.Add("El servicio no tiene medias vueltas " + item.servicio);
                            }
                            else
                            {
                                var firstMV = serv.HMediasVueltas.OrderBy(e => e.Orden).FirstOrDefault();
                                var lastMV = serv.HMediasVueltas.OrderBy(e => e.Orden).LastOrDefault();

                                if (item.GetPrimersale().TimeOfDay != firstMV.Sale.TimeOfDay)
                                {
                                    item.IsValid = false;
                                    item.Errors.Add("El sale tiene que igual al primer sale del servicio " + item.servicio);
                                }

                                if (item.GetUltimpoLlega().TimeOfDay != lastMV.Llega.TimeOfDay)
                                {
                                    item.IsValid = false;
                                    item.Errors.Add("El ultimo llega tiene que ser igual ultimo llega del servicio " + item.servicio);
                                }
                            }


                        }

                        //Validamos las duraciones

                        //if ((item.LlegaDate.Value - item.SaleDate.Value).TotalMinutes != item.duracion.ToTimeSpan().GetValueOrDefault().TotalMinutes)
                        //{
                        //    item.IsValid = false;
                        //    item.Errors.Add("La duracion en minutos del sale y llega no coincide");
                        //}

                        //if (item.TieneRelevo && (item.LlegaRelevoDate.Value- item.SaleRelevoDate.Value).TotalMinutes != item.duracionRelevo.ToTimeSpan().GetValueOrDefault().TotalMinutes)
                        //{
                        //    item.IsValid = false;
                        //    item.Errors.Add("La duracion en minutos del sale y llega del Relevo no coincide");
                        //}

                        //if (item.TieneAuxiliar && (item.LlegaAuxiliarDate.Value - item.SaleAuxiliarDate.Value).TotalMinutes != item.duracionAuxiliar.ToTimeSpan().GetValueOrDefault().TotalMinutes)
                        //{
                        //    item.IsValid = false;
                        //    item.Errors.Add("La duracion en minutos del sale y llega del Auxiliar no coincide");
                        //}



                        //Validamos los solapamientos

                        if (item.TieneRelevo && item.TieneAuxiliar && item.LlegaRelevoDate >= item.SaleAuxiliarDate.GetValueOrDefault())
                        {
                            item.IsValid = false;
                            item.Errors.Add("Llega del relevo se solapa con el sale del Auxiliar");
                        }

                        if (item.TieneTitular && item.TieneRelevo && item.LlegaDate >= item.SaleRelevoDate.GetValueOrDefault())
                        {
                            item.IsValid = false;
                            item.Errors.Add("Llega se solapa con el sale del Relevo");
                        }

                        if (item.TieneTitular && item.SaleDate >= item.LlegaDate)
                        {
                            item.IsValid = false;
                            item.Errors.Add("Sale y Llega se solapan");
                        }

                        if (item.TieneRelevo && item.SaleRelevoDate >= item.LlegaRelevoDate)
                        {
                            item.IsValid = false;
                            item.Errors.Add("Sale y Llega del relevo se solapan");
                        }

                        if (item.TieneAuxiliar && item.SaleAuxiliarDate >= item.LlegaAuxiliarDate)
                        {
                            item.IsValid = false;
                            item.Errors.Add("Sale y Llega del Auxiliar se solapan");
                        }
                    }


                    catch (Exception ex)
                    {
                        item.IsValid = false;
                        item.Errors.Add(ex.ToString());
                    }
                }


                result.List.Add(item);
                
            }


           

            return result;
        }

        public async Task<List<HChoxserExtendedDto>> RecuperarDuraciones(HHorariosConfiFilter filter)
        {
            return await this.repository.RecuperarDuraciones(filter);
        }

        public async  Task ImportarDuraciones(HChoxserFilter input)
        {
            var planilla = await this.RecuperarPlanilla(input); 

            await this.repository.ImportarDuraciones(planilla);
        }

        public async Task DeleteDuracionesServicio(int idServicio)
        {
            await this.repository.DeleteDuracionesServicio(idServicio);
        }
    }
    
}
