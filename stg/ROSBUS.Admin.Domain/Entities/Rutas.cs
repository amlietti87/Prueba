using Newtonsoft.Json;
using ROSBUS.Admin.Domain.Entities.Partials;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class GpsRecorridos : FullAuditedEntity<int>
    {
        public GpsRecorridos()
        {
            Puntos = new HashSet<PlaPuntos>();
            Sectores = new HashSet<PlaSector>();
            HBasec = new HashSet<HBasec>();
        }

        //[NotMapped]
        //public override int Id { get => base.Id; set => base.Id = value; }

        public DateTime Fecha { get; set; }

        // public int CodRec { get; set; }

        public int CodLin { get; set; }
        public int CodBan { get; set; }
        public int CodSec { get; set; }
        public int CodMap { get; set; }
        public string Activo { get; set; }
        public int? EstadoRutaId { get; set; }

        public DateTime? FechaVigenciaHasta { get; set; }
        public int EsOriginal { get; set; }
        public string Calles { get; set; }
        public string Nombre { get; set; }

        public string Instructions { get; set; }

        public HBanderas Bandera { get; set; }

        public PlaEstadoRuta EstadoRuta { get; set; }

        public ICollection<PlaPuntos> Puntos { get; set; }
        public ICollection<PlaSector> Sectores { get; set; }

        public ICollection<HBasec> HBasec { get; set; }




        public List<GpsDetaReco> getDetaReco()
        {
            List<GpsDetaReco> positions = new List<GpsDetaReco>();

            var i = 1;

            var c = 0;

            foreach (var item in this.Puntos.OrderBy(e => e.Orden))
            {

                DataInfoPuntos dataInfoPuntos = this.getInfoData(item);

                if (dataInfoPuntos.steps != null)
                {
                    foreach (var path in dataInfoPuntos.steps.SelectMany(e => e.path))
                    {
                        var positionpath = new GpsDetaReco();
                        positionpath.Lat = Convert.ToDecimal(path.lat);
                        positionpath.Lon = Convert.ToDecimal(path.lng);
                        positionpath.Cuenta = i++;
                        positionpath.CodRec = this.Id;
                        positionpath.Sector = "0";
                        positionpath.DscSector = String.Empty;
                        positions.Add(positionpath);
                    }
                }
                else if (c > 0)
                {
                    throw new DomainValidationException("Al menos un punto del mapa no tiene la información de Google generada correctamente");
                }

                c = 1;

                var position = new GpsDetaReco();
                position.Lat = Convert.ToDecimal(item.Lat);
                position.Lon = Convert.ToDecimal(item.Long);
                position.CodRec = this.Id;
                position.Sector = (item.EsCambioSector || item.EsPuntoInicio || item.EsPuntoTermino) ? "1" : "0";
                if (item.EsCambioSector)
                {
                    if (item.PlaCoordenada != null)
                    {
                        position.DscSector = !string.IsNullOrEmpty(item.PlaCoordenada.Descripcion) ? item.PlaCoordenada.Descripcion : string.Format("{0}-{1}", item.PlaCoordenada.DescripcionCalle1, item.PlaCoordenada.DescripcionCalle2);
                    }

                }
                else
                {
                    position.DscSector = string.Empty;
                }

                position.Cuenta = i++;

                positions.Add(position);
            }

            return positions;
        }


        private DataInfoPuntos getInfoData(PlaPuntos item)
        {
            try
            {
                if (!string.IsNullOrEmpty(item.Data))
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<DataInfoPuntos>(item.Data);
            }
            catch (Exception)
            {
                return new DataInfoPuntos();
            }

            return new DataInfoPuntos();
        }

        internal void ValidarPuntosEnEstadoaprobado(IEnumerable<PlaPuntos> puntos, int tipoBanderaId)
        {
            if (puntos.Count() < 2)
            {
                throw new DomainValidationException("El mapa tiene que tener al menos dos puntos");
            }

            if (puntos.Any(e => e.PlaCoordenada?.Anulado == true))
            {
                var puntosCoorAnul = puntos.Where(e => e.PlaCoordenada?.Anulado == true);
                String Coordanul = "";
                foreach (var item in puntosCoorAnul)
                {
                    Coordanul = string.Concat(Coordanul, "Coordenada: " + item.PlaCoordenada.Calle1 + "-" + item.PlaCoordenada.Calle2);
                }
                throw new DomainValidationException("El mapa no puede tener ninguna coordenada anulada. " + Coordanul);
            }

            if (puntos.Any(e => !string.IsNullOrWhiteSpace(e.Data)) == false)
            {
                throw new DomainValidationException("El mapa tiene que estar recreado");
            }


            if (puntos.Any(e => !e.EsPuntoInicio && (e.Data == "" || e.Data == "{}" || e.Data == "{\"steps\":[],\"instructions\":[]}" || e.Data == "{\"steps\":[]}"  || e.Data == "{\"instructions\":[]}")))
            {
                throw new DomainValidationException("El mapa contiene al menos un punto sin datos, revisar mapa");
            }

            if (tipoBanderaId == PlaTipoBandera.Comerciales && !puntos.Any(e => e.EsPuntoInicio && e.EsCambioSectorTarifario && e.CodSectorTarifario.HasValue))
            {
                throw new DomainValidationException("El punto de inicio debe contener un sector tarifario");
            }

            foreach (PlaPuntos punto in puntos.OrderBy(e => e.Orden))
            {
                if (punto.PlaCoordenadaId == null)
                {

                    if (punto.EsPuntoInicio)
                    {
                        throw new DomainValidationException("Falta completar información en el punto inicio");
                    }

                    if (punto.EsCambioSector)
                    {
                        throw new DomainValidationException("Falta completar información en un punto de tipo cambio de sector");
                    }

                    if (punto.EsPuntoTermino)
                    {
                        throw new DomainValidationException("Falta completar información en el punto de término");
                    }
                }

                var steps = JsonConvert.DeserializeObject<PuntoData>(punto.Data);
                if (steps.steps != null && steps.steps.Length == 0 && steps.instructions != null && steps.instructions.Length > 0 && !punto.EsPuntoInicio 
                    || 
                    (steps.steps == null && !punto.EsPuntoInicio )
                    || 
                    (steps.instructions == null && !punto.EsPuntoInicio)
                    )
                {
                    throw new DomainValidationException("El mapa contiene al menos un punto sin datos, revisar mapa");
                }
            }



            var data = puntos.Where(e => e.PlaCoordenadaId != null).GroupBy(e => e.PlaCoordenadaId).Where(g => g.Count() > 1);

            if (data.Any())
            {
                var x = data.SelectMany(g => g.Select(p => p.Orden)).OrderBy(e => e).Select(s => s.ToString());
                throw new DomainValidationException(string.Format("No se permite asignar una coordenada a mas de un sector. Revisar Puntos de Orden {0}", string.Join(",", x)));
            }
        }

        public enum EnumOriginal
        {
            No = 0,
            AnteriorOriginal = 1,
            Original = 2,

        }

    }
}
