namespace GoBike.Member.Core.Resource
{
    /// <summary>
    /// Smtp 設定資料
    /// </summary>
    public class SmtpSetting
    {
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
    }
}