using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinInvolucrados : FullAuditedEntity<int>
    {

        public SinInvolucrados()
        {
            SinDetalleLesion = new HashSet<SinDetalleLesion>();
            SinInvolucradosAdjuntos = new HashSet<SinInvolucradosAdjuntos>();
            SinReclamos = new HashSet<SinReclamos>();
        }

        public int SiniestroId { get; set; }
        public int TipoInvolucradoId { get; set; }
        public string NroInvolucrado { get; set; }
        public string ApellidoNombre { get; set; }
        public int? TipoDocId { get; set; }
        public string NroDoc { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? LocalidadId { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Detalle { get; set; }
        public int? ConductorId { get; set; }
        public int? VehiculoId { get; set; }
        public int? LesionadoId { get; set; }
        public int? MuebleInmuebleId { get; set; }

        public SinConductores Conductor { get; set; }
        public SinLesionados Lesionado { get; set; }
        public SinMuebleInmueble MuebleInmueble { get; set; }
        public SinSiniestros Siniestro { get; set; }
        public TipoDni TipoDoc { get; set; }
        public SinTipoInvolucrado TipoInvolucrado { get; set; }
        public SinVehiculos Vehiculo { get; set; }
        public ICollection<SinDetalleLesion> SinDetalleLesion { get; set; }
        public ICollection<SinInvolucradosAdjuntos> SinInvolucradosAdjuntos { get; set; }
        public ICollection<SinReclamos> SinReclamos { get; set; }
        public ICollection<SinReclamosHistoricos> SinReclamosHistoricos { get; set; }
        public string getDescription()
        {
            string result;

            result = string.Format("{0} - {1} - {2} - {3} {4}", this.NroInvolucrado, this.TipoInvolucrado?.Descripcion, this.ApellidoNombre, this.TipoDoc?.Descripcion, this.NroDoc);
            return result;

        }

        public string GetEstadoInsercion()
        {
            string str = string.Empty;

            if (Siniestro != null)
            {
                if (Siniestro.CreatedDate.HasValue && CreatedDate.HasValue && Siniestro.CreatedDate.Value.Date != CreatedDate.Value.Date)
                {
                    str = "Insertado con fecha posterior";
                }
            }

            return str;
        }
    }
}
