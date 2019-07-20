﻿using GoBike.Service.Repository.Models.Team;
using System;
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
        /// 取得車隊列表資料 (By CityID)
        /// </summary>
        /// <param name="cityID">cityID</param>
        /// <returns>TeamDatas</returns>
        Task<IEnumerable<TeamData>> GetTeamDataListByCityID(int cityID);

        /// <summary>
        /// 取得車隊列表資料 (By CreateDate)
        /// </summary>
        /// <param name="createDate">createDate</param>
        /// <returns>TeamDatas</returns>
        Task<IEnumerable<TeamData>> GetTeamDataListByCreateDate(TimeSpan timeSpan);

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
    }
}