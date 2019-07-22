using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace GoBike.Service.Repository.Models.Team
{
    public class TeamData
    {
        #region Register Data

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
        /// Gets or sets TeamID
        /// </summary>
        [BsonElement("TeamID")]
        public string TeamID { get; set; }

        #endregion Register Data

        #region Info Data

        /// <summary>
        /// Gets or sets CityID
        /// </summary>
        [BsonElement("CityID")]
        public int CityID { get; set; }

        /// <summary>
        /// Gets or sets ExamineStatus
        /// </summary>
        [BsonElement("ExamineStatus")]
        public int ExamineStatus { get; set; }

        /// <summary>
        /// Gets or sets FrontCoverUrl
        /// </summary>
        [BsonElement("FrontCoverUrl")]
        public string FrontCoverUrl { get; set; }

        /// <summary>
        /// Gets or sets PhotoUrl
        /// </summary>
        [BsonElement("PhotoUrl")]
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Gets or sets SearchStatus
        /// </summary>
        [BsonElement("SearchStatus")]
        public int SearchStatus { get; set; }

        /// <summary>
        /// Gets or sets TeamInfo
        /// </summary>
        [BsonElement("TeamInfo")]
        public string TeamInfo { get; set; }

        /// <summary>
        /// Gets or sets TeamName
        /// </summary>
        [BsonElement("TeamName")]
        public string TeamName { get; set; }

        #endregion Info Data

        #region Management Data

        /// <summary>
        /// Gets or sets TeamApplyForJoinIDs
        /// </summary>
        [BsonElement("TeamApplyForJoinIDs")]
        public IEnumerable<string> TeamApplyForJoinIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamInviteJoinIDs
        /// </summary>
        [BsonElement("TeamInviteJoinIDs")]
        public IEnumerable<string> TeamInviteJoinIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamLeaderID
        /// </summary>
        [BsonElement("TeamLeaderID")]
        public string TeamLeaderID { get; set; }

        /// <summary>
        /// Gets or sets TeamMemberIDs
        /// </summary>
        [BsonElement("TeamMemberIDs")]
        public IEnumerable<string> TeamMemberIDs { get; set; }

        /// <summary>
        /// Gets or sets TeamViceLeaderIDs
        /// </summary>
        [BsonElement("TeamViceLeaderIDs")]
        public IEnumerable<string> TeamViceLeaderIDs { get; set; }

        #endregion Management Data
    }
}