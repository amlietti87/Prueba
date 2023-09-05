using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Entities.Partials;
using System.Threading.Tasks;
using ROSBUS.Admin.Domain.Entities.Filters;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IFdDocumentosProcesadosRepository : IRepositoryBase<FdDocumentosProcesados, long>
    {
        void SaveDocumentosImportados(List<FdDocumentosProcesados> procesadosCorrectos, List<FdDocumentosError> procesadosError);
        Task<List<Destinatario>> GetNotificacionesMail(string token);
        Task<List<ArchivosTotalesPorEstado>> HistorialArchivosPorEstado(FdDocumentosProcesadosFilter documentosProcesadosFilter);
        void GuardarDocumento(FdDocumentosProcesados doc, FdDocumentosProcesadosHistorico historico, FdFirmador fdFirmador);

        Task<List<FdDocumentosProcesados>> ExportarExcel(FdDocumentosProcesadosFilter filter);
    }
}
