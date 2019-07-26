using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoBike.Service.Repository.Models.Common
{
    /// <summary>
    /// 市區資料
    /// </summary>
    public class CityData
    {
        /// <summary>
        /// Gets or sets CityID
        /// </summary>
        [BsonElement("CityID")]
        public string CityID { get; set; }

        /// <summary>
        /// Gets or sets CityName
        /// </summary>
        [BsonElement("CityName")]
        public string CityName { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public ObjectId Id { get; set; }
    }
}