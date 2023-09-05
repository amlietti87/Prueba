using System;
using System.Collections.Generic;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SysUsersAd : TecsoUser<SysUsersAd>
    {
        public SysUsersAd()
        {
            DshUsuarioDashboardItem = new HashSet<DshUsuarioDashboardItem>();
            PermissionsUsers = new HashSet<SysPermissionsUsers>();
            UserRoles = new HashSet<SysUsersRoles>();
            PlaLineasUsuarios = new HashSet<PlaLineasUsuarios>();
            FdDocumentosProcesadosHistorico = new HashSet<FdDocumentosProcesadosHistorico>();
            FdCertificadosUsuario = new HashSet<FdCertificados>();
            FdCertificadosCreatedUser = new HashSet<FdCertificados>();
            FdCertificadosLastUpdatedUser = new HashSet<FdCertificados>();
        }

        public string NomUsuario { get; set; }
        public string LogonName { get; set; }
        public string LogicalLogon { get; set; }
        public string DisplayName { get; set; }
        public string Mail { get; set; }
        public string Description { get; set; }
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
        

        public bool? PermiteLoginManual { get; set; }
        public string PasswordHash { get; set; }


        public string PasswordResetCode { get; set; }

        public bool? EmailConfirmed { get; set; }

        public int? AccessFailedCount { get; set; }


        public ICollection<SysPermissionsUsers> PermissionsUsers { get; set; }
        public ICollection<SysUsersRoles> UserRoles { get; set; }


        public int? SucursalId { get; set; }
        public int? DashboardLayoutId { get; set; }
        public long? GruposInspectoresId { get; set; }
        public int? EmpleadoId { get; set; }
        public int? TurnoId { get; set; }

        public string DescEmpleado { get; set; }

        public ICollection<PlaLineasUsuarios> PlaLineasUsuarios { get; set; }
        public ICollection<DshUsuarioDashboardItem> DshUsuarioDashboardItem { get; set; }
        public ICollection<FdDocumentosProcesadosHistorico> FdDocumentosProcesadosHistorico { get; set; }
        public IEnumerable<FdCertificados> FdCertificadosCreatedUser { get; set; }
        public IEnumerable<FdCertificados> FdCertificadosLastUpdatedUser { get; set; }
        public IEnumerable<FdCertificados> FdCertificadosUsuario { get; set; }

        public void SetNewPasswordResetCode()
        {            
            PasswordResetCode = Guid.NewGuid().ToString("N").Truncate(10).ToUpperInvariant();
        }

    }
}
