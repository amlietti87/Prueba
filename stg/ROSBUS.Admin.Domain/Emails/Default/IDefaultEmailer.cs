using ROSBUS.Admin.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ROSBUS.Admin.Domain
{
    public interface IDefaultEmailer
    {
        Task SendDefaultAsync(string emailto, string title = null, string content = null, List<KeyValuePair<System.IO.Stream, string>> adjuntos = null);
        bool IsValidOneEmail(string email);

        Task<string> ValidateEmails(string mails);

    }
}
