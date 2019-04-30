using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.Collections.Generic;

namespace GoBike.Interactive.Repository.Models
{
    /// <summary>
    /// 互動狀態資料
    /// </summary>
    public enum InteractiveStatusType
    {
        /// <summary>
        /// 無關聯
        /// </summary>
        None = -2,

        /// <summary>
        /// 黑名單
        /// </summary>
        Black = -1,

        /// <summary>
        /// 請求 - 等待確認
        /// </summary>
        Request = 0,

        /// <summary>
        /// 請求 - 處理確認
        /// </summary>
        RequestHandler = 1,

        /// <summary>
        /// 好友
        /// </summary>
        Friend = 2
    }

    /// <summary>
    /// 互動資料
    /// </summary>
    public class InteractiveData
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        [BsonElement("MemberID")]
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets BlacklistIDs
        /// </summary>
        [BsonElement("BlacklistIDs")]
        public IEnumerable<string> BlacklistIDs { get; set; }

        /// <summary>
        /// Gets or sets FriendListIDs
        /// </summary>
        [BsonElement("FriendListIDs")]
        public IEnumerable<string> FriendListIDs { get; set; }

        /// <summary>
        /// Gets or sets RequestListIDs
        /// </summary>
        [BsonElement("RequestListIDs")]
        public IEnumerable<string> RequestListIDs { get; set; }
    }
}