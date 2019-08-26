﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GoBike.Service.Repository.Models.Member
{
    /// <summary>
    /// 騎乘資料
    /// </summary>
    public class RideData
    {
        /// <summary>
        /// Gets or sets CityID
        /// </summary>
        [BsonElement("CityID")]
        public int CityID { get; set; }

        /// <summary>
        /// Gets or sets Climb
        /// </summary>
        [BsonElement("Climb")]
        public double Climb { get; set; }

        /// <summary>
        /// Gets or sets Content
        /// </summary>
        [BsonElement("Content")]
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets Distance
        /// </summary>
        [BsonElement("Distance")]
        public double Distance { get; set; }

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
        /// Gets or sets MapUrl
        /// </summary>
        [BsonElement("MapUrl")]
        public string MapUrl { get; set; }

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
        /// Gets or sets RideTime
        /// </summary>
        [BsonElement("RideTime")]
        public long RideTime { get; set; }

        /// <summary>
        /// Gets or sets Title
        /// </summary>
        [BsonElement("Title")]
        public string Title { get; set; }
    }
}