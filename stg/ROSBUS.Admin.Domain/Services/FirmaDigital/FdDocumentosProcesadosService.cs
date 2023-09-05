using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Services;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Entities.Partials;
using System.Threading.Tasks;
using ROSBUS.Admin.Domain.Entities.Filters;

namespace ROSBUS.Admin.Domain.Services
{
    public class FdDocumentosProcesadosService : ServiceBase<FdDocumentosProcesados,long, IFdDocumentosProcesadosRepository>, IFdDocumentosProcesadosService
    { 
        public FdDocumentosProcesadosService(IFdDocumentosProcesadosRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public Task<List<Destinatario>> GetNotificacionesMail(string token)
        {
            return this.repository.GetNotificacionesMail(token);
        }

        public void SaveDocumentosImportados(List<FdDocumentosProcesados> procesadosCorrectos, List<FdDocumentosError> procesadosError)
        {
            this.repository.SaveDocumentosImportados(procesadosCorrectos,procesadosError);
        }

        public async Task<List<ArchivosTotalesPorEstado>> HistorialArchivosPorEstado(FdDocumentosProcesadosFilter documentosProcesadosFilter)
        {
            return await this.repository.HistorialArchivosPorEstado(documentosProcesadosFilter);
        }

        public async Task<List<FdDocumentosProcesados>> ExportarExcel(FdDocumentosProcesadosFilter filter)
        {
            return await this.repository.ExportarExcel(filter);
        }
        public void GuardarDocumento(FdDocumentosProcesados doc, FdDocumentosProcesadosHistorico historico, FdFirmador fdFirmador)
        {
            this.repository.GuardarDocumento(doc, historico, fdFirmador);
        }
    }
    
}
