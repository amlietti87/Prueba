using System;
using System.Collections.Generic;
using System.Text;

namespace TECSO.FWK.ApiServices.Model
{

    public class ResponseModel<T> : AbstractModel
    {
        public ResponseModel()
        {
            this.Messages = new List<string>();
        }

        public T DataObject { get; set; }
    }


    public abstract class AbstractModel
    {
        public String Status { get; set; }

        public List<String> Messages { get; set; }
    }
}
