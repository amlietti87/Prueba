//using System;
//using System.Collections.Generic;
//using System.Linq.Expressions;
//using System.Text;
//using TECSO.FWK.Domain.Entities;

//namespace ROSBUS.Admin.Domain.Entities.Filters
//{
//    public class ConfiguFilter : FilterPagedListBase<Configu, decimal>
//    {

//        public override List<Expression<Func<Configu, object>>> GetIncludesForPageList()
//        {
//            return new List<Expression<Func<Configu, object>>>
//            {
//                e=> e.Grupo,
//                e=> e.Empresa,
//                e=> e.Sucursal,
//                e=> e.Linea,
//                e=> e.Galpon,
//                e=> e.PlanCamNav
//            };
//        }
//    }
//}
