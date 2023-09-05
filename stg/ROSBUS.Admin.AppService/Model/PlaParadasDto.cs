using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class PlaParadasDto :EntityDto<int>
    {
    

        public string Codigo { get; set; }
        public string Calle { get; set; }
        public string Cruce { get; set; }
        public int LocalidadId { get; set; }
        public string Sentido { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public bool Anulada { get; set; }
        public string Localidad { get; set; }
        public bool PickUpType { get; set; }
        public bool DropOffType { get; set; }
        public int LocationType { get; set; }
        public int? ParentStationId { get; set; }
        public ItemDto ParentStation { get; set; }
        public string TipoParadaDesc
        {
            get
            {
                if (LocationType == 0)
                {
                    return "Parada";
                }
                else if (LocationType == 1)
                {
                    return "Estación";
                }
                else if (LocationType == 2)
                {
                    return "Entrada o salida de una estación";
                }
                else
                {
                    return "";
                }
            } }
        public override string Description => this.Codigo;
    }
}
