using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace TECSO.FWK.Domain
{
    [Serializable]
    public class LoginValidationException : TecsoException
    {

        public const int Code_UserPasswordInvalid = 1;
        public const int Code_RequiredCaptcha = 2;
        public const int Code_InvalidCaptcha = 3;

        public LoginValidationException(string message)
         : base(message)
        {
        }


        public LoginValidationException(string message, int code)
         : base(message)
        {
            this.HResult = code;
        }
    }

}
