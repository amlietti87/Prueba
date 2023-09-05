using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;

using TECSO.FWK.ApiServices;
using ROSBUS.Admin.AppService.Model;
using TECSO.FWK.AppService.Model;
using Microsoft.Extensions.Configuration;
using TECSO.FWK.Domain.Entities;

namespace TECSO.LogServices.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class LogController : ManagerControllerBase<Error, Int64,ErrorFilter>
    {
        public readonly IErrorAppService errorAppService;

        public readonly ILogAppService logAppService;
        private readonly TECSO.FWK.Domain.Entities.LogLevel availableLogs;

        public LogController(IErrorAppService service, ILogAppService _logAppService, IConfiguration configuration)
            
        {
            errorAppService = service;
            logAppService = _logAppService;
            availableLogs = configuration.GetValue<TECSO.FWK.Domain.Entities.LogLevel>("LogsLevels");

        }


        [HttpPost]
        public async Task SaveNewEntity([FromBody]LogDto log)
        {
            try
            {

                if (availableLogs.HasFlag(log.LogLevel))
                {
                    if (log.LogType == LogType.Error)
                        await errorAppService.AddAsync(log);
                    else if (log.LogType == LogType.Log)
                        await logAppService.AddAsync(log);
                }

            }
            catch (Exception ex)
            {

                this.ReturnError<String>(ex);
            }
        }

    

    }
}
