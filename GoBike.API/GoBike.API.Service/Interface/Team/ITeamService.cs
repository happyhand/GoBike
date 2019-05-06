using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Team.Command;
using System.Threading.Tasks;

namespace GoBike.API.Service.Interface.Team
{
    /// <summary>
    /// 車隊服務
    /// </summary>
    public interface ITeamService
    {
        /// <summary>
        /// 取得申請請求列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetApplyForRequestList(TeamCommandDto teamCommand);
    }
}