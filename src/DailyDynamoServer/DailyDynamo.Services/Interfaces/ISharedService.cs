using DailyDynamo.Shared.Common;
using DailyDynamo.Shared.Models.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DailyDynamo.Services.Interfaces
{
    public interface ISharedService
    {
        Task<(bool, string)> EmailSendingAsync(string subject, string body, List<string> to, List<string>? cc = null, List<string>? bcc = null);
    }
}
