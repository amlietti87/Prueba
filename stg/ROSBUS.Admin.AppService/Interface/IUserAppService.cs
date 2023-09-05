using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IUserAppService : IAppServiceBase<SysUsersAd, UserDto,int>
    {
        Task<SysUsersAd> Login(string Username, string Password, string CaptchaValue, Boolean isWebSite = false);

        Task<SysUsersAd> GetByUserLdap(string Username);

        Task<SysUsersAd> GetByUserLdapForAdd(string Username);
        Task<GetPermissionsForEditOutput> GetUserPermissionsForEdit(int id);
        Task UpdateUserPermissions(UpdateUserPermissionsInput input);

        //Task<string[]> GetPermissionForCurrentUser();

        Task<ResetPasswordOutput> ResetPassword(ResetPasswordInput input);
        Task<String> ResetPassword(int id);

        

        Task<GetUserLineasForEdit> GetUserLineasForEdit(int id);
        Task SetUserLineasForEdit(GetUserLineasForEdit input);
        Task<List<ItemDto>> GetUserIntrabus(CredentialsIntrabusModel accessToken);
    }
}
