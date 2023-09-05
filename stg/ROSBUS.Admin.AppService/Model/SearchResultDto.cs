using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class SearchResultDto
    {
        public SearchResultDto()
        {
            this.Banderas = new List<SearchResultBandera>();
            this.Lineas = new List<SearchResultLinea>();
            this.Ramales = new List<SearchResultRamal>();
        }

        
        public int? TotalRegistrosEncontrados  { get; set; }

        public List<SearchResultBandera> Banderas { get; set; }

        public List<SearchResultLinea> Lineas { get; set; }

        public List<SearchResultRamal> Ramales { get; set; }
  
    }


    public class SearchResultBandera : ItemDecimalDto
    {
        public int? SucursalId { get; set; }
    }

    public class SearchResultLinea : ItemDecimalDto
    {
        public int? SucursalId { get; set; }
    }

    public class SearchResultRamal : ItemLongDto
    {
        public SearchResultRamal()
        {
        }

        public SearchResultRamal(long value, string displayText,  decimal LineaID ) : base(value, displayText)
        {
            this.LineaID = LineaID;
        }
        public int? SucursalId { get; internal set; }

        public decimal LineaID   { get; set; }
    }
}
