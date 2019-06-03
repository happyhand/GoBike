using System.Collections.Generic;

namespace GoBike.API.Service.Models.Team.View
{
    /// <summary>
    /// 會員車隊資訊可視資料
    /// </summary>
    public class MemberTeamInfoViewDto
    {
        /// <summary>
        /// Gets or sets InviteJoinUpdateType
        /// </summary>
        public int InviteJoinUpdateType { get; set; }

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
        /// Gets or sets JoinedEventList
        /// </summary>
        public IEnumerable<dynamic> JoinedEventList { get; set; }

        /// <summary>
        /// Gets or sets notYetJoinEventList
        /// </summary>
        public IEnumerable<dynamic> notYetJoinEventList { get; set; }

        #endregion TODO
    }
}