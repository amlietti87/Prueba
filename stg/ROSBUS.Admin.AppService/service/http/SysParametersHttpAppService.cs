using Microsoft.Extensions.Configuration;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Interface.ART;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Entities.Filters.ART;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Admin.Domain.Report;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Caching;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Domain.Services;
using TECSO.FWK.Extensions;
using Microsoft.Extensions.DependencyInjection;
using TECSO.FWK.AppService.Interface;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using TECSO.FWK.Domain.Interfaces.Entities;
using ROSBUS.Admin.Domain.ParametersHelper;
using System.Linq;
using ROSBUS.Admin.Domain.Entities.Filters;

namespace ROSBUS.Admin.AppService.service.http
{
    public  class SysParametersHttpAppService : HttpAppServiceBase<SysParameters, SysParametersDto, long>, ISysParametersAppService, IParametersHelper
    {
        private const string cacheKey = "ParametersKey";
        public SysParametersHttpAppService(IAuthService _authService)
            : base(_authService)
        {
            this.useAdminToken = true;
        }

        public override string EndPoint => "SysParameters";

        public void Delete(long id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteAsync(long id)
        {
            throw new NotImplementedException();
        }

        public SysParameters GetById(long id)
        {
            throw new NotImplementedException();
        }

        public Task<SysParameters> GetByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        public Task<SysParametersDto> GetDtoByIdAsync(long id)
        {
            throw new NotImplementedException();
        }

        private List<SysParameters> _parameters;

        private List<SysParameters> Parameters
        {
            get
            {
                if (_parameters == null)
                {
                    _parameters = this.cacheManager.GetCache(cacheKey).Get<string, List<SysParameters>>(cacheKey, this.cacheFunction);
                }

                return _parameters;
            }

        }
        private List<SysParameters> cacheFunction(string arg)
        {
            return this.GetPagedList(new SysParametersFilter()).Items.ToList();
        }

        public T GetParameter<T>(string token)
        {
            SysParameters par = this.Parameters.FirstOrDefault(e => e.Token == token);

            if (par != null)
            {
                var cnv = this.GetValue<T>(par.Value);
                return cnv;
            }

            return default(T);
        }

        private T GetValue<T>(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return default(T);
            }
            return (T)Convert.ChangeType(input, typeof(T));
        }

        Task<List<ItemDto<long>>> IAppServiceBase<SysParameters, long>.GetItemsAsync<TFilter>(TFilter filter)
        {
            throw new NotImplementedException();
        }
    }
}
