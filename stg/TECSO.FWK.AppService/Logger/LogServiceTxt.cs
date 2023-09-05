using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace TECSO.FWK.AppService
{
    public class LogServicetxt : ILogger
    {
        private IConfiguration Configuration { get; }

        private readonly IAuthService authService;
        private readonly LogLevel availableLogs;

        public LogServicetxt(IConfiguration configuration, IAuthService _authService)
        {
            this.Configuration = configuration;
            authService = _authService;
            var strLog= configuration.GetValue<String>("LogsLevels");
            if (!String.IsNullOrEmpty(strLog))
            {
                availableLogs = Enum.Parse<LogLevel>(strLog);
            }
            
        }

        public async Task Log(LogDto log)
        {
            try
            {
                var folder = Configuration.GetValue<String>("ErrorPathFolder") ?? "temp";

                if (!Directory.Exists(folder))
                    Directory.CreateDirectory(folder);

                
                await File.AppendAllTextAsync(String.Format("{0}\\{1}_{2}.txt", folder, log.UserName, log.SessionId), this.GetMessageLog(log));
            }
            catch (Exception ex)
            {
                Console.Write(ex);
                Debugger.Log(1, "log", this.GetMessageLog(log));
                Console.Write(this.GetMessageLog(log));
            }
            

        }

        private string GetMessageLog(LogDto log)
        {
            StringBuilder sb = new StringBuilder();

            sb.AppendLine("-".PadRight(100, '-'));
            sb.AppendLine(String.Format("LogDate: {0}",log.LogDate));
            sb.AppendLine(String.Format("LogMessage: {0}", log.LogMessage));
            sb.AppendLine(String.Format("LogType: {0}", log.LogType));
            sb.AppendLine(String.Format("LogLevel: {0}", log.Level.ToString()));
            sb.AppendLine(String.Format("SessionId: {0}", log.SessionId));
            sb.AppendLine(String.Format("UserName: {0}", log.UserName));
            sb.AppendLine(String.Format("StackTrace: {0}", log.StackTrace));
            return sb.ToString();
        }

        public async Task Log(LogLevel level, string message)
        {
            LogDto log = new LogDto()
            {
                LogDate = DateTime.Now,
                LogLevel = level,
                LogMessage=message,
                LogType= LogType.Log,
                SessionId= authService.GetSessionID(),
                StackTrace= "",
                UserName= authService.GetCurretUserName()
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

        public bool IsEnableLog(LogDto log)
        {
            return availableLogs.HasFlag(log.LogLevel);
        }
    }


    public class LogServiceDebug : ILogger
    {
        public bool IsEnableLog(LogDto log)
        {
            return true;
        }

        public async Task Log(LogDto log)
        {
            //TODO: se puede sacar el Break

            //if (System.Diagnostics.Debugger.IsAttached)
            //    System.Diagnostics.Debugger.Break();


        }

        public async Task Log(LogLevel level, string message)
        {
            await Task.Delay(10);
        }

        public async Task LogCritical(string message)
        {
            await Task.Delay(10);
        }

        public async Task LogError(string message)
        {
            await Task.Delay(10);
        }

        public async Task LogInformation(string message)
        {
            await Task.Delay(10);
        }

        public async Task LogWarning(string message)
        {
            await Task.Delay(10);
        }
    }
}
