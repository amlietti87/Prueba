using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Caching;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class LocalidadesService : ServiceBase<Localidades, int, ILocalidadesRepository>, ILocalidadesService
    {
        private readonly ICacheManager _cacheManager;

        public LocalidadesService(ILocalidadesRepository produtoRepository, ICacheManager cacheManager)
            : base(produtoRepository)
        {
            _cacheManager = cacheManager;
        }

        public async Task<PagedResult<Localidades>> GetAllLocalidades()
        {

            var localidadesCache = await _cacheManager.GetCache<string, PagedResult<Localidades>>("Localidades")
                                            .GetAsync("AllLocalidades", e => this.GetAllAsync(l => true));



            return localidadesCache;
        }
    }
    
}
