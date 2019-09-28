using GoBike.Service.Service.Models.Team;
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
        /// 取得推薦車隊資料列表
        /// </summary>
        /// <returns>Tuple(TeamDtos, string)</returns>
        Task<Tuple<IEnumerable<TeamDto>, string>> GetRecommendationTeamDataList();

        /// <summary>
        /// 取得車隊資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamDto, string)</returns>
        Task<Tuple<TeamDto, string>> GetTeamData(TeamDto teamDto);

        /// <summary>
        /// 取得會員的車隊資料列表
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

        #region 車隊互動資料

        /// <summary>
        /// 同意邀請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        Task<string> AgreeInviteJoinTeam(TeamDto teamDto);

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
        /// 取得車隊互動資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamInteractiveDtos, string)</returns>
        Task<Tuple<IEnumerable<TeamInteractiveDto>, string>> GetTeamInteractiveDataList(TeamDto teamDto);

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

        #endregion 車隊互動資料

        #region 車隊公告資料

        /// <summary>
        /// 建立車隊公告資料
        /// </summary>
        /// <param name="teamAnnouncementDto">teamAnnouncementDto</param>
        /// <returns>string</returns>
        Task<string> CreateTeamAnnouncementData(TeamAnnouncementDto teamAnnouncementDto);

        /// <summary>
        /// 刪除車隊公告資料
        /// </summary>
        /// <param name="teamAnnouncementDto">teamAnnouncementDto</param>
        /// <returns>string</returns>
        Task<string> DeleteTeamAnnouncementData(TeamAnnouncementDto teamAnnouncementDto);

        /// <summary>
        /// 編輯車隊公告資料
        /// </summary>
        /// <param name="teamAnnouncementDto">teamAnnouncementDto</param>
        /// <returns>string</returns>
        Task<string> EditTeamAnnouncementData(TeamAnnouncementDto teamAnnouncementDto);

        /// <summary>
        /// 取得車隊公告資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamAnnouncementDtos, string)</returns>
        Task<Tuple<IEnumerable<TeamAnnouncementDto>, string>> GetTeamAnnouncementDataList(TeamDto teamDto);

        #endregion 車隊公告資料

        #region 車隊活動資料

        /// <summary>
        /// 建立車隊活動資料
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>string</returns>
        Task<string> CreateTeamEventData(TeamEventDto teamEventDto);

        /// <summary>
        /// 刪除車隊活動資料
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>string</returns>
        Task<string> DeleteTeamEventData(TeamEventDto teamEventDto);

        /// <summary>
        /// 編輯車隊活動資料
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>string</returns>
        Task<string> EditTeamEventData(TeamEventDto teamEventDto);

        /// <summary>
        /// 取得車隊活動資料
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>Tuple(TeamEventDto, string)</returns>
        Task<Tuple<TeamEventDto, string>> GetTeamEventData(TeamEventDto teamEventDto);

        /// <summary>
        /// 取得車隊活動資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamEventDtos, string)</returns>
        Task<Tuple<IEnumerable<TeamEventDto>, string>> GetTeamEventDataList(TeamDto teamDto);

        /// <summary>
        /// 加入車隊活動
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>string</returns>
        Task<string> JoinTeamEvent(TeamEventDto teamEventDto);

        /// <summary>
        /// 離開車隊活動
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>string</returns>
        Task<string> LeaveTeamEvent(TeamEventDto teamEventDto);

        #endregion 車隊活動資料
    }
}