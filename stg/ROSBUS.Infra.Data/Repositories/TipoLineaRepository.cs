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
using ROSBUS.Admin.Domain.Entities.Filters;
using System.Data.SqlClient;
using Snickler.EFCore;

namespace ROSBUS.infra.Data.Repositories
{
    public class TipoLineaRepository : RepositoryBase<AdminContext, PlaTipoLinea, int>, ITipoLineaRepository
    {

        public TipoLineaRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<PlaTipoLinea, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        public async Task<List<PlaTipoLinea>> RecuperarTipoLineaPorSector(HDesignarFilter Filtro)
        {
            List<PlaTipoLinea> itemDtos = new List<PlaTipoLinea>();

            var sp = this.Context.LoadStoredProc("dbo.sp_Insp_RecuperarTipoLineaPorSector")

                .WithSqlParam("fecha", new SqlParameter("fecha", Filtro.Fecha))
                .WithSqlParam("CoordenadaId", new SqlParameter("CoordenadaId", (Object)Filtro.Sector))
                .WithSqlParam("SentidoBanderaId", new SqlParameter("SentidoBanderaId", (Object)Filtro.Sentido));

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                itemDtos = handler.ReadToList<PlaTipoLinea>().ToList();
            });

            return itemDtos;
        }



    }


}
