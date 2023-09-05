using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Caching;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class PlaDistribucionDeCochesPorTipoDeDiaService : ServiceBase<PlaDistribucionDeCochesPorTipoDeDia, int, IPlaDistribucionDeCochesPorTipoDeDiaRepository>, IPlaDistribucionDeCochesPorTipoDeDiaService
    {

        public PlaDistribucionDeCochesPorTipoDeDiaService(IPlaDistribucionDeCochesPorTipoDeDiaRepository produtoRepository, ICacheManager _cacheManager,
          IBanderaService banderaservice, IHTposHorasService HTposHorasService, ISubGalponService SubGalponService, IBolBanderasCartelService _bolBanderasCartelService, ILineaRepository lineaRepository)
          : base(produtoRepository)
        {

            cacheManager = _cacheManager;
            this.banderaservice = banderaservice;
            this.HTposHorasService = HTposHorasService;
            this.SubGalponService = SubGalponService;
            this.bolBanderasCartelService = _bolBanderasCartelService;
            this.lineaRepository = lineaRepository;


        }


        public ICacheManager cacheManager;
        public IBanderaService banderaservice;
        private IHTposHorasService HTposHorasService;
        private ISubGalponService SubGalponService;
        private readonly IBolBanderasCartelService bolBanderasCartelService;
        private readonly ILineaRepository lineaRepository;

        
        public async Task<List<HMediasVueltasImportadaDto>> RecuperarPlanilla(PlaDistribucionDeCochesPorTipoDeDiaFilter filter)
        {
            var mvCache = await cacheManager.GetCache<string, List<HMediasVueltasImportadaDto>>("HMediasVueltas")
                                           .GetAsync(filter.PlanillaId, e => TransformarDatos(filter));

            return mvCache;
        }


        private async Task<List<HMediasVueltasImportadaDto>> TransformarDatos(PlaDistribucionDeCochesPorTipoDeDiaFilter filtro)
        {
            var Importados = await cacheManager.GetCache<string, List<ImportarHorariosDto>>("ImportarHorariosDto").GetAsync(filtro.PlanillaId, e => null);

            List<HMediasVueltasImportadaDto> mediasvueltas = new List<HMediasVueltasImportadaDto>();

            var lineainclude = new List<Expression<Func<Linea, object>>>
            {                
                e=> e.SucursalesxLineas 
            };

            //opcion1
            var linea = (await this.lineaRepository.GetAllAsync(e => e.HFechasConfi.Any(a => a.Id == filtro.CodHfecha), lineainclude)).Items.FirstOrDefault();
            var cod_lin = linea.Id;
            var suc_Id = linea.SucursalesxLineas.FirstOrDefault()?.Id;

            ////opcion2
            //cod_lin = (await this.GetByIdAsync(filtro.Id)).CodHfechaNavigation.CodLin;


            var banderafilter = new BanderaFilter() { LineaIdRelacionadas = filtro.cod_lin ?? cod_lin };
            var banderas = (await banderaservice.GetAllAsync(e => !e.IsDeleted && e.SucursalId == suc_Id)).Items;
            var banderasRelacionadas = (await banderaservice.GetAllAsync(banderafilter.GetFilterExpression())).Items;


            var subgalpones = (await SubGalponService.GetAllAsync(e => e.FecBaja == null)).Items;
            var TposHoras = (await HTposHorasService.GetAllAsync(e => true)).Items;



            foreach (var item in Importados)
            {
                HMediasVueltasImportadaDto mv = new HMediasVueltasImportadaDto();
                mv.IsValid = true;

                try
                {
                    mv.NumServicio = int.Parse(item.Servicio);

                    var s = item.Sale.Split(":");
                    int horas = int.Parse(s[0]);
                    int min = int.Parse(s[1]);
                    int dia = item.Sale_EsDiaPosterior ? 2 : 1;
                    mv.Sale = new DateTime(2000, 1, dia, horas, min, 0);

                    var l = item.Llega.Split(":");
                    horas = int.Parse(l[0]);
                    min = int.Parse(l[1]);
                    dia = item.Llega_EsDiaPosterior ? 2 : 1;
                    mv.Llega = new DateTime(2000, 1, dia, horas, min, 0);

                    if (item.TieneFormatoFechaInvalido)
                    {
                        mv.IsValid = false;
                        mv.Errors.Add("El registro de Sale o Llega Tiene Formato Inválido.");
                    }


                    mv.DescripcionBandera = item.Bandera?.Trim();

                    var banderaAsoc = banderas.Where(e => e.AbrBan?.Trim() == item.Bandera?.Trim() && banderasRelacionadas.Any(b=> b.Id == e.Id));
                    if (banderaAsoc.Count() == 1)
                    {
                        var bandera = banderaAsoc.FirstOrDefault();
                        mv.CodBan = bandera.Id;
                        mv.DescripcionBandera = item.Bandera;
                    }
                    else if (banderaAsoc.Count() > 1)
                    {
                        mv.IsValid = false;
                        mv.Errors.Add("Existe mas de una Bandera con esta abreviación");                    
                    }
                    else
                    {
                        var banderasAbr =  banderas.Where(e => e.AbrBan?.Trim() == item.Bandera?.Trim());
                        if (banderasAbr.Count() > 0)
                        {
                            if (banderasAbr.Any(e => e.TipoBanderaId == 2))
                            {
                                var bandera = banderasAbr.FirstOrDefault(e => e.TipoBanderaId == 2);                                
                                mv.CodBan = bandera.Id;
                                mv.DescripcionBandera = item.Bandera;
                                mv.EsPosicionamiento = true;

                            }
                            else {
                                //la bandera no existe
                                mv.IsValid = false;
                                mv.Errors.Add("La bandera existe pero no esta asociada a ninguna de las linea, y no es de Posicionamiento");
                            }
                        }
                        else
                        {
                            //la bandera no existe
                            mv.IsValid = false;
                            mv.Errors.Add("No se encontro Bandera");
                        }
                        
                    }

                    var hTposHorasService = TposHoras.Where(e => e.DscTpoHora?.Trim() == item.dsc_TpoHora?.Trim()).FirstOrDefault();
                    if (hTposHorasService != null)
                    {
                        mv.CodTpoHora = hTposHorasService.Id;
                        mv.DescripcionTpoHora = item.dsc_TpoHora;

                    }
                    else
                    {
                        mv.IsValid = false;
                        mv.Errors.Add("No se encontro tipo de hora");
                    }

                    var subgalpon = subgalpones.Where(e => e.DesSubg?.Trim() == item.des_subg?.Trim()).FirstOrDefault();
                    if (subgalpon != null)
                    {
                        mv.CodSubGalpon = subgalpon.Id;
                        mv.DescripcionSubGalpon = subgalpon.DesSubg;
                    }
                    else
                    {
                        mv.IsValid = false;
                        mv.Errors.Add("No se encontro subgalpon");
                    }

                }
                catch (Exception ex)
                {
                    mv.Errors.Add(ex.ToString());
                    mv.IsValid = false;
                }




                mediasvueltas.Add(mv);
            }

            try
            {

                foreach (var servicio in mediasvueltas.GroupBy(e => e.NumServicio))
                {
                    var orden = 0;
                    servicio.ToList().ForEach(e =>
                    {
                        e.OrdenImportado = ++orden;
                    }
                            );


                    //var sale = servicio.FirstOrDefault().Sale;
                    //var Llega = servicio.FirstOrDefault().Llega;

                    //foreach (var mv in servicio)
                    //{
                    //    if (mv.Sale < sale && mv.Sale.Hour <= 1)
                    //    {
                    //        servicio.Where(e => e.OrdenImportado >= mv.OrdenImportado).ToList().ForEach(e =>
                    //        {
                    //            e.Sale = e.Sale.AddDays(1);
                    //        }
                    //        );
                    //    }
                    //    sale = mv.Sale;


                    //    if (mv.Llega < Llega && mv.Llega.Hour <= 1)
                    //    {
                    //        servicio.Where(e => e.OrdenImportado >= mv.OrdenImportado).ToList().ForEach(e =>
                    //        {
                    //            e.Llega = e.Llega.AddDays(1);
                    //        }
                    //        );
                    //    }
                    //    Llega = mv.Llega;






                    //}

                }


                foreach (var servicio in mediasvueltas.GroupBy(e => e.NumServicio))
                {
                    foreach (var mv in servicio)
                    {
                        if (mv.Sale > mv.Llega)
                        {
                            mv.Errors.Add("Verifique Sale es mayor que LLega");    
                            mv.IsValid = false;
                        }
                    }

                    var mevlist = servicio.ToList();

                    for (var i = 1; i < mevlist.Count() ; i++)
                    {
                        if (mevlist[i].Sale  < mevlist[i - 1].Llega )
                        {
                            mevlist[i].IsValid = false;
                            mevlist[i - 1].IsValid = false;
                            mevlist[i - 1].Errors.Add("Verifique LLega se solapa con el Sale de la mediavuelta siguiente");
                            mevlist[i].Errors.Add("Verifique Sale se solapa con el Llega de la mediavuelta anterior");
                        }
                    }

                }




            }
            catch (Exception)
            {

                throw;
            }




            return mediasvueltas;
        }

        

        public async Task ImportarServicios(ImportarServiciosInput input)
        {
            var planilla = await this.RecuperarPlanilla(new PlaDistribucionDeCochesPorTipoDeDiaFilter() { CodHfecha = input.CodHfecha, PlanillaId = input.PlanillaId });

            if (input.desde.HasValue)
            {
                planilla = planilla.Where(e => e.NumServicio >= input.desde.Value).ToList();
            }

            if (input.hasta.HasValue)
            {
                planilla = planilla.Where(e => e.NumServicio <= input.hasta.Value).ToList();
            }

            input.MediasVueltas = planilla;

            await this.repository.ImportarServiciosAsync(input);
        }

        public async Task<PlaDistribucionEstadoView> ExistenMediasVueltasIncompletas(PlaDistribucionDeCochesPorTipoDeDia input)
        {
            return await this.repository.ExistenMediasVueltasIncompletas(input);
        }

        public async Task RecrearSabanaSector(PlaDistribucionDeCochesPorTipoDeDia input)
        {
            await this.repository.RecrearSabanaSector(input);
        }

        public async Task<Boolean> TieneMinutosAsignados(PlaDistribucionDeCochesPorTipoDeDiaFilter filter)
        {
            return await this.repository.TieneMinutosAsignados(filter);
        }

        public async Task<PlaDuracionesEstadoView> ExistenDuracionesIncompletas(PlaDistribucionDeCochesPorTipoDeDia item)
        {
            return await this.repository.ExistenDuracionesIncompletas(item);
        }
    }

}
