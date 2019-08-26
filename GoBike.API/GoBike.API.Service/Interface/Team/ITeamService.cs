﻿using GoBike.API.Service.Models.Response;
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
        /// 同意邀請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AgreeInviteJoinTeam(TeamDto teamDto);

        /// <summary>
        /// 允許申請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AllowApplyForJoinTeam(TeamDto teamDto);

        /// <summary>
        /// 允許邀請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> AllowInviteJoinTeam(TeamDto teamDto);

        /// <summary>
        /// 申請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> ApplyForJoinTeam(TeamDto teamDto);

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

        /// <summary>
        /// 取得會員的車隊列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetTeamListOfMember(TeamDto teamDto);

        /// <summary>
        /// 邀請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> InviteJoinTeam(TeamDto teamDto);

        /// <summary>
        /// 拒絕申請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> RejectApplyForJoinTeam(TeamDto teamDto);

        /// <summary>
        /// 拒絕邀請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> RejectInviteJoinTeam(TeamDto teamDto);

        /// <summary>
        /// 搜尋車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> SearchTeam(TeamDto teamDto);
    }
}