using GoBike.API.Service.Models.Member.Command.Data;
using System.Collections.Generic;

namespace GoBike.API.Service.Models.Member.View
{
    /// <summary>
    /// 會員名片資訊可視資料
    /// </summary>
    public class MemberCardInfoViewDto : MemberSimpleInfoViewDto
    {
        #region 會員互動資料

        /// <summary>
        /// Gets or sets InteractiveStatus (-1:黑名單，0:無狀態，1:等待加入好友請求確認，2:處理加入好友請求，3:好友)
        /// </summary>
        public int InteractiveStatus { get; set; }

        #endregion 會員互動資料

        #region 會員騎乘資料資料

        /// <summary>
        /// Gets or sets RideDataList
        /// </summary>
        public IEnumerable<MemberRideRecordDto> RideDataList { get; set; }

        #endregion 會員騎乘資料資料

        #region 車隊設定資料

        /// <summary>
        /// Gets or sets TeamJoinSetting (-2:取消邀請加入車隊，-1:拒絕加入車隊，0:無設定，1:允許加入車隊，2:邀請加入車隊)
        /// </summary>
        public int TeamJoinSetting { get; set; }

        /// <summary>
        /// Gets or sets TeamKickOutSetting (0:無設定，1:請離車隊)
        /// </summary>
        public int TeamKickOutSetting { get; set; }

        /// <summary>
        /// Gets or sets TeamViceLeaderSetting (-1:取消副隊長，0:無設定，1:設為副隊長)
        /// </summary>
        public int TeamViceLeaderSetting { get; set; }

        #endregion 車隊設定資料
    }
}