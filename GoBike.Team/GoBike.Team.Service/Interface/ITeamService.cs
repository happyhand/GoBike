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
        #region 車隊資料

        /// <summary>
        /// 車隊編輯
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(TeamInfoDto, string)</returns>
        Task<Tuple<TeamInfoDto, string>> EditData(TeamCommandDto teamCommand);

        /// <summary>
        /// 強制離開車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> ForceLeaveTeam(TeamCommandDto teamCommand);

        /// <summary>
        /// 取得我的車隊資訊列表
        /// </summary>
        /// <param name="memberCommand">memberCommand</param>
        /// <returns>Tuple(TeamInfoDto, TeamInfoDtos, string)</returns>
        Task<Tuple<TeamInfoDto, IEnumerable<TeamInfoDto>, string>> GetMyTeamInfoList(MemberCommandDto memberCommand);

        /// <summary>
        /// 取得車隊資訊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(TeamInfoDto, string)</returns>
        Task<Tuple<TeamInfoDto, string>> GetTeamInfo(TeamCommandDto teamCommand);

        /// <summary>
        /// 加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <param name="isExamine">isExamine</param>
        /// <returns>string</returns>
        Task<string> JoinTeam(TeamCommandDto teamCommand, bool isExamine);

        /// <summary>
        /// 離開車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> LeaveTeam(TeamCommandDto teamCommand);

        /// <summary>
        /// 建立車隊
        /// </summary>
        /// <param name="teamInfo">teamInfo</param>
        /// <returns>string</returns>
        Task<string> Register(TeamInfoDto teamInfo);

        /// <summary>
        /// 拒絕加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> RejectJoinTeam(TeamCommandDto teamCommand);

        /// <summary>
        /// 搜尋車隊資訊列表
        /// </summary>
        /// <param name="teamSearchCommand">teamSearchCommand</param>
        /// <returns>Tuple(TeamInfoDtos, string)</returns>
        Task<Tuple<IEnumerable<TeamInfoDto>, string>> SearchTeamInfoList(TeamSearchCommandDto teamSearchCommand);

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

        #endregion 車隊資料

        #region 車隊互動資料

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
        /// 取得申請請求列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(strings, string)</returns>
        Task<Tuple<IEnumerable<string>, string>> GetApplyForRequestList(TeamCommandDto teamCommand);

        /// <summary>
        /// 取得邀請請求列表
        /// </summary>
        /// <param name="memberCommand">memberCommand</param>
        /// <returns>Tuple(TeamInfoDtos, string)</returns>
        Task<Tuple<IEnumerable<TeamInfoDto>, string>> GetInviteRequestList(MemberCommandDto memberCommand);

        /// <summary>
        /// 邀請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        Task<string> InviteJoinTeam(TeamCommandDto teamCommand);

        #endregion 車隊互動資料
    }
}