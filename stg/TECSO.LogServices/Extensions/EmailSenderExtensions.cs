using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using TECSO.LogServices.Services;

namespace TECSO.LogServices.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string link)
        {
            return emailSender.SendEmailAsync(email, "Confirme su email",
                $"Por favor confirme su cuenta por <a href='{HtmlEncoder.Default.Encode(link)}'>haciendo clic aqu�</a>.");
        }

        public static Task SendResetPasswordAsync(this IEmailSender emailSender, string email, string callbackUrl)
        {
            return emailSender.SendEmailAsync(email, "Blanqueo de contrase�a",
                $"Restablece tu contrase�a <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>haciendo clic aqu�</a>.");
        }
    }
}
