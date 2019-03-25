using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoBike.API.App.Models.Response
{
    /// <summary>
    /// 回應資料
    /// </summary>
    public class ResultModel
    {
        /// <summary>
        /// Gets or sets ResultCode
        /// </summary>
        public int ResultCode { get; set; }

        /// <summary>
        /// Gets or sets ResultData
        /// </summary>
        public dynamic ResultData { get; set; }

        /// <summary>
        /// Gets or sets ResultMessage
        /// </summary>
        public string ResultMessage { get; set; }
    }
}