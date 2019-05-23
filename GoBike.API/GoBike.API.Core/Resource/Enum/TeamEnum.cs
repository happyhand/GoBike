namespace GoBike.API.Core.Resource.Enum
{
    /// <summary>
    /// 車隊身分類型資料
    /// </summary>
    public enum TeamIdentityType
    {
        /// <summary>
        /// 非隊員
        /// </summary>
        None = 0,

        /// <summary>
        /// 隊員
        /// </summary>
        Normal = 1,

        /// <summary>
        /// 副隊長
        /// </summary>
        ViceLeader = 2,

        /// <summary>
        /// 隊長
        /// </summary>
        Leader = 3
    }

    /// <summary>
    /// 車隊審核狀態
    /// </summary>
    public enum TeamExamineStatusType
    {
        /// <summary>
        /// 無設定
        /// </summary>
        None = 0,

        /// <summary>
        /// 關閉
        /// </summary>
        Close = 1,

        /// <summary>
        /// 開啟
        /// </summary>
        Open = 2
    }

    /// <summary>
    /// 車隊搜尋狀態
    /// </summary>
    public enum TeamSearchStatusType
    {
        /// <summary>
        /// 無設定
        /// </summary>
        None = 0,

        /// <summary>
        /// 關閉
        /// </summary>
        Close = 1,

        /// <summary>
        /// 開啟
        /// </summary>
        Open = 2
    }

    /// <summary>
    /// 車隊選單設定資料
    /// 隊長   :歷史公告、發起公告、發起活動、邀請好友、編輯資料、移交隊長、解散車隊
    /// 副隊長 :歷史公告、發起公告、發起活動、邀請好友、編輯資料、離開車隊
    /// 隊員   :歷史公告、發起活動、離開車隊
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
        /// 發起活動
        /// </summary>
        HoldEvent = 2,

        /// <summary>
        /// 離開車隊
        /// </summary>
        Leave = 4,

        /// <summary>
        /// 編輯資料
        /// </summary>
        EditData = 8,

        /// <summary>
        /// 邀請好友
        /// </summary>
        InviteFriend = 16,

        /// <summary>
        /// 發起公告
        /// </summary>
        SendAnnouncement = 32,

        /// <summary>
        /// 移交隊長
        /// </summary>
        Transfer = 64,

        /// <summary>
        /// 解散車隊
        /// </summary>
        Disband = 128
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
    /// 車隊活動設定資料
    /// </summary>
    public enum TeamEventSettingType
    {
        /// <summary>
        /// 無設定
        /// </summary>
        None = 0,

        /// <summary>
        /// 編輯資料
        /// </summary>
        Edit = 1
    }
}