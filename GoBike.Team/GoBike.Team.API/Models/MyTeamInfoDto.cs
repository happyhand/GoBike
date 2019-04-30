using GoBike.Team.Service.Models.Data;
using System.Collections.Generic;

namespace GoBike.Team.API.Models
{
    /// <summary>
    /// 我的車隊資料
    /// </summary>
    public class MyTeamInfoDto
    {
        /// <summary>
        /// Gets or sets LeaderTeamDatas
        /// </summary>
        public IEnumerable<TeamInfoDto> LeaderTeamDatas { get; set; }

        /// <summary>
        /// Gets or sets JoinTeamDatas
        /// </summary>
        public IEnumerable<TeamInfoDto> JoinTeamDatas { get; set; }
    }
}