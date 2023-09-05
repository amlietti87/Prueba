using System;
using System.Collections.Generic;
using System.Text;

namespace ROSBUS.Admin.Domain.ParametersHelper
{
    public interface IParametersHelper
    {
        T GetParameter<T>(string token);
    }
}
