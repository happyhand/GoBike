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
        /// <param name="memberCommand">memberCommand</param>
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

        #endregion 車隊互動資料
    }
}