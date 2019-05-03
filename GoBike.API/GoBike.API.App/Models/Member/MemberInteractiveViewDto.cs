﻿namespace GoBike.API.App.Models.Member
{
    /// <summary>
    /// 會員可視互動資料
    /// </summary>
    public class MemberInteractiveViewDto : MemberViewDto
    {
        /// <summary>
        /// Gets or sets InteractiveStatus (0:等待加入好友請求確認,1:處理加入好友請求，2:好友，-1:黑名單，-2:無互動資料)
        /// </summary>
        public int InteractiveStatus { get; set; }
    }
}