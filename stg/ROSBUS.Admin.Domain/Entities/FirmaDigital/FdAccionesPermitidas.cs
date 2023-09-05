using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.FirmaDigital
{
    public partial class FdAccionesPermitidas : Entity<int>
    {

        public const int AbrirArchivo = -1;
        public const int DescargarArchivo = -2;
        public const int RechazarDocumento = -3;
        public const int VerDetalleDocumento = -4;
        public const int ExportarExcel = -5;
        public const int RevisarArchivo = -6;
        public const int EnviarCorreo = -7;

        public const int AprobarDocumento = 1;
        public const int FirmarEmpleador = 2;
        public const int FirmarEmpleado = 3;
        public const int RespuestaMinisterio = 4;

        public FdAccionesPermitidas()
        {
            FdAcciones = new HashSet<FdAcciones>();
        }

        public long PermissionId { get; set; }
        public string DisplayName { get; set; }

        public SysPermissions Permiso { get; set; }

        public ICollection<FdAcciones> FdAcciones { get; set; }
    }
}
