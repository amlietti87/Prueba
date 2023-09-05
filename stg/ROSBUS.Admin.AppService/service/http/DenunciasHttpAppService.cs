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

namespace ROSBUS.Admin.AppService.service
{
    public class DenunciasHttpAppService : HttpAppServiceBase<ArtDenuncias, ArtDenunciasDto, int>, IDenunciasAppService
    {


        public DenunciasHttpAppService(IAuthService _authService)
            : base(_authService)
        {
            this.useAdminToken = true;
        }

        public override string EndPoint => "Denuncias";


        protected override string GetUrlBase()
        {
            return configuration.GetValue<string>("ARTUrl").EnsureEndsWith('/');
        }


        public Task AgregarAdjuntos(int DenunciaId, List<AdjuntosDto> result)
        {
            throw new NotImplementedException();
        }

        public Task<ArtDenunciasDto> Anular(ArtDenunciasDto denuncia)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFileById(Guid id)
        {
            throw new NotImplementedException();
        }

        public Task<List<AdjuntosDto>> GetAdjuntosDenuncias(int DenunciaId)
        {
            throw new NotImplementedException();
        }

        public Task<ReportModel> GetDatosReporte(ArtDenunciasDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<FileDto> GetReporteExcel(ExcelDenunciasFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<HistorialDenuncias>> HistorialDenunciaPorPrestador(int EmpleadoId)
        {
            throw new NotImplementedException();
        }

        public Task<List<HistorialDenunciasPorEstado>> HistorialDenunciasPorEstado(int EmpleadoId)
        {
            throw new NotImplementedException();
        }

        public Task<List<HistorialReclamosEmpleado>> HistorialReclamosEmpleado(int EmpleadoId)
        {
            throw new NotImplementedException();
        }

        public Task ImportarDenuncias(DenunciaImportadorFileFilter input)
        {
            throw new NotImplementedException();
        }

        public async Task<Boolean> ImportWithTask()
        {
            try
            {
                string action = "ImportWithTask";

                var pList = await this.httpClient.PostRequest<Boolean>(action, timeOut: TimeSpan.FromMinutes(30));

                return pList;
            }
            catch (Exception ex)
            {
                throw;
            }

        }

        public Task<List<ImportadorExcelDenuncias>> RecuperarPlanilla(DenunciaImportadorFileFilter filter)
        {
            throw new NotImplementedException();
        }

        public Task<List<ImportadorExcelDenuncias>> UploadExcel(byte[] excelFile)
        {
            throw new NotImplementedException();
        }
    }
}
