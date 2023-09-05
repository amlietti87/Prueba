using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.ApiServices
{
    public abstract class ManagerControllerBase<TModel,TPrimaryKey, TFilter>:ControllerBase
        where TModel : Entity<TPrimaryKey>, new()
        where TFilter : FilterCriteriaBase<TPrimaryKey>
    {
        public ManagerControllerBase()
            :base()
        {

        }
    }
}