using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.AppService.service.http
{
    public class SignalRHttpService : HttpAppServiceBase, ISignalRHttpService
    {
        private readonly ILogger logger;

        public SignalRHttpService(IAuthService _authService, ILogger _logger) 
            : base(_authService)
        {
            logger = _logger;
        }

        public override string EndPoint => "api/SendMessage";

        public async Task SendMessage(string message, string groupName)
        {
            IDictionary<string, string> parameters = new Dictionary<string, string>();
            parameters.Add("message", message);
            parameters.Add("groupName", groupName);
            string action = "Send";
            try
            {
                var pList = await this.httpClient.GetRequest<string>(action, parameters);
            }
            catch(Exception ex)
            {
                await this.logger.LogError(ex.ToString());   
            }
            
        }

        protected override string GetUrlBase()
        {
            return configuration.GetValue<string>("SignalrUrl").EnsureEndsWith('/');
        }
    }


    public interface ISignalRHttpService {
        Task SendMessage(string message, string groupName);

    }
}
