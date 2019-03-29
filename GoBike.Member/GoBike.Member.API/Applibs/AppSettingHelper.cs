namespace GoBike.Member.App.Applibs
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
    }
}