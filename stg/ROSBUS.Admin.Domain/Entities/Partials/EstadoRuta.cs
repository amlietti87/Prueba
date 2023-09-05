using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaEstadoRuta
    {
        public const int BORRADOR = 1;

        public const int APROBADO = 2;

        public const int DESCARTADO = 3;
    }

    public partial class PlaTipoBandera
    {
        public const int Comerciales = 1;

        public const int Posicionamiento = 2;
         
    }
}
