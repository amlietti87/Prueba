using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Repositories;
using System.Linq;
using TECSO.FWK.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Snickler.EFCore;
using System.Data.SqlClient;
using ROSBUS.Admin.Domain.Entities.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using TECSO.FWK.Extensions;
using ROSBUS.Admin.Domain.Interfaces.Services;
using TECSO.FWK.Domain;
using System.ComponentModel.DataAnnotations;

namespace ROSBUS.infra.Data.Repositories
{
    public class HChoxserRepository : RepositoryBase<AdminContext, HChoxser, int>, IHChoxserRepository
    {

        public HChoxserRepository(IAdminDbContext _context, IHFechasConfiService HFechasConfiService)
            : base(new DbContextProvider<AdminContext>(_context))
        {
            this._HFechasConfiService = HFechasConfiService;
        }

        public readonly IHFechasConfiService _HFechasConfiService;

        public override Expression<Func<HChoxser, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public async Task ImportarDuraciones(ImportadorHChoxserResult input)
        {
            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {

                    var h_horarios_confi = await Context.HHorariosConfi.Include(e => e.HServicios).Where(r => r.CodHfecha == input.filtro.CodHfecha && r.CodTdia == input.filtro.CodTdia).ToListAsync();

                    //Context.HChoxser.RemoveRange(Context.HChoxser.Where(e=> e.Id==h_horarios_confi.Id))


                    foreach (var item in h_horarios_confi)
                    {
                        var lisgsubgalpon = input.List.Where(e => item.HServicios.Any(a => e.servicioId == a.Id));

                        if (lisgsubgalpon.Any())
                        {
                            item.CantidadConductoresReal = lisgsubgalpon.Count(e => e.TieneTitular);
                            item.CantidadConductoresReal += lisgsubgalpon.Count(e => e.TieneRelevo);
                            item.CantidadConductoresReal += lisgsubgalpon.Count(e => e.TieneAuxiliar);


                            item.CantidadCochesReal = lisgsubgalpon.Count();


                            var solorelevo = lisgsubgalpon.Where(e => !e.TieneTitular && e.TieneRelevo).ToList();
                            var solotitular = lisgsubgalpon.Where(e => e.TieneTitular && !e.TieneRelevo);
                            var soloauxiliar = lisgsubgalpon.Where(e => !e.TieneTitular && !e.TieneRelevo && e.TieneAuxiliar).ToList();

                            if (solorelevo.Any() && solotitular.Any())
                            {
                                foreach (var titular in solotitular.OrderBy(e => e.LlegaDate))
                                {
                                    var machearelevo = solorelevo.OrderBy(e => e.SaleRelevoDate).Where(e => titular.LlegaDate <= e.SaleRelevoDate).FirstOrDefault();
                                    if (machearelevo != null)
                                    {
                                        solorelevo.Remove(machearelevo);
                                        item.CantidadCochesReal--;
                                    }
                                    else
                                    {
                                        var macheaaxiliar = soloauxiliar.OrderBy(e => e.SaleAuxiliarDate).Where(e => titular.LlegaDate <= e.SaleAuxiliarDate).FirstOrDefault();
                                        if (macheaaxiliar != null)
                                        {
                                            soloauxiliar.Remove(macheaaxiliar);
                                            item.CantidadCochesReal--;
                                        }
                                    }
                                }
                            }

                            if (soloauxiliar.Any())
                            {
                                var relevoSinAuxiliar = lisgsubgalpon.Where(e => e.TieneRelevo && !e.TieneAuxiliar).ToList();
                                if (relevoSinAuxiliar.Any())
                                {
                                    foreach (var titular in relevoSinAuxiliar.OrderBy(e => e.LlegaRelevoDate))
                                    {
                                        var macheaaxiliar = soloauxiliar.OrderBy(e => e.SaleAuxiliarDate).Where(e => titular.LlegaDate <= e.SaleAuxiliarDate).FirstOrDefault();
                                        if (macheaaxiliar != null)
                                        {
                                            soloauxiliar.Remove(macheaaxiliar);
                                            item.CantidadCochesReal--;
                                        }
                                    }
                                }
                            }

                        }

                    }


                    var serviciosIds = h_horarios_confi.SelectMany(h => h.HServicios.Select(e => e.Id));


                    if ((await Context.HFechasConfi.SingleAsync(e => e.Id == input.filtro.CodHfecha)).PlaEstadoHorarioFechaId == PlaEstadoHorarioFecha.Aprobado)
                    {
                        if (await _HFechasConfiService.HorarioDiagramado(input.filtro.CodHfecha.Value, input.filtro.CodTdia, serviciosIds.ToList()))
                        {
                            throw new DomainValidationException("No se puede importar la duración por que el horario ya fue diagramado");
                        }
                    }


                    var allItems = await Context.HChoxser.Where(e => serviciosIds.Contains(e.Id)).ToListAsync();


                    Context.HChoxser.RemoveRange(allItems);
                    Context.SaveChanges();

                    foreach (var itemImportado in input.List)
                    {
                        HChoxser current = new HChoxser();

                        if (itemImportado.TieneTitular)
                        {
                            current = new HChoxser();
                            current.Id = itemImportado.servicioId.GetValueOrDefault();
                            current.CodUni = 1;
                            current.CodEmp = "";
                            current.Sale = itemImportado.SaleDate.GetValueOrDefault();
                            current.Llega = itemImportado.LlegaDate.GetValueOrDefault();
                            current.SalePlanificado = current.Sale;
                            current.LlegaPlanificado = current.Llega;
                            current.DuracionPlanificada = Convert.ToInt32(itemImportado.duracion.ToTimeSpan().GetValueOrDefault().TotalMinutes);
                            Context.HChoxser.Add(current);
                            Context.SaveChanges();

                        }


                        if (itemImportado.TieneRelevo)
                        {
                            HChoxser relevo = new HChoxser();
                            relevo = new HChoxser();
                            relevo.Id = itemImportado.servicioId.GetValueOrDefault();
                            relevo.CodUni = 2;
                            relevo.CodEmp = "";
                            relevo.Sale = itemImportado.SaleRelevoDate.GetValueOrDefault();
                            relevo.Llega = itemImportado.LlegaRelevoDate.GetValueOrDefault();
                            relevo.SalePlanificado = relevo.Sale;
                            relevo.LlegaPlanificado = relevo.Llega;
                            relevo.DuracionPlanificada = Convert.ToInt32(itemImportado.duracionRelevo.ToTimeSpan().GetValueOrDefault().TotalMinutes);
                            Context.HChoxser.Add(relevo);
                            Context.SaveChanges();
                        }

                        if (itemImportado.TieneAuxiliar)
                        {
                            HChoxser aux = new HChoxser();
                            aux = new HChoxser();
                            aux.Id = itemImportado.servicioId.GetValueOrDefault();
                            aux.CodUni = 3;
                            aux.CodEmp = "";
                            aux.Sale = itemImportado.SaleAuxiliarDate.GetValueOrDefault();
                            aux.Llega = itemImportado.LlegaAuxiliarDate.GetValueOrDefault();
                            aux.SalePlanificado = aux.Sale;
                            aux.LlegaPlanificado = aux.Llega;
                            aux.DuracionPlanificada = Convert.ToInt32(itemImportado.duracionAuxiliar.ToTimeSpan().GetValueOrDefault().TotalMinutes);
                            Context.HChoxser.Add(aux);
                            Context.SaveChanges();
                        }
                    }



                    ts.Commit();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public async Task<List<HChoxserExtendedDto>> RecuperarDuraciones(HHorariosConfiFilter filter)
        {
            var result = new RecuperarHorariosSectorPorSectorDto();

            var sp = this.Context.LoadStoredProc("dbo.sp_h_choxser_RecuperarAsignacionDuracion")
                .WithSqlParam("CodHfecha", new SqlParameter("CodHfecha", filter.CodHfecha))
                .WithSqlParam("CodSubg", new SqlParameter("CodSubg", filter.CodSubg))
                .WithSqlParam("CodTdia", new SqlParameter("CodTdia", filter.CodTdia))
                .WithSqlParam("cod_servicio", new SqlParameter("cod_servicio", (Object)filter.ServicioId ?? DBNull.Value ))
                ;

            List<HChoxserExtendedDto> items = new List<HChoxserExtendedDto>();

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                items = handler.ReadToList<HChoxserExtendedDto>().ToList();
            });

            return items;

        }

        public async Task DeleteDuracionesServicio(int idServicio)
        {
            try
            {
                var itemsToRemove = await this.Context.HChoxser.Where(e => e.Id == idServicio).ToListAsync();
                if (!itemsToRemove.Any())
                {
                    throw new ValidationException("El servicio no posee Duraciones");
                }
                this.Context.HChoxser.RemoveRange(itemsToRemove);
                await this.Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                throw;
            }

        }
    }
}
