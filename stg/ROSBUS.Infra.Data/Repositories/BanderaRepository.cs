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
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data.SqlClient;
using ROSBUS.Admin.Domain.Entities.Filters;
using TECSO.FWK.Domain.Interfaces.Entities;
using Snickler.EFCore;
using ROSBUS.Admin.Domain.Model;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.infra.Data.Repositories
{
    public class BanderaRepository : RepositoryBase<AdminContext, HBanderas, int>, IBanderaRepository
    {

        public BanderaRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public async Task<List<HBanderas>> GetAllBanderasWithRamal()
        {
            return await this.Context.HBanderas.Where(e => !e.IsDeleted).Include(e => e.RamalColor).ToListAsync();
        }

        protected override IQueryable<HBanderas> AddIncludeForGet(DbSet<HBanderas> dbSet)
        {

            IQueryable<HBanderas> q = base.AddIncludeForGet(dbSet).AsQueryable();

            q = q.Include(e => e.RamalColor).AsQueryable();
            q = q.Include(e => e.RamalColor.PlaLinea).AsQueryable();
            q = q.Include(e => e.PlaCodigoSubeBandera).ThenInclude(y => y.CodEmpresaNavigation);
            q = q.Include(e => e.PlaCodigoSubeBandera).ThenInclude(y => y.PlaSentidoBanderaSubeNavigation);

            return q;
        }

        public async Task<List<string>> OrigenPredictivo(BanderaFilter filtro)
        {
            var list = new List<string>();

            list = await this.Context.HBanderas.Where(e => e.Origen.TrimStart().StartsWith(filtro.FilterText)).Select(f => f.Origen).ToListAsync();

            var list2 = await this.Context.HBanderas.Where(e => e.Destino.TrimStart().StartsWith(filtro.FilterText)).Select(f => f.Destino).ToListAsync();

            return list.Union(list2).Distinct().ToList();
        }
        public async Task<List<string>> DestinoPredictivo(BanderaFilter filtro)
        {
            var list = new List<string>();

            list = await this.Context.HBanderas.Where(e => e.Origen.TrimStart().StartsWith(filtro.FilterText)).Select(f => f.Origen).ToListAsync();

            var list2 = await this.Context.HBanderas.Where(e => e.Destino.TrimStart().StartsWith(filtro.FilterText)).Select(f => f.Destino).ToListAsync();

            return list.Union(list2).Distinct().ToList();
        }

        //public override async Task<HBanderas> GetByIdAsync(int id)
        //{
        //    try
        //    {
        //        IQueryable<HBanderas> query = Context.Set<HBanderas>().Where(e => e.Id == id).AsQueryable();

        //        query = query.Include(e => e.PlaCodigoSubeBandera);
        //        query = query.Include(e => e.PlaCodigoSubeBandera).ThenInclude(y => y.CodEmpresaNavigation);

        //        return await query.FirstOrDefaultAsync(); 
        //    }
        //    catch (Exception ex)
        //    {
        //        HandleException(ex);
        //        throw;
        //    }
        //}

        public override Expression<Func<HBanderas, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        public async override Task DeleteAsync(HBanderas entity)
        {

            foreach (var item in Context.GpsRecorridos.Where(e => e.CodBan == entity.Id))
            {
                Context.Set<GpsRecorridos>().Remove(item);
            }

            await base.DeleteAsync(entity);
        }

        public async Task<Linea> GetLinea(int idBandera)
        {
            var bandera = await this.Context.HBanderas.Where(e => e.Id == idBandera).Include(f => f.RamalColor).FirstOrDefaultAsync();
            if (bandera != null && bandera.RamalColor != null)
            {
                return await this.Context.Linea.Where(e => e.Id == bandera.RamalColor.LineaId).FirstOrDefaultAsync();
            }
            else
            {
                return null;
            }
        }

        public override async Task<HBanderas> UpdateAsync(HBanderas entity)
        {

            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {
                    Context.Entry(entity).State = EntityState.Modified;
                    var bEspecial = await this.Context.HBanderasEspeciales.FirstOrDefaultAsync(e => e.CodBan == entity.Id);
                    if (bEspecial == null)
                    {
                        if (entity.TipoBanderaId == PlaTipoBandera.Posicionamiento)
                        {
                            bEspecial = new HBanderasEspeciales() { CodBan = entity.Id, cortado = true };
                            this.Context.HBanderasEspeciales.Add(bEspecial);
                        }
                    }
                    else
                    {
                        bEspecial.cortado = entity.TipoBanderaId == PlaTipoBandera.Posicionamiento;
                        this.Context.Entry(bEspecial).State = EntityState.Modified;
                    }

                    await this.SaveChangesAsync();
                    ts.Commit();
                    return entity;
                }


            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }


        }

        public override async Task<HBanderas> AddAsync(HBanderas entity)
        {
            using (var ts = await this.Context.Database.BeginTransactionAsync())
            {
                entity.Id = this.Context.HBanderas.Max(e => e.Id) + 1;
                var r = await base.AddAsync(entity);

                if (entity.TipoBanderaId == PlaTipoBandera.Posicionamiento)
                {
                    var bEspecial = new HBanderasEspeciales() { CodBan = entity.Id, cortado = true };
                    this.Context.HBanderasEspeciales.Add(bEspecial);
                    await this.Context.SaveChangesAsync();
                }


                ts.Commit();
                return r;
            }

        }


        protected override void CompleteEntityAfterRead(HBanderas entity)
        {
            entity.Cortado = this.Context.HBanderasEspeciales.Any(e => e.CodBan == entity.Id && e.cortado == true);
        }

        public async Task<string> RecuperarCartel(int idBandera)
        {

            var result = await this.Context.Set<BolBanderasCartelDetalle>().FromSql("exec sp_H_bandera_RecuperarCartelVigente @idBandera ", new SqlParameter("idBandera", idBandera)).FirstOrDefaultAsync();

            return result?.TextoBandera;
        }
        public async Task<RecuperarHorariosSectorPorSectorDto> RecuperarHorariosSectorPorSectorDto(BanderaFilter filtro)
        {

            var result = new RecuperarHorariosSectorPorSectorDto();

            var sp = this.Context.LoadStoredProc("dbo.sp_H_bandera_RecuperarHorariosSectorPorSector")

                .WithSqlParam("cod_lin", new SqlParameter("cod_lin", filtro.LineaId))
                .WithSqlParam("fecha", new SqlParameter("fecha", filtro.Fecha.GetValueOrDefault(DateTime.Today).Date))
                .WithSqlParam("banderasList", new SqlParameter("banderasList", string.Join(",", filtro.BanderasSeleccionadas)))
                .WithSqlParam("cod_servicio", new SqlParameter("cod_servicio", (Object)filtro.cod_servicio ?? DBNull.Value))
                .WithSqlParam("cod_Conductor", new SqlParameter("cod_Conductor", (Object)filtro.cod_Conductor ?? DBNull.Value))

                .WithSqlParam("CodHfecha", new SqlParameter("@CodHfecha", (Object)filtro.CodHfecha ?? DBNull.Value))
                .WithSqlParam("CodTdia", new SqlParameter("CodTdia", (Object)filtro.CodTdia ?? DBNull.Value));


            List<hProcMin> hProcMins = new List<hProcMin>();

            List<HorarioTrabajoConductor> HorarioTrabajoConductor = new List<HorarioTrabajoConductor>();

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                result.HMediasVueltas = handler.ReadToList<HMediasVueltasDto>().ToList();
                if (handler.NextResult()) hProcMins = handler.ReadToList<hProcMin>().ToList();
                if (handler.NextResult()) HorarioTrabajoConductor = handler.ReadToList<HorarioTrabajoConductor>().ToList();

            });


            var sectoresDistic = hProcMins.Select(e => e.cod_hsector).Distinct();

            foreach (var cod_hsector in sectoresDistic)
            {
                if (hProcMins.Where(e => e.cod_hsector == cod_hsector).Count() == result.HMediasVueltas.Count())
                {
                    hProcMins.Where(e => e.cod_hsector == cod_hsector).ToList().ForEach(f => f.TrueForAll = true);
                }
            }

            if (filtro.NoDescartarPrimeryUltimoMV)
            {
                foreach (var item in result.HMediasVueltas.OrderBy(e => e.sale))
                {
                    var minutos = hProcMins.Where(e => item.cod_mvuelta == e.cod_mvuelta).OrderBy(e => e.orden).ToList();
                    result.Minutos.AddRange(minutos);
                }
            }
            else
            {
                foreach (var item in result.HMediasVueltas.OrderBy(e => e.sale))
                {
                    var minutos = hProcMins.Where(e => item.cod_mvuelta == e.cod_mvuelta).OrderBy(e => e.orden).ToList();
                    minutos.Remove(minutos.FirstOrDefault());
                    minutos.Remove(minutos.LastOrDefault());
                    result.Minutos.AddRange(minutos);
                }
            }
            





            foreach (var item in result.HMediasVueltas.OrderBy(e => e.sale))
            {
                var minutos = result.Minutos.Where(e => item.cod_mvuelta == e.cod_mvuelta).OrderBy(e => e.orden).ToList();
                foreach (var minuto in minutos)
                {
                    var hora = item.sale.Hour;
                    //  var dia = item.sale.Day;



                    var minutoanterior = minutos.Where(e => e.orden == minuto.orden - 1).FirstOrDefault();

                    if (minutoanterior != null)
                    {
                        if (minutoanterior.Hora.HasValue && (minutoanterior.Hora.GetValueOrDefault().Minutes > minuto.minuto))
                        {
                            hora = minutoanterior.Hora.GetValueOrDefault().Hours + 1;
                        }
                        else if (minutoanterior.Hora.HasValue)
                        {
                            hora = minutoanterior.Hora.GetValueOrDefault().Hours;
                        }
                    }
                    else
                    {
                        if (item.sale.Minute > minuto.minuto)
                        {
                            hora = item.sale.Hour + 1;
                        }
                    }

                    int segundos = Convert.ToInt32((minuto.minuto - Math.Truncate(minuto.minuto)) * 100);

                    if (hora >= 24)
                    {
                        minuto.Hora = new TimeSpan(hora, Convert.ToInt32(Math.Truncate(minuto.minuto)), segundos);
                    }

                    minuto.Hora = new TimeSpan(hora, Convert.ToInt32(Math.Truncate(minuto.minuto)), segundos);




                    minuto.Fecha = filtro.Fecha.GetValueOrDefault().AddTicks(minuto.Hora.Value.Ticks);
                    minuto.cod_servicio = item.cod_servicio;
                }
            }


            if (!string.IsNullOrEmpty(filtro.cod_Conductor) && HorarioTrabajoConductor.Any())
            {

                var todosMinutos = result.Minutos.ToList();
                result.Minutos.Clear();

                //var filter = filtroresultMinutos.Where(e => HorarioTrabajoConductor.Any(item =>  e.cod_servicio == item.cod_servicio
                //       && e.Fecha >= item.sale
                //       && e.Fecha <= item.llega
                // )).ToList();
                //result.Minutos.AddRange(filter);

                foreach (var item in HorarioTrabajoConductor)
                {
                    //var filter = todosMinutos.Where(e => e.cod_servicio == item.cod_servicio
                    //      && e.Fecha >= item.sale
                    //      && e.Fecha <= item.llega
                    //).ToList();



                    var filter = todosMinutos.Where(e => e.cod_servicio == item.cod_servicio).ToList();
                    filter = filter.Where(e => e.Fecha >= item.sale).ToList();
                    filter = filter.Where(e => e.Fecha <= item.llega).ToList();



                    result.Minutos.AddRange(filter);

                    if (item.cod_uni.GetValueOrDefault(0) == 1)
                    {
                        var mayor = filter.OrderBy(e => e.Fecha).LastOrDefault();
                        if (mayor != null)
                        {

                            var elconductornosebajo = todosMinutos.Where(e => e.cod_mvuelta == mayor.cod_mvuelta && e.Fecha > mayor.Fecha).FirstOrDefault();
                            if (elconductornosebajo != null)
                            {
                                result.Minutos.Add(elconductornosebajo);
                            }
                        }
                    }






                }


                var todasMedia = result.HMediasVueltas.ToList();
                result.HMediasVueltas.Clear();

                foreach (var item in todasMedia)
                {
                    if (result.Minutos.Any(e => e.cod_mvuelta == item.cod_mvuelta))
                    {
                        result.HMediasVueltas.Add(item);
                    }
                }



                foreach (var item in result.HMediasVueltas)
                {
                    var minutosmediavueltafiltrada = result.Minutos.Where(e => e.cod_mvuelta == item.cod_mvuelta).ToList();

                    var minnutosmediavuelta = todosMinutos.Where(e => e.cod_mvuelta == item.cod_mvuelta).ToList();

                    //if (minnutosmediavuelta.Count() == minutosmediavueltafiltrada.Count())
                    {
                        var maxvfo = minutosmediavueltafiltrada.Max(e => e.orden);
                        if (maxvfo != minnutosmediavuelta.Max(e => e.orden))
                        {
                            var m = minutosmediavueltafiltrada.Where(e => e.orden == maxvfo).FirstOrDefault();
                            if (m != null)
                            {
                                m.EsRelevo = true;
                            }
                        }


                        var minvfo = minutosmediavueltafiltrada.Min(e => e.orden);
                        if (minvfo != minnutosmediavuelta.Min(e => e.orden))
                        {
                            var m = minutosmediavueltafiltrada.Where(e => e.orden == minvfo).FirstOrDefault();
                            if (m != null)
                            {
                                m.EsRelevo = true;
                            }
                        }


                    }
                }


                result.Minutos.ForEach(e => e.TrueForAll = false);

                foreach (var cod_hsector in sectoresDistic)
                {
                    if (result.Minutos.Where(e => e.cod_hsector == cod_hsector).Count() == result.HMediasVueltas.Count())
                    {
                        result.Minutos.Where(e => e.cod_hsector == cod_hsector).ToList().ForEach(f => f.TrueForAll = true);
                    }
                }

            }


            return result;
        }




        public async Task<HorariosPorSectorDto> RecuperarHorariosSectorPorSector(BanderaFilter filtro)
        {
            return await this.RecuperarHorariosSectorPorSectorOrdenado(filtro);
        }

        public async Task<List<HorariosPorSectorDto>> RecuperarHorarios(BanderaFilter filtro)
        {

            List<HorariosPorSectorDto> result = new List<HorariosPorSectorDto>();


            RecuperarHorariosSectorPorSectorDto dto = await this.RecuperarHorariosSectorPorSectorDto(filtro);


            var banderas = dto.HMediasVueltas.GroupBy(e => e.cod_ban);


            foreach (var bandera in banderas)
            {
                var banderaresult = new HorariosPorSectorDto();

                foreach (var item in bandera.OrderBy(e => e.sale))
                {
                    var lista = new RowHorariosPorSectorDto();

                    //Columnas Fijas
                    lista.Servicio = item.num_ser.ToString();
                    lista.Sale = item.sale.ToString("HH:mm");
                    lista.SaleValue = item.sale;
                    lista.Llega = item.llega.ToString("HH:mm");
                    lista.TotalDeMinutos = item.dif_min.ToString();
                    lista.TipoHora = item.DescripcionTpoHora;
                    lista.Bandera = item.abr_ban.ToString();
                    lista.cod_mvuelta = item.cod_mvuelta;

                    var sector = 1;

                    var minutos = dto.Minutos.Where(e => item.cod_mvuelta == e.cod_mvuelta);
                    var minOrden = minutos.Min(e => e.ordenNuevo);
                    var maxnOrden = minutos.Max(e => e.ordenNuevo);

                    for (int i = minOrden; i <= maxnOrden; i++)
                    {
                        sector++;
                        //Columnas dinamicas
                        var minuto = dto.Minutos.Where(e => item.cod_mvuelta == e.cod_mvuelta && e.ordenNuevo == i).FirstOrDefault();
                        lista.ColumnasDinamicas.Add(new ColumnasDataDto()
                        {
                            Key = minuto.cod_hsector.ToString(),
                            value = Math.Truncate(minuto.minuto),
                            Orden = minuto.ordenNuevo,
                            Hora = minuto.Hora,
                            DescripcionSector = minuto.descripcion_Sector,
                            EsRelevo = minuto.EsRelevo
                        });

                    }

                    banderaresult.Items.Add(lista);
                }
                result.Add(banderaresult);
            }





            return null;

        }


        public async Task<HorariosPorSectorDto> RecuperarHorariosSectorPorSectorOrdenado(BanderaFilter filtro)
        {
            HorariosPorSectorDto result = new HorariosPorSectorDto();
            hProcMin sectorPatron = new hProcMin();
            RecuperarHorariosSectorPorSectorDto dto = await this.RecuperarHorariosSectorPorSectorDto(filtro);

            var Cod_Ban = filtro.BanderasSeleccionadas.LastOrDefault();

            //if (!filtro.BanderasSeleccionadas.Any())
            {
                Cod_Ban = dto.HMediasVueltas.Where(e => dto.Minutos.Any(a => a.cod_mvuelta == e.cod_mvuelta)).Select(e => e.cod_ban).FirstOrDefault();
            }

            if (Cod_Ban == 0)
            {
                return result;
            }


            var mediavueltapatron = dto.HMediasVueltas.Where(e => e.cod_ban == Cod_Ban && dto.Minutos.Any(a => a.cod_mvuelta == e.cod_mvuelta)).FirstOrDefault();

            if (filtro.BanderasSeleccionadas.Count > 1)
            {
                sectorPatron = dto.Minutos.Where(e => e.cod_mvuelta == mediavueltapatron.cod_mvuelta && e.TrueForAll && e.orden > 1).OrderBy(e => e.orden).FirstOrDefault();
            }
            else
            {
                sectorPatron = dto.Minutos.Where(e => e.cod_mvuelta == mediavueltapatron.cod_mvuelta && e.TrueForAll).OrderBy(e => e.orden).FirstOrDefault();
            }
            
            var ordenarporpatron = true;

            if (sectorPatron == null)
            {
                ordenarporpatron = false;
                sectorPatron = dto.Minutos.Where(e => e.cod_mvuelta == mediavueltapatron.cod_mvuelta).OrderBy(e => e.orden).FirstOrDefault();

            }



            foreach (var item in dto.HMediasVueltas.OrderBy(e => e.sale))
            {
                var sppmv = dto.Minutos.Where(e => item.cod_mvuelta == e.cod_mvuelta && sectorPatron.cod_hsector == e.cod_hsector).FirstOrDefault();



                foreach (var minuto in dto.Minutos.Where(e => item.cod_mvuelta == e.cod_mvuelta).OrderBy(e => e.orden))
                {
                    minuto.ordenNuevo = minuto.orden - (sppmv?.orden).GetValueOrDefault(minuto.orden);
                }

            }

            var minOrden = dto.Minutos.Min(e => e.ordenNuevo);
            var maxnOrden = dto.Minutos.Max(e => e.ordenNuevo);


            foreach (var item in dto.HMediasVueltas.OrderBy(e => e.sale))
            {
                var lista = new RowHorariosPorSectorDto();

                //Columnas Fijas
                lista.Servicio = item.num_ser.ToString();
                lista.Sale = item.sale.ToString("HH:mm");
                lista.SaleValue = item.sale;
                lista.Llega = item.llega.ToString("HH:mm");
                lista.TotalDeMinutos = item.dif_min.ToString();
                lista.TipoHora = item.cod_tpoHora;
                lista.DesTipoHora = item.DescripcionTpoHora;
                lista.Bandera = item.abr_ban.ToString();
                lista.cod_mvuelta = item.cod_mvuelta;

                var sector = 1;
                TimeSpan Llega = item.sale.TimeOfDay.Add(TimeSpan.FromMinutes(Convert.ToDouble(item.dif_min)));
                for (int i = minOrden; i <= maxnOrden; i++)
                {
                    sector++;
                    //Columnas dinamicas
                    var minuto = dto.Minutos.Where(e => item.cod_mvuelta == e.cod_mvuelta && e.ordenNuevo == i).FirstOrDefault();

                    if (minuto != null && minuto.orden != 1 )
                    {
 
                        lista.ColumnasDinamicas.Add(new ColumnasDataDto()
                        {
                            Key = minuto.cod_hsector.ToString(),
                            value = filtro.ShowDecimalValues ? minuto.minuto : Math.Truncate(minuto.minuto),
                            Orden = minuto.ordenNuevo,
                            Hora = minuto.Hora,
                            EsPatron = sectorPatron.cod_hsector == minuto.cod_hsector,
                            DescripcionSector = minuto.descripcion_Sector,
                            EsRelevo = minuto.EsRelevo
                            //  Text = minuto.TrueForAll ? minuto.descripcion_Sector + " " + minuto.ordenNuevo.ToString() : sector.ToString()
                        });
                    }
                    else
                    {

                        lista.ColumnasDinamicas.Add(new ColumnasDataDto()
                        {
                            Key = i.ToString(),
                            value = null,
                            Orden = i,
                            Hora = minuto?.Hora,
                            EsPatron = sectorPatron.cod_hsector == minuto?.cod_hsector,
                            DescripcionSector = minuto?.descripcion_Sector,
                            EsRelevo = minuto?.EsRelevo ?? false
                            //    Text = sector.ToString()
                        });
                    }
                }

                result.Items.Add(lista);
            }



            var sectorn = 1;
            for (int i = minOrden; i <= maxnOrden; i++)
            {
                
                var minuto = dto.Minutos.Where(e => e.ordenNuevo == i).FirstOrDefault();
                if (filtro.NoDescartarPrimeryUltimoMV)
                {
                    if (minuto.ordenNuevo > minOrden && minuto.ordenNuevo < maxnOrden)
                    {
                        sectorn++;
                        if (minuto != null)
                        {
                            if (filtro.BanderasSeleccionadas.Count > 1)
                            {
                                var l = minuto.TrueForAll && minuto.orden != 1 ? minuto.descripcion_Sector : sectorn.ToString();
                                result.Colulmnas.Add(new ColumnasDto() { Key = i.ToString(), Label = l });
                            }
                            else
                            {
                                var l = minuto.TrueForAll ? minuto.descripcion_Sector : sectorn.ToString();
                                result.Colulmnas.Add(new ColumnasDto() { Key = i.ToString(), Label = l });
                            }
                            
                        }
                        else
                        {
                            result.Colulmnas.Add(new ColumnasDto() { Key = i.ToString(), Label = sectorn.ToString() });
                        }
                    }
                }
                else
                {
                    sectorn++;
                    if (minuto != null)
                    {
                        if (filtro.BanderasSeleccionadas.Count > 1)
                        {
                            var l = minuto.TrueForAll && minuto.orden != 1 ? minuto.descripcion_Sector : sectorn.ToString();
                            result.Colulmnas.Add(new ColumnasDto() { Key = i.ToString(), Label = l });
                        }
                        else
                        {
                            var l = minuto.TrueForAll ? minuto.descripcion_Sector : sectorn.ToString();
                            result.Colulmnas.Add(new ColumnasDto() { Key = i.ToString(), Label = l });
                        }
                    }
                    else
                    {
                        result.Colulmnas.Add(new ColumnasDto() { Key = i.ToString(), Label = sectorn.ToString() });
                    }
                }

            }

            if (ordenarporpatron)
            {

                foreach (var item in result.Items)
                {
                    var date = item.SaleValue;

                    item.HoraPatron = new TimeSpan(item.SaleValue.Day, item.SaleValue.Hour, item.SaleValue.Minute, item.SaleValue.Second);

                    foreach (var cd in item.ColumnasDinamicas.OrderBy(e => e.Orden))
                    {
                        if (cd.value != null)
                        {
                            TimeSpan tsNew = TimeSpan.FromDays(item.HoraPatron.Value.Days);

                            var hora = item.HoraPatron.Value.Hours + (item.HoraPatron.Value.Minutes > Convert.ToInt32(cd.value) ? 1 : 0);

                            tsNew = tsNew.Add(TimeSpan.FromHours(hora));
                            tsNew = tsNew.Add(TimeSpan.FromMinutes(Convert.ToDouble(cd.value)));
                            item.HoraPatron = tsNew;
                        }

                        if (cd.EsPatron)
                            break;
                    }

                }

                result.Items = result.Items.OrderBy(e => e.HoraPatron).ToList();

                for (int i = 0; i < result.Items.Count - 1; i++)
                {
                    var hact = result.Items[i].ColumnasDinamicas.FirstOrDefault(e => e.EsPatron).Hora.Value;
                    var hsig = result.Items[i + 1].ColumnasDinamicas.FirstOrDefault(e => e.EsPatron).Hora.Value;
                    if (hact.Hours > hsig.Hours && hact.Days == hsig.Days)
                    {
                        hsig = hsig.Add(TimeSpan.FromDays(1));
                    }
                    if (hact.Hours == hsig.Hours && hact.Days > hsig.Days)
                    {
                        hsig = hsig.Add(TimeSpan.FromDays(1));
                    }
                    var resta = hsig.Subtract(hact);
                    result.Items[i + 1].Diferencia = resta.TotalMinutes.ToString();
                }


            }


            if (!string.IsNullOrEmpty(filtro.cod_Conductor))
            {



            }

            if (filtro.NoDescartarPrimeryUltimoMV)
            {
                foreach (var item in result.Items)
                {
                    item.ColumnasDinamicas.Remove(item.ColumnasDinamicas.FirstOrDefault());
                    item.ColumnasDinamicas.Remove(item.ColumnasDinamicas.LastOrDefault());
                    foreach (var columnadinamica in item.ColumnasDinamicas)
                    {
                        var HoraFormated = columnadinamica.Hora?.ToString("hh\\:mm");
                        if (HoraFormated != null && HoraFormated.Equals(item.Llega))
                        {
                            columnadinamica.value = null;
                        }
                    }
                }
            }

            return await Task.FromResult(result);


        }


        public async Task<List<ItemDto<int>>> RecuperarBanderasRelacionadasPorSector(BanderaFilter Filtro)
        {
            List<ItemDto<int>> itemDtos = new List<ItemDto<int>>();

            var sp = this.Context.LoadStoredProc("dbo.sp_H_bandera_RecuperarBanderasRelacionadasPorSector")

                .WithSqlParam("cod_lin", new SqlParameter("cod_lin", Filtro.LineaId))
                .WithSqlParam("BanderaRelacionadaID", new SqlParameter("BanderaRelacionadaID", Filtro.BanderaRelacionadaID))
                .WithSqlParam("fecha", new SqlParameter("fecha", Filtro.Fecha.GetValueOrDefault(DateTime.Today).Date))
                .WithSqlParam("CodHfecha", new SqlParameter("cod_hfecha", (Object)Filtro.CodHfecha ?? DBNull.Value))
                .WithSqlParam("codTipoDia", new SqlParameter("cod_tdia", (Object)Filtro.CodTdia ?? DBNull.Value));



            await sp.ExecuteStoredProcAsync((handler) =>
            {
                itemDtos = handler.ReadToList<ItemDto<int>>().ToList();
            });

            return itemDtos;
        }

        public async Task<List<ItemDto>> RecuperarLineasActivasPorFecha(BanderaFilter Filtro)
        {
            List<ItemDto> itemDtos = new List<ItemDto>();

            var sp = this.Context.LoadStoredProc("dbo.sp_H_bandera_RecuperarActivasPorFecha")
                .WithSqlParam("cod_lin", new SqlParameter("cod_lin", Filtro.LineaId))
                .WithSqlParam("fecha", new SqlParameter("fecha", Filtro.Fecha.GetValueOrDefault(DateTime.Today).Date))
                .WithSqlParam("SentidoBanderaId", new SqlParameter("SentidoBanderaId", Filtro.SentidoBanderaId))
                .WithSqlParam("CodHfecha", new SqlParameter("CodHfecha", (Object)Filtro.CodHfecha ?? DBNull.Value))
                .WithSqlParam("codTipoDia", new SqlParameter("codTipoDia", (Object)Filtro.CodTdia ?? DBNull.Value));


            await sp.ExecuteStoredProcAsync((handler) =>
            {
                itemDtos = handler.ReadToList<ItemDto>().ToList();
            });

            return itemDtos;
        }

        public async Task<List<ItemDto>> RecuperarBanderasPorServicio(BanderaFilter Filtro)
        {
            List<ItemDto> itemDtos = new List<ItemDto>();

            var sp = this.Context.LoadStoredProc("dbo.sp_H_bandera_RecuperarPorServicio")
                .WithSqlParam("cod_lin", new SqlParameter("cod_lin", Filtro.LineaId))
                .WithSqlParam("codServicio", new SqlParameter("codServicio", Filtro.cod_servicio))
                .WithSqlParam("CodHfecha", new SqlParameter("CodHfecha", (Object)Filtro.CodHfecha))
                .WithSqlParam("codTipoDia", new SqlParameter("codTipoDia", (Object)Filtro.CodTdia));


            await sp.ExecuteStoredProcAsync((handler) =>
            {
                itemDtos = handler.ReadToList<ItemDto>().ToList();
            });

            return itemDtos;
        }

        public async Task<List<ReporteCambiosPorSector>> GetReporteCambiosDeSector(BanderaFilter filter)
        {
            List<ReporteCambiosPorSector> items = new List<ReporteCambiosPorSector>();

            var sp = this.Context.LoadStoredProc("dbo.sp_HBandera_ReporteCambioSectores")
                .WithSqlParam("cod_lin", new SqlParameter("banderaId", filter.cod_band));


            await sp.ExecuteStoredProcAsync((handler) =>
            {
                items = handler.ReadToList<ReporteCambiosPorSector>().ToList();
            });

            return items;
        }

        public override async Task<PagedResult<HBanderas>> GetAllAsync(Expression<Func<HBanderas, bool>> predicate, List<Expression<Func<HBanderas, object>>> includeExpression = null)
        {
            try
            {
                IQueryable<HBanderas> query = Context.Set<HBanderas>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }

                query = query.Include(e => e.RamalColor).Include(e => e.RamalColor.PlaLinea);

                return new PagedResult<HBanderas>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
    }

}
