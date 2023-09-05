using Microsoft.Extensions.Configuration;
using TECSO.FWK.Domain.Mail.Smtp;

namespace ROSBUS.Admin.Domain.Emailing
{
    public class ROSBUSSmtpEmailSenderConfiguration : SmtpEmailSenderConfiguration
    {
        public ROSBUSSmtpEmailSenderConfiguration(IConfiguration settingManager) : base(settingManager)
        {
           
        }
        //public override string Password => SimpleStringCipher.Instance.Decrypt(GetNotEmptySettingValue(EmailSettingNames.Smtp.Password));
    }
}