using System.Collections.Generic;

namespace GoBike.API.Service.Models.Team.View
{
    /// <summary>
    /// 會員車隊資訊可視資料
    /// </summary>
    public class MemberTeamInfoViewDto
    {
        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public IEnumerable<TeamSimpleInfoViewDto> InviteJoinList { get; set; }

        /// <summary>
        /// Gets or sets LeaderTeam
        /// </summary>
        public TeamSimpleInfoViewDto LeaderTeam { get; set; }

        /// <summary>
        /// Gets or sets TeamID
        /// </summary>
        public IEnumerable<TeamSimpleInfoViewDto> TeamList { get; set; }

        #region TODO

        /// <summary>
        /// Gets or sets EventList
        /// </summary>
        public IEnumerable<dynamic> EventList { get; set; }

        #endregion TODO
    }
}