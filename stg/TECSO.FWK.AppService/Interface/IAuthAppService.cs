using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.AppService.Interface
{
    public interface IAuthAppService : IAppServiceBase
    {
        Task<LoginOutput> Login(string username, string password, string captchaValue);
    }
}
