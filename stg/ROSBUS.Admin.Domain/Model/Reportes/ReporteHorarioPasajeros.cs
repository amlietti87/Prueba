using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Model
{

    public partial class ReporteHorarioPasajerosFilter
    {

        public Nullable<decimal> cod_lin { get; set; }
        public Nullable<int> codHfecha { get; set; }
        public string banderasList { get; set; }
        public Nullable<int> codTdia { get; set; }

        public List<int> BanderasIda { get; set; }
        public List<int> BanderasVueltas { get; set; }
        public string SentidoOrigen { get; set; }
        public string SentidoDestino { get; set; }


    }


    public partial class ReporteHorarioPasajeros
    {

        public ReporteHorarioPasajeros()
        {
            this.MediasVueltasIda = new List<ReporteHorarioPasajerosMV>();
            this.MinutosIda = new List<ReporteHorarioPasajerosMinutos>();

            this.MediasVueltasVueltas = new List<ReporteHorarioPasajerosMV>();
            this.MinutosVueltas = new List<ReporteHorarioPasajerosMinutos>();

        }
        public List<ReporteHorarioPasajerosMV> MediasVueltasIda { get; set; }
        public List<ReporteHorarioPasajerosMinutos> MinutosIda { get; set; }

        public List<ReporteHorarioPasajerosMV> MediasVueltasVueltas { get; set; }
        public List<ReporteHorarioPasajerosMinutos> MinutosVueltas { get; set; }

    }
    public partial class ReporteHorarioPasajerosMV
    {
        public int cod_mvuelta { get; set; }
        public int cod_servicio { get; set; }
        public System.DateTime sale { get; set; }
        public System.DateTime llega { get; set; }
        public decimal dif_min { get; set; }
        public decimal orden { get; set; }
        public int cod_ban { get; set; }
        public string cod_tpoHora { get; set; }
        public string num_ser { get; set; }
        public string abr_ban { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string PorDonde { get; set; }
        public int cod_tdia { get; set; }
    }
     
    public partial class ReporteHorarioPasajerosMinutos
    {
        public int cod_mvuelta { get; set; }
        public int cod_hsector { get; set; }
        public int cod_sec { get; set; }
        public string descripcion_Sector { get; set; }
        public System.DateTime sale { get; set; }
        public decimal minuto { get; set; }
        public bool MostrarEnReporte { get; set; }

        public int cod_tdia { get; set; }

        public Nullable<System.DateTime> SaleCalculado { get; set; }
        public int orden { get; set; }
        public string Descripcion { get; set; }
    }

    public partial class ReporteHorarioPasajerosItemGalpon
    {
        public int key { get; set; }
        public string Name { get; set; }
        public int Orden { get; set; }
        public decimal? OrdenNuevo { get; set; }

        public ReporteHorarioPasajerosItemGalpon(int cod_hsector, string descripcion_Sector, int orden)
        {
            this.key = cod_hsector;
            this.Name = descripcion_Sector;
            this.Orden = orden;
        }


    }

    public partial class ReporteHorarioRuta
    {

        public ReporteHorarioRuta()
        {
            this.MediasVueltas = new List<ReporteHorarioRutaMV>();
            this.Minutos = new List<ReporteHorarioRutaMinutos>();

        }
        public List<ReporteHorarioRutaMV> MediasVueltas { get; set; }
        public List<ReporteHorarioRutaMinutos> Minutos { get; set; }

    }
    public partial class ReporteHorarioRutaMV
    {
        public int cod_mvuelta { get; set; }
        public int cod_servicio { get; set; }
        public System.DateTime sale { get; set; }
        public System.DateTime llega { get; set; }
        public decimal dif_min { get; set; }
        public decimal orden { get; set; }
        public int cod_ban { get; set; }
        public decimal cod_Lin { get; set; }
        public string num_ser { get; set; }
        public string abr_ban { get; set; }
        public string Origen { get; set; }
        public string Destino { get; set; }
        public string PorDonde { get; set; }
        public int cod_tdia { get; set; }
    }
    public partial class ReporteHorarioRutaMinutos
    {
        public int codigoMediaVuelta { get; set; }
        public int cod_hsector { get; set; }
        public int cod_sec { get; set; }
        public string descripcion_Sector { get; set; }
        public System.DateTime sale { get; set; }
        public decimal minuto { get; set; }
        public bool MostrarEnReporte { get; set; }

        public int cod_tdia { get; set; }
        public int cod_ban { get; set; }
        public Nullable<System.DateTime> SaleCalculado { get; set; }
        public decimal orden { get; set; }
    }


}
