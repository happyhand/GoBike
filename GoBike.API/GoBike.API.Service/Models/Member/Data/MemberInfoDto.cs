using System;

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
    /// 車隊選單設定資料
    /// </summary>
    public enum TeamActionSettingType
    {
        /// <summary>
        /// 無設定
        /// </summary>
        None = 0,

        /// <summary>
        /// 歷史公告
        /// </summary>
        HistoricalAnnouncement = 1,

        /// <summary>
        /// 邀請好友
        /// </summary>
        InviteFriend = 2,

        /// <summary>
        /// 發起活動
        /// </summary>
        HoldEvent = 4,

        /// <summary>
        /// 編輯資料
        /// </summary>
        EditData = 8,

        /// <summary>
        /// 發起公告
        /// </summary>
        SendAnnouncement = 16
    }

    /// <summary>
    /// 車隊身分類型資料
    /// </summary>
    public enum TeamIdentityType
    {
        /// <summary>
        /// 隊員
        /// </summary>
        Normal = 0,

        /// <summary>
        /// 副隊長
        /// </summary>
        ViceLeader = 1,

        /// <summary>
        /// 隊長
        /// </summary>
        Leader = 2
    }

    /// <summary>
    /// 車隊加入設定資料
    /// </summary>
    public enum TeamJoinSettingType
    {
        /// <summary>
        /// 取消邀請加入車隊
        /// </summary>
        CancelInviteJoin = -2,

        /// <summary>
        /// 拒絕加入車隊
        /// </summary>
        RejectJoin = -1,

        /// <summary>
        /// 無設定
        /// </summary>
        None = 0,

        /// <summary>
        /// 允許加入車隊
        /// </summary>
        AllowJoin = 1,

        /// <summary>
        /// 邀請加入車隊
        /// </summary>
        InviteJoin = 2
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
        KickOut = 1
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
        Appoint = 1
    }

    /// <summary>
    /// 會員資訊
    /// </summary>
    public class MemberInfoDto
    {
        #region 會員資料

        /// <summary>
        /// Gets or sets BirthDayDate
        /// </summary>
        public string BirthDayDate { get; set; }

        /// <summary>
        /// Gets or sets BodyHeight
        /// </summary>
        public decimal BodyHeight { get; set; }

        /// <summary>
        /// Gets or sets BodyWeight
        /// </summary>
        public decimal BodyWeight { get; set; }

        /// <summary>
        /// Gets or sets CreateDate
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// Gets or sets Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets Gender
        /// </summary>
        public int Gender { get; set; }

        /// <summary>
        /// Gets or sets LoginDate
        /// </summary>
        public DateTime LoginDate { get; set; }

        /// <summary>
        /// Gets or sets MemberID
        /// </summary>
        public string MemberID { get; set; }

        /// <summary>
        /// Gets or sets Mobile
        /// </summary>
        public string Mobile { get; set; }

        /// <summary>
        /// Gets or sets Nickname
        /// </summary>
        public string Nickname { get; set; }

        /// <summary>
        /// Gets or sets Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets Photo
        /// </summary>
        public string Photo { get; set; }

        /// <summary>
        /// Gets or sets Token
        /// </summary>
        public string Token { get; set; }

        #endregion 會員資料

        #region 會員互動資料

        /// <summary>
        /// Gets or sets InteractiveStatus (-1:黑名單，0:無狀態，1:等待加入好友請求確認，2:處理加入好友請求，3:好友)
        /// </summary>
        public int InteractiveStatus { get; set; }

        #endregion 會員互動資料

        #region 車隊設定資料

        /// <summary>
        /// Gets or sets TeamActionSetting (0:無設定，1:歷史公告，2:邀請好友，4:發起活動，8:編輯資料，16:發起公告)
        /// </summary>
        public int TeamActionSetting { get; set; }

        /// <summary>
        /// Gets or sets TeamEventSetting (0:無設定，1:編輯資料)
        /// </summary>
        public int TeamEventSetting { get; set; }

        /// <summary>
        /// Gets or sets TeamIdentity (0:隊員，1:副隊長，2:隊長)
        /// </summary>
        public int TeamIdentity { get; set; }

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