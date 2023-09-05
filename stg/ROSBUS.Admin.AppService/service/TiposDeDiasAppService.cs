using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class TiposDeDiasAppService : AppServiceBase<HTipodia, TiposDeDiasDto, int, ITiposDeDiasService>, ITiposDeDiasAppService
    {
        public TiposDeDiasAppService(ITiposDeDiasService serviceBase) 
            :base(serviceBase)
        {
         
        }

        public async Task<List<KeyValuePair<bool, string>>> DescripcionPredictivo(TiposDeDiasFilter filtro)
        {
            return await this._serviceBase.DescripcionPredictivo(filtro);
        }
    }
}
