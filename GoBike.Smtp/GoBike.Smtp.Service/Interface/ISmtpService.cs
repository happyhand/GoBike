using GoBike.Smtp.Service.Models;
using System.Threading.Tasks;

namespace GoBike.Smtp.Service.Interface
{
    public interface ISmtpService
    {
        /// <summary>
        /// 發送郵件
        /// </summary>
        /// <param name="mailContext">mailContext</param>
        /// <returns>string</returns>
        Task<string> SendEmail(MailContext mailContext);
    }
}