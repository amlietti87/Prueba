using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class SectorDto : EntityDto<Int64>
    {
        [StringLength(100)]
        public string Descripcion { get; set; }

        public override string Description => this.Descripcion;
        public Guid PuntoInicioId { get; set; }
        public Guid PuntoFinId { get; set; }
        public string Data { get; set; }
        public decimal? DistanciaKm { get; set; }

        public long RutaId { get; set; }
        public string Color { get; set; }
        public int BanderaId { get; set; }
    }

    public class RutaSectoresDto : EntityDto<Int64>
    {
        public RutaSectoresDto()
        {
            this.Sectores = new List<SectorViewDto>();
        }

        public List<SectorViewDto> Sectores { get; set; }
        
        public override string Description => this.Nombre;

        public string Nombre { get; set; }

        public int BanderaId { get; set; }

    }

    public class SectorViewDto : EntityDto<Int64>
    {
        public override string Description => this.desc;
        public string name { get; set; }
        public int value { get; set; }
        public string desc { get; set; }
        public Boolean EsCambioSector { get; set; }

        public SectorViewDto()
        {
            this.Items = new List<ItemSectorViewDto>();
        }

        public SectorViewDto(
       int _value,
        string _name,
       string _desc,
       Boolean _EsCambioSector
            ) : this()
        {
            this.name = _name;
            this.value = _value;
            this.desc = _desc;
            this.EsCambioSector = _EsCambioSector;
        }


        public List<ItemSectorViewDto> Items { get; set; }


    }

    public class ItemSectorViewDto
    {


        public ItemSectorViewDto()
        {

        }

        public ItemSectorViewDto(
            int _value,
            string _name,
            string _desc
            )
        {
            this.name = _name;
            this.value = _value;
            this.desc = _desc; 
        }

        public string name { get; set; }
        public int value { get; set; }
        public string desc { get; set; }
    }
}
