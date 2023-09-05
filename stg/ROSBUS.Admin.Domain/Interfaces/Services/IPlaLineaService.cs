using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters.ComoLlegoBus;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IPlaLineaService : IServiceBase<PlaLinea, int>
    {
        Task <List<int>> GetMedioPagosRuta(string name);
        Task<List<PlaLinea>> GetLineasValidas(string nlRosBusValidas, string tiposdias);
        Task<Decimal> GetTarifaUrbana(string linename);
        Task<ReporteParadasRuta> GetParadasRutas(int? codBan, string Lineas);
        Task<ReporteHorarioRuta> GetHorariosRuta(string routelongname);
        Task<ReporteTarifasRuta> GetTarifasRutasAsync(int? codBan, string NombresLineas);
        Task<List<PuntosRuta>> GetPuntosRutaBanderaAsync(int? codBan);
    }
}
