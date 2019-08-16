using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GoBike.Service.Repository.Models.Team
{
    /// <summary>
    /// 車隊互動資料
    /// </summary>
    public class TeamInteractiveData
    {
        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets InteractiveType
        /// </summary>
        [BsonElement("InteractiveType")]
        public int InteractiveType { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        [BsonElement("InviteID")]
        public string InviteID { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        [BsonElement("MemberID")]
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets ReviewFlag
        /// </summary>
        [BsonElement("ReviewFlag")]
        public int ReviewFlag { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        [BsonElement("TeamID")]
        public string TeamID { get; set; }
    }
}