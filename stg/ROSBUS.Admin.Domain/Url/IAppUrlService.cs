using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Extensions;
using System.Linq;
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.Domain.Url
{
    public interface IAppUrlService
    {
        string CreatePasswordResetUrlFormat();

        string GetIconMarkerUrl(int idType);
        Dictionary<string,string> GetAllIconMarkerUrl();
    }


    public abstract class AppUrlServiceBase : IAppUrlService
    {
        public abstract string PasswordResetRoute { get; }

        protected readonly IWebUrlService WebUrlService;

        protected readonly Microsoft.Extensions.Configuration.IConfiguration Configuration;

        protected AppUrlServiceBase(IWebUrlService webUrlService, IConfiguration configuration)
        {
            WebUrlService = webUrlService;
            Configuration = configuration;
        }

        public string CreatePasswordResetUrlFormat()
        {
            var resetLink = WebUrlService.GetSiteRootAddress().EnsureEndsWith('/') + PasswordResetRoute + "?userId={userId}&resetCode={resetCode}";

            return resetLink;
        }

        public string GetIconMarkerUrl(int idType)
        {
            var xx = this.GetAllIconMarkerUrl();

            if (xx.ContainsKey(idType.ToString()))
            {
                return xx[idType.ToString()];
            }

            return null;
        }

        public Dictionary<string, string> GetAllIconMarkerUrl()
        {
            var start = WebUrlService.GetSiteRootAddress().EnsureEndsWith('/') + "assets/img/icons/";

            var xx = Configuration.GetSection("IconConfig").GetChildren().Select(item => new KeyValuePair<string, string>(item.Key, start + item.Value)).ToDictionary(x => x.Key, x => x.Value);

            return xx;
        }
    }
}
