using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Team.Data;
using System.Threading.Tasks;

namespace GoBike.API.Service.Interface.Team
{
    /// <summary>
    /// 車隊服務
    /// </summary>
    public interface ITeamService
    {
        #region 車隊資訊

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
        /// 取得附近車隊資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetNearbyTeamDataList(TeamDto teamDto);

        /// <summary>
        /// 取得新創車隊資料列表
        /// </summary>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetNewCreationTeamDataList();

        /// <summary>
        /// 取得推薦車隊資料列表
        /// </summary>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetRecommendationTeamDataList();

        /// <summary>
        /// 取得車隊資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetTeamData(TeamDto teamDto);

        /// <summary>
        /// 取得會員的車隊資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetTeamDataListOfMember(TeamDto teamDto);

        /// <summary>
        /// 搜尋車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> SearchTeam(TeamDto teamDto);

        #endregion 車隊資訊

        #region 車隊互動

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
        /// 邀請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> InviteJoinTeam(TeamDto teamDto);

        /// <summary>
        /// 離開車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> LeaveTeam(TeamDto teamDto);

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

        #endregion 車隊互動

        #region 車隊公告

        /// <summary>
        /// 建立車隊公告資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> CreateTeamAnnouncementData(TeamAnnouncementDto teamAnnouncementDto);

        /// <summary>
        /// 刪除車隊公告資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> DeleteTeamAnnouncementData(TeamAnnouncementDto teamAnnouncementDto);

        /// <summary>
        /// 編輯車隊公告資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> EditTeamAnnouncementData(TeamAnnouncementDto teamAnnouncementDto);

        #endregion 車隊公告
    }
}