using GoBike.Team.Repository.Models;
using System;
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
        /// 取得車隊資料 (By TeamCreatorID)
        /// </summary>
        /// <param name="teamCreatorID">teamCreatorID</param>
        /// <returns>TeamData</returns>
        Task<TeamData> GetTeamDataByTeamCreatorID(string teamCreatorID);

        /// <summary>
        /// 取得車隊資料 (By TeamID)
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>TeamData</returns>
        Task<TeamData> GetTeamDataByTeamID(string teamID);

        /// <summary>
        /// 取得車隊列表資料
        /// </summary>
        /// <param name="teamIDs">teamIDs</param>
        /// <returns>TeamDatas</returns>
        Task<IEnumerable<TeamData>> GetTeamDataList(IEnumerable<string> teamIDs);

        /// <summary>
        /// 取得會員的車隊列表資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>TeamDatas</returns>
        Task<IEnumerable<TeamData>> GetTeamDataListOfMember(string memberID);

        /// <summary>
        /// 更新車隊黑名單資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="blacklistIDs">blacklistIDs</param>
        /// <returns>Tuple(bool, string)</returns>
        Task<Tuple<bool, string>> UpdateTeamBlacklistData(string teamID, IEnumerable<string> blacklistIDs);

        /// <summary>
        /// 更新車隊被列入黑名單資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>Tuple(bool, string)</returns>
        Task<Tuple<bool, string>> UpdateTeamBlacklistedData(string teamID, IEnumerable<string> memberIDs);

        /// <summary>
        /// 更新車隊資料
        /// </summary>
        /// <param name="teamData">teamData</param>
        /// <returns>Tuple(bool, string)</returns>
        Task<Tuple<bool, string>> UpdateTeamData(TeamData teamData);

        /// <summary>
        /// 更新車隊副隊長
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="viceLeaderID">viceLeaderID</param>
        /// <returns>Tuple(bool, string)</returns>
        Task<Tuple<bool, string>> UpdateTeamViceLeader(string teamID, string viceLeaderID);

        #endregion 車隊資料

        #region 車隊互動資料

        /// <summary>
        /// 建立車隊互動資料
        /// </summary>
        /// <param name="interactiveData">interactiveData</param>
        /// <returns>bool</returns>
        Task<bool> CreateTeamInteractiveData(InteractiveData interactiveData);

        /// <summary>
        /// 刪除車隊互動資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberID">memberID</param>
        /// <returns>bool</returns>
        Task<bool> DeleteTeamInteractiveData(string teamID, string memberID);

        /// <summary>
        /// 取得車隊指定互動資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveData</returns>
        Task<InteractiveData> GetTeamInteractiveData(string teamID, string memberID);

        /// <summary>
        /// 取得申請加入互動資料列表
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>InteractiveDatas</returns>
        Task<IEnumerable<InteractiveData>> GetTeamInteractiveDataListForApplyForJoin(string teamID);

        /// <summary>
        /// 取得邀請加入互動資料列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveDatas</returns>
        Task<IEnumerable<InteractiveData>> GetTeamInteractiveDataListForInviteJoin(string memberID);

        #endregion 車隊互動資料
    }
}