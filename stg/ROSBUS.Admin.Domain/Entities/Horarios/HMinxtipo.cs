using System;
using System.Collections.Generic;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HMinxtipo : TECSO.FWK.Domain.Entities.Entity<int>
    {
        public HMinxtipo()
        {
            HDetaminxtipo = new HashSet<HDetaminxtipo>();
        }

     //   public int CodMinxtipo { get; set; }
        public int CodHfecha { get; set; }
        public int CodTdia { get; set; }
        public int CodBan { get; set; }
        public string TipoHora { get; set; }
        public decimal TotalMin { get; set; }
        public int? CodMinxtipobsas { get; set; }

        public HBanderas CodBanNavigation { get; set; }
        public HFechasConfi CodHfechaNavigation { get; set; }
        public HTipodia CodTdiaNavigation { get; set; }
        public HTposHoras TipoHoraNavigation { get; set; }
        public ICollection<HDetaminxtipo> HDetaminxtipo { get; set; }
    }



    public class ImportadorHMinxtipo
    {
        public ImportadorHMinxtipo()
        {
            this.Cabeceras = new List<CabeceraExcelImportado>();
            this.Sectores = new List<SectorImportador>();
        }

        public List<CabeceraExcelImportado> Cabeceras { get; set; }

        public List<SectorImportador> Sectores { get; set; }


        public int CodHFecha { get; set; }

        public int Cod_Band { get; set; }

    }

    public class CabeceraExcelImportado
    {

        public CabeceraExcelImportado()
        {
            this.Detalle = new List<DetalleExcelImportado>();
        }
        public string TipoHora { get; set; }
        public string TotalMin { get; set; }
        public decimal Suma { get; set; }

        public string TipoDia { get; set; }
        public string Bandera { get; set; }

        public List<string> Errors { get; set; }
        public bool IsValid { get; set; }

        public List<DetalleExcelImportado> Detalle { get; set; }
    }


    public class SectorImportador
    {
        public int Orden { get; set; }
        public string Descripcion { get; set; }
    }

    public class DetalleExcelImportado
    {
        public string Minuto { get; set; }

        public int Orden { get; set; }

        public SectorImportador Sector { get; set; }


    }




    //Resultado Importador

    public class ImportadorHMinxtipoResult
    {
        public ImportadorHMinxtipoResult()
        {
            this.HMinxtipoImportados = new List<HMinxtipoImportado>();
            this.Sectores = new List<SectorImportador>();
        }

        public List<SectorImportador> Sectores { get; set; }

        public List<HMinxtipoImportado> HMinxtipoImportados { get; set; }


    }


    public class HMinxtipoImportado
    {
        public HMinxtipoImportado()
        {
            this.Errors = new List<string>();
            this.Detalles = new List<HDetaminxtipoImportado>();
        }

        public int CodTdia { get; set; }
        public string DescripcionTdia { get; set; }

        public int CodBan { get; set; }
        public String AbrBan { get; set; }


        public string TipoHora { get; set; }
        public string DescripcionTipoHora { get; set; }


        public decimal TotalMin { get; set; }




        public List<HDetaminxtipoImportado> Detalles { get; set; }


        public List<string> Errors { get; set; }
        public bool IsValid { get; set; }
        public int Id_HMinxtipo { get; set; }
    }

    public class HDetaminxtipoImportado
    {
        
        public decimal? Minuto { get; set; }
        public int CodHsector { get; set; }
        public string DescripcionCodHsector { get; set; }
        public int Orden { get; set; }
    }


    public class HMinxtipoImporarInput
    {
        public string PlanillaId { get; set; }

        public int CodHFecha { get; set; }

        public int CodBan { get; set; }
    }
}
