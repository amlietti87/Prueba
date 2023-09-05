using System.Threading.Tasks;
using ROSBUS.Admin.AppService.Model;

namespace ROSBUS.Admin.AppService.service.FirmaDigital
{
    public interface IFirmadorHelper
    {
        Task<FdFirmadorDto> RecuperarJNLP(AplicarAccioneDto dto);
    }
}