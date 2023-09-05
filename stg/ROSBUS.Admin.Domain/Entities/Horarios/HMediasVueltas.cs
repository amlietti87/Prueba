using System;
using System.Collections.Generic;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HMediasVueltas : TECSO.FWK.Domain.Entities.Entity<int>
    {
        public HMediasVueltas()
        {
            HProcMin = new HashSet<HProcMin>();
        }

        //    public int CodMvuelta { get; set; }

        public int CodServicio { get; set; }
        public DateTime Sale { get; set; }
        public DateTime Llega { get; set; }
        public decimal DifMin { get; set; }
        public decimal Orden { get; set; }
        public int CodBan { get; set; }
        public string CodTpoHora { get; set; }
        public int? CodMvueltabsas { get; set; }

        public HBanderas CodBanNavigation { get; set; }
        public HServicios CodServicioNavigation { get; set; }
        public HTposHoras CodTpoHoraNavigation { get; set; }
        public ICollection<HProcMin> HProcMin { get; set; }


    }




    public class HMediasVueltasView
    {
        public HMediasVueltasView()
        {
           
        }
        public int CodMvuelta { get; set; }
        public int CodServicio { get; set; }

        public string NumServicio { get; set; }
        public DateTime Sale { get; set; }
        public DateTime Llega { get; set; }
        public decimal DifMin { get; set; }        

        public int CodBan { get; set; }
        public string DescripcionBandera { get; set; }

        public string CodTpoHora { get; set; }
        public string DescripcionTpoHora { get; set; }

        public decimal CodSubGalpon { get; set; }
        public string DescripcionSubGalpon { get; set; }
    }


    public class HMediasVueltasImportadaDto
    {
        public HMediasVueltasImportadaDto()
        {
            Errors = new List<string>();
        }


        public int NumServicio { get; set; }
        public DateTime Sale { get; set; }
        public DateTime Llega { get; set; }
        public decimal DifMin { get; set; }
        public int OrdenImportado { get; set; } 
        public int CodBan { get; set; } 
        public Boolean EsPosicionamiento { get; set; } 
        public string DescripcionBandera { get; set; }

        public string CodTpoHora { get; set; }
        public string DescripcionTpoHora { get; set; }

        public decimal CodSubGalpon { get; set; }
        public string DescripcionSubGalpon { get; set; }

        public List<string> Errors { get; set; }
        public bool IsValid { get; set; }
    }

    public partial class ImportarHorariosDto
    {
        public string Servicio { get; set; }
        public string Sale { get; set; }
        public Boolean Sale_EsDiaPosterior { get; set; }

        public string Llega { get; set; }

        public Boolean Llega_EsDiaPosterior { get; set; }

        public string Bandera { get; set; }
        public string dsc_TpoHora { get; set; }
        public string des_subg { get; set; }

        public Boolean TieneFormatoFechaInvalido { get; set; }
    }
}
