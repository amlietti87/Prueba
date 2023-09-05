using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class ErrorAppService : AppServiceBase<Error,LogDto, Int64, IErrorService>, IErrorAppService
    {
        public ErrorAppService(IErrorService serviceBase) 
            :base(serviceBase)
        {
        }

        public async Task Log(LogDto log)
        {
            await this.AddAsync(log);

            //if (log.LogType== LogType.Error)
            //{
            //    Error err = new Error()
            //    {
            //        ErrorDate = log.LogDate,
            //        ErrorMessage = log.LogMessage,
            //        UserName = log.UserName,
            //        SessionId = log.SessionId,
            //        StackTrace = log.StackTrace
            //    };
            //    await _serviceBase.AddAsync(err);
            //}
            //else if (log.LogType== LogType.Log)
            //{
            //    await this.AddAsync(log);
            //}
        }
    }

   
}
