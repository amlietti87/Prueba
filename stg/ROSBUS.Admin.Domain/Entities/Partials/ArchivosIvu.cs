using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Entities.Partials
{
    public class IVUViajes
    {
        public const string tableName = "REC_DIENST_ZU_FAHRT";

        public const string ViajeIdPropertyName = "FRT_FID";
        public const string TipoDeDiaIdPropertyName = "TAGESART_NR";
        public const string TurnoIdPropertyName = "ED_NR";
        public const string UbicacionInicioIdPropertyName = "ANF_ORT";
        public const string TipoUbicacionInicioIdPropertyName = "ANF_ONR_TYP";
        public const string UbicacionFinIdPropertyName = "END_ORT";
        public const string TipoUbicacionFinIdPropertyName = "END_ONR_TYP";
        public const string SalePropertyName = "BEGINNZEIT";
        public const string LlegaPropertyName = "ENDZEIT";
        public const string BanderaIdPropertyName = "STR_LI_VAR";
        public const string NroSecuencialInicioPropertyName = "LFD_NR_BEGINN";
        public const string NroSecuencialFinPropertyName = "LFD_NR_ENDE";

        public string ViajeId { get; set; }
        public string TipoDeDiaId { get; set; }
        public string TurnoId { get; set; }
        public string UbicacionInicioId { get; set; }
        public string TipoUbicacionInicioId { get; set; }
        public string UbicacionFinId { get; set; }
        public string TipoUbicacionFinId { get; set; }
        public string Sale { get; set; }
        public string Llega { get; set; }
        public string BanderaId { get; set; }
        public string NroSecuencialInicio { get; set; }
        public string NroSecuencialFin { get; set; }
    }


    public class IVUViajesPorVehiculo
    {
        public const string tableName = "REC_FRT";

        public const string ViajeIdPropertyName = "FRT_FID";
        public const string TipoDeHoraIdPropertyName = "FGR_NR";
        public const string NroDeViajePropertyName = "FRT_EXT_NR";
        public string ViajeId { get; set; }
        public string TipoDeHoraId { get; set; }
        public string NroDeViaje { get; set; }
    }

    public class IVUUbicaciones
    {
        public const string tableName = "REC_ORT";

        public const string TipoUbicacionPropertyName = "ONR_TYP_NR";
        public const string UbicacionIdPropertyName = "ORT_NR";
        public const string DescripcionUbicacionPropertyName = "ORT_NAME";
        public const string AbreviaturaPropertyName = "ORT_KUERZEL";
        public string TipoUbicacion { get; set; }
        public string UbicacionId { get; set; }
        public string DescripcionUbicacion { get; set; }
        public string Abreviatura { get; set; }
    }

    public class IVUBanderas
    {
        public const string tableName = "REC_LID";

        public const string LineaIdPropertyName = "LI_NR";
        public const string BanderaIdPropertyName = "STR_LI_VAR";
        public const string BanderaNroPropertyName = "ROUTEN_NR";
        public const string SentidoBanderaPropertyName = "LI_RI_NR";
        public const string LineaNombrePropertyName = "LIDNAME";
        public const string TipoBanderaPropertyName = "ROUTEN_ART";
        public string LineaId { get; set; }
        public string BanderaId { get; set; }
        public string BanderaNro { get; set; }
        public string SentidoBandera { get; set; }
        public string LineaNombre { get; set; }
        public string TipoBandera { get; set; }
    }

    public class IVUTurnos
    {
        public const string tableName = "REC_UMLAUF";

        public const string TipoDeDiaIdPropertyName = "TAGESART_NR";
        public const string TurnoIdPropertyName = "UM_UID";
        public const string TurnoExternoIdPropertyName = "UM_UID_EXTERN";
        public string TipoDeDiaId { get; set; }
        public string TurnoId { get; set; }
        public string TurnoExternoId { get; set; }
    }


    public class IVUServicios
    {
        public const string tableName = "REC_DIENSTSTUECK";

        public const string DuracionPropertyName = "DST_DAUER";
        public const string SalePropertyName = "DST_ANF_ZEIT";
        public const string LlegaPropertyName = "DST_END_ZEIT";
        public const string TipoDeDiaIdPropertyName = "TAGESART_NR";
        public const string TurnoIdPropertyName = "ED_NR";
        public const string OrdenSecuencialIdPropertyName = "LFD_DIENSTSTUECKNR";
        public const string ServicioIdPropertyName = "UM_UID";
        public string Duracion { get; set; }
        public string Sale { get; set; }
        public string Llega { get; set; }
        public string TipoDeDiaId { get; set; }
        public string TurnoId { get; set; }
        public string OrdenSecuencialId { get; set; }
        public string ServicioId { get; set; }
    }


    public class IVUTiposDeDia
    {
        public const string tableName = "MENGE_TAGESART";

        public const string TipoDeDiaIdPropertyName = "TAGESART_NR";
        public const string DescripcionTipoDeDiaPropertyName = "TAGESART_TEXT";
        public string TipoDeDiaId { get; set; }
        public string DescripcionTipoDeDia { get; set; }
    }


    public class IVUMinutosPorSector
    {
        public const string tableName = "SEL_FZT_FELD";

        public const string TipoDeHoraIdPropertyName = "FGR_NR";
        public const string TipoDeUbicacionInicioIdPropertyName = "ONR_TYP_NR";
        public const string UbicacionInicioIdPropertyName = "ORT_NR";
        public const string UbicacionFinIdPropertyName = "SEL_ZIEL";
        public const string TipoDeUbicacionFinIdPropertyName = "SEL_ZIEL_TYP";
        public const string DuracionPropertyName = "SEL_FZT";
        public string TipoDeHoraId { get; set; }
        public string TipoDeUbicacionInicioId { get; set; }
        public string UbicacionInicioId { get; set; }
        public string UbicacionFinId { get; set; }
        public string TipoDeUbicacionFinId { get; set; }
        public string Duracion { get; set; }
    }

    public class IVUSectoresPorMediaVueltas
    {
        public const string tableName = "LID_VERLAUF";

        public const string UbicacionMediaVueltaPropertyName = "LI_LFD_NR";
        public const string LineaIdPropertyName = "LI_NR";
        public const string BanderaIdPropertyName = "STR_LI_VAR";
        public const string TipoUbicacionIdPropertyName = "ONR_TYP_NR";
        public const string UbicacionIdPropertyName = "ORT_NR";
        public const string TipoViajePropertyName = "PRODUKTIV";
        public string UbicacionMediaVuelta { get; set; }
        public string LineaId { get; set; }
        public string BanderaId { get; set; }
        public string TipoUbicacionId { get; set; }
        public string UbicacionId { get; set; }
        public string TipoViaje { get; set; }
    }

    public class IVUMetrosPorSector
    {
        public const string tableName = "REC_SEL";

        public const string TipoUbicacionIdPropertyName = "ONR_TYP_NR";
        public const string UbicacionInicioIdPropertyName = "ORT_NR";
        public const string UbicacionFinIdPropertyName = "SEL_ZIEL";
        public const string TipoUbicacionFinIdPropertyName = "SEL_ZIEL_TYP";
        public const string DistanciaPropertyName = "SEL_LAENGE";
        public string TipoUbicacionId { get; set; }
        public string UbicacionInicioId { get; set; }
        public string UbicacionFinId { get; set; }
        public string TipoUbicacionFinId { get; set; }

        public string Distancia { get; set; }
    }

    public class IVUInfoSinProcesar
    {
        public string Servicio { get; set; }
        public string Turno { get; set; }
        public string Sale { get; set; }
        public string Llega { get; set; }
        public string Bandera { get; set; }
        public string TipoDeHora { get; set; }
        public string Ubicacion { get; set; }
    }
}
