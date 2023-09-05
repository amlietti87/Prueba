using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Caching;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class HMinxtipoService : ServiceBase<HMinxtipo, int, IHMinxtipoRepository>, IHMinxtipoService
    {
        public ICacheManager cacheManager;
        private readonly IBanderaService banderaService;
        private readonly ITiposDeDiasService tiposDeDiasService;
        private readonly IHTposHorasService hTposHorasService;
        private readonly ILogger logger;

        public HMinxtipoService(IHMinxtipoRepository repository, ICacheManager _cacheManager, IBanderaService banderaService, ITiposDeDiasService _tiposDeDiasService, IHTposHorasService _hTposHorasService, 
            ILogger logger)
            : base(repository)
        {
            cacheManager = _cacheManager;
            this.banderaService = banderaService;
            tiposDeDiasService = _tiposDeDiasService;
            hTposHorasService = _hTposHorasService;
            this.logger = logger;
        }

        public async Task<List<HSectores>> GetHSectores(HMinxtipoFilter filter)
        {
            return await this.repository.GetHSectores(filter);
        }

        public async Task<ImportadorHMinxtipoResult> RecuperarPlanilla(HMinxtipoFilter filter)
        {
            var mvCache = await cacheManager.GetCache<string, ImportadorHMinxtipoResult>("HMinxtipoImportado")
                                          .GetAsync(filter.PlanillaId, e => TransformarDatos(filter));

            return mvCache;
        }

        private async Task<ImportadorHMinxtipoResult> TransformarDatos(HMinxtipoFilter filtro)
        {
            ImportadorHMinxtipoResult result = new ImportadorHMinxtipoResult();

            var Importados = await cacheManager.GetCache<string, ImportadorHMinxtipo>("CabeceraExcelImportado").GetAsync(filtro.PlanillaId, e => null);

            var sectores = await this.GetHSectores(filtro);

            var bandera = await banderaService.GetByIdAsync(filtro.CodBan.GetValueOrDefault());

            var tiposDias = (await tiposDeDiasService.GetAllAsync(e => e.Activo)).Items;

            var tiposDeHora = (await hTposHorasService.GetAllAsync(e => true)).Items;

            var minXtipos = await this.GetAllAsync(e => e.CodHfecha == filtro.CodHfecha && e.CodBan == filtro.CodBan);


            if (sectores.Count!= Importados.Sectores.Count)
            {
                throw new ValidationException("Los sectores no coinciden");
            }

            result.Sectores = Importados.Sectores;

            foreach (var item in Importados.Cabeceras)
            {
                HMinxtipoImportado hMinxtipo = new HMinxtipoImportado();
                hMinxtipo.IsValid = true;

                try
                {




                    //Tipo de dia
                    var td = tiposDias.FirstOrDefault(e => e.DesTdia.Trim() == item.TipoDia.Trim());
                    if (td == null)
                    {
                        hMinxtipo.IsValid = false;
                        hMinxtipo.Errors.Add("El tipo de dia es incorrecto");
                    }
                    else
                    {
                        hMinxtipo.CodTdia = td.Id;
                        hMinxtipo.DescripcionTdia = item.TipoDia;
                    }




                    //bandera
                    if (bandera.AbrBan?.Trim().ToLower() != item.Bandera?.Trim().ToLower())
                    {
                        hMinxtipo.IsValid = false;
                        hMinxtipo.Errors.Add(String.Format("La bandera {0} no es correcta", item.Bandera));
                    }
                    else
                    {
                        hMinxtipo.CodBan = bandera.Id;
                        hMinxtipo.AbrBan = bandera.AbrBan;
                    }


                    //Tipo De Hora
                    var th = tiposDeHora.FirstOrDefault(e => e.DscTpoHora.Trim().ToLower() == item.TipoHora.Trim().ToLower());
                    if (th == null)
                    {
                        hMinxtipo.IsValid = false;
                        hMinxtipo.Errors.Add("El tipo de Hora es incorrecto");
                    }
                    else
                    {
                        hMinxtipo.TipoHora = th.Id;
                        hMinxtipo.DescripcionTipoHora = item.TipoHora;
                    }

                    //Tiempo
                    hMinxtipo.TotalMin = decimal.Parse(item.TotalMin);
                    TimeSpan ts = new TimeSpan();
                    foreach (var Hsector in sectores.OrderBy(e=> e.Orden))
                    {
                        var det = new HDetaminxtipoImportado();
                        var impDet = item.Detalle.FirstOrDefault(s => s.Orden == Hsector.Orden);

                        det.CodHsector = Hsector.CodHsector;
                        det.DescripcionCodHsector = impDet.Sector.Descripcion;
                        det.Orden = Hsector.Orden;
                        var minutoSplit = impDet.Minuto.Replace("AM", "").Replace("PM", "").Split(":");
                        int minutos = int.Parse(minutoSplit[1]);
                        int segundos = int.Parse(minutoSplit[2]);
                        

                        det.Minuto = Convert.ToDecimal(minutos) + (Convert.ToDecimal(segundos) / 100);

                        ts = ts.Add(TimeSpan.FromMinutes(Convert.ToDouble(minutos)));
                        ts = ts.Add(TimeSpan.FromSeconds(Convert.ToDouble(segundos)));

                        if (det.Orden == 1 && det.Minuto != 0)
                        {
                            hMinxtipo.IsValid = false;
                            hMinxtipo.Errors.Add("El primer sector debe tener los minutos en cero");
                        }

                        hMinxtipo.Detalles.Add(det);
                    }

                    hMinxtipo.Detalles = hMinxtipo.Detalles.OrderBy(e => e.Orden).ToList();

                    //Validamos la suma de los minutos = total de minutos 
                    if (Convert.ToDecimal(ts.TotalMinutes) != hMinxtipo.TotalMin)
                    {
                        hMinxtipo.IsValid = false;
                        hMinxtipo.Errors.Add("El total de minutos no coincide");
                    }

                    var MxTReal = minXtipos.Items.FirstOrDefault(e =>
                                                                      e.CodBan == hMinxtipo.CodBan
                                                                      && e.CodTdia == hMinxtipo.CodTdia
                                                                      && e.TipoHora == hMinxtipo.TipoHora
                                                                      && e.TotalMin == hMinxtipo.TotalMin);
                    if (MxTReal == null)
                    {
                        hMinxtipo.IsValid = false;
                        hMinxtipo.Errors.Add(String.Format("No existe una configuracion válida para TD: {0}, Band: {1}, TH: {2}, Tiempo MV: {3}", hMinxtipo.DescripcionTdia, hMinxtipo.AbrBan, hMinxtipo.DescripcionTipoHora, hMinxtipo.TotalMin));
                    }
                    else
                    {
                        hMinxtipo.Id_HMinxtipo = MxTReal.Id;
                    }

                    
                    
                    


                }
                catch (Exception ex)
                {
                    hMinxtipo.IsValid = false;
                    hMinxtipo.Errors.Add(ex.Message);
                    await this.logger.LogError(ex.Message);
                    await this.logger.LogError(ex.StackTrace);
                    throw ex;
                }

                result.HMinxtipoImportados.Add(hMinxtipo);
            }


            return result;
        }

        public async Task UpdateHMinxtipo(IEnumerable<HMinxtipo> items)
        {
            await this.repository.UpdateHMinxtipo(items);
        }

        public async Task ImportarMinutos(HMinxtipoImporarInput input)
        {
            var planilla = await this.RecuperarPlanilla(new HMinxtipoFilter() { CodBan = input.CodBan, CodHfecha = input.CodHFecha, PlanillaId = input.PlanillaId });


            await this.repository.ImportarMinutos(planilla);
        }
        public async Task<string> CopiarMinutosAsync(CopiarHMinxtipoInput input)
        {
           return await this.repository.CopiarMinutosAsync(input);
        }

        public async Task SetHSectores(IEnumerable<HSectores> entities)
        {
           await this.repository.SetHSectores(entities);
        }

    }

}
