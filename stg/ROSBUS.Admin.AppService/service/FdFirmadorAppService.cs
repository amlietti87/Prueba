using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class FdFirmadorAppService : AppServiceBase<FdFirmador, FdFirmadorDto, long, IFdFirmadorService>, IFdFirmadorAppService
    {
        public FdFirmadorAppService(IFdFirmadorService serviceBase) 
            :base(serviceBase)
        {
         
        }

        public async Task<FdFirmadorDto> GetFirmadorByToken(string token, int idRecibo)
        {
            var e= await this._serviceBase.GetFirmadorByToken(token, idRecibo);

            return this.MapObject<FdFirmador, FdFirmadorDto>(e);
        }

        public Task UpdateLogs(FdFirmadorDto dto)
        {
            var entity = this._serviceBase.GetById(dto.Id);

            foreach (var item in dto.FdFirmadorLog.Where(e=> e.Id==0))
            {
                item.FirmadorId = entity.Id;
                entity.FdFirmadorLog.Add(this.MapObject<FdFirmadorLogDto, FdFirmadorLog>(item));
            }

            return this._serviceBase.UpdateLogs(entity);
        }
    }
}
