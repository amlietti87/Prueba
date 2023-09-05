using System;
using System.Collections.Generic;
using System.Text;

namespace TECSO.FWK.Domain.Extensions
{
    public static class LDapUtils
    {


        public static LdapConfiguration LdapConfiguration { get; set; }



    }

    public class LdapConfiguration
    {
        public LdapConfiguration()
        {
            //TODO: Ver LDAP quitar
            //"172.16.17.19:389" datos de prueba se sacan del appconfig
            //this.Domain = "172.16.17.19";
            //this.Port = "389";
        }

        public String UserName { get; set; }

        public String Password { get; set; }


        public String Domain { get; set; }

        public String Port { get; set; }
        
        public Boolean IsDevelopment { get; set; }

        public String GetFulldomain()
        {
            return string.Format("{0}:{1}", this.Domain, this.Port);
        }
    }
}
