using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GoBike.Service.Repository.Models.Member
{
    /// <summary>
    /// 互動資料
    /// </summary>
    public class InteractiveData
    {
        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets InteractiveID
        /// </summary>
        [BsonElement("InteractiveID")]
        public string InteractiveID { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        [BsonElement("MemberID")]
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets Status
        /// </summary>
        [BsonElement("Status")]
        public int Status { get; set; }
    }
}