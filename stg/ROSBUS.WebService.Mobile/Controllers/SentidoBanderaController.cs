using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Domain.Services;
using TECSO.FWK.Extensions;

namespace ROSBUS.WebService.Mobile.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize()]
    public class SentidoBanderaController : TECSO.FWK.ApiServices.ControllerBase
    {
        public SentidoBanderaController(IPlaSentidoBanderaAppService _service)
        {
            this.Service = _service;
        }

        public IPlaSentidoBanderaAppService Service { get; }



        [HttpGet]
        public virtual async Task<IActionResult> GetItemsAsync(PlaSentidoBanderaFilter filter)
        {
            try
            {
                var r = await this.Service.GetItemsAsync(filter);
                return ReturnData<List<ItemDto<int>>>(r);

            }
            catch (Exception ex)
            {
                return ReturnError<List<ItemDto<int>>>(ex);
            }
        }


    }



    public abstract class RedirectBaseController : TECSO.FWK.ApiServices.ControllerBase
    {
        public RedirectBaseController(IConfiguration _configuration)
            :base()
        {
            
            configuration = _configuration;
            urlPath = string.Format("{0}{1}/", GetUrlBase(), "SentidoBandera");
            BuildClientHttp();
        }


        public abstract string EndPoint { get; }

        protected HttpCustomClient httpClient;

        
        protected IConfiguration configuration;

        public string urlPath { get; }

        protected virtual string GetUrlBase()
        {
            return configuration.GetValue<string>("IdentityUrl").EnsureEndsWith('/'); 
        }
        
        protected virtual void BuildClientHttp()
        {
            this.httpClient = new HttpCustomClient(urlPath, ()=> authService.GetCurretToken());
        }
    }

}
