using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Repositories.AppInspectores;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Interface;
using TECSO.FWK.Infra.Data.Repositories;

namespace ROSBUS.Infra.Data.Repositories.AppInspectores
{
    public class GeoRepository : RepositoryBase<AdminContext, InspGeo, long>, IGeoRepository
    {
        public GeoRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {
        }

        public override Expression<Func<InspGeo, bool>> GetFilterById(long id)
        {
            return e => e.Id == id;
        }

        public async Task SaveEntityList(List<InspGeo_Hist> historicos)
        {
            try
            {

                foreach (var item in historicos)
                {
                    await this.Context.InspGeoHist.AddAsync(item);
                }


                foreach (var item in historicos.GroupBy(e=> e.UserName))
                {
                    var inspGeo = this.Context.InspGeo.Where(e => e.UserName == item.Key).FirstOrDefault();

                    var masCercano = item.OrderBy(h => h.CurrentTime).LastOrDefault();

                    bool isNew = false;

                    if (inspGeo==null)
                    {
                        inspGeo = new InspGeo();
                        isNew = true;
                        inspGeo.UserName = item.Key;
                    }

                    inspGeo.Accion = masCercano.Accion;
                    inspGeo.CurrentTime = masCercano.CurrentTime;
                    inspGeo.Latitud = masCercano.Latitud;
                    inspGeo.Longitud = masCercano.Longitud;

                    if (isNew)
                    {
                        this.Context.Entry(inspGeo).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                    }
                    else
                    {
                        this.Context.Entry(inspGeo).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    }
                    

                }


                
                await this.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                throw;
            }
        }
    }
}
