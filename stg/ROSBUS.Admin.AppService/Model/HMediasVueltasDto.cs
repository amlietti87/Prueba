using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class HMediasVueltasDto : EntityDto<int>
    {
        public int CodServicio { get; set; }
        public DateTime Sale { get; set; }
        public DateTime Llega { get; set; }
        public decimal DifMin { get; set; }
        public decimal Orden { get; set; }
        public int CodBan { get; set; }
        public string CodTpoHora { get; set; }
        public int? CodMvueltabsas { get; set; }
        public string DescripcionTpoHora { get; set; }
        public int Dia
        {
            get
            {
                return this.Sale.Day;
            }
        }
        public string DescripcionBandera { get; set; }

        public override string Description => this.CodServicio.ToString();
    }



}
