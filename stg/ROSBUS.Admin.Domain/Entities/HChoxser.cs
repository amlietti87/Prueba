using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class HChoxser : Entity<int>
    {

        //cod_servicio --> entity.Property(e => e.Id).HasColumnName("cod_servicio");

        public int CodUni { get; set; }
        public DateTime Sale { get; set; }
        public DateTime Llega { get; set; }
        public string CodEmp { get; set; }
        public int? TipoMultiple { get; set; }

        public DateTime? SalePlanificado { get; set; }

        public DateTime? LlegaPlanificado { get; set; }

        public int? DuracionPlanificada { get; set; }

    }

    public class HChoxserExtendedDto
    {
        public string DescripcionServicio { get; set; }

        public DateTime? Sale { get; set; }
        public DateTime? Llega { get; set; }


        public DateTime? SaleRelevo { get; set; }
        public DateTime? LlegaRelevo { get; set; }

        public DateTime? SaleAuxiliar { get; set; }
        public DateTime? LlegaAuxiliar { get; set; }


        public int? Duracion { get; set; }
        public int? DuracionRelevo { get; set; }
        public int? DuracionAuxiliar { get; set; }


        public string DuracionFomat
        {
            get
            {
                return FormatearMinutos(this.Duracion);
            }
        }


        public string DuracionRelevoFomat
        {
            get
            {
                return FormatearMinutos(this.DuracionRelevo);
            }
        }


        public string DuracionAuxiliarFomat
        {
            get
            {
                return FormatearMinutos(this.DuracionAuxiliar);
            }
        }

        private string FormatearMinutos(int? duracion)
        {
            if (duracion.HasValue)
            {
                TimeSpan ts = TimeSpan.FromMinutes(Convert.ToDouble(duracion));

                return String.Format("{0:hh}:{0:mm}", ts);
            }

            return string.Empty;
        }


    }

    public class ImportadorHChoxser
    {
        public ImportadorHChoxser()
        {
            this.List = new List<ItemImportadorHChoxser>();

        }

        public List<ItemImportadorHChoxser> List { get; set; }



    }

    public class ItemImportadorHChoxser
    {

        public ItemImportadorHChoxser()
        {
            this.Errors = new List<string>();
        }

        public string servicio { get; set; }

        public string sale { get; private set; }
        public string saleRelevo { get; private set; }
        public string saleAuxiliar { get; private set; }


        public string llega { get; private set; }
        public string llegaRelevo { get; private set; }
        public string llegaAuxiliar { get; private set; }


        public string duracion { get; set; }
        public string duracionRelevo { get; set; }
        public string duracionAuxiliar { get; set; }

        public string tipoDeDia { get; set; }
        public double? DuracionNumber
        {
            get
            {
                return this.duracion.ToTimeSpan()?.TotalMinutes;
            }
        }


        public string DuracionFomat
        {
            get
            {
                return FormatearMinutos(this.DuracionNumber);
            }
        }


        public string DuracionRelevoFomat
        {
            get
            {
                return FormatearMinutos(this.DuracionRelevoNumber);
            }
        }

        public double? DuracionRelevoNumber
        {
            get
            {
                return this.duracionRelevo.ToTimeSpan()?.TotalMinutes;
            }
        }


        public string DuracionAuxiliarFomat
        {
            get
            {
                return FormatearMinutos(this.DuracionAuxiliarNumber);
            }
        }

        public double? DuracionAuxiliarNumber
        {
            get
            {
                return this.duracionAuxiliar.ToTimeSpan()?.TotalMinutes;
            }
        }

        private string FormatearMinutos(double? duracion)
        {
            if (duracion.HasValue)
            {
                TimeSpan ts = TimeSpan.FromMinutes(duracion.Value);

                return String.Format("{0:hh}:{0:mm}", ts);
            }

            return string.Empty;
        }






        public List<string> Errors { get; set; }
        public bool IsValid { get; set; }





        public int? servicioId { get; set; }

        private DateTime? _SaleDate;

        public DateTime? SaleDate
        {
            get { return _SaleDate; }
            set
            {
                _SaleDate = value;
                if (value.HasValue)
                {
                    this.sale = value.Value.ToString("HH:mm");
                }
            }
        }
        


        private DateTime? _SaleRelevoDate;
        public DateTime? SaleRelevoDate
        {
            get { return _SaleRelevoDate; }
            set
            {
                _SaleRelevoDate = value;
                if (value.HasValue)
                {
                    this.saleRelevo = value.Value.ToString("HH:mm");
                }
            }
        }


        private DateTime? _SaleAuxiliarDate;
        public DateTime? SaleAuxiliarDate
        {
            get { return _SaleAuxiliarDate; }
            set
            {
                _SaleAuxiliarDate = value;
                if (value.HasValue)
                {
                    this.saleAuxiliar = value.Value.ToString("HH:mm");
                }
            }
        }
        

        private DateTime? _LlegaDate;
        public DateTime? LlegaDate
        {
            get { return _LlegaDate; }
            set
            {
                _LlegaDate = value;
                if (value.HasValue)
                {
                    this.llega = value.Value.ToString("HH:mm");
                }
            }
        }


        private DateTime? _LlegaRelevoDate;

        public DateTime? LlegaRelevoDate
        {
            get { return _LlegaRelevoDate; }
            set {
                _LlegaRelevoDate = value;
                if (value.HasValue)
                {
                    this.llegaRelevo = value.Value.ToString("HH:mm");
                }
            }
        }

        private DateTime? _LlegaAuxiliarDate;

        public DateTime? LlegaAuxiliarDate
        {
            get { return _LlegaAuxiliarDate; }
            set { _LlegaAuxiliarDate = value;
                if (value.HasValue)
                {
                    this.llegaAuxiliar = value.Value.ToString("HH:mm");
                }
            }
        }

        public int OrdenImportado { get; set; }


        public Boolean TieneTitular
        {
            get
            {
                Boolean tiene = this.SaleDate.HasValue && this.LlegaDate.HasValue && !string.IsNullOrWhiteSpace(this.duracion);

                return tiene;

            }
        }



        public Boolean TieneRelevo
        {
            get
            {
                
                Boolean tiene = this.SaleRelevoDate.HasValue && this.LlegaRelevoDate.HasValue && !string.IsNullOrWhiteSpace(this.duracionRelevo);

                return tiene;

            }
        }


        public Boolean TieneAuxiliar
        {
            get
            {
                Boolean tiene = this.SaleAuxiliarDate.HasValue && this.LlegaAuxiliarDate.HasValue && !string.IsNullOrWhiteSpace(this.duracionAuxiliar);

                return tiene;

            }
        }



        internal DateTime GetPrimersale()
        {
            if (this.TieneTitular)
            {
                return this.SaleDate.Value;
            }
            else if (this.TieneRelevo)
            {
                return this.SaleRelevoDate.Value;
            }
            else
            {
                return this.SaleAuxiliarDate.Value;
            }
        }
        internal DateTime GetUltimpoLlega()
        {
            if (this.TieneAuxiliar)
            {
                return this.LlegaAuxiliarDate.Value;
            }
            else if (this.TieneRelevo)
            {
                return this.LlegaRelevoDate.Value;
            }
            else
            {
                return this.LlegaDate.Value;
            }
        }
    }




    //Resultado Importador

    public class ImportadorHChoxserResult
    {
        public ImportadorHChoxserResult()
        {
             
            this.List = new List<ItemImportadorHChoxser>();

        }

 

        public HChoxserFilter filtro { get; set; }
        public List<ItemImportadorHChoxser> List { get; set; }


    }




}
