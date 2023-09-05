using ROSBUS.Admin.Domain.Emailing;
using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.bus;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Mail;
using TECSO.FWK.Extensions;

namespace ROSBUS.Admin.Domain.Interfaces.Mail
{
    public class DefaultEmailer : IDefaultEmailer, ITransientDependency
    {
        private readonly IEmailSender _emailSender;
        private readonly IEmailTemplateProvider _emailTemplateProvider;
        public DefaultEmailer(
            IEmailTemplateProvider emailTemplateProvider,
            IEmailSender emailSender)
        {
            _emailTemplateProvider = emailTemplateProvider;
            _emailSender = emailSender;
        }



        public async Task SendDefaultAsync(string emailto, string title = null, string content = null, List<KeyValuePair<System.IO.Stream, string>> adjuntos = null)
        {

            var emailTemplate = GetTitleAndSubTitle(string.Empty, string.Empty);
            var mailMessage = new StringBuilder();
            mailMessage.AppendLine(content + "<br />");

            await ReplaceBodyAndSend(emailto, string.Format("Rosario Bus | {0}", title), emailTemplate, mailMessage, adjuntos);
        }




        private StringBuilder GetTitleAndSubTitle(string title, string subTitle)
        {
            var emailTemplate = new StringBuilder(_emailTemplateProvider.GetDefaultTemplate());
            emailTemplate.Replace("{EMAIL_TITLE}", title);
            emailTemplate.Replace("{EMAIL_SUB_TITLE}", subTitle);

            return emailTemplate;
        }

        public bool IsValidOneEmail(string email)
        {
            try
            {
                Regex _regex = new Regex(
    @"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$",
    RegexOptions.CultureInvariant | RegexOptions.Singleline);

                var result = _regex.IsMatch(email);

                return result;
            }
            catch
            {
                return false;
            }
        }

        public async Task<string> ValidateEmails(string mails)
        {
            List<string> mailserroneos = new List<string>();
            foreach (var address in mails.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                var isvalidmail = this.IsValidOneEmail(address.Trim());
                if (isvalidmail == false)
                {
                    mailserroneos.Add(address.Trim());
                }
            }
            if (mailserroneos.Count != 0)
            {
                StringBuilder build = new StringBuilder();
                build.AppendLine("Los siguientes mails no son válidos");
                foreach (var item in mailserroneos)
                {
                    build.AppendLine("<br/> " + item);
                }
                return build.ToString();
            }

            return string.Empty;
        }

        private async Task ReplaceBodyAndSend(string emailAddress, string subject, StringBuilder emailTemplate, StringBuilder mailMessage, List<KeyValuePair<System.IO.Stream, string>> adjuntos = null)
        {
            emailTemplate.Replace("{EMAIL_BODY}", mailMessage.ToString());

            MailMessage message = new MailMessage
            {
                Subject = subject,
                Body = emailTemplate.ToString(),
                IsBodyHtml = true
            };

            foreach (var address in emailAddress.Split(new[] { ";" }, StringSplitOptions.RemoveEmptyEntries))
            {
                message.To.Add(address.Trim());
            }
            if (adjuntos != null)
            {
                foreach (var item in adjuntos)
                {
                    message.Attachments.Add(new System.Net.Mail.Attachment(item.Key, item.Value));
                }
            }

            await _emailSender.SendAsync(message);
        }
    }
}
