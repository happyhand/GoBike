using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoBike.Team.Repository.Models
{
    /// <summary>
    /// 車隊活動資料
    /// </summary>
    public class EventData
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets EventID
        /// </summary>
        [BsonElement("EventID")]
        public string EventID { get; set; }

        /// <summary>
        /// Gets or sets EventTitle
        /// </summary>
        [BsonElement("EventTitle")]
        public string EventTitle { get; set; }

        /// <summary>
        /// Gets or sets EventInfo
        /// </summary>
        [BsonElement("EventInfo")]
        public string EventInfo { get; set; }

        /// <summary>
        /// Gets or sets EventSite
        /// </summary>
        [BsonElement("EventSite")]
        public string EventSite { get; set; }

        /// <summary>
        /// Gets or sets EventTime
        /// </summary>
        [BsonElement("EventTime")]
        public string EventTime { get; set; }

        /// <summary>
        /// Gets or sets EventRegisterDeadline
        /// </summary>
        [BsonElement("EventRegisterDeadline")]
        public string EventRegisterDeadline { get; set; }

        /// <summary>
        /// Gets or sets EventRoutePoints
        /// </summary>
        [BsonElement("EventRoutePoints")]
        public string EventRoutePoints { get; set; }
    }
}