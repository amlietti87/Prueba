using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class BolSectoresTarifariosFilter : FilterPagedListBase<BolSectoresTarifarios, int>
    {       

        public override Func<BolSectoresTarifarios, ItemDto<int>> GetItmDTO()
        {
            return GetItmDTOFunc();
        }

        public static Func<BolSectoresTarifarios, ItemDto<int>> GetItmDTOFunc()
        {
            return e => new ItemDto<int>(e.Id, e.DscSectorTarifario.Trim());
        }


        public override Expression<Func<BolSectoresTarifarios, bool>> GetFilterExpression()
        {
            Expression<Func<BolSectoresTarifarios, bool>> Exp = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {

                Expression<Func<BolSectoresTarifarios, bool>> filterTextExp = e => e.DscSectorTarifario.Contains(FilterText);
                Exp = Exp.And(filterTextExp);
            }

            return Exp;
        }


    }
}
