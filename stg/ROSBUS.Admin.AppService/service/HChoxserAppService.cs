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

    public class HChoxserAppService : AppServiceBase<HChoxser, HChoxserDto, int, IHChoxserService>, IHChoxserAppService
    {
        private readonly IHHorariosConfiService _hHorariosConfiService;
        private readonly IHHorariosConfiAppService _hHorariosConfiAppService;
        public HChoxserAppService(IHChoxserService serviceBase, IHHorariosConfiService hHorariosConfiService, IHHorariosConfiAppService hHorariosConfiAppService) 
            :base(serviceBase)
        {
            this._hHorariosConfiService = hHorariosConfiService;
            this._hHorariosConfiAppService = hHorariosConfiAppService;
        }

        public async Task ImportarDuraciones(HChoxserFilter input)
        {
            await this._serviceBase.ImportarDuraciones(input);
        }

        public async Task<List<HChoxserExtendedDto>> RecuperarDuraciones(HHorariosConfiFilter filter)
        {
            var list= await this._serviceBase.RecuperarDuraciones(filter);


            foreach (var item in list)
            {
                if (item.Duracion.HasValue)
                {
                    //if (item.Duracion > )
                    //{

                    //}
                }
            }

            return list;
        }

        public async Task<ImportadorHChoxserResult> RecuperarPlanilla(HChoxserFilter filter)
        {
            return await this._serviceBase.RecuperarPlanilla(filter);
        }

        public async Task UpdateDurYSer(HChoxserExtendedDto Duracion, HHorariosConfiDto Horario)
        {
            await this._hHorariosConfiAppService.AddOrUpdateDurYSer(Horario, Duracion);

        }

    }
}
