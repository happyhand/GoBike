using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GoBike.Team.Repository.Models
{
    /// <summary>
    /// 公告資料
    /// </summary>
    public class AnnouncementData
    {
        /// <summary>
        /// Gets or sets AnnouncementID
        /// </summary>
        [BsonElement("AnnouncementID")]
        public string AnnouncementID { get; set; }

        /// <summary>
        /// Gets or sets Context
        /// </summary>
        [BsonElement("Context")]
        public string Context { get; set; }

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
        /// Gets or sets LimitDate
        /// </summary>
        [BsonElement("LimitDate")]
        public int LimitDate { get; set; }

        /// <summary>
        /// Gets or sets SaveDeadline
        /// </summary>
        [BsonElement("SaveDeadline")]
        public DateTime SaveDeadline { get; set; }

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
    }
}