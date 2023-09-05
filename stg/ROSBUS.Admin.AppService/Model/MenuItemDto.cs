using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{

    public class AppMenu
    {
        public AppMenu()
        {
            items = new List<MenuItemDto>();
        }
        public AppMenu(string name, string displayName, List<MenuItemDto> items = null) : this()
        {
            this.name = name;
            this.displayName = displayName;
            this.items = items ?? new List<MenuItemDto>();
        }

        public string name { get; set; }
        public string displayName { get; set; } 
        public List<MenuItemDto> items { get; set; }
    }


    public class MenuItemDto
    {
        public MenuItemDto()
        {
            granted = true;
            this.items = new List<MenuItemDto>();
        }
        public string name { get; set; }
        public string permissionName { get; set; }
        public string icon { get; set; }
        public string route { get; set; }
        public Boolean granted { get; set; }
        public List<MenuItemDto> items { get; set; }

        public MenuItemDto(string name, string permissionName, string icon , string route, List<MenuItemDto> items = null) : this()
        {

            this.name = name;
            this.permissionName = permissionName;
            this.icon = icon;
            this.route = route;

            if (items != null)
            {
                this.items = items;
            }
        }
    }

}
