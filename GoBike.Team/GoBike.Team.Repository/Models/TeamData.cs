using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GoBike.Team.Repository.Models
{
    /// <summary>
    /// 車隊資料
    /// </summary>
    public class TeamData
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets TeamBlacklistedIDs
        /// </summary>
        [BsonElement("TeamBlacklistedIDs")]
        public IEnumerable<string> TeamBlacklistedIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamBlacklistIDs
        /// </summary>
        [BsonElement("TeamBlacklistIDs")]
        public IEnumerable<string> TeamBlacklistIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamCoverPhoto
        /// </summary>
        [BsonElement("TeamCoverPhoto")]
        public string TeamCoverPhoto { get; set; }

        /// <summary>
        /// Gets or sets TeamCreateDate
        /// </summary>
        [BsonElement("TeamCreateDate")]
        public DateTime TeamCreateDate { get; set; }

        /// <summary>
        /// Gets or sets TeamCreatorID
        /// </summary>
        [BsonElement("TeamCreatorID")]
        public string TeamCreatorID { get; set; }

        /// <summary>
        /// Gets or sets TeamEventIDs
        /// </summary>
        [BsonElement("TeamEventIDs")]
        public IEnumerable<string> TeamEventIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamExamineStatus (0:close, 1:open)
        /// </summary>
        [BsonElement("TeamExamineStatus")]
        public int TeamExamineStatus { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        [BsonElement("TeamID")]
        public string TeamID { get; set; }

        /// <summary>
        /// Gets or sets TeamInfo
        /// </summary>
        [BsonElement("TeamInfo")]
        public string TeamInfo { get; set; }

        /// <summary>
        /// Gets or sets TeamLocation
        /// </summary>
        [BsonElement("TeamLocation")]
        public string TeamLocation { get; set; }

        /// <summary>
        /// Gets or sets TeamName
        /// </summary>
        [BsonElement("TeamName")]
        public string TeamName { get; set; }

        /// <summary>
        /// Gets or sets TeamPhoto
        /// </summary>
        [BsonElement("TeamPhoto")]
        public string TeamPhoto { get; set; }

        /// <summary>
        /// Gets or sets TeamPlayerIDs
        /// </summary>
        [BsonElement("TeamPlayerIDs")]
        public IEnumerable<string> TeamPlayerIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamSaveDeadline
        /// </summary>
        [BsonElement("TeamSaveDeadline")]
        public DateTime TeamSaveDeadline { get; set; }

        /// <summary>
        /// Gets or sets TeamSearchStatus (0:close, 1:open)
        /// </summary>
        [BsonElement("TeamSearchStatus")]
        public int TeamSearchStatus { get; set; }

        /// <summary>
        /// Gets or sets TeamViceLeaderID
        /// </summary>
        [BsonElement("TeamViceLeaderID")]
        public string TeamViceLeaderID { get; set; }
    }
}