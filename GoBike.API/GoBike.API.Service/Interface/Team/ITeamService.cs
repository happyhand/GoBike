using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Team.Data;
using System.Threading.Tasks;

namespace GoBike.API.Service.Interface.Team
{
    /// <summary>
    /// 車隊服務
    /// </summary>
    public interface ITeamService
    {
        /// <summary>
        /// 建立車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> CreateTeam(TeamDto teamDto);

        /// <summary>
        /// 編輯車隊資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> EditTeamData(TeamDto teamDto);
    }
}