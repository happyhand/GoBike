using GoBike.Team.Service.Models.Command;
using GoBike.Team.Service.Models.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.Service.Interface
{
    /// <summary>
    /// 車隊服務
    /// </summary>
    public interface ITeamService
    {
        /// <summary>
        /// 解散車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> DisbandTeam(TeamCommandDto teamCommand);

        /// <summary>
        /// 車隊編輯
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(TeamInfoDto, string)</returns>
        Task<Tuple<TeamInfoDto, string>> EditData(TeamCommandDto teamCommand);

        /// <summary>
        /// 取得車隊資訊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(TeamInfoDto, string)</returns>
        Task<Tuple<TeamInfoDto, string>> GetTeamInfo(TeamCommandDto teamCommand);

        /// <summary>
        /// 取得會員的車隊資訊列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(TeamInfoArray, string)</returns>
        Task<Tuple<dynamic[], string>> GetTeamInfoListOfMember(TeamCommandDto teamCommand);

        /// <summary>
        /// 建立車隊
        /// </summary>
        /// <param name="teamInfo">teamInfo</param>
        /// <returns>string</returns>
        Task<string> Register(TeamInfoDto teamInfo);

        /// <summary>
        /// 搜尋車隊
        /// </summary>
        /// <param name="teamSearchCommand">teamSearchCommand</param>
        /// <returns>Tuple(TeamInfoDtos, string)</returns>
        Task<Tuple<IEnumerable<TeamInfoDto>, string>> SearchTeam(TeamSearchCommandDto teamSearchCommand);
    }
}