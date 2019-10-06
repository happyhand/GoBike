namespace GoBike.Service.Core.Applibs
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
        /// Gets or sets DaysOfNewCreation
        /// </summary>
        public int DaysOfNewCreation { get; set; }

        /// <summary>
        /// Gets or sets MinDaysOfEvent
        /// </summary>
        public int MinDaysOfEvent { get; set; }

        /// <summary>
        /// Gets or sets TeamAnnouncementMaxLength
        /// </summary>
        public int TeamAnnouncementMaxLength { get; set; }

        #region MongoDB 設定資料

        /// <summary>
        /// Gets or sets MongoDBConfig
        /// </summary>
        public MongoDBSetting MongoDBConfig { get; set; }

        /// <summary>
        /// MongoDB 設定資料
        /// </summary>
        public class MongoDBSetting
        {
            /// <summary>
            /// Gets or sets CollectionFlag
            /// </summary>
            public CollectionSetting CollectionFlag { get; set; }

            /// <summary>
            /// Gets or sets CommonDatabase
            /// </summary>
            public string CommonDatabase { get; set; }

            /// <summary>
            /// Gets or sets ConnectionString
            /// </summary>
            public string ConnectionString { get; set; }

            /// <summary>
            /// Gets or sets MemberDatabase
            /// </summary>
            public string MemberDatabase { get; set; }

            /// <summary>
            /// Gets or sets TeamDatabase
            /// </summary>
            public string TeamDatabase { get; set; }

            /// <summary>
            /// Collection 設定資料
            /// </summary>
            public class CollectionSetting
            {
                /// <summary>
                /// Gets or sets City
                /// </summary>
                public string City { get; set; }

                /// <summary>
                /// Gets or sets Interactive
                /// </summary>
                public string Interactive { get; set; }

                /// <summary>
                /// Gets or sets Member
                /// </summary>
                public string Member { get; set; }

                /// <summary>
                /// Gets or sets Ride
                /// </summary>
                public string Ride { get; set; }

                /// <summary>
                /// Gets or sets SerialNumber
                /// </summary>
                public string SerialNumber { get; set; }

                /// <summary>
                /// Gets or sets Team
                /// </summary>
                public string Team { get; set; }

                /// <summary>
                /// Gets or sets TeamAnnouncement
                /// </summary>
                public string TeamAnnouncement { get; set; }

                /// <summary>
                /// Gets or sets TeamEvent
                /// </summary>
                public string TeamEvent { get; set; }

                /// <summary>
                /// Gets or sets TeamInteractive
                /// </summary>
                public string TeamInteractive { get; set; }
            }
        }

        #endregion MongoDB 設定資料
    }
}