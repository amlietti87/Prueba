using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Caching;

namespace ROSBUS.Admin.Domain.ParametersHelper
{
    public class ParametersHelper: IParametersHelper
    {
        private readonly ISysParametersService sysParametersService;
        private readonly ICacheManager cacheManager;
        private const string cacheKey = "ParametersKey";

        public const string CantidadIntentosLoginKey = "Loguin_CantidadIntentos";

        public ParametersHelper(ISysParametersService _sysParametersService, ICacheManager _cacheManager)
        {
            this.sysParametersService = _sysParametersService;
            this.cacheManager = _cacheManager;
        }

        private List<SysParameters> _parameters;

        private List<SysParameters> Parameters
        {
            get
            {
                if (_parameters==null)
                {
                    _parameters = this.cacheManager.GetCache(cacheKey).Get<string, List<SysParameters>>(cacheKey, this.cacheFunction);
                }

                return _parameters;
            }
            
        }

        private List<SysParameters> cacheFunction(string arg)
        {
            return this.sysParametersService.GetAll(e => true).Items.ToList();
        }


        public T GetParameter<T>(string token)
        {
            SysParameters par = this.Parameters.FirstOrDefault(e => e.Token == token);

            if (par!=null)
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
    }
}
