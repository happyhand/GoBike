namespace GoBike.API.Core.Applibs
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
            /// <summary>
            /// Gets or sets InteractiveService
            /// </summary>
            public string InteractiveService { get; set; }

            /// <summary>
            /// Gets or sets MemberService
            /// </summary>
            public string MemberService { get; set; }

            /// <summary>
            /// Gets or sets SmtpService
            /// </summary>
            public string SmtpService { get; set; }
        }

        #endregion Service Domain 設定資料
    }
}