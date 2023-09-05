using Microsoft.Extensions.Configuration;
using ROSBUS.Admin.Domain.Url;
using TECSO.FWK.Domain.bus;

namespace ROSBUS.WebService.Shared
{
    public class AngularAppUrlService : AppUrlServiceBase
    {
        public override string PasswordResetRoute => "account/reset";

        public AngularAppUrlService(IWebUrlService webUrlService, IConfiguration configuration) : 
            base(webUrlService, configuration
            )
        {

        }
    }
 
}