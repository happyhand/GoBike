using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Team.Command;
using GoBike.API.Service.Models.Team.Command.Data;
using System.Threading.Tasks;

namespace GoBike.API.Service.Interface.Team
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
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> EditData(TeamCommandDto teamCommand);

        /// <summary>
        /// 取得我的車隊資訊
        /// </summary>
        /// <param name="membrID">membrID</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetMyTeamInfo(string membrID);

        /// <summary>
        /// 取得車隊明細資訊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetTeamDetailInfo(TeamCommandDto teamCommand);

        /// <summary>
        /// 建立車隊
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="teamInfo">teamInfo</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> Register(string memberID, TeamInfoDto teamInfo);

        /// <summary>
        /// 搜尋車隊
        /// </summary>
        /// <param name="teamSearchCommand">teamSearchCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> SearchTeam(TeamSearchCommandDto teamSearchCommand);

        #endregion 車隊資料

        #region 車隊互動資料

        /// <summary>
        /// 申請加入車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> ApplyForJoinTeam(TeamInteractiveCommandDto teamInteractiveCommand);

        /// <summary>
        /// 取消申請加入車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> CancelApplyForJoinTeam(TeamInteractiveCommandDto teamInteractiveCommand);

        /// <summary>
        /// 取消邀請加入車隊
        /// </summary>
        /// <param name="inviter">inviter</param>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> CancelInviteJoinTeam(string inviter, TeamInteractiveCommandDto teamInteractiveCommand);

        /// <summary>
        /// 強制離開車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> ForceLeaveTeam(string examinerID, TeamInteractiveCommandDto teamInteractiveCommand);

        /// <summary>
        /// 邀請加入車隊
        /// </summary>
        /// <param name="inviter">inviter</param>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> InviteJoinTeam(string inviter, TeamInteractiveCommandDto teamInteractiveCommand);

        /// <summary>
        /// 邀請多人加入車隊
        /// </summary>
        /// <param name="inviter">inviter</param>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> InviteManyJoinTeam(string inviter, TeamInteractiveCommandDto teamInteractiveCommand);

        /// <summary>
        /// 加入車隊
        /// </summary>
        /// <param name="examinerID">examinerID</param>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <param name="isInvite">isInvite</param>
        /// <returns>string</returns>
        Task<ResponseResultDto> JoinTeam(string examinerID, TeamInteractiveCommandDto teamInteractiveCommand, bool isInvite);

        /// <summary>
        /// 離開車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>string</returns>
        Task<ResponseResultDto> LeaveTeam(TeamInteractiveCommandDto teamInteractiveCommand);

        /// <summary>
        /// 拒絕申請加入車隊
        /// </summary>
        /// <param name="examinerID">examinerID</param>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> RejectApplyForJoinTeam(string examinerID, TeamInteractiveCommandDto teamInteractiveCommand);

        /// <summary>
        /// 拒絕邀請加入車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> RejectInviteJoinTeam(TeamInteractiveCommandDto teamInteractiveCommand);

        /// <summary>
        /// 更新車隊隊長
        /// </summary>
        /// <param name="examinerID">examinerID</param>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> UpdateTeamLeader(string examinerID, TeamInteractiveCommandDto teamInteractiveCommand);

        /// <summary>
        /// 更新車隊副隊長
        /// </summary>
        /// <param name="examinerID">examinerID</param>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <param name="isAdd">isAdd</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> UpdateTeamViceLeader(string examinerID, TeamInteractiveCommandDto teamInteractiveCommand, bool isAdd);

        #endregion 車隊互動資料
    }
}