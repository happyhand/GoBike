using GoBike.Team.Service.Models;
using System.Collections.Generic;

namespace GoBike.Team.API.Models
{
    /// <summary>
    /// 我的車隊資料
    /// </summary>
    public class MyTeamInfoDto
    {
        /// <summary>
        /// Gets or sets TeamCoverPhoto
        /// </summary>
        public TeamInfoDto CreatorTeamData { get; set; }

        /// <summary>
        /// Gets or sets TeamCreatorID
        /// </summary>
        public IEnumerable<TeamInfoDto> JoinTeamDatas { get; set; }
    }
}