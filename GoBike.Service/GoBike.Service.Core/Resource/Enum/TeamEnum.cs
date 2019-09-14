namespace GoBike.Service.Core.Resource.Enum
{
    /// <summary>
    /// 車隊公告編輯類別資料
    /// </summary>
    public enum TeamAnnouncementEditType
    {
        /// <summary>
        /// 未設定
        /// </summary>
        None = 0,

        /// <summary>
        /// 可編輯
        /// </summary>
        Edit = 1
    }

    /// <summary>
    /// 車隊審核狀態類別資料
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
    /// 車隊互動類別資料
    /// </summary>
    public enum TeamInteractiveType
    {
        /// <summary>
        /// 申請
        /// </summary>
        ApplyFor = 1,

        /// <summary>
        /// 邀請
        /// </summary>
        Invite = 2
    }

    /// <summary>
    /// 車隊審查狀態類別資料
    /// </summary>
    public enum TeamReviewStatusType
    {
        /// <summary>
        /// 未審查
        /// </summary>
        None = 0,

        /// <summary>
        /// 審查中
        /// </summary>
        Review = 1
    }

    /// <summary>
    /// 車隊搜尋狀態類別資料
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
    /// 車隊加入狀態類型資料
    /// </summary>
    public enum TeamJoinStatusType
    {
        /// <summary>
        /// 未加入
        /// </summary>
        None = 0,

        /// <summary>
        /// 已申請
        /// </summary>
        ApplyFor = 1,

        /// <summary>
        /// 被邀請
        /// </summary>
        BeInvited = 2,

        /// <summary>
        /// 等待邀請審核
        /// </summary>
        WaitInviteExamined = 3,

        /// <summary>
        /// 已加入
        /// </summary>
        Join = 4
    }
}