﻿namespace GoBike.Team.Core.Applibs
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
            /// Gets or sets ConnectionString
            /// </summary>
            public string ConnectionString { get; set; }

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
                /// Gets or sets Announcement
                /// </summary>
                public string Announcement { get; set; }

                /// <summary>
                /// Gets or sets Event
                /// </summary>
                public string Event { get; set; }

                /// <summary>
                /// Gets or sets Interactive
                /// </summary>
                public string Interactive { get; set; }

                /// <summary>
                /// Gets or sets Team
                /// </summary>
                public string Team { get; set; }
            }
        }

        /// <summary>
        /// 活動預約最大天數
        /// </summary>
        /// <param name=""></param>
        /// <returns></returns>
        public int EventMaxDate { get; set; }

        #endregion MongoDB 設定資料
    }
}