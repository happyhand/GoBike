namespace GoBike.Member.Core.Applibs
{
    /// <summary>
    /// APP 設定資料
    /// </summary>
    public class AppSettingHelper
    {
        /// <summary>
        /// Appsetting
        /// </summary>
        public static AppSettingHelper Appsetting;

        /// <summary>
        /// Gets or sets SmtpConfig
        /// </summary>
        public SmtpSetting SmtpConfig { get; set; }

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

            /// <summary>
            /// Gets or sets SmtpUser
            /// </summary>
            public string SmtpUser { get; set; }
        }
    }
}