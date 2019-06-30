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
        #region Login Data

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
        /// Gets or sets LoginDate
        /// </summary>
        [BsonElement("LoginDate")]
        public DateTime LoginDate { get; set; }

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

        #endregion Login Data
    }
}