using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters.ComoLlegoBus;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IPlaLineaRepository : IRepositoryBase<PlaLinea, int>
    {
        Task<List<int>> GetMedioPagosRuta(string name);
        Task<List<PlaLinea>> GetLineasValidas(string nlRosBusValidas, string tiposdias);
        Task<decimal> GetTarifaUrbana(string linename);
        Task<ReporteParadasRuta> GetParadasRuta(int? codBan, string Lineas);
        Task<ReporteHorarioRuta> GetHorarioRuta(string routelongname);
        Task<ReporteTarifasRuta> GetTarifasRutaAsync(int? codBan, string NombresLineas);
        Task<List<PuntosRuta>> GetPuntosRutaBanderaAsync(int? codBan);
    }
}
