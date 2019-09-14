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

    ///// <summary>
    ///// 會員更新【尚有未處理的車隊邀請】狀態
    ///// </summary>
    //public enum InviteJoinUpdateType
    //{
    //    /// <summary>
    //    /// 無資料
    //    /// </summary>
    //    None = 0,

    //    /// <summary>
    //    /// 待處理
    //    /// </summary>
    //    WaitHandler = 1
    //}

    ///// <summary>
    ///// 車隊可執行指令設定資料
    ///// 隊長         :歷史公告、發起公告、編輯公告、刪除公告、發起活動、邀請好友、審核加入、編輯資料、移交隊長、解散車隊
    ///// 副隊長       :歷史公告、發起公告、編輯公告、刪除公告、發起活動、邀請好友、審核加入、編輯資料、離開車隊
    ///// 隊員         :歷史公告、發起活動、離開車隊
    ///// 非隊員       :申請加入
    ///// </summary>
    //public enum TeamActionSettingType
    //{
    //    /// <summary>
    //    /// 無設定
    //    /// </summary>
    //    None = 0,

    //    /// <summary>
    //    /// 編輯資料
    //    /// </summary>
    //    EditData = 1,

    //    /// <summary>
    //    /// 申請加入
    //    /// </summary>
    //    ApplyForJoin = 2,

    //    /// <summary>
    //    /// 邀請好友
    //    /// </summary>
    //    InviteFriend = 4,

    //    /// <summary>
    //    /// 審核加入
    //    /// </summary>
    //    ExamineJoin = 8,

    //    /// <summary>
    //    /// 歷史公告
    //    /// </summary>
    //    HistoricalAnnouncement = 16,

    //    /// <summary>
    //    /// 發佈公告
    //    /// </summary>
    //    PublishAnnouncement = 32,

    //    /// <summary>
    //    /// 編輯公告
    //    /// </summary>
    //    EditAnnouncement = 64,

    //    /// <summary>
    //    /// 刪除公告
    //    /// </summary>
    //    RemoveAnnouncement = 128,

    //    /// <summary>
    //    /// 發起活動
    //    /// </summary>
    //    HoldEvent = 256,

    //    /// <summary>
    //    /// 移交隊長
    //    /// </summary>
    //    Transfer = 512,

    //    /// <summary>
    //    /// 解散車隊
    //    /// </summary>
    //    Disband = 1024,

    //    /// <summary>
    //    /// 離開車隊
    //    /// </summary>
    //    Leave = 2048
    //}

    ///// <summary>
    ///// 車隊公告設定資料
    ///// </summary>
    //public enum TeamAnnouncementSettingType
    //{
    //    /// <summary>
    //    /// 無設定
    //    /// </summary>
    //    None = 0,

    //    /// <summary>
    //    /// 可執行動作
    //    /// </summary>
    //    Action = 1
    //}

    ///// <summary>
    ///// 車隊更新【已閱最新公告】狀態
    ///// </summary>
    //public enum TeamAnnouncementUpdateType
    //{
    //    /// <summary>
    //    /// 無資料
    //    /// </summary>
    //    None = 0,

    //    /// <summary>
    //    /// 已閱
    //    /// </summary>
    //    Read = 1
    //}

    ///// <summary>
    ///// 車隊更新【尚有未處理的會員申請】狀態
    ///// </summary>
    //public enum TeamApplyForUpdateType
    //{
    //    /// <summary>
    //    /// 無資料
    //    /// </summary>
    //    None = 0,

    //    /// <summary>
    //    /// 待處理
    //    /// </summary>
    //    WaitHandler = 1
    //}

    ///// <summary>
    ///// 車隊更新【已閱最新活動】狀態
    ///// </summary>
    //public enum TeamEventUpdateType
    //{
    //    /// <summary>
    //    /// 無資料
    //    /// </summary>
    //    None = 0,

    //    /// <summary>
    //    /// 已閱
    //    /// </summary>
    //    Read = 1
    //}

    ///// <summary>
    ///// 車隊審核狀態
    ///// </summary>
    //public enum TeamExamineStatusType
    //{
    //    /// <summary>
    //    /// 無設定
    //    /// </summary>
    //    None = 0,

    //    /// <summary>
    //    /// 關閉
    //    /// </summary>
    //    Close = 1,

    //    /// <summary>
    //    /// 開啟
    //    /// </summary>
    //    Open = 2
    //}

    ///// <summary>
    ///// 車隊最新訊息狀態資料
    ///// </summary>
    //public enum TeamHasNewsType
    //{
    //    /// <summary>
    //    /// 無資料
    //    /// </summary>
    //    None = 0,

    //    /// <summary>
    //    /// 待處理
    //    /// </summary>
    //    News = 1
    //}

    ///// <summary>
    ///// 車隊加入設定資料
    ///// </summary>
    //public enum TeamJoinSettingType
    //{
    //    /// <summary>
    //    /// 無設定
    //    /// </summary>
    //    None = 0,

    //    /// <summary>
    //    /// 處理申請加入
    //    /// </summary>
    //    HandleApplyFor = 1,

    //    /// <summary>
    //    /// 邀請加入車隊
    //    /// </summary>
    //    InviteJoin = 2,

    //    /// <summary>
    //    /// 取消邀請加入車隊
    //    /// </summary>
    //    CancelInviteJoin = 3,
    //}

    ///// <summary>
    ///// 車隊請離設定資料
    ///// </summary>
    //public enum TeamKickOutSettingType
    //{
    //    /// <summary>
    //    /// 無設定
    //    /// </summary>
    //    None = 0,

    //    /// <summary>
    //    /// 請離車隊
    //    /// </summary>
    //    KickOut = 1
    //}

    ///// <summary>
    ///// 車隊搜尋狀態
    ///// </summary>
    //public enum TeamSearchStatusType
    //{
    //    /// <summary>
    //    /// 無設定
    //    /// </summary>
    //    None = 0,

    //    /// <summary>
    //    /// 關閉
    //    /// </summary>
    //    Close = 1,

    //    /// <summary>
    //    /// 開啟
    //    /// </summary>
    //    Open = 2
    //}

    ///// <summary>
    ///// 車隊副隊長設定資料
    ///// </summary>
    //public enum TeamViceLeaderSettingType
    //{
    //    /// <summary>
    //    /// 取消副隊長
    //    /// </summary>
    //    Cancel = -1,

    //    /// <summary>
    //    /// 無設定
    //    /// </summary>
    //    None = 0,

    //    /// <summary>
    //    /// 設為副隊長
    //    /// </summary>
    //    Appoint = 1
    //}
}