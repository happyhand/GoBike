using GoBike.Team.Repository.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.Repository.Interface
{
    /// <summary>
    /// 車隊資料庫
    /// </summary>
    public interface ITeamRepository
    {
        /// <summary>
        /// 建立車隊資料
        /// </summary>
        /// <param name="teamData">teamData</param>
        /// <returns>bool</returns>
        Task<bool> CreateTeamData(TeamData teamData);

        /// <summary>
        /// 刪除車隊資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>bool</returns>
        Task<bool> DeleteTeamData(string teamID);

        /// <summary>
        /// 取得車隊資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>TeamData</returns>
        Task<TeamData> GetTeamData(string teamID);

        /// <summary>
        /// 取得車隊列表資料
        /// </summary>
        /// <param name="teamIDs">teamIDs</param>
        /// <returns>TeamDatas</returns>
        Task<IEnumerable<TeamData>> GetTeamDataList(IEnumerable<string> teamIDs);

        /// <summary>
        /// 更新車隊資料
        /// </summary>
        /// <param name="teamData">teamData</param>
        /// <returns>bool</returns>
        Task<bool> UpdateTeamData(TeamData teamData);
    }
}