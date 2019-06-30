namespace GoBike.API.Service.Models.Response
{
    /// <summary>
    /// 回應資料
    /// </summary>
    public class ResponseResultDto
    {
        /// <summary>
        /// Gets or sets Data
        /// </summary>
        public dynamic Data { get; set; }

        /// <summary>
        /// Gets or sets Ok
        /// </summary>
        public bool Ok { get; set; }
    }
}