using System;
using System.Collections.Generic;

namespace GoBike.API.Service.Models.Team.Command.Data
{
    /// <summary>
    /// 活動詳細資料
    /// </summary>
    public class EventDetailInfoDto : EventSimpleInfoDto
    {
        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets Highest
        /// </summary>
        public string Highest { get; set; }

        /// <summary>
        /// Gets or sets RoutePoints
        /// </summary>
        public IEnumerable<string> RoutePoints { get; set; }

        /// <summary>
        /// Gets or sets SaveDeadline
        /// </summary>
        public DateTime SaveDeadline { get; set; }

        /// <summary>
        /// Gets or sets Site
        /// </summary>
        public string Site { get; set; }
    }
}