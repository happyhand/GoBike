namespace GoBike.API.Core.Resource
{
    /// <summary>
    /// 共用標記
    /// </summary>
    public class CommonFlagHelper
    {
        /// <summary>
        /// CommonFlag
        /// </summary>
        public static CommonFlagHelper CommonFlag;

        /// <summary>
        /// Gets or sets RedisFlag
        /// </summary>
        public RedisSetting RedisFlag { get; set; }

        /// <summary>
        /// Gets or sets SeparateFlag
        /// </summary>
        public string SeparateFlag { get; set; }

        /// <summary>
        /// Gets or sets SessionFlag
        /// </summary>
        public SessionSetting SessionFlag { get; set; }

        /// <summary>
        /// Redis 共用標記
        /// </summary>
        public class RedisSetting
        {
            /// <summary>
            /// Gets or sets VerifierCode
            /// </summary>
            public string VerifierCode { get; set; }
        }

        /// <summary>
        /// Session 共用標記
        /// </summary>
        public class SessionSetting
        {
            /// <summary>
            /// Gets or sets MemberID
            /// </summary>
            public string MemberID { get; set; }
        }
    }
}