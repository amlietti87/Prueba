using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Operaciones.Domain.Interfaces.Repositories;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class SiniestrosService : ServiceBase<SinSiniestros, int, ISiniestrosRepository>, ISiniestrosService
    { 
        public SiniestrosService(ISiniestrosRepository produtoRepository, IEmpleadosRepository empleadosRepository)
            : base(produtoRepository)
        {
            _empleadosRepository = empleadosRepository;
        }
        
        private readonly IEmpleadosRepository _empleadosRepository;

        public async Task<HistorialSiniestros> GetHistorialEmpPract(bool empleado, int id)
        {
            string dni;
            if (empleado)
            {
                dni = _empleadosRepository.GetById(id).Dni;
            }
            else
            {
                dni = null;
            }

            return await this.repository.GetHistorialEmpPract(empleado, id, dni);
        }

        public async Task<List<Licencias>> GetLicencias(int cod_empleado)
        {
            return await this.repository.GetLicencias(cod_empleado);
        }

        public async Task<int> GetNroSiniestroMax()
        {
            return await this.repository.GetNroSiniestroMax();
        }

        public async Task<string> GenerarInforme(int SiniestroId)
        {
            return await this.repository.GenerarInforme(SiniestroId);
        }

        public async Task<List<SinSiniestroAdjuntos>> GetAdjuntosSiniestros(int siniestroId)
        {
            return await this.repository.GetAdjuntosSiniestros(siniestroId);
        }

        public Task DeleteFileById(Guid id)
        {
            return this.repository.DeleteFileById(id);
        }

        public async  Task<string> GetInforme(string codInforme)
        {
            return await this.repository.GetInforme(codInforme);
        }
    }
    
}
