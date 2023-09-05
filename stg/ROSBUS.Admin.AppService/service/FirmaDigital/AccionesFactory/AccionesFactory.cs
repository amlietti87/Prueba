using Microsoft.Extensions.DependencyInjection;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Interfaces.Services;


namespace ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory
{
    public class AccionesFactory : IAccionesFactory
    {

        public readonly IDictionary<int, IAccionBuilder> acctionList = new Dictionary<int, IAccionBuilder>();

        protected IServiceProvider serviceProvider;
        private IAccion_DescargarArchivo _accion_DescargarArchivo;
        private IAccion_AbrirArchivo _accion_AbrirArchivo;
        private IAccion_RechazarDocumento _accion_RechazarDocumento;
        private IAccion_VerDetalleDocumento _accion_VerDetalleDocumento;
        private IAccion_RevisarArchivo _accion_RevisarArchivo;
        private IAccion_AprobarDocumento _accion_AprobarDocumento;
        private IAccion_FirmarEmpleador _accion_FirmarEmpleador;
        private IAccion_FirmarEmpleado _accion_FirmarEmpleado;
        private IAccion_RespuestaMinisterio _accion_RespuestaMinisterio;
        private IAccion_EnviarCorreo _accion_EnviarCorreo;
        public AccionesFactory(IServiceProvider _serviceProvider)
        {
            serviceProvider = _serviceProvider;

            
        }




        public void Inicialize()
        {

            if (acctionList.Count == 0)
            {


                _accion_AbrirArchivo = serviceProvider.GetRequiredService<IAccion_AbrirArchivo>();
                _accion_DescargarArchivo = serviceProvider.GetRequiredService<IAccion_DescargarArchivo>();
                _accion_RechazarDocumento = serviceProvider.GetRequiredService<IAccion_RechazarDocumento>();
                _accion_VerDetalleDocumento = serviceProvider.GetRequiredService<IAccion_VerDetalleDocumento>();
                _accion_RevisarArchivo = serviceProvider.GetRequiredService<IAccion_RevisarArchivo>();
                _accion_AprobarDocumento = serviceProvider.GetRequiredService<IAccion_AprobarDocumento>();
                _accion_FirmarEmpleador = serviceProvider.GetRequiredService<IAccion_FirmarEmpleador>();
                _accion_FirmarEmpleado = serviceProvider.GetRequiredService<IAccion_FirmarEmpleado>();
                _accion_RespuestaMinisterio = serviceProvider.GetRequiredService<IAccion_RespuestaMinisterio>();
                _accion_EnviarCorreo = serviceProvider.GetRequiredService<IAccion_EnviarCorreo>();


                acctionList.Add(FdAccionesPermitidas.AbrirArchivo, _accion_AbrirArchivo);
                acctionList.Add(FdAccionesPermitidas.DescargarArchivo, _accion_DescargarArchivo);
                acctionList.Add(FdAccionesPermitidas.RechazarDocumento, _accion_RechazarDocumento);
                acctionList.Add(FdAccionesPermitidas.VerDetalleDocumento, _accion_VerDetalleDocumento);
                acctionList.Add(FdAccionesPermitidas.RevisarArchivo, _accion_RevisarArchivo);
                acctionList.Add(FdAccionesPermitidas.AprobarDocumento, _accion_AprobarDocumento);
                acctionList.Add(FdAccionesPermitidas.FirmarEmpleador, _accion_FirmarEmpleador);
                acctionList.Add(FdAccionesPermitidas.FirmarEmpleado, _accion_FirmarEmpleado);
                acctionList.Add(FdAccionesPermitidas.RespuestaMinisterio, _accion_RespuestaMinisterio);
                acctionList.Add(FdAccionesPermitidas.EnviarCorreo, _accion_EnviarCorreo);
            }

        }


        public IAccionBuilder GetAccionFromId(int id)
        {
            this.Inicialize();
            return acctionList[id];
        }
    }
}
