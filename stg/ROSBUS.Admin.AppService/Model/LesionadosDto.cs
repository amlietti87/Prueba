using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class LesionadosDto : EntityDto<int>
    {
        public int? TipoLesionadoId { get; set; }

        public string TipoLesionadoDescripcion { get; set; }


        public override string Description => string.Empty;

    }
}
