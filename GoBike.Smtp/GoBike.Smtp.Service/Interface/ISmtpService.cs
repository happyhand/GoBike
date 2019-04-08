using GoBike.Smtp.Service.Models;
using System.Threading.Tasks;

namespace GoBike.Smtp.Service.Interface
{
    /// <summary>
    /// 郵件管理微服務
    /// </summary>
    public interface ISmtpService
    {
        /// <summary>
        /// 發送郵件
        /// </summary>
        /// <param name="emailContext">emailContext</param>
        /// <returns>string</returns>
        Task<string> SendEmail(EmailContext emailContext);
    }
}