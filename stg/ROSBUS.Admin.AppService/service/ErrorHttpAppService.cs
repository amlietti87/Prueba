using Microsoft.Extensions.Configuration;
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
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.AppService.service
{
    public class ErrorHttpAppService : HttpAppServiceBase<Error, LogDto, Int64>, ILogger
    {
        private readonly LogLevel availableLogs;

        public override string EndPoint => "Log";

        protected override string GetUrlBase()
        {
            return configuration.GetValue<String>("LogsUrl").EnsureEndsWith('/');
        }


        public ErrorHttpAppService(IAuthService authService)
            : base(authService)
        {
            var strLog = configuration.GetValue<String>("LogsLevels");
            if (!String.IsNullOrEmpty(strLog))
            {
                availableLogs = Enum.Parse<LogLevel>(strLog);
            }
        }

        public async Task Log(LogDto log)
        {
            if (this.IsEnableLog(log))
            {
                await this.AddAsync(log);
            }
            

            //if (log.LogType == LogType.Error)
            //{
            //    Error err = new Error()
            //    {
            //        ErrorDate = log.LogDate,
            //        ErrorMessage = log.LogMessage,
            //        UserName = log.UserName,
            //        SessionId = log.SessionId,
            //        StackTrace = log.StackTrace

            //    };
            //    await this.AddAsync(err);
            //}
            //else if (log.LogType == LogType.Log)
            //{
            //    await this.AddAsync(log);
            //}
        }

        public bool IsEnableLog(LogDto log)
        {
            return availableLogs.HasFlag(log.LogLevel);
        }

        public async Task Log(LogLevel level, string message)
        {
            LogDto log = new LogDto()
            {
                LogDate = DateTime.Now,
                LogLevel = level,
                LogMessage = message,
                LogType = LogType.Log,
                SessionId = this.authService.GetSessionID(),
                StackTrace = "",
                UserName = authService.GetCurretUserName()
            };

            await this.Log(log);
        }

        public async Task LogCritical(string message)
        {
            await this.Log(LogLevel.Critical, message);
        }

        public async Task LogError(string message)
        {
            await this.Log(LogLevel.Error, message);
        }

        public async Task LogInformation(string message)
        {
            await this.Log(LogLevel.Information, message);
        }

        public async Task LogWarning(string message)
        {
            await this.Log(LogLevel.Warning, message);
        }
    }
}
