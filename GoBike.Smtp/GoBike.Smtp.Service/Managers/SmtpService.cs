using GoBike.Member.Core.Applibs;
using GoBike.Smtp.Core.Resource;
using GoBike.Smtp.Service.Interface;
using GoBike.Smtp.Service.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;

namespace GoBike.Smtp.Service.Managers
{
    /// <summary>
    /// 郵件管理微服務
    /// </summary>
    public class SmtpService : ISmtpService
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<SmtpService> logger;

        public SmtpService(ILogger<SmtpService> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 發送郵件
        /// </summary>
        /// <param name="emailContext">emailContext</param>
        /// <returns>string</returns>
        public async Task<string> SendEmail(EmailContext emailContext)
        {
            try
            {
                if (string.IsNullOrEmpty(emailContext.Address))
                {
                    return "郵件網址無效.";
                }
                if (string.IsNullOrEmpty(emailContext.Subject))
                {
                    return "空白郵件主旨.";
                }

                if (string.IsNullOrEmpty(emailContext.Body))
                {
                    return "空白郵件內容.";
                }

                using (MailMessage message = new MailMessage())
                {
                    message.To.Add(new MailAddress(emailContext.Address));
                    message.From = new MailAddress(AppSettingHelper.Appsetting.SmtpConfig.SmtpMail, AppSettingHelper.Appsetting.SmtpConfig.SmtpUser);
                    message.Subject = emailContext.Subject;
                    message.Body = emailContext.Body;
                    message.IsBodyHtml = true;

                    using (SmtpClient client = new SmtpClient(AppSettingHelper.Appsetting.SmtpConfig.SmtpServer))
                    {
                        client.Port = 587;//// Google Port 587
                        client.Credentials = new NetworkCredential(AppSettingHelper.Appsetting.SmtpConfig.SmtpMail, AppSettingHelper.Appsetting.SmtpConfig.SmtpPassword);
                        client.EnableSsl = true;
                        await client.SendMailAsync(message);
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Send Email Error >>> Data:{Utility.GetPropertiesData(emailContext)}\n{ex}");
                return "發送郵件發生錯誤.";
            }
        }
    }
}