using System;
using System.Collections.Generic;
using System.Text;

namespace TECSO.FWK.Domain.Interfaces.Entities
{
    public interface IConcurrencyEntity
    {
        DateTime? BlockDate { get; set; }

        String GetEnityId();

    }
}
