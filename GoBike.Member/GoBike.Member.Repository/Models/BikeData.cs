using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoBike.Member.Repository.Models
{
    /// <summary>
    /// 車輛資料
    /// </summary>
    public class BikeData
    {
        /// <summary>
        /// Gets or sets BikeID
        /// </summary>
        [BsonElement("BikeID")]
        public string BikeID { get; set; }

        /// <summary>
        /// Gets or sets BuyDate
        /// </summary>
        [BsonElement("BuyDate")]
        public string BuyDate { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets ManufactureYaer
        /// </summary>
        [BsonElement("ManufactureYaer")]
        public int ManufactureYaer { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        [BsonElement("MemberID")]
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets Model
        /// </summary>
        [BsonElement("Model")]
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets Photo
        /// </summary>
        [BsonElement("Photo")]
        public string Photo { get; set; }

        /// <summary>
        /// Gets or sets Size
        /// </summary>
        [BsonElement("Size")]
        public string Size { get; set; }

        /// <summary>
        /// Gets or sets Type
        /// </summary>
        [BsonElement("Type")]
        public string Type { get; set; }
    }
}