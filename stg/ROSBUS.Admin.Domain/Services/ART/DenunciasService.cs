using ROSBUS.Admin.Domain.Interfaces.Repositories.ART;
using ROSBUS.Admin.Domain.Interfaces.Services.ART;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;
using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Admin.Domain.Entities.Filters.ART;
using TECSO.FWK.Caching;
using TECSO.FWK.Domain.Interfaces.Services;
using System.IO;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System.Linq;

namespace ROSBUS.Admin.Domain.Services.ART
{
    public class DenunciasService : ServiceBase<ArtDenuncias, int, IDenunciasRepository>, IDenunciasService
    {
        public ICacheManager cacheManager;
        private readonly ILogger logger;
        private readonly ISysParametersService parametersService;

        public DenunciasService(IDenunciasRepository repository, ICacheManager _cacheManager, ILogger logger, ISysParametersService parametersService) : base(repository)
        {
            cacheManager = _cacheManager;
            this.logger = logger;
            this.parametersService = parametersService;
        }
        public async Task<List<HistorialDenuncias>> HistorialDenunciaPorPrestador(int EmpleadoId)
        {
            return await this.repository.HistorialDenunciaPorPrestador(EmpleadoId);
        }

        public async Task<List<HistorialReclamosEmpleado>> HistorialReclamosEmpleado(int EmpleadoId)
        {
            return await this.repository.HistorialReclamosEmpleado(EmpleadoId);
        }

        public async Task<List<HistorialDenunciasPorEstado>> HistorialDenunciasPorEstado(int EmpleadoId)
        {
            return await this.repository.HistorialDenunciasPorEstado(EmpleadoId);
        }


        public async Task<List<ArtDenunciaAdjuntos>> GetAdjuntosDenuncias(int DenunciaId)
        {
            return await this.repository.GetAdjuntosDenuncias(DenunciaId);
        }

        public async Task<List<Destinatario>> GetNotificacionesMail(string Token)
        {
            return await this.repository.GetNotificacionesMail(Token);
        }

        public Task DeleteFileById(Guid id)
        {
            return this.repository.DeleteFileById(id);
        }

        public async Task<ArtDenuncias> Anular(ArtDenuncias denuncia)
        {
            return await this.repository.Anular(denuncia);
        }

        public async Task<List<ReporteDenunciasExcel>> GetReporteExcel(ExcelDenunciasFilter filter)
        {
            return await this.repository.GetReporteExcel(filter);
        }

        public async Task<List<ImportadorExcelDenuncias>> RecuperarPlanilla(DenunciaImportadorFileFilter filter)
        {
            var mvCache = await cacheManager.GetCache<string, List<ImportadorExcelDenuncias>>("ImportadorExcelDenuncias")
                                          .GetAsync(filter.PlanillaId, e => TransformarDatos(filter));

            return mvCache;
        }

        public async Task ImportarDenuncias(DenunciaImportadorFileFilter input)
        {
            var planilla = await this.RecuperarPlanilla(new DenunciaImportadorFileFilter() {  PlanillaId = input.PlanillaId });


            await this.repository.ImportarDenuncias(planilla);
        }

        public async Task ImportarDenunciasFromTask(List<ImportadorExcelDenuncias> planilla, string file)
        {
            string path = (await this.parametersService.GetAllAsync(e => e.Token == "ImportadorDenuncias")).Items.FirstOrDefault().Value;

            var onlyfilename = System.IO.Path.GetFileName(file);
            onlyfilename = String.Format("{0} - {1}", DateTime.Now.ToString("yyyy-MM-dd HHmm"), onlyfilename);
            var pathprocesadoscorrectamente = System.IO.Path.Combine(path, "Procesados Correctamente");
            var pathprocesadoserroneamente = System.IO.Path.Combine(path, "Procesados Erroneamente");

            try
            {
                await this.repository.ImportarDenuncias(planilla);

                bool exists = System.IO.Directory.Exists(pathprocesadoscorrectamente);

                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(pathprocesadoscorrectamente);
                }

                var archivodestino = System.IO.Path.Combine(pathprocesadoscorrectamente, onlyfilename);
                await this.logger.LogInformation(String.Format("Se procede a mover el archivo a {0}", archivodestino));
                File.Move(file, archivodestino);
            }
            catch (Exception ex)
            {
                await this.logger.LogError(ex.Message);

                bool exists = System.IO.Directory.Exists(pathprocesadoserroneamente);

                if (!exists)
                {
                    System.IO.Directory.CreateDirectory(pathprocesadoserroneamente);
                }

                var archivodestino = System.IO.Path.Combine(pathprocesadoserroneamente, onlyfilename);
                await this.logger.LogInformation(String.Format("Se procede a mover el archivo a {0}", archivodestino));
                File.Move(file, archivodestino);

            }


            

            

           

        }

        private async Task<List<ImportadorExcelDenuncias>> TransformarDatos(DenunciaImportadorFileFilter filtro)
        {
            List<ImportadorExcelDenuncias> result = new List<ImportadorExcelDenuncias>();

            var Importados = await cacheManager.GetCache<string, List<ImportadorExcelDenuncias>>("ImportadorExcelDenuncias").GetAsync(filtro.PlanillaId, e => null);



            return result;
        }

    }
}
