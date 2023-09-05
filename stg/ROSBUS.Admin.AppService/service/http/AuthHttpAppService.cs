using Microsoft.Extensions.Configuration;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService.service
{
    public class AuthHttpAppService : HttpAppServiceBase, IAuthAppService
    {
        public override string EndPoint => "api/Auth";


        public AuthHttpAppService(IAuthService authService)
            : base(authService)
        {

        }


        protected override string GetCurretToken()
        {
            //return base.GetCurretToken();
            return "";
        }

        public async Task<LoginOutput> Login(string username, string password, string captchaValue)
        {

            string action = "Login";

            var param = new List<KeyValuePair<string, string>>()
                    {
                        new KeyValuePair<string, string>("Username", username),
                        new KeyValuePair<string, string>("Password",password),
                        new KeyValuePair<string, string>("CaptchaValue", captchaValue)
                    };


            var input = new CredentialsViewModel()
            {
                Username = username,
                Password = password,
                CaptchaValue = captchaValue
            };


            var pList = await this.httpClient.PostRequest<LoginOutput>(action, Newtonsoft.Json.JsonConvert.SerializeObject(input));

            return pList;

        }
    }
}
