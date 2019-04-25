using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoBike.Interactive.Repository.Models
{
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
        /// Gets or sets InitiatorID
        /// </summary>
        [BsonElement("InitiatorID")]
        public string InitiatorID { get; set; }

        /// <summary>
        /// Gets or sets FriendID
        /// </summary>
        [BsonElement("PassiveID")]
        public string PassiveID { get; set; }

        #endregion Base Data

        #region InfoData

        /// <summary>
        /// Gets or sets Status (0:等待確認，1:好友，-1:黑名單)
        /// </summary>
        [BsonElement("Status")]
        public int Status { get; set; }

        #endregion InfoData
    }
}