using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

using TECSO.FWK.Domain.Entities;

namespace TECSO.FWK.Domain.Interfaces.Services
{
    public interface ILogger
    {
        Task Log(LogDto log);

        Task Log(LogLevel level, string message);

        Task LogInformation(string message);

        Task LogWarning(string message);

        Task LogError(string message);

        Task LogCritical(string message);

        Boolean IsEnableLog(LogDto log);

    }
}
