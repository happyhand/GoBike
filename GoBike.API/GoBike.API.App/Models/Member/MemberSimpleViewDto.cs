using System;

namespace GoBike.API.App.Models.Member
{
    /// <summary>
    /// 會員簡易資料
    /// </summary>
    public class MemberSimpleViewDto
    {
        #region 會員資料

        /// <summary>
        /// Gets or sets LoginDate
        /// </summary>
        public DateTime LoginDate { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets Nickname
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets Photo
        /// </summary>
        public string Photo { get; set; }

        #endregion 會員資料

        #region 會員互動資料

        /// <summary>
        /// Gets or sets InteractiveStatus (-1:黑名單，0:無狀態，1:等待加入好友請求確認，2:處理加入好友請求，3:好友)
        /// </summary>
        public int InteractiveStatus { get; set; }

        /// <summary>
        /// Gets or sets TeamJoinSetting (0:無設定，1:允許加入車隊，2:邀請加入車隊)
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

        #endregion 會員互動資料
    }
}