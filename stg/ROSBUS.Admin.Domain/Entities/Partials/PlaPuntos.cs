using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaPuntos
    {


        [NotMapped]
        public string CodigoVarianteLinea { get; set; }

        [NotMapped]
        public string DescripcionCoordenada {
            get
            {
                return this.PlaCoordenada?.Descripcion;
            } }


        public PlaPuntos Clone()
        {
            var punto = new PlaPuntos();
            punto.CodRec = this.CodRec;
            punto.Lat = this.Lat;
            punto.Long = this.Long;
            punto.CodigoNombre = this.CodigoNombre;
            punto.Data = this.Data;
            punto.EsPuntoInicio = this.EsPuntoInicio;
            punto.EsPuntoTermino = this.EsPuntoTermino;
            punto.EsParada = this.EsParada;
            punto.EsCambioSector = this.EsCambioSector;
            punto.EsPuntoRelevo = this.EsPuntoRelevo;
            punto.EsCambioSectorTarifario = this.EsCambioSectorTarifario;
            punto.CodSectorTarifario = this.CodSectorTarifario;
            punto.Orden = this.Orden;
            punto.TipoParadaId = this.TipoParadaId;
            punto.Abreviacion = this.Abreviacion;
            punto.MostrarEnReporte = this.MostrarEnReporte;
            punto.Color = this.Color;
            punto.PlaCoordenadaId = this.PlaCoordenadaId;
            punto.PlaParadaId = this.PlaParadaId;


            return punto;
        }




    }
}
