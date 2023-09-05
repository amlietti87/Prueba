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
using Snickler.EFCore;
using ROSBUS.Infra.Data;
using System.Data.SqlClient;
using TECSO.FWK.Domain;
using ROSBUS.Admin.Domain.Interfaces.Services;

namespace ROSBUS.infra.Data.Repositories
{
    public class HMediasVueltasRepository : RepositoryBase<AdminContext,HMediasVueltas, int>, IHMediasVueltasRepository
    {
        private readonly IEmpresaService _empresa;

        public HMediasVueltasRepository(IAdminDbContext _context, IEmpresaService empresa)
            :base(new DbContextProvider<AdminContext>(_context))
        {
            this._empresa = empresa;
        }

        public override Expression<Func<HMediasVueltas, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        public async Task<List<HMediasVueltasView>> LeerMediasVueltasIncompletas(HMediasVueltasFilter Filtro)
        {
            List<HMediasVueltasView> viewResult = new List<HMediasVueltasView>();

            var sp = this.Context.LoadStoredProc(SqlObjects.sp_pla_LeerMediasVueltasIncompletas)
             .WithSqlParam("cod_hfecha", new SqlParameter("cod_hfecha", Filtro.CodHfecha))
             .WithSqlParam("cod_tdia", new SqlParameter("cod_tdia", Filtro.CodTdia));
 
            await sp.ExecuteStoredProcAsync((handler) =>
            {
                viewResult = handler.ReadToList<HMediasVueltasView>().ToList();
            });

            return viewResult;
        }

        public async Task<Empresa> GetCodigoEmpresa(decimal CodLinea)
        {  
            var codigoempresa = this.Context.Configu.Where(e => e.CodLin == CodLinea).FirstOrDefault()?.CodEmpr;
            if (codigoempresa == null)
            {
                throw new DomainValidationException(String.Format("No encuentra el código de empresa en la Linea con CodLin: {0}", CodLinea.ToString()));
            }
            var empresaentity = (await _empresa.GetByIdAsync(codigoempresa.Value));
            if (empresaentity == null)
            {
                throw new DomainValidationException(String.Format("No encuentra la empresa número: {0}", codigoempresa.Value.ToString()));
            }
            return empresaentity;
        }
    }
}
