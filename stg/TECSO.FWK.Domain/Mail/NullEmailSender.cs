using System.Net.Mail;
using System.Threading.Tasks; 
namespace TECSO.FWK.Domain.Mail
{
    /// <summary>
    /// This class is an implementation of <see cref="IEmailSender"/> as similar to null pattern.
    /// It does not send emails but logs them.
    /// </summary>
    public class NullEmailSender : EmailSenderBase
    {


        /// <summary>
        /// Creates a new <see cref="NullEmailSender"/> object.
        /// </summary>
        /// <param name="configuration">Configuration</param>
        public NullEmailSender(IEmailSenderConfiguration configuration)
            : base(configuration)
        {

        }

        protected override Task SendEmailAsync(MailMessage mail)
        {
          
            return Task.FromResult(0);
        }

        protected override void SendEmail(MailMessage mail)
        {
            
        }

        private void LogEmail(MailMessage mail)
        {
           
        }
    }
}