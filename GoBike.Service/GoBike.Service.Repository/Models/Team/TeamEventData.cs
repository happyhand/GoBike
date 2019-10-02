using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GoBike.Service.Repository.Models.Team
{
    /// <summary>
    /// 車隊活動資料
    /// </summary>
    public class TeamEventData
    {
        /// <summary>
        /// Gets or sets Altitude
        /// </summary>
        [BsonElement("Altitude")]
        public long Altitude { get; set; }

        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets Distance
        /// </summary>
        [BsonElement("Distance")]
        public long Distance { get; set; }

        /// <summary>
        /// Gets or sets EventDate
        /// </summary>
        [BsonElement("EventDate")]
        public DateTime EventDate { get; set; }

        /// <summary>
        /// Gets or sets EventID
        /// </summary>
        [BsonElement("EventID")]
        public string EventID { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets JoinMemberIDs
        /// </summary>
        [BsonElement("JoinMemberIDs")]
        public IEnumerable<string> JoinMemberIDs { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        [BsonElement("MemberID")]
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets RoadLines (TODO)
        /// </summary>
        [BsonElement("RoadLines")]
        public IEnumerable<dynamic> RoadLines { get; set; }

        /// <summary>
        /// Gets or sets RoadRemarks (TODO)
        /// </summary>
        [BsonElement("RoadRemarks")]
        public IEnumerable<dynamic> RoadRemarks { get; set; }

        /// <summary>
        /// Gets or sets Site
        /// </summary>
        [BsonElement("Site")]
        public string Site { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        [BsonElement("TeamID")]
        public string TeamID { get; set; }

        /// <summary>
        /// Gets or sets Title
        /// </summary>
        [BsonElement("Title")]
        public string Title { get; set; }
    }
}