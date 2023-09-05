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
using Microsoft.EntityFrameworkCore;
using ROSBUS.Admin.Domain.Entities.Filters;

namespace ROSBUS.infra.Data.Repositories
{
    public class TiposDeDiasRepository : RepositoryBase<AdminContext, HTipodia, int>, ITiposDeDiasRepository
    {

        public TiposDeDiasRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<HTipodia, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public async Task<List<KeyValuePair<bool, string>>> DescripcionPredictivo(TiposDeDiasFilter filtro)
        {
            var list = new List<KeyValuePair<bool, string>>();
            var list2 = new List<KeyValuePair<bool, string>>();
            if (!String.IsNullOrWhiteSpace(filtro.FilterText))
            {
                if (filtro.TipoDiaId.HasValue)
                {
                    list = await this.Context.PlaDistribucionDeCochesPorTipoDeDia.Where(e => e.CodTdia == filtro.TipoDiaId && e.Descripcion.TrimStart().StartsWith(filtro.FilterText)).Select(f => new KeyValuePair<bool, string>(false, f.Descripcion)).Distinct().ToListAsync();
                    list2 = await this.Context.HTipodia.Where(e => e.Descripcion.StartsWith(filtro.FilterText) && e.IsDeleted == false).Select(f => new KeyValuePair<bool, string>(true, f.Descripcion)).Distinct().ToListAsync();
                    return list.Union(list2).Distinct().ToList();
                }
                else
                {
                    return list2 = await this.Context.HTipodia.Where(e => e.Descripcion.StartsWith(filtro.FilterText) && e.IsDeleted == false).Select(f => new KeyValuePair<bool, string>(true, f.Descripcion)).Distinct().ToListAsync();
                }
            }
            else
            {
                if (filtro.TipoDiaId.HasValue)
                {
                    return await this.Context.HTipodia.Where(e => e.Id == filtro.TipoDiaId && e.IsDeleted == false).Select(f => new KeyValuePair<bool, string>(true, f.Descripcion)).Distinct().ToListAsync();
                }
                else
                {
                    return null;
                }
            }
        }

        public async override Task<HTipodia> AddAsync(HTipodia entity)
        {
            try
            {



                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {


                    entity.Id = this.Context.HTipodia.Max(e => e.Id) + 1;
                    DbSet<HTipodia> dbSet = Context.Set<HTipodia>();
                    var entry = await dbSet.AddAsync(entity);


                    await this.SaveChangesAsync();


                    if (entity.CopiaTipoDiaId.HasValue)
                    {
                        var listaaclonar = this.Context.PlaTiempoEsperadoDeCarga.Where(e => e.TipodeDiaId == entity.CopiaTipoDiaId.Value &&
                        e.TipodeDia.IsDeleted == false
                        );

                        foreach (var item in listaaclonar)
                        {
                            var newt = new PlaTiempoEsperadoDeCarga();

                            newt.TipodeDiaId = entity.Id;
                            newt.HoraDesde = item.HoraDesde;
                            newt.HoraHasta = item.HoraHasta;
                            newt.TiempoDeCarga = item.TiempoDeCarga;
                            newt.TipoParadaId = item.TipoParadaId;
                            Context.Entry(newt).State = EntityState.Added;
                        }

                    }

                    await this.SaveChangesAsync();

                    ts.Commit();
                    return entry.Entity;

                }




            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                throw;
            }

        }

    }
}
