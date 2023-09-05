using System;
using System.Collections.Generic;
using System.Text;

namespace TECSO.FWK.ApiServices
{
    public interface IPermissionContainer
    {
        List<PermissionType> Permissions { get; set; }

        void InitializePermissionByDefault(string area, string page);
        void AddPermission(string controllerAction, string area, string page, string action);
    }

    public class PermissionType
    {
        public string ActionName { get; set; }
        public string PermissionName { get; set; }

        public PermissionType(string actionName, string permissionName)
        {
            ActionName = actionName;
            PermissionName = permissionName;
        }


    }


    public abstract class PermissionContainerBase : IPermissionContainer
    {
        public PermissionContainerBase()
        {
            this.Permissions = new List<PermissionType>();
        }

        public void InitializePermissionByDefault(string area, string page)
        {
            
            this.AddPermission(GetPagedList, area, page, Administracion);
            //this.permissions.Add(this.getTokenPermission(GetByIdAsync, area, page, Administracion));TODO ver si se necesita
            this.AddPermission(DeleteById, area, page, Eliminar);
            this.AddPermission(DeleteEntity, area, page, Eliminar);
            this.AddPermission(UpdateEntity, area, page, Modificar);
            this.AddPermission(SaveNewEntity, area, page, Agregar);

        }

        public void AddPermission(string controllerAction, string area, string page, string action)
        {
            if (this.Permissions == null)
            {
                this.Permissions = new List<PermissionType>();
            }

            this.Permissions.Add(this.GetPermissionType(controllerAction, area, page, action));
        }

        protected virtual PermissionType GetPermissionType(string controllerAction, string area, string page, string action)
        {

            var per = new PermissionType(controllerAction, string.Format("{0}.{1}.{2}", area, page, action));
            return per;
        }

        public const string GetPagedList = "GetPagedList";
        public const string GetByIdAsync = "GetByIdAsync";
        public const string DeleteById = "DeleteById";
        public const string DeleteEntity = "DeleteEntity";
        public const string UpdateEntity = "UpdateEntity";
        public const string SaveNewEntity = "SaveNewEntity";


        public const string Administracion = "Administracion";
        public const string Agregar = "Agregar";
        public const string Modificar = "Modificar";
        public const string Eliminar = "Eliminar";



        public List<PermissionType> Permissions { get; set; }
    }
}
