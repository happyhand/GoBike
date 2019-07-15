using System;

namespace GoBike.API.Service.Models.Member.Data
{
    /// <summary>
    /// 騎乘資料
    /// </summary>
    public class RideDto
    {
        /// Gets or sets CityID
        /// </summary>
        public int CityID { get; set; }

        /// <summary>
        /// Gets or sets Climb
        /// </summary>
        public double Climb { get; set; }

        /// <summary>
        /// Gets or sets Content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets Distance
        /// </summary>
        public double Distance { get; set; }

        /// <summary>
        /// Gets or sets Level
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        /// Gets or sets MapUrl
        /// </summary>
        public string MapUrl { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets PhotoUrl
        /// </summary>
        public string PhotoUrl { get; set; }

        /// <summary>
        /// Gets or sets RideTime
        /// </summary>
        public long RideTime { get; set; }

        /// <summary>
        /// Gets or sets Title
        /// </summary>
        public string Title { get; set; }
    }
}