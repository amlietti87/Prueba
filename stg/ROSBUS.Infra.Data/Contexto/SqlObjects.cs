using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Infra.Data
{
    public static class  SqlObjects
    {

        /// <summary>
        /// elimina h_proc_min y copia de h_detaminxtipo a h_proc_min, recrando la sabana teniendo en cuenta la salida
        /// </summary>
        internal const string sp_pla_recrearHProcMin = "dbo.sp_pla_recrearHProcMin";

        /// <summary>
        /// llama a sp_pla_recrearHProcMin desde un temple (h_minxtipo) por bandera, y puede pasar una lista de 
        /// elimina h_proc_min y copia de h_detaminxtipo a h_proc_min, recrando la sabana teniendo en cuenta la salida
        /// </summary>
        internal const string sp_pla_recrearHProcMinPorMin = "dbo.sp_pla_recrearHProcMinPorMin";

        internal const string sp_pla_ExistenMediasVueltasIncompletas = "dbo.sp_pla_ExistenMediasVueltasIncompletas";
        internal const string sp_pla_DistribucionDeCochesPorTipoDeDia_TieneHorarioAsignado = "dbo.sp_pla_DistribucionDeCochesPorTipoDeDia_TieneHorarioAsignado";
        internal const string sp_pla_ExistenDuracionesIncompletas = "dbo.sp_pla_ExistenDuracionesIncompletas";




        internal const string pla_BorrarServiciosFueraDelRango = "dbo.sp_pla_BorrarServiciosFueraDelRango";

        internal const string sp_pla_LeerMediasVueltasIncompletas = "dbo.sp_pla_LeerMediasVueltasIncompletas";


        /// <summary>
        /// inserta en h_sectores y h_basec  banderas que no estan en el horario , lanza  un error si no se encuentra un recorido vijente para el horario (fecha desde)
        /// </summary>
        internal const string sp_pla_CrearSectores= "dbo.sp_pla_CrearSectores";


        /// <summary>
        /// Crea los h_minxtipo y h_detaminxtipo  faltantes en minutos Media vuelta (min en null) y elimina los mismos que ya no existan por minutos Media vuelta   
        /// </summary>        
        internal const string sp_pla_RecrearMinutosPorSector = "dbo.sp_pla_RecrearMinutosPorSector";
         
        /// <summary>
        /// verifica si un horario h_fechas_confi , ya fue diagramado  
        /// </summary>        
        internal const string sp_pla_HorarioDiagramado = "dbo.sp_pla_HorarioDiagramado";

        internal const string pla_CopiarHorario = "dbo.sp_pla_CopiarHorario";
        

        /// <summary>
        /// copiar minutos  desde un origen a un destino por h_fechas
        /// </summary>
        internal const string sp_pla_CopiarMinutos = "dbo.sp_pla_CopiarMinutos";


        /// <summary>
        /// elimina todo lo relacionado a los sectores y remapea todo llamando a 
        /// sp_pla_CrearSectores , sp_pla_RecrearMinutosPorSector , sp_pla_recrearHProcMin
        /// </summary>
        internal const string sp_pla_RemapearRecoridoBandera = "dbo.sp_pla_RemapearRecoridoBandera";


        /// <summary>
        /// validar solapado y huecos en horarios
        /// </summary>
        internal const string sp_pla_validarTimeLineHorario = "dbo.sp_pla_validarTimeLineHorario";



        /// <summary>
        /// elimino una fecha horario con todos sus datos elacionados
        /// </summary>
        internal const string sp_pla_EliminarHorario = "dbo.sp_pla_EliminarHorario";


        internal const string sp_pla_FechaHorarioDiagramado = "dbo.sp_pla_FechaHorarioDiagramado";


        


    }
}
