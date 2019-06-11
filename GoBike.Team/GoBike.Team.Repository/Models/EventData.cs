using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GoBike.Team.Repository.Models
{
    /// <summary>
    /// 活動資料
    /// </summary>
    public class EventData
    {
        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets CreatorID
        /// </summary>
        [BsonElement("CreatorID")]
        public string CreatorID { get; set; }

        /// <summary>
        /// Gets or sets Description
        /// </summary>
        [BsonElement("Description")]
        public string Description { get; set; }

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
        /// Gets or sets HaveSeenMemberIDs
        /// </summary>
        [BsonElement("HaveSeenMemberIDs")]
        public IEnumerable<string> HaveSeenMemberIDs { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets JoinMemberList
        /// </summary>
        [BsonElement("JoinMemberList")]
        public IEnumerable<string> JoinMemberList { get; set; }

        /// <summary>
        /// Gets or sets RoutePoints
        /// </summary>
        [BsonElement("RoutePoints")]
        public IEnumerable<string> RoutePoints { get; set; }

        /// <summary>
        /// Gets or sets SaveDeadline
        /// </summary>
        [BsonElement("SaveDeadline")]
        public DateTime SaveDeadline { get; set; }

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
    }
}