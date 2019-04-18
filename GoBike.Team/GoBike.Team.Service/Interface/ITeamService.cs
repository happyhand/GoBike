using GoBike.Team.Service.Models;
using System;
using System.Threading.Tasks;

namespace GoBike.Team.Service.Interface
{
    /// <summary>
    /// 車隊服務
    /// </summary>
    public interface ITeamService
    {
        /// <summary>
        /// 建立車隊
        /// </summary>
        /// <param name="teamInfoDto">teamInfoDto</param>
        /// <returns>string</returns>
        Task<string> Register(TeamInfoDto teamInfoDto);

        /// <summary>
        /// 車隊編輯
        /// </summary>
        /// <param name="teamInfoDto">teamInfoDto</param>
        /// <returns>Tuple(TeamInfoDto, string)</returns>
        Task<Tuple<TeamInfoDto, string>> EditData(TeamInfoDto teamInfoDto);

        /// <summary>
        /// 取得車隊資訊
        /// </summary>
        /// <param name="teamInfoDto">teamInfoDto</param>
        /// <returns>Tuple(TeamInfoDto, string)</returns>
        Task<Tuple<TeamInfoDto, string>> GetTeamInfo(TeamInfoDto teamInfoDto);

        /// <summary>
        /// 取得車隊資訊列表
        /// </summary>
        /// <param name="teamInfoDto">teamInfoDto</param>
        /// <returns>Tuple(TeamInfoDto, string)</returns>
        Task<Tuple<TeamInfoDto, string>> GetTeamInfoList(TeamInfoDto teamInfoDto);
    }
}