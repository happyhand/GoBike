namespace GoBike.Member.Core.Resource
{
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
}