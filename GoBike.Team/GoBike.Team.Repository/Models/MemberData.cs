using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace GoBike.Team.Repository.Models
{
    /// <summary>
    /// 會員資料
    /// </summary>
    public class MemberData
    {
        #region Base Data

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

        #endregion Base Data

        #region Info Data

        /// <summary>
        /// Gets or sets BirthDayDate
        /// </summary>
        [BsonElement("BirthDayDate")]
        public string BirthDayDate { get; set; }

        /// <summary>
        /// Gets or sets BodyHeight
        /// </summary>
        [BsonElement("BodyHeight")]
        public decimal BodyHeight { get; set; }

        /// <summary>
        /// Gets or sets BodyWeight
        /// </summary>
        [BsonElement("BodyWeight")]
        public decimal BodyWeight { get; set; }

        /// <summary>
        /// Gets or sets Gender (0:Girl, 1:Boy)
        /// </summary>
        [BsonElement("Gender")]
        public int Gender { get; set; }

        /// <summary>
        /// Gets or sets Mobile
        /// </summary>
        [BsonElement("Mobile")]
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets Nickname
        /// </summary>
        [BsonElement("Nickname")]
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets Photo
        /// </summary>
        [BsonElement("Photo")]
        public string Photo { get; set; }

        #endregion Info Data
    }
}