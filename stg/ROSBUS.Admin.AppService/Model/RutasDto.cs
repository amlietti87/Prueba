using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class RutasDto : EntityDto<int>
    {
        [StringLength(100)]
        public string Nombre { get; set; }
        public override string Description => this.Nombre;
        public int BanderaId { get; set; }
        public int EstadoRutaId { get; set; }
        public int CodHFecha { get; set; }

        [Required]
        public DateTime? FechaVigenciaDesde { get; set; }
        public DateTime? FechaVigenciaHasta { get; set; }

        private string _BanderaNombre;

        public string BanderaNombre
        {
            set
            {
                _BanderaNombre = value;
            }
            get
            {
                return (_BanderaNombre ?? "").TrimEnd();
            }

        }

        public string AbrBan { get; set; }

        private string _CodigoVarianteLinea;

        public string CodigoVarianteLinea
        {
            get { return (_CodigoVarianteLinea ?? "").Trim(); }
            set { _CodigoVarianteLinea = value; }
        }

        public string Instructions { get; set; }

        public String EstadoRutaNombre { get; set; }
        public List<PuntosDto> Puntos { get; set; }
        public List<SectorDto> Sectores { get; set; }
        public int? CopyFromRutaId { get; set; }
        public int? EsOriginal { get; set; }
        public string Calles { get; set; }
        public Boolean Activo { get; set; }
        public int TipoBanderaId { get; set; }
        public int CodLin { get; set; }
        public int? CodSec { get; set; }
        public int? SucursalId { get; set; }


        public bool Vigente
        {
            get
            {
                return this.EstadoRutaId == 2 
                    && this.FechaVigenciaDesde.GetValueOrDefault().Date <= DateTime.Now.Date 
                    && (!this.FechaVigenciaHasta.HasValue || this.FechaVigenciaHasta.Value.Date >= DateTime.Now.Date);
            }
        }

    }
}
