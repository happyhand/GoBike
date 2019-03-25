﻿using GoBike.API.Repository.Models;

namespace GoBike.API.Service.Models.Response
{
    /// <summary>
    /// 會員修改回應資料
    /// </summary>
    public class EditDataRespone
    {
        /// <summary>
        /// Gets or sets MemberData
        /// </summary>
        public MemberData MemberData { get; set; }

        /// <summary>
        /// Gets or sets ResultCode
        /// </summary>
        public int ResultCode { get; set; }

        /// <summary>
        /// Gets or sets ResultMessage
        /// </summary>
        public string ResultMessage { get; set; }
    }
}