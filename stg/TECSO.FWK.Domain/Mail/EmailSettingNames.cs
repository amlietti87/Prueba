namespace TECSO.FWK.Domain.Mail
{    
    public static class EmailSettingNames
    {
        /// <summary>
        /// Mail:DefaultFromAddress
        /// </summary>
        public const string DefaultFromAddress = "Mail:DefaultFromAddress";

        /// <summary>
        /// Mail:DefaultFromDisplayName
        /// </summary>
        public const string DefaultFromDisplayName = "Mail:DefaultFromDisplayName";

        /// <summary>
        /// SMTP related email settings.
        /// </summary>
        public static class Smtp
        {
            /// <summary>
            /// Mail:Smtp.Host
            /// </summary>
            public const string Host = "Mail:Smtp.Host";

            /// <summary>
            /// Mail:Smtp.Port
            /// </summary>
            public const string Port = "Mail:Smtp.Port";

            /// <summary>
            /// Mail:Smtp.UserName
            /// </summary>
            public const string UserName = "Mail:Smtp.UserName";

            /// <summary>
            /// Mail:Smtp.Password
            /// </summary>
            public const string Password = "Mail:Smtp.Password";

            /// <summary>
            /// Mail:Smtp.Domain
            /// </summary>
            public const string Domain = "Mail:Smtp.Domain";

            /// <summary>
            /// Mail:Smtp.EnableSsl
            /// </summary>
            public const string EnableSsl = "Mail:Smtp.EnableSsl";

            /// <summary>
            /// Mail:Smtp.UseDefaultCredentials
            /// </summary>
            public const string UseDefaultCredentials = "Mail:Smtp.UseDefaultCredentials";
        }
    }
}