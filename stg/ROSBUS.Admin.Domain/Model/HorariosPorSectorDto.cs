using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ROSBUS.Admin.Domain
{

    public class HorariosPorSectorDto
    {
        public HorariosPorSectorDto()
        {
            this.Colulmnas = new List<ColumnasDto>();
            this.Items = new List<RowHorariosPorSectorDto>();
        }

        public string Linea { get; set; }
        public decimal LineaId { get; set; }
        public string Empresa { get; set; }
        public decimal EmpresaId { get; set; }
        public string TipoDia { get; set; }
        public string FechaHorario { get; set; }
        public DateTime FechaDesde { get; set; }

        public string LabelBandera { get; set; }
        public List<ColumnasDto> Colulmnas { get; set; }

        public List<RowHorariosPorSectorDto> Items { get; set; }

        public int TipoInforme { get; set; }

    }

    public class ColumnasDto
    {
        public bool EsFija { get; set; }
        public String Key { get; set; }
        public String Label { get; set; }
    }


    public class RowHorariosPorSectorDto
    {
        public int cod_mvuelta { get; set; }

        public RowHorariosPorSectorDto()
        {
            this.ColumnasDinamicas = new List<ColumnasDataDto>();
        }

        public List<ColumnasDataDto> ColumnasDinamicas { get; set; }


        public string Sale { get; set; }

        public string Llega { get; set; }
        public string Servicio { get; set; }
        public string TotalDeMinutos { get; set; }
        public string TipoHora { get; set; }
        public string Bandera { get; set; }
        public string Diferencia { get; set; }

        public decimal CodSubGalpon { get; set; }
        public string SubGalpon { get; set; }
        public TimeSpan? HoraPatron { get; set; }
        public DateTime SaleValue { get; set; }
        public string DesTipoHora { get; set; }

        //public TimeSpan? HoraPatron { 
        //    get
        //    {
        //        var hp = this.ColumnasDinamicas.Where(e => e.EsPatron).FirstOrDefault();
        //        if (hp == null)
        //        {
        //            hp = this.ColumnasDinamicas.FirstOrDefault();
        //        }                    

        //        return hp.Hora;
        //    }
        //}

    }


    public class ColumnasDataDto
    {
        public bool EsFija { get; set; }
        public String Key { get; set; }
        public object value { get; set; }
        public int Orden { get; set; }
        public TimeSpan? Hora { get; set; }

        public String HoraFormated
        {
            get
            {
                if (this.Hora.HasValue)
                {
                    return this.Hora.Value.ToString(@"hh\:mm\:ss");
                }
                return "";
            }
        }

        public bool EsPatron { get; set; }
        public string DescripcionSector { get; set; }
        public bool EsRelevo { get; set; }
    }


    public class RecuperarHorariosSectorPorSectorDto
    {

        public RecuperarHorariosSectorPorSectorDto()
        {
            HMediasVueltas = new List<HMediasVueltasDto>();
            Minutos = new List<hProcMin>();
        }

        public List<HMediasVueltasDto> HMediasVueltas { get; set; }
        public List<hProcMin> Minutos { get; set; }
    }


    public class HMediasVueltasDto
    {
        public int cod_mvuelta { get; set; }

        public int cod_servicio { get; set; }

        public DateTime sale { get; set; }

        public DateTime llega { get; set; }

        public Decimal dif_min { get; set; }

        public Decimal orden { get; set; }

        public int cod_ban { get; set; }

        public string cod_tpoHora { get; set; }

        public string num_ser { get; set; }

        public string abr_ban { get; set; }
        public string DescripcionTpoHora { get; set; }
    }


    public class hProcMin
    {
        public int cod_mvuelta { get; set; }
        public int cod_hsector { get; set; }
        public int cod_sec { get; set; }

        public Decimal minuto { get; set; }

        public string descripcion_Sector { get; set; }
        public int orden { get; set; }
        public bool TrueForAll { get; set; }

        public int ordenNuevo { get; set; }
        public TimeSpan? Hora { get; set; }
        public DateTime? Fecha { get; set; }
        public int cod_servicio { get; set; }
        public bool EsRelevo { get; set; }
        
    }

    public class HorarioTrabajoConductor
    {
        public int cod_servicio { get; set; }
        public DateTime sale { get; set; }
        public DateTime llega { get; set; }
		public string cod_emp { get; set; }

        public int? cod_uni { get; set; }
    }


}
