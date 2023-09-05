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
using TECSO.FWK.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using ROSBUS.Admin.Domain.ParametersHelper;

namespace ROSBUS.infra.Data.Repositories
{
    public class MotivoInfraRepository : RepositoryBase<AdminContext,MotivoInfra, String>, IMotivoInfraRepository
    {
        private readonly IParametersHelper parametersHelper;

        public MotivoInfraRepository(IAdminDbContext _context, IParametersHelper _parametersHelper)
            :base(new DbContextProvider<AdminContext>(_context))
        {
            parametersHelper = _parametersHelper;
        }

        public override Expression<Func<MotivoInfra, bool>> GetFilterById(String id)
        {
            return e => e.Id == id;
        }

        public override Task<PagedResult<MotivoInfra>> GetAllAsync(Expression<Func<MotivoInfra, bool>> predicate, List<Expression<Func<MotivoInfra, object>>> includeExpression = null)
        {
            var codMotivoParameter = this.parametersHelper.GetParameter<string>("insp_InformesMotivos");

            List<string> tiponum = codMotivoParameter.Split(',').ToList();

            var motivos = base.GetAllAsync(predicate, includeExpression);

            List<MotivoInfra> motivosFiltrados = new List<MotivoInfra>();

            foreach(var mot in motivos.Result.Items)
            {
                bool param = tiponum.Contains(mot.Tipo.ToString());

                if(param)
                {
                    motivosFiltrados.Add(mot);
                }
            }
            motivos.Result.TotalCount = motivosFiltrados.Count;
            motivos.Result.Items = motivosFiltrados;

            return motivos;
        }

    }
}
