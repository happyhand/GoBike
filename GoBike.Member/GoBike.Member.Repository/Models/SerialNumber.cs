﻿using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoBike.Member.Repository.Models
{
    /// <summary>
    /// 流水序號
    /// </summary>
    public class SerialNumber
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets SequenceName
        /// </summary>
        [BsonElement("SequenceName")]
        public string SequenceName { get; set; }

        /// <summary>
        /// Gets or sets SequenceValue
        /// </summary>
        [BsonElement("SequenceValue")]
        public string SequenceValue { get; set; }
    }
}