using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GoBike.Service.Repository.Models.Member
{
    /// <summary>
    /// 會員資料
    /// </summary>
    public class MemberData
    {
        #region Register \ Login Data

        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        [BsonElement("CreateDate")]
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets Email
        /// </summary>
        [BsonElement("Email")]
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets FBToken
        /// </summary>
        [BsonElement("FBToken")]
        public string FBToken { get; set; }

        /// <summary>
        /// Gets or sets GoogleToken
        /// </summary>
        [BsonElement("GoogleToken")]
        public string GoogleToken { get; set; }

        /// <summary>
        /// Gets or sets Id
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        [BsonElement("MemberID")]
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets Password
        /// </summary>
        [BsonElement("Password")]
        public string Password { get; set; }

        #endregion Register \ Login Data

        #region Info Data

        /// <summary>
        /// Gets or sets Birthday
        /// </summary>
        [BsonElement("Birthday")]
        public string Birthday { get; set; }

        /// <summary>
        /// Gets or sets BodyHeight
        /// </summary>
        [BsonElement("BodyHeight")]
        public double BodyHeight { get; set; }

        /// <summary>
        /// Gets or sets BodyWeight
        /// </summary>
        [BsonElement("BodyWeight")]
        public double BodyWeight { get; set; }

        /// <summary>
        /// Gets or sets FrontCoverUrl
        /// </summary>
        [BsonElement("FrontCoverUrl")]
        public string FrontCoverUrl { get; set; }

        /// <summary>
        /// Gets or sets Gender
        /// </summary>
        [BsonElement("Gender")]
        public int Gender { get; set; }

        /// <summary>
        /// Gets or sets Mobile
        /// </summary>
        [BsonElement("Mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets MoblieBindType
        /// </summary>
        [BsonElement("MoblieBindType")]
        public int MoblieBindType { get; set; }

        /// <summary>
        /// Gets or sets PhotoUrl
        /// </summary>
        [BsonElement("PhotoUrl")]
        public string PhotoUrl { get; set; }

        #endregion Info Data
    }
}