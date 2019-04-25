using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GoBike.Team.Repository.Models
{
    /// <summary>
    /// 互動狀態資料
    /// </summary>
    public enum InteractiveStatusType
    {
        /// <summary>
        /// 會員申請加入車隊
        /// </summary>
        ApplyForJoin = 0,

        /// <summary>
        /// 車隊邀請會員加入
        /// </summary>
        InviteJoin = 1
    }

    /// <summary>
    /// 互動資料
    /// </summary>
    public class InteractiveData
    {
        #region Base Data

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
        /// Gets or sets TeamID
        /// </summary>
        [BsonElement("TeamID")]
        public string TeamID { get; set; }

        #endregion Base Data

        #region InfoData

        /// <summary>
        /// Gets or sets SaveDeadline
        /// </summary>
        [BsonElement("SaveDeadline")]
        public DateTime SaveDeadline { get; set; }

        /// <summary>
        /// Gets or sets Status (0:會員申請加入車隊，1:車隊邀請會員加入)
        /// </summary>
        [BsonElement("Status")]
        public int Status { get; set; }

        #endregion InfoData
    }
}