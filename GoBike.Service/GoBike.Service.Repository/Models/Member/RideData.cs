using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoBike.Service.Repository.Models.Member
{
    /// <summary>
    /// 騎乘資料
    /// </summary>
    public class RideData
    {
        /// <summary>
        /// Gets or sets Altitude
        /// </summary>
        [BsonElement("Altitude")]
        public string Altitude { get; set; }

        /// <summary>
        /// Gets or sets CountyID
        /// </summary>
        [BsonElement("CountyID")]
        public int CountyID { get; set; }

        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets Distance
        /// </summary>
        [BsonElement("Distance")]
        public string Distance { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets Level
        /// </summary>
        [BsonElement("Level")]
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        [BsonElement("MemberID")]
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets PhotoUrl
        /// </summary>
        [BsonElement("PhotoUrl")]
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Gets or sets RideID
        /// </summary>
        [BsonElement("RideID")]
        public string RideID { get; set; }

        /// <summary>
        /// Gets or sets Route
        /// </summary>
        [BsonElement("Route")]
        public IEnumerable<RideRouteData> Route { get; set; }

        /// <summary>
        /// Gets or sets ShareContent
        /// </summary>
        [BsonElement("ShareContent")]
        public IEnumerable<RideContentData> ShareContent { get; set; }

        /// <summary>
        /// Gets or sets SharedType
        /// </summary>
        [BsonElement("SharedType")]
        public int SharedType { get; set; }

        /// <summary>
        /// Gets or sets Time
        /// </summary>
        [BsonElement("Time")]
        public string Time { get; set; }

        /// <summary>
        /// Gets or sets Title
        /// </summary>
        [BsonElement("Title")]
        public string Title { get; set; }
    }
}