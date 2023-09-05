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
    public class InvolucradosDto : EntityDto<int>
    {
        public InvolucradosDto()
        {
            this.DetalleLesion = new List<DetalleLesionDto>();
        }

        public int SiniestroId { get; set; }
        public int TipoInvolucradoId { get; set; }
        public string NroInvolucrado { get; set; }
        public string ApellidoNombre { get; set; }
        public int? TipoDocId { get; set; }
        public string NroDoc { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? LocalidadId { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string Detalle { get; set; }
        public int? ConductorId { get; set; }
        public int? VehiculoId { get; set; }
        public int? LesionadoId { get; set; }
        public int? MuebleInmuebleId { get; set; }
        public string DescripcionInv { get; set; }
        public override string Description => DescripcionInv;
        public int? NroInvolucradoPuro {
            get
            {
                   
                if (!String.IsNullOrWhiteSpace(this.NroInvolucrado))
                {
                    var split = this.NroInvolucrado.Split('/').LastOrDefault();
                    if (!String.IsNullOrWhiteSpace(split))
                    {
                        int nro;
                        if (int.TryParse(split, out nro))
                            return nro;

                        return null;
                    }
                    else
                    {
                        return null;
                    }
                        
                }
                else
                {
                    return null;
                }
                

            }

        }

        public ConductorDto Conductor { get; set; }
        public VehiculosDto Vehiculo { get; set; }

        public LesionadosDto Lesionado { get; set; }

        public List<DetalleLesionDto> DetalleLesion { get; set; }

        public MuebleInmuebleDto MuebleInmueble { get; set; }



        public string TipoInvolucradoNombre { get; set; }
        public string TipoDocNombre { get; set; }
        public string VehiculoNombre { get; set; }
        public string MuebleInmuebleNombre { get; set; }
        public int?  MuebleInmuebleLocalidadID { get; set; }

        public string LesionadoNombre { get; set; }


        public string ConductorNombre { get; set; }

        public string InvolucradoColumn
        {
            get
            {
                string result;
                result = ApellidoNombre;
                if (!String.IsNullOrWhiteSpace(TipoDocNombre))
                {
                    result = result + " - " + TipoDocNombre;
                }
                if (!String.IsNullOrWhiteSpace(NroDoc))
                {
                    result = result + " - " + NroDoc;
                }
                return result;
            }
        }

        public string EstadoInsercion { get; set; }
        public Boolean TieneConductor { get; set; }
        public Boolean TieneVehiculo { get; set; }
        public Boolean TieneLesionado { get; set; }
        public Boolean TieneMuebleInmueble { get; set; }
        public ItemDto selectLocalidades { get; set; }
    }
}
