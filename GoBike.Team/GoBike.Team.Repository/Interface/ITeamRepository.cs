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
        /// 取得會員的邀請加入車隊列表資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>TeamDatas</returns>
        Task<IEnumerable<TeamData>> GetTeamDataListOfInviteJoin(string memberID);

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

        /// <summary>
        /// 更新車隊副隊長群
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="teamViceLeaderIDs">teamViceLeaderIDs</param>
        /// <returns>bool</returns>
        Task<bool> UpdateTeamViceLeaders(string teamID, IEnumerable<string> teamViceLeaderIDs);

        /// <summary>
        /// 驗證車隊資料 (By TeamLeaderID)
        /// </summary>
        /// <param name="teamName">teamName</param>
        /// <returns>bool</returns>
        Task<bool> VerifyTeamDataByTeamLeaderID(string memberID);

        /// <summary>
        /// 驗證車隊資料 (By TeamName)
        /// </summary>
        /// <param name="teamName">teamName</param>
        /// <returns>bool</returns>
        Task<bool> VerifyTeamDataByTeamName(string teamName);

        #endregion 車隊資料

        #region 車隊互動資料

        /// <summary>
        /// 更新申請加入名單資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>bool</returns>
        Task<bool> UpdateTeamApplyForJoinIDs(string teamID, IEnumerable<string> memberIDs);

        /// <summary>
        /// 更新車隊黑名單資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>bool</returns>
        Task<bool> UpdateTeamBlacklistData(string teamID, IEnumerable<string> memberIDs);

        /// <summary>
        /// 更新車隊被列入黑名單資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>bool</returns>
        Task<bool> UpdateTeamBlacklistedData(string teamID, IEnumerable<string> memberIDs);

        /// <summary>
        /// 更新邀請加入名單資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>bool</returns>
        Task<bool> UpdateTeamInviteJoinIDs(string teamID, IEnumerable<string> memberIDs);

        #endregion 車隊互動資料
    }
}