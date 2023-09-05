using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using GeoJSON.Net.Feature;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;

using TECSO.FWK.ApiServices;
using TECSO.FWK.ApiServices.Filters;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.WebService.Admin.Controllers
{
    [Route("[controller]/[action]")]
    [TECSO.FWK.ApiServices.Filters.ApiAuthorize]
    public class UserController : ManagerSecurityController<SysUsersAd, int, UserDto, UserFilter, IUserAppService>
    {
        private IUserAppService userAppService;
        private IMemoryCache _cache;
        private readonly IPuntosAppService puntosAppService;
        private readonly IPermissionAppService permissionAppService;

        public UserController(IUserAppService service, IUserAppService _userAppService, IMemoryCache memoryCache, IPuntosAppService _puntosAppService, IPermissionAppService _permissionAppService)
            : base(service)
        {
            userAppService = _userAppService;
            _cache = memoryCache;
            puntosAppService = _puntosAppService;
            permissionAppService = _permissionAppService;
        }


        protected override void InitializePermission()
        {
            this.InitializePermissionByDefault("Admin", "User");
            this.PermissionContainer.AddPermission("UpdateUserPermissions", "Admin", "User", "Permisos");
            this.PermissionContainer.AddPermission("ResetPassword", "Admin", "User", "Modificar");
            this.PermissionContainer.AddPermission("SetUserLineasForEdit", "Admin", "User", "Modificar");
        }


        [HttpGet]
        public async Task<IActionResult> GetByUserLdapAsync(String userName)
        {
            try
            {

                var entity = await userAppService.GetByUserLdapForAdd(userName);
                var dto = this.MapObject<SysUsersAd, UserDto>(entity);

                return ReturnData(dto);
            }
            catch (Exception ex)
            {
                return ReturnError<UserDto>(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserPermissionsForEdit(int id)
        {
            try
            {
                return ReturnData(await (this.Service as IUserAppService).GetUserPermissionsForEdit(id));
            }
            catch (Exception ex)
            {
                return ReturnError<GetPermissionsForEditOutput>(ex);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetPermissionForCurrentUser()
        {
            try
            {
                return ReturnData(await permissionAppService.GetPermissionForCurrentUser());
            }
            catch (Exception ex)
            {
                return ReturnError<string[]>(ex);
            }
        }
        

        [HttpPost]
        [ActionAuthorize()]
        public async Task<IActionResult> UpdateUserPermissions([FromBody] UpdateUserPermissionsInput input)
        {
            try
            {
                await (this.Service as IUserAppService).UpdateUserPermissions(input);
                return ReturnData<string>(string.Empty);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }

        }



        [HttpPost]
        [ActionAuthorize()]
        public async Task<IActionResult> ResetPassword(int id)
        { 
            try
            {
               var code =  await this.Service.ResetPassword(id);
                return ReturnData<string>(code);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetUserLineasForEdit(int id)
        {
            try
            {
                return ReturnData(await (this.Service as IUserAppService).GetUserLineasForEdit(id));
            }
            catch (Exception ex)
            {
                return ReturnError<GetPermissionsForEditOutput>(ex);
            }
        }

        [HttpPost]
        [ActionAuthorize()]
        public async Task<IActionResult> SetUserLineasForEdit([FromBody] GetUserLineasForEdit input)
        {
            try
            {
                await (this.Service as IUserAppService).SetUserLineasForEdit(input);
                return ReturnData<string>(string.Empty);
            }
            catch (Exception ex)
            {
                return ReturnError<string>(ex);
            }

        }



        [HttpGet]
        public IActionResult SetCahe(string value)
        { 
            _cache.Set("ejemplodeMemory", value);          
            return Ok();  
        }

        [HttpGet]
        public IActionResult GetCahe()
        {
            var r =_cache.Get<string>("ejemplodeMemory");
            return Ok(r);

        }



        //[HttpGet]
        //public FileResult GetKml()
        //{
        //    return this.PhysicalFile(@"C:\ss\1.kml", "application/vnd.google-earth.kml+xml");

        //}

        


    }


}