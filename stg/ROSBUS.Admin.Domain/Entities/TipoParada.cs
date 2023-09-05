using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaTipoParada : Entity<int>
    {
        public PlaTipoParada()
        {
            PlaPuntos = new HashSet<PlaPuntos>();
            PlaTiempoEsperadoDeCarga = new HashSet<PlaTiempoEsperadoDeCarga>();
        }

        public string Nombre { get; set; }
        public string Abreviatura { get; set; }

        public ICollection<PlaPuntos> PlaPuntos { get; set; }
        public ICollection<PlaTiempoEsperadoDeCarga> PlaTiempoEsperadoDeCarga { get; set; }
    }
}
