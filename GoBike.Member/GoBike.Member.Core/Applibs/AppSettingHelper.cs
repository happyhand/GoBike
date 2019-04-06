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
        /// Gets or sets MongoDBConfig
        /// </summary>
        public MongoDBSetting MongoDBConfig { get; set; }

        /// <summary>
        /// Gets or sets RedisConnection
        /// </summary>
        public string RedisConnection { get; set; }

        /// <summary>
        /// Gets or sets SmtpConfig
        /// </summary>
        public SmtpSetting SmtpConfig { get; set; }

        /// <summary>
        /// MongoDB 設定資料
        /// </summary>
        public class MongoDBSetting
        {
            /// <summary>
            /// Gets or sets ConnectionString
            /// </summary>
            public string ConnectionString { get; set; }

            /// <summary>
            /// Gets or sets MemberDatabase
            /// </summary>
            public string MemberDatabase { get; set; }
        }

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
}