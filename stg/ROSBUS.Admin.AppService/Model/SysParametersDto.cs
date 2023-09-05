using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class SysParametersDto : EntityDto<long>
    {

        public string Token { get; set; }
        public string Value { get; set; }
        public int? DataTypeId { get; set; }
        public SysDataTypesDto SysDataType { get; set; }
        public string Descripcion { get; set; }
        public override string Description => Descripcion;
    }
}
