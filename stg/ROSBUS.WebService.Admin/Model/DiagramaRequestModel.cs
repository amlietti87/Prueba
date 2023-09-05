using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ROSBUS.WebService.Admin.Model
{
    public class DiagramaRequestModel
    {
        public int Id { get; set; }
        public List<int> TurnoId { get; set; }

        public Boolean Blockentity { get; set; }

    }
}
