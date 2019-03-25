namespace GoBike.API.Core.Models
{
    public class MailContext
    {
        /// <summary>
        /// Gets or sets Body
        /// </summary>
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets FromEmail
        /// </summary>
        public string FromEmail { get; set; }

        /// <summary>
        /// Gets or sets FromUserName
        /// </summary>
        public string FromUserName { get; set; }

        /// <summary>
        /// Gets or sets SmtpMail
        /// </summary>
        public string SmtpMail { get; set; }

        /// <summary>
        /// Gets or sets SmtpPassword
        /// </summary>
        public string SmtpPassword { get; set; }

        /// <summary>
        /// Gets or sets SmtpServer
        /// </summary>
        public string SmtpServer { get; set; }

        /// <summary>
        /// Gets or sets Subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets ToEmail
        /// </summary>
        public string ToEmail { get; set; }

        /// <summary>
        /// Gets or sets ToUserName
        /// </summary>
        public string ToUserName { get; set; }
    }
}