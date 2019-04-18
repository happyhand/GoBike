using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

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
        /// Gets or sets TeamID
        /// </summary>
        [BsonElement("TeamID")]
        public string TeamID { get; set; }

        /// <summary>
        /// Gets or sets TeamCreateDate
        /// </summary>
        [BsonElement("TeamCreateDate")]
        public string TeamCreateDate { get; set; }

        /// <summary>
        /// Gets or sets TeamName
        /// </summary>
        [BsonElement("TeamName")]
        public string TeamName { get; set; }

        /// <summary>
        /// Gets or sets TeamLocation
        /// </summary>
        [BsonElement("TeamLocation")]
        public string TeamLocation { get; set; }

        /// <summary>
        /// Gets or sets TeamInfo
        /// </summary>
        [BsonElement("TeamInfo")]
        public string TeamInfo { get; set; }

        /// <summary>
        /// Gets or sets TeamSearchStatus
        /// </summary>
        [BsonElement("TeamSearchStatus")]
        public string TeamSearchStatus { get; set; }

        /// <summary>
        /// Gets or sets TeamExamineStatus
        /// </summary>
        [BsonElement("TeamExamineStatus")]
        public string TeamExamineStatus { get; set; }

        /// <summary>
        /// Gets or sets TeamPhoto
        /// </summary>
        [BsonElement("TeamPhoto")]
        public string TeamPhoto { get; set; }

        /// <summary>
        /// Gets or sets TeamCoverPhoto
        /// </summary>
        [BsonElement("TeamCoverPhoto")]
        public string TeamCoverPhoto { get; set; }
    }
}