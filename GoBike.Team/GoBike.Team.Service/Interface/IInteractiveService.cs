﻿using GoBike.Team.Service.Models.Command;
using GoBike.Team.Service.Models.Data;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.Service.Interface
{
    /// <summary>
    /// 互動服務
    /// </summary>
    public interface IInteractiveService
    {
        /// <summary>
        /// 申請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> ApplyForJoinTeam(TeamCommandDto teamCommand);

        /// <summary>
        /// 取消申請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> CancelApplyForJoinTeam(TeamCommandDto teamCommand);

        /// <summary>
        /// 取消邀請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> CancelInviteJoinTeam(TeamCommandDto teamCommand);

        /// <summary>
        /// 強制離開車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> ForceLeaveTeam(TeamCommandDto teamCommand);

        /// <summary>
        /// 取得申請加入名單
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(strings, string)</returns>
        Task<Tuple<IEnumerable<string>, string>> GetApplyForRequestList(TeamCommandDto teamCommand);

        /// <summary>
        /// 取得邀請加入名單
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(TeamInfoDtos, string)</returns>
        Task<Tuple<IEnumerable<TeamInfoDto>, string>> GetInviteRequestList(TeamCommandDto teamCommand);

        /// <summary>
        /// 邀請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> InviteJoinTeam(TeamCommandDto teamCommand);

        /// <summary>
        /// 邀請多人加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> InviteManyJoinTeam(TeamCommandDto teamCommand);

        /// <summary>
        /// 加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <param name="isInvite">isInvite</param>
        /// <returns>string</returns>
        Task<string> JoinTeam(TeamCommandDto teamCommand, bool isInvite);

        /// <summary>
        /// 離開車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> LeaveTeam(TeamCommandDto teamCommand);

        /// <summary>
        /// 拒絕申請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> RejectApplyForJoinTeam(TeamCommandDto teamCommand);

        /// <summary>
        /// 拒絕邀請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> RejectInviteJoinTeam(TeamCommandDto teamCommand);

        /// <summary>
        /// 更新車隊隊長
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> UpdateTeamLeader(TeamCommandDto teamCommand);

        /// <summary>
        /// 更新車隊副隊長
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <param name="isAdd">isAdd</param>
        /// <returns>string</returns>
        Task<string> UpdateTeamViceLeader(TeamCommandDto teamCommand, bool isAdd);
    }
}