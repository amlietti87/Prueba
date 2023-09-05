using System;
using Microsoft.Extensions.Configuration;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Extensions;

namespace TECSO.FWK.Domain.Mail
{
    /// <summary>
    /// Implementation of <see cref="IEmailSenderConfiguration"/> that reads settings
    /// from <see cref="ISettingManager"/>.
    /// </summary>
    public abstract class EmailSenderConfiguration : IEmailSenderConfiguration
    {
        public virtual string DefaultFromAddress
        {
            get { return GetNotEmptySettingValue(EmailSettingNames.DefaultFromAddress); }
        }

        public virtual string DefaultFromDisplayName
        {
            get { return SettingManager.GetValue<String>(EmailSettingNames.DefaultFromDisplayName); }
        }


        protected readonly Microsoft.Extensions.Configuration.IConfiguration SettingManager;

        /// <summary>
        /// Creates a new <see cref="EmailSenderConfiguration"/>.
        /// </summary>
        protected EmailSenderConfiguration(IConfiguration settingManager)
        {
            SettingManager = settingManager;
        }

        /// <summary>
        /// Gets a setting value by checking. Throws <see cref="AbpException"/> if it's null or empty.
        /// </summary>
        /// <param name="name">Name of the setting</param>
        /// <returns>Value of the setting</returns>
        protected string GetNotEmptySettingValue(string name)
        {
            var value = SettingManager.GetValue<string>(name);

            if (value.IsNullOrEmpty())
            {
                throw new TecsoException($"Setting value for '{name}' is null or empty!");
            }

            return value;
        }
    }
}