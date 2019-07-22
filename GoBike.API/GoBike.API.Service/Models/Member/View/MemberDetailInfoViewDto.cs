using System.Collections.Generic;

namespace GoBike.API.Service.Models.Member.View
{
    /// <summary>
    /// 會員詳細資訊可視資料
    /// </summary>
    public class MemberDetailInfoViewDto : MemberSimpleInfoViewDto
    {
        /// <summary>
        /// Gets or sets FrontCoverUrl
        /// </summary>
        public string FrontCoverUrl { get; set; }

        /// <summary>
        /// Gets or sets RideDtoList
        /// </summary>
        public IEnumerable<RideSimpleInfoViewDto> RideDtoList { get; set; }
    }
}