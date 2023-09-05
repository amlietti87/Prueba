using Microsoft.Extensions.Configuration;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Interface.ART;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Entities.Filters.ART;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Admin.Domain.Report;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Caching;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Domain.Services;
using TECSO.FWK.Extensions;
using Microsoft.Extensions.DependencyInjection;
using TECSO.FWK.AppService.Interface;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Entities.Filters;
using Microsoft.AspNetCore.Http;

namespace ROSBUS.Admin.AppService.service
{
    public class ImportadorDocumentosFDHttpAppService : HttpAppServiceBase<FdDocumentosProcesados, FdDocumentosProcesadosDto, long>, IFdDocumentosProcesadosAppService
    {


        public ImportadorDocumentosFDHttpAppService(IAuthService _authService)
            : base(_authService)
        {
            this.useAdminToken = true;
        }

        public override string EndPoint => "Importador";


        protected override string GetUrlBase()
        {
            return configuration.GetValue<string>("FirmaDigitalUrl").EnsureEndsWith('/');
        }


        
        

        public async Task<Boolean> ImportarDocumentos()
        {
            try
            {
                string action = "ImportarDocumentos";

                var resp = await this.httpClient.GetRequest<ResponseModel<Boolean>>(action, timeOut: TimeSpan.FromMinutes(30));

                return resp.DataObject;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public Task<List<ArchivosTotalesPorEstado>> HistorialArchivosPorEstado(FdDocumentosProcesadosFilter documentosProcesadosFilter)
        {
            throw new NotImplementedException();
        }

        public void GuardarDocumento(FdDocumentosProcesados doc, FdDocumentosProcesadosHistorico historico, FdFirmadorDto fdFirmadorDto)
        {
            throw new NotImplementedException();
        }

        public Task<FileDto> ExportarExcel(FdDocumentosProcesadosFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetEmailDefault()
        {
            throw new NotImplementedException();
        }

       

        public Task<FdFirmadorDto> GetMetadatos(string token, string idUsuario, HttpRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<FdFirmadorDto> GetCertificado(string token, string idUsuario, HttpRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<FdFirmadorDto> GetDocumento(string token, int idRecibo, HttpRequest request)
        {
            throw new NotImplementedException();
        }

        public Task<FdFirmadorDto> RecibirDocumento(string token, int idRecibo, Boolean? conforme, HttpRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
