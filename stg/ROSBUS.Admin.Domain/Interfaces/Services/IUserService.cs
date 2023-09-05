using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IUserService : IServiceBase<SysUsersAd, int>
    {
        Task<SysUsersAd> Login(string Username, string Password);

        Task<List<SysUsersRoles>> GetUserRoles(int id);

        Task<SysUsersAd> GetByUserLdap(string Username);
        Task<List<SysPermissions>> GetGrantedPermissionsAsync(int userId);
        Task SetGrantedPermissionsAsync(int id, List<string> grantedPermissionNames);
        Task<List<PlaLineasUsuarios>> GetUserLineasForEdit(int id);
        Task SetUserLineasForEdit(int id, List<ItemDecimalDto> lineas);
        Task<bool> TieneDiagramaActivo(int userId);
        Task<List<ItemDto>> GetUserIntrabus(CredentialsIntrabusModel accessToken);
    }
}
