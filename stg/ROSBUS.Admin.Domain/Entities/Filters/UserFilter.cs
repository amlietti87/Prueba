using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using System.Linq;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class UserFilter : FilterPagedListFullAudited<SysUsersAd, int>
    {
        public int? RoleId { get; set; }

        public string LogonName { get; set; }

        public int? GruposInspectoresId { get; set; }
        public int? TurnoId { get; set; }
        public int? EmpleadoId { get; set; }
        public ItemDto<int> selectEmpleados { get; set; }

        public override List<Expression<Func<SysUsersAd, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<SysUsersAd, object>>>
            {
                e=> e.UserRoles
               // e=> e.Grupolinea
            };
        }


        public override Expression<Func<SysUsersAd, bool>> GetFilterExpression()
        {

            Expression<Func<SysUsersAd, bool>> baseFE = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<SysUsersAd, bool>> filterTextExp = e => e.NomUsuario.Contains(this.FilterText) || e.LogonName.Contains(this.FilterText)
                || e.Mail.Contains(this.FilterText) || e.DisplayName.Contains(this.FilterText)
                 || e.NroDoc.Contains(this.FilterText)
                || e.CanonicalName.Contains(this.FilterText);
                baseFE = baseFE.And(filterTextExp);
            }

            if (!String.IsNullOrEmpty(this.LogonName))
            {
                baseFE = baseFE.And(e => e.LogonName == LogonName);
            } 

            if (this.RoleId.HasValue)
            {
                var _RoleId = RoleId.Value;
                Expression<Func<SysUsersAd, bool>> RoleIdExp = e => e.UserRoles.Any(a => a.RoleId == _RoleId);
                baseFE = baseFE.And(RoleIdExp);
            }

            if (this.GruposInspectoresId.HasValue)
            {
                baseFE = baseFE.And(e => e.GruposInspectoresId == GruposInspectoresId);
            }

            if (this.TurnoId.HasValue)
            {
                baseFE = baseFE.And(e => e.TurnoId == TurnoId);
            }

            if (selectEmpleados != null)
            {
                Expression<Func<SysUsersAd, bool>> filterTextExp = e => e.EmpleadoId == selectEmpleados.Id;
                baseFE = baseFE.And(filterTextExp);
            }
            return baseFE;
        }
    }





}
