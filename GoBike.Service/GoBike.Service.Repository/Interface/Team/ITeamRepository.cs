using GoBike.Service.Repository.Models.Team;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Service.Repository.Interface.Team
{
    /// <summary>
    /// 車隊資料庫
    /// </summary>
    public interface ITeamRepository
    {
        #region 車隊資料

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
        /// 取得車隊列表資料 (By TeamID)
        /// </summary>
        /// <param name="teamIDs">teamIDs</param>
        /// <returns>TeamDatas</returns>
        Task<IEnumerable<TeamData>> GetTeamDataListByTeamID(IEnumerable<string> teamIDs);

        /// <summary>
        /// 取得車隊資料列表 (By TeamName)
        /// </summary>
        /// <param name="teamName">teamName</param>
        /// <param name="isStrict">isStrict</param>
        /// <returns>TeamDatas</returns>
        Task<IEnumerable<TeamData>> GetTeamDataListByTeamName(string teamName, bool isStrict);

        /// <summary>
        /// 取得會員的車隊列表資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>TeamDatas</returns>
        Task<IEnumerable<TeamData>> GetTeamDataListOfMember(string memberID);

        /// <summary>
        /// 更新車隊資料
        /// </summary>
        /// <param name="teamData">teamData</param>
        /// <returns>bool</returns>
        Task<bool> UpdateTeamData(TeamData teamData);

        #endregion 車隊資料
    }
}