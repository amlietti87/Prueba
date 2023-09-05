using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ROSBUS.Admin.Domain.Model
{


    public class DiagramaMesAnioDto
    {
        public string Estado { get; set; }
        public int Mes { get; set; }
        public int Anio { get; set; }
        public string GrupoInspectores { get; set; }
        public long GrupoInspectoresId { get; set; }
        public List<DiasMesDto> DiasMes { get; set; }
        public List<DiasMesDto> DiasMesAP { get; set; }

        public DateTime BlockDate { get; set; }

    }

 
    public class DiasMesDto
    {
        public DateTime Fecha { get; set; }
        public int CodDia { get; set; }
        public string NombreDia { get; set; }
        public int NumeroDia { get; set; }
        public string Color { get; set; }
        public string EsFeriadoString { get; set; }


        public DateTime? BlockDate { get; set; }

        public bool EsFeriado => this.EsFeriadoString == "S" ? true : false;

        public List<InspectorDiaDto> Inspectores { get; set; }

    }

    public class DiagramacionSave
    {
        public int Id { get; set; }

        public DateTime BlockDate { get; set; }

        public List<InspectorDiaDto> Inspectores { get; set; }
    }


    public class InspectorDiaDto {
        public InspectorDiaDto()
        {
            this.Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }
        public string Apellido { get; set; }
        public string Nombre { get; set; }
        public string Legajo { get; set; }
        public string CodEmpleado { get; set; }

        public int? EmpleadoId { get; set; }

        public string InspColor { get; set; }
        public string InspTurno { get; set; }
        public int InspTurnoId { get; set; }
        public string DescripcionInspector
        {
            get
            {
                return String.Format("{0} {1}", this.Apellido.Trim(), this.Nombre?.ToCharArray().FirstOrDefault());
            }
        }
        public bool EsJornada { get; set; }
        public int CodJornada { get; set; }
        public bool EsFranco { get; set; }
        public int CantFrancos { get; set; }
        public bool EsFrancoTrabajado { get; set; } 
        public int Pago { get; set; }
        public bool EsNovedad { get; set; }
        public bool FrancoNovedad { get; set; }
        public int? RangoHorarioId { get; set; }
        public int? ZonaId { get; set; }
        public DateTime? HoraDesde { get; set; }
        public DateTime? HoraHasta { get; set; }
        public DateTime? HoraDesdeModificada { get; set; }
        public DateTime? HoraHastaModificada { get; set; }
        public string Color { get; set; }
        public string NombreZona { get; set; }
        public string DetalleZona { get; set; }
        public string NombreRangoHorario { get; set; }
        public string DescNovedad { get; set; }
        public string DetalleNovedad { get; set; }
        public string PasadaSueldos { get; set; }
        public long GrupoInspectoresId { get; set; }
        public DateTime? diaMesFecha { get; set; }
    }


}
