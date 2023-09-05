using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ROSBUS.Admin.Domain.Entities;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class UserDto : EntityDto<int>
    {
        public UserDto()
        {
            UserRoles = new List<UserRoleDto>();
        }

        public string NomUsuario { get; set; }
        public string LogonName { get; set; }
        public string LogicalLogon { get; set; }
        public string DisplayName { get; set; }
        public string Mail { get; set; }
        public string CanonicalName { get; set; }
        public string DistinguishedName { get; set; }
        public string UserPrincipalName { get; set; }
        public string Area { get; set; }
        public string TpoNroDoc { get; set; }
        public string TpoDoc { get; set; }
        public string NroDoc { get; set; }
        public string TelTrabajo { get; set; }
        public string TelPersonal { get; set; }
        public string Baja { get; set; }
        public string CodEmp { get; set; }
        public long? GruposInspectoresId { get; set; }
        public int? EmpleadoId { get; set; }
        public int? TurnoId { get; set; }
        public string DescEmpleado { get; set; }

        public bool? PermiteLoginManual { get; set; }

        public override string Description => this.DisplayName;


        public List<UserRoleDto> UserRoles { get; set; }
        public string Password { get; set; }

        public int? SucursalId { get; set; }
        public Boolean EsInspector { get; set; }
        public int? TurnoIdAnterior { get; set; }
    }

    public class UserRoleDto
    {
        public int RoleId { get; set; }

        public string RoleName { get; set; }

        public string RoleDisplayName { get; set; }

        public bool IsAssigned { get; set; }
    }



    public class GetPermissionsForEditOutput
    {
        public List<FlatPermissionDto> Permissions { get; set; }

        public List<string> GrantedPermissionNames { get; set; }

        internal static GetPermissionsForEditOutput GetPermissionsForEdit(IReadOnlyList<SysPermissions> permissions, List<SysPermissions> grantedPermissions)
        {
            var result = new GetPermissionsForEditOutput
            {
                Permissions = permissions.ToList().Select(e => new FlatPermissionDto
                {
                    ParentName = e.Area + "." + e.Page,
                    Description = e.DisplayName,
                    Name = e.Token,
                    DisplayName = e.DisplayName,
                    IsGrantedByDefault = false
                }).ToList(),
                GrantedPermissionNames = grantedPermissions.Select(p => p.Token).ToList()
            };



            foreach (var item in permissions.GroupBy(e => e.Area + "." + e.Page))
            {
                result.Permissions.Add(new FlatPermissionDto()
                {
                    ParentName = item.FirstOrDefault().Area,
                    Description = item.FirstOrDefault().Pages.DisplayName,
                    Name = item.Key,
                    DisplayName = item.FirstOrDefault().Pages.DisplayName,
                    IsGrantedByDefault = false
                });
            }


            foreach (var item in permissions.GroupBy(e => e.Area))
            {
                result.Permissions.Add(new FlatPermissionDto()
                {
                    ParentName = null,
                    Description = item.FirstOrDefault().Areas.DisplayName,
                    Name = item.Key,
                    DisplayName = item.FirstOrDefault().Areas.DisplayName,
                    IsGrantedByDefault = false
                });
            }

            return result;
        }
    }
    public class UpdateUserPermissionsInput
    {
        public int Id { get; set; }
        public List<string> GrantedPermissionNames { get; set; }
    }

    

    public class GetUserLineasForEdit
    {
        public int Id { get; set; }
        public List<ItemDecimalDto> Lineas { get; set; }

        public String Ususario;

    }

 

    public class FlatPermissionDto
    {
        public string ParentName { get; set; }

        public string Name { get; set; }

        public string DisplayName { get; set; }

        public string Description { get; set; }

        public bool IsGrantedByDefault { get; set; }
    }


}
