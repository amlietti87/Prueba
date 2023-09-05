using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace ROSBUS.Admin.Domain.Model
{
    public class ReporteReclamosExcel : ReporteReclamosExcelGrouped
    {
        public int Id { get; set; }
    }
}
