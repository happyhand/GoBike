namespace GoBike.API.Service.Models.Member.Data
{
    /// <summary>
    /// 互動狀態資料
    /// </summary>
    public enum InteractiveStatusType
    {
        /// <summary>
        /// 黑名單
        /// </summary>
        Black = -1,

        /// <summary>
        /// 無狀態
        /// </summary>
        None = 0,

        /// <summary>
        /// 請求 - 等待確認
        /// </summary>
        Request = 1,

        /// <summary>
        /// 請求 - 處理確認
        /// </summary>
        RequestHandler = 2,

        /// <summary>
        /// 好友
        /// </summary>
        Friend = 3
    }

    /// <summary>
    /// 車隊加入設定資料
    /// </summary>
    public enum TeamJoinSettingType
    {
        /// <summary>
        /// 無設定
        /// </summary>
        None = 0,

        /// <summary>
        /// 允許加入
        /// </summary>
        ApplyForJoin = 1,

        /// <summary>
        /// 邀請加入
        /// </summary>
        InviteJoin = 2,
    }

    /// <summary>
    /// 車隊請離設定資料
    /// </summary>
    public enum TeamKickOutSettingType
    {
        /// <summary>
        /// 無設定
        /// </summary>
        None = 0,

        /// <summary>
        /// 請離車隊
        /// </summary>
        KickOut = 1,
    }

    /// <summary>
    /// 車隊副隊長設定資料
    /// </summary>
    public enum TeamViceLeaderSettingType
    {
        /// <summary>
        /// 取消副隊長
        /// </summary>
        Cancel = -1,

        /// <summary>
        /// 無設定
        /// </summary>
        None = 0,

        /// <summary>
        /// 設為副隊長
        /// </summary>
        Appoint = 1,
    }

    /// <summary>
    /// 會員互動資料
    /// </summary>
    public class MemberInteractiveDto
    {
        /// <summary>
        /// Gets or sets InteractiveStatus (-1:黑名單，0:無狀態，1:等待加入好友請求確認，2:處理加入好友請求，3:好友)
        /// </summary>
        public int InteractiveStatus { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

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
    }
}