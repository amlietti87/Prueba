using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Report.Report.Model
{
    public class SiniestroReportModel
    {
        // Datos del Siniestro
        public string NroSiniestro { get; set; }
        public string FechaSiniestro { get; set; }
        public string Lugar { get; set; }
        public string UnidadDeNegocio { get; set; }
        public string Hora { get; set; }
        public string Localidad { get; set; }
        public string FechaDenuncia { get; set; }
        public string HoraDenuncia { get; set; }
        public string Comentario { get; set; }

        // Datos del Coche
        public string Empresa { get; set; }
        public string NroInterno { get; set; }
        public string Ficha { get; set; }
        public string Dominio { get; set; }
        public string Linea { get; set; }
        public string Modelo { get; set; }

        // Datos del Conductor/ Practicante
        public string LabelDatosEmpPract { get; set; }
        public string LabelEmpPract { get; set; }
        public string ApNomEmpPract { get; set; }
        public string DocEmpPract { get; set; }
        //public string LegajoEmpPract { get; set; }
        public string EmpresaEmpPract { get; set; }
        public string DomicilioEmpPract { get; set; }
        public string LocalidadEmpPract { get; set; }
        public string Licencias { get; set; }
        // Detalle del siniestro
        public string FormaSiniestro { get; set; }
        public string DescripcionDanios { get; set; }
        public string Croqui { get; set; }
    }           
    public class InvolucradoTercero
    {
        // Cabecera
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string NroInvolucrado { get; set; }
        public string ApellidoNombre { get; set; }
        public string TipoNroDoc { get; set; }
        public string Domicilio { get; set; }
        public string Localidad { get; set; }
        public string Telefono { get; set; }
        public string DetalleDanio { get; set; }

        // Conductor
        public int ConductorId { get; set; }
        public string ConductorApellidoNombre { get; set; }
        public string ConductorTipoNroDoc { get; set; }
        public string ConductorDomicilio { get; set; }
        public string ConductorLocalidad { get; set; }
        public string ConductorTelefono { get; set; }

        // Vehiculo
        public int VehiculoId { get; set; }
        public string VehiculoMarca { get; set; }
        public string VehiculoModelo { get; set; }
        public string VehiculoDominio { get; set; }
        public string VehiculoCiaSeguro { get; set; }
        public string VehiculoNroPoliza { get; set; }
    }
    public class InvolucradoMuebleInmueble
    {
        // Cabecera
        public int Id { get; set; }
        public string Tipo { get; set; }
        public string NroInvolucrado { get; set; }
        public string ApellidoNombre { get; set; }
        public string TipoNroDoc { get; set; }
        public string Domicilio { get; set; }
        public string Localidad { get; set; }
        public string Telefono { get; set; }
        public string DetalleDanio { get; set; }

        // Mueble / Inmueble
        public int MuebleInmuebleId { get; set; }
        public string MuebleInmuebleTipo { get; set; }
        public string MuebleInmuebleDomicilio { get; set; }
        public string MuebleInmuebleLocalidad { get; set; }
    }
    public class InvolucradoLesionado
    {
        public string NroInvolucrado { get; set; }
        public string TipoLesionado { get; set; }
        public string ApellidoNombre { get; set; }
        public string TipoNroDoc { get; set; }
        public string DomicilioLocalidad { get; set; }
        public string Telefono { get; set; }
        public string DetalleDanio { get; set; }
    }
    public class InvolucradoTestigo
    {
        public string ApellidoNombre { get; set; }
        public string TipoNroDoc { get; set; }
        public string Telefono { get; set; }

        public string Domicilio { get; set; }
        
    }

    public class LicenciasVencimiento
    {
        public string Descripcion { get; set; }
        public string FechaVencimientoLicencia { get; set; }

    }

}
