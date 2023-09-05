using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.AppService.Interface;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Entities.Filters;
using System.Threading.Tasks;
using ROSBUS.Admin.Domain.Entities.Partials;
using Microsoft.AspNetCore.Http;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IFdDocumentosProcesadosAppService : IAppServiceBase<FdDocumentosProcesados, FdDocumentosProcesadosDto, long>
    {
        Task<Boolean> ImportarDocumentos();
        Task<List<ArchivosTotalesPorEstado>> HistorialArchivosPorEstado(FdDocumentosProcesadosFilter documentosProcesadosFilter);
        Task<FileDto> ExportarExcel(FdDocumentosProcesadosFilter filter);
        void GuardarDocumento(FdDocumentosProcesados doc, FdDocumentosProcesadosHistorico historico, FdFirmadorDto fdFirmadorDto);
        Task<string> GetEmailDefault();
        Task<FdFirmadorDto> GetMetadatos(string token, string idUsuario, HttpRequest request);
        Task<FdFirmadorDto> GetCertificado(string token, string idUsuario, HttpRequest request);
        Task<FdFirmadorDto> GetDocumento(string token, int idRecibo, HttpRequest request);
        Task<FdFirmadorDto> RecibirDocumento(string token, int idRecibo,Boolean? conforme, HttpRequest request);
    }
}
