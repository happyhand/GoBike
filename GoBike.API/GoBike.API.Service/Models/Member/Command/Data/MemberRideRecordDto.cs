using System;

namespace GoBike.API.Service.Models.Member.Command.Data
{
    /// <summary>
    /// 會員騎乘資料
    /// </summary>
    public class MemberRideRecordDto
    {
        /// <summary>
        /// Gets or sets RecordDate
        /// </summary>
        public DateTime RecordDate { get; set; }

        /// <summary>
        /// Gets or sets RideSecond
        /// </summary>
        public long RideTime { get; set; }

        /// <summary>
        /// Gets or sets RideHeight
        /// </summary>
        public long RideHeight { get; set; }

        /// <summary>
        /// Gets or sets RideHeight
        /// </summary>
        public long RideDistance { get; set; }

        //// TODO
    }
}