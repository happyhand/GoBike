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
        /// <param name="mailContext">mailContext</param>
        /// <returns>string</returns>
        public async Task<string> SendEmail(MailContext mailContext)
        {
            try
            {
                if (string.IsNullOrEmpty(mailContext.Addressee))
                {
                    return "收信人無效.";
                }
                if (string.IsNullOrEmpty(mailContext.EmailAddress))
                {
                    return "郵件網址無效.";
                }
                if (string.IsNullOrEmpty(mailContext.Subject))
                {
                    return "空白郵件主旨.";
                }

                if (string.IsNullOrEmpty(mailContext.Body))
                {
                    return "空白郵件內容.";
                }

                using (var message = new MailMessage())
                {
                    message.To.Add(new MailAddress(mailContext.EmailAddress, mailContext.Addressee));
                    message.From = new MailAddress(AppSettingHelper.Appsetting.SmtpConfig.SmtpMail, AppSettingHelper.Appsetting.SmtpConfig.SmtpUser);
                    message.Subject = mailContext.Subject;
                    message.Body = mailContext.Body;
                    message.IsBodyHtml = true;

                    using (var client = new SmtpClient(AppSettingHelper.Appsetting.SmtpConfig.SmtpServer))
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
                this.logger.LogError($"Send Email Error >>> Data:{Utility.GetPropertiesData(mailContext)}\n{ex}");
                return "發送郵件發生錯誤.";
            }
        }
    }
}