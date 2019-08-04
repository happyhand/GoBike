using GoBike.Service.Repository.Models.Team;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Service.Service.Interface.Team
{
    public interface ITeamService
    {
        #region 車隊資料

        /// <summary>
        /// 建立車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        Task<string> CreateTeam(TeamDto teamDto);

        /// <summary>
        /// 解散車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        Task<string> DisbandTeam(TeamDto teamDto);

        /// <summary>
        /// 編輯車隊資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        Task<string> EditTeamData(TeamDto teamDto);

        /// <summary>
        /// 取得附近車隊資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamDtos, string)</returns>
        Task<Tuple<IEnumerable<TeamDto>, string>> GetNearbyTeamDataList(TeamDto teamDto);

        /// <summary>
        /// 取得新創車隊資料列表
        /// </summary>
        /// <returns>Tuple(TeamDtos, string)</returns>
        Task<Tuple<IEnumerable<TeamDto>, string>> GetNewCreationTeamDataList();

        /// <summary>
        /// 取得車隊資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamDto, string)</returns>
        Task<Tuple<TeamDto, string>> GetTeamData(TeamDto teamDto);

        /// <summary>
        /// 取得會員的車隊列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamDtos Of List , string)</returns>
        Task<Tuple<IEnumerable<IEnumerable<TeamDto>>, string>> GetTeamDataListOfMember(TeamDto teamDto);

        /// <summary>
        /// 搜尋車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamDtos, string)</returns>
        Task<Tuple<IEnumerable<TeamDto>, string>> SearchTeam(TeamDto teamDto);

        #endregion 車隊資料

        #region 互動資料

        /// <summary>
        /// 申請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        Task<string> ApplyForJoinTeam(TeamDto teamDto);

        /// <summary>
        /// 取消申請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        Task<string> CancelApplyForJoinTeam(TeamDto teamDto);

        /// <summary>
        /// 強制離開車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        Task<string> ForceLeaveTeam(TeamDto teamDto);

        /// <summary>
        /// 邀請多人加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        Task<string> InviteManyJoinTeam(TeamDto teamDto);

        /// <summary>
        /// 加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <param name="isInvite">isInvite</param>
        /// <returns>string</returns>
        Task<string> JoinTeam(TeamDto teamDto, bool isInvite);

        /// <summary>
        /// 離開車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        Task<string> LeaveTeam(TeamDto teamDto);

        /// <summary>
        /// 拒絕申請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        Task<string> RejectApplyForJoinTeam(TeamDto teamDto);

        /// <summary>
        /// 拒絕邀請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        Task<string> RejectInviteJoinTeam(TeamDto teamDto);

        /// <summary>
        /// 更新車隊隊長
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        Task<string> UpdateTeamLeader(TeamDto teamDto);

        /// <summary>
        /// 更新車隊副隊長
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <param name="isAdd">isAdd</param>
        /// <returns>string</returns>
        Task<string> UpdateTeamViceLeader(TeamDto teamDto, bool isAdd);

        #endregion 互動資料
    }
}