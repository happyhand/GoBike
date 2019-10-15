namespace GoBike.MGT.Core.Applibs
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

        #region Redis 設定資料

        /// <summary>
        /// Gets or sets RedisConnection
        /// </summary>
        public string RedisConnection { get; set; }

        #endregion Redis 設定資料

        #region Service Domain 設定資料

        /// <summary>
        /// Gets or sets ServiceDomain
        /// </summary>
        public ServiceDomainSetting ServiceDomain { get; set; }

        public class ServiceDomainSetting
        {
        }

        #endregion Service Domain 設定資料
    }
}