using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices.Filters;
using TECSO.FWK.ApiServices.Model;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.ApiServices
{

    public interface ISecurityController
    {
        IPermissionContainer PermissionContainer { get; set; }
    }


    public abstract class ManagerSecurityController<TModel, TPrimaryKey, TDto, TFilter, TService> : ManagerController<TModel, TPrimaryKey, TDto, TFilter, TService>, ISecurityController
        where TModel : Entity<TPrimaryKey>, new()
        where TDto : EntityDto<TPrimaryKey>, new()
        where TFilter : FilterPagedListBase<TModel, TPrimaryKey>, IFilterPagedListBase<TModel, TPrimaryKey>, new()
         where TService : IAppServiceBase<TModel, TDto, TPrimaryKey>
    {

        public IPermissionContainer PermissionContainer { get; set; }

        protected ManagerSecurityController(TService service)
           : base(service)
        {
            PermissionContainer = new ManagerPermissionContainerBase();

            this.InitializePermission();
        }

        protected virtual void InitializePermission()
        {

        }

        protected void InitializePermissionByDefault(string area, string page)
        {
            this.PermissionContainer = new ManagerPermissionContainerBase();
            this.PermissionContainer.InitializePermissionByDefault(area, page);
        }



        [HttpPost]
        [ActionAuthorize()]
        public override Task<IActionResult> GetPagedList([FromBody] TFilter filter)
        {
            return base.GetPagedList(filter);
        }


        [HttpPost]
        [ActionAuthorize()]
        public override Task<IActionResult> DeleteById(TPrimaryKey Id)
        {
            return base.DeleteById(Id);
        }

        [HttpPost]
        [ActionAuthorize()]
        public override Task<IActionResult> DeleteEntity([FromBody] TFilter filter)
        {
            return base.DeleteEntity(filter);
        }

        [HttpPost]
        [ActionAuthorize()]
        public override Task<IActionResult> UpdateEntity([FromBody] TDto dto)
        {
            return base.UpdateEntity(dto);
        }


        [HttpPost]
        [ActionAuthorize()]
        public override Task<IActionResult> SaveNewEntity([FromBody] TDto dto)
        {
            return base.SaveNewEntity(dto);
        }
    }
}
