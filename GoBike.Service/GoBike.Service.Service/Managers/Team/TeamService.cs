using AutoMapper;
using GoBike.Service.Core.Applibs;
using GoBike.Service.Core.Resource;
using GoBike.Service.Core.Resource.Enum;
using GoBike.Service.Repository.Interface.Member;
using GoBike.Service.Repository.Interface.Team;
using GoBike.Service.Repository.Models.Member;
using GoBike.Service.Repository.Models.Team;
using GoBike.Service.Service.Interface.Team;
using GoBike.Service.Service.Models.Team;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoBike.Service.Service.Managers.Team
{
    /// <summary>
    /// 車隊服務
    /// </summary>
    public class TeamService : ITeamService
    {
        /// <summary>
        /// System
        /// </summary>
        private const string SYSTEM_FLAG = "System";

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<TeamService> logger;

        /// <summary>
        /// mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// memberRepository
        /// </summary>
        private readonly IMemberRepository memberRepository;

        /// <summary>
        /// teamRepository
        /// </summary>
        private readonly ITeamRepository teamRepository;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        /// <param name="teamRepository">teamRepository</param>
        public TeamService(ILogger<TeamService> logger, IMapper mapper, IMemberRepository memberRepository, ITeamRepository teamRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.memberRepository = memberRepository;
            this.teamRepository = teamRepository;
        }

        #region 車隊資料

        /// <summary>
        /// 建立車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        public async Task<string> CreateTeam(TeamDto teamDto)
        {
            try
            {
                string verifyCreateTeamResult = await this.VerifyCreateTeam(teamDto);
                if (!string.IsNullOrEmpty(verifyCreateTeamResult))
                {
                    return verifyCreateTeamResult;
                }

                TeamData teamData = this.CreateTeamData(teamDto);
                bool isSuccess = await this.teamRepository.CreateTeamData(teamData);
                if (!isSuccess)
                {
                    return "建立車隊失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Team Error >>> Data:{JsonConvert.SerializeObject(teamDto)}\n{ex}");
                return "建立車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 解散車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        public async Task<string> DisbandTeam(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(teamDto.ExecutorID))
                {
                    return "無法進行解散車隊審核.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (!teamData.TeamLeaderID.Equals(teamDto.ExecutorID))
                {
                    return "非車隊隊長無法解散車隊.";
                }

                bool deleteTeamDataResult = await this.teamRepository.DeleteTeamData(teamData.TeamID);
                if (!deleteTeamDataResult)
                {
                    return "解散車隊失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Disband Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return "解散車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 編輯車隊資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        public async Task<string> EditTeamData(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(teamDto.ExecutorID))
                {
                    return "無法進行編輯車隊資料審核.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (!teamData.TeamLeaderID.Equals(teamDto.ExecutorID) && !teamData.TeamViceLeaderIDs.Contains(teamDto.ExecutorID))
                {
                    return "無編輯車隊資料權限.";
                }

                this.UpdateTeamDataHandler(teamDto, teamData);
                bool updateTeamDataResult = await this.teamRepository.UpdateTeamData(teamData);
                if (!updateTeamDataResult)
                {
                    return "車隊資料更新失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Team Data Error >>> Data:{JsonConvert.SerializeObject(teamDto)}\n{ex}");
                return "編輯車隊資料發生錯誤.";
            }
        }

        /// <summary>
        /// 取得附近車隊資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamDtos, string)</returns>
        public async Task<Tuple<IEnumerable<TeamDto>, string>> GetNearbyTeamDataList(TeamDto teamDto)
        {
            try
            {
                if (teamDto.CityID == (int)CityType.None)
                {
                    return Tuple.Create<IEnumerable<TeamDto>, string>(null, "無法查詢該市區附近車隊.");
                }

                int searchOpenStatus = (int)TeamSearchStatusType.Open;
                IEnumerable<TeamData> teamDatas = await this.teamRepository.GetTeamDataListByCityID(teamDto.CityID);
                IEnumerable<TeamData> allowTeamDatas = teamDatas.Where(data => data.SearchStatus == searchOpenStatus);
                return Tuple.Create(this.mapper.Map<IEnumerable<TeamDto>>(allowTeamDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Nearby Team Data List Error >>> CityID:{teamDto.CityID}\n{ex}");
                return Tuple.Create<IEnumerable<TeamDto>, string>(null, "取得附近車隊列表發生錯誤.");
            }
        }

        /// <summary>
        /// 取得新創車隊資料列表
        /// </summary>
        /// <returns>Tuple(TeamDtos, string)</returns>
        public async Task<Tuple<IEnumerable<TeamDto>, string>> GetNewCreationTeamDataList()
        {
            try
            {
                //// 時間定義待確認
                TimeSpan timeSpan = new TimeSpan(AppSettingHelper.Appsetting.DaysOfNewCreation, 0, 0, 0, 0);
                int searchOpenStatus = (int)TeamSearchStatusType.Open;
                IEnumerable<TeamData> teamDatas = await this.teamRepository.GetTeamDataListByTimeLimit(timeSpan);
                IEnumerable<TeamData> allowTeamDatas = teamDatas.Where(data => data.SearchStatus == searchOpenStatus);
                return Tuple.Create(this.mapper.Map<IEnumerable<TeamDto>>(allowTeamDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get New Creation Team Data List Error\n{ex}");
                return Tuple.Create<IEnumerable<TeamDto>, string>(null, "取得新創車隊列表發生錯誤.");
            }
        }

        /// <summary>
        /// 取得推薦車隊資料列表
        /// </summary>
        /// <returns>Tuple(TeamDtos, string)</returns>
        public async Task<Tuple<IEnumerable<TeamDto>, string>> GetRecommendationTeamDataList()
        {
            try
            {
                //// TODO
                IEnumerable<TeamDto> teams = new List<TeamDto>();
                return Tuple.Create(teams, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Recommendation Team Data List Error\n{ex}");
                return Tuple.Create<IEnumerable<TeamDto>, string>(null, "取得推薦車隊資料列表發生錯誤.");
            }
        }

        /// <summary>
        /// 取得車隊資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamDto, string)</returns>
        public async Task<Tuple<TeamDto, string>> GetTeamData(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return Tuple.Create<TeamDto, string>(null, "車隊編號無效.");
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                TeamDto targetTeamDto = this.mapper.Map<TeamDto>(teamData);
                TeamInteractiveData teamInteractiveData = await this.teamRepository.GetAppointTeamInteractiveData(teamData.TeamID, teamDto.ExecutorID);
                if (teamInteractiveData != null)
                {
                    targetTeamDto.JoinStatus = teamInteractiveData.InteractiveType == (int)TeamInteractiveType.Invite ?
                        teamInteractiveData.ReviewFlag == (int)TeamReviewStatusType.Review ?
                        (int)TeamJoinStatusType.WaitInviteExamined : (int)TeamJoinStatusType.BeInvited :
                        (int)TeamJoinStatusType.ApplyFor;
                }
                else
                {
                    targetTeamDto.JoinStatus = teamData.TeamMemberIDs.Contains(teamDto.ExecutorID) ? (int)TeamJoinStatusType.Join : (int)TeamJoinStatusType.None;
                }

                return Tuple.Create(targetTeamDto, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data Error >>> TeamID:{teamDto.TeamID}\n{ex}");
                return Tuple.Create<TeamDto, string>(null, "取得車隊資料發生錯誤.");
            }
        }

        /// <summary>
        /// 取得會員的車隊資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamDtos Of List , string)</returns>
        public async Task<Tuple<IEnumerable<IEnumerable<TeamDto>>, string>> GetTeamDataListOfMember(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.ExecutorID))
                {
                    return Tuple.Create<IEnumerable<IEnumerable<TeamDto>>, string>(null, "會員編號無效.");
                }

                IEnumerable<TeamData> teamDatas = await this.teamRepository.GetTeamDataListOfMember(teamDto.ExecutorID);
                IEnumerable<TeamData> teamLeaderDatas = teamDatas.Where(data => data.TeamLeaderID.Equals(teamDto.ExecutorID));
                IEnumerable<TeamData> teamMemberDatas = teamDatas.Except(teamLeaderDatas);
                IEnumerable<TeamInteractiveData> teamInteractiveDatas = await this.teamRepository.GetTeamInteractiveDataListOfMember(teamDto.ExecutorID);
                IEnumerable<string> inviteTeamIDs = teamInteractiveDatas.Select(data => data.TeamID).Distinct();
                IEnumerable<TeamData> invietTeamDatas = await this.teamRepository.GetTeamDataListByTeamID(inviteTeamIDs);
                IEnumerable<IEnumerable<TeamDto>> teamDtos = new List<IEnumerable<TeamDto>>()
                {
                    this.mapper.Map<IEnumerable<TeamDto>>(teamLeaderDatas),
                    this.mapper.Map<IEnumerable<TeamDto>>(teamMemberDatas),
                    this.mapper.Map<IEnumerable<TeamDto>>(invietTeamDatas)
                };
                return Tuple.Create(teamDtos, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data List Of Member Error >>> ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return Tuple.Create<IEnumerable<IEnumerable<TeamDto>>, string>(null, "取得會員的車隊資料列表發生錯誤.");
            }
        }

        /// <summary>
        /// 搜尋車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamDtos, string)</returns>
        public async Task<Tuple<IEnumerable<TeamDto>, string>> SearchTeam(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.SearchKey))
                {
                    return Tuple.Create<IEnumerable<TeamDto>, string>(null, "搜尋關鍵字無效.");
                }

                int searchOpenStatus = (int)TeamSearchStatusType.Open;
                IEnumerable<TeamData> teamDatas = await this.teamRepository.GetTeamDataListByTeamName(teamDto.SearchKey, false);
                IEnumerable<TeamData> allowTeamDatas = teamDatas.Where(data => data.SearchStatus == searchOpenStatus);
                return Tuple.Create(this.mapper.Map<IEnumerable<TeamDto>>(allowTeamDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Team Error >>> SearchKey:{teamDto.SearchKey}\n{ex}");
                return Tuple.Create<IEnumerable<TeamDto>, string>(null, "搜尋車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 建立車隊資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>TeamData</returns>
        private TeamData CreateTeamData(TeamDto teamDto)
        {
            DateTime createDate = DateTime.Now;
            TeamData teamData = new TeamData()
            {
                CreateDate = createDate,
                TeamID = Utility.GetSerialID(createDate),
                TeamName = teamDto.TeamName,
                CityID = teamDto.CityID,
                TeamInfo = teamDto.TeamInfo,
                SearchStatus = teamDto.SearchStatus,
                ExamineStatus = teamDto.ExamineStatus,
                FrontCoverUrl = teamDto.FrontCoverUrl,
                PhotoUrl = teamDto.PhotoUrl,
                TeamLeaderID = teamDto.ExecutorID,
                TeamViceLeaderIDs = new List<string>(),
                TeamMemberIDs = new List<string>() { teamDto.ExecutorID }
            };

            return teamData;
        }

        /// <summary>
        /// 車隊資料更新處理
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <param name="teamData">teamData</param>
        private void UpdateTeamDataHandler(TeamDto teamDto, TeamData teamData)
        {
            //// 不修改車隊編號、車隊名稱、車隊所在地

            if (!string.IsNullOrEmpty(teamDto.TeamInfo))
            {
                teamData.TeamInfo = teamDto.TeamInfo;
            }

            if (teamDto.SearchStatus != (int)TeamSearchStatusType.None)
            {
                teamData.SearchStatus = teamDto.SearchStatus;
            }

            if (teamDto.ExamineStatus != (int)TeamExamineStatusType.None)
            {
                teamData.ExamineStatus = teamDto.ExamineStatus;
            }

            if (!string.IsNullOrEmpty(teamDto.FrontCoverUrl))
            {
                teamData.FrontCoverUrl = teamDto.FrontCoverUrl;
            }

            if (!string.IsNullOrEmpty(teamDto.PhotoUrl))
            {
                teamData.PhotoUrl = teamDto.PhotoUrl;
            }
        }

        /// <summary>
        /// 驗證車隊建立資料
        /// </summary>
        /// <param name="memberDto">memberDto</param>
        /// <param name="isVerifyPassword">isVerifyPassword</param>
        /// <returns>string</returns>
        private async Task<string> VerifyCreateTeam(TeamDto teamDto)
        {
            if (string.IsNullOrEmpty(teamDto.ExecutorID))
            {
                return "會員編號無效.";
            }
            else
            {
                bool isMultipleTeam = await this.teamRepository.VerifyTeamDataByTeamLeaderID(teamDto.ExecutorID);
                if (isMultipleTeam)
                {
                    return "無法創建多個車隊.";
                }
            }

            if (string.IsNullOrEmpty(teamDto.TeamName))
            {
                return "車隊名稱無效.";
            }
            else
            {
                bool isRepeatTeamName = await this.teamRepository.VerifyTeamDataByTeamName(teamDto.TeamName);
                if (isRepeatTeamName)
                {
                    return "車隊名稱重複.";
                }
            }

            if (teamDto.CityID == (int)CityType.None)
            {
                return "未設定車隊所在地.";
            }

            if (string.IsNullOrEmpty(teamDto.TeamInfo))
            {
                return "車隊簡介無效.";
            }

            if (string.IsNullOrEmpty(teamDto.PhotoUrl))
            {
                return "未上傳車隊頭像.";
            }

            if (string.IsNullOrEmpty(teamDto.FrontCoverUrl))
            {
                return "未上傳車隊封面.";
            }

            if (teamDto.SearchStatus == (int)TeamSearchStatusType.None)
            {
                return "未設定搜尋狀態.";
            }

            if (teamDto.ExamineStatus == (int)TeamExamineStatusType.None)
            {
                return "未設定審核狀態.";
            }

            return string.Empty;
        }

        #endregion 車隊資料

        #region 車隊互動資料

        /// <summary>
        /// 同意邀請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        public async Task<string> AgreeInviteJoinTeam(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(teamDto.ExecutorID))
                {
                    return "會員編號無效.";
                }

                TeamInteractiveData teamInteractiveData = await this.teamRepository.GetAppointTeamInteractiveData(teamDto.TeamID, teamDto.ExecutorID);
                if (teamInteractiveData == null)
                {
                    return "車隊互動資料不存在.";
                }

                if (teamInteractiveData.InteractiveType != (int)TeamInteractiveType.Invite)
                {
                    return "車隊無邀請資料.";
                }

                teamInteractiveData.ReviewFlag = (int)TeamReviewStatusType.Review;

                bool isSuccess = await this.teamRepository.UpdateTeamInteractiveData(teamInteractiveData);
                if (!isSuccess)
                {
                    return "同意邀請加入車隊失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Agree Invite Join Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return "同意邀請加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 申請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        public async Task<string> ApplyForJoinTeam(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(teamDto.ExecutorID))
                {
                    return "會員編號無效.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (teamData.TeamMemberIDs.Contains(teamDto.ExecutorID))
                {
                    return "已加入車隊.";
                }

                TeamInteractiveData teamInteractiveData = await this.teamRepository.GetAppointTeamInteractiveData(teamData.TeamID, teamDto.ExecutorID);
                if (teamInteractiveData != null)
                {
                    switch (teamInteractiveData.InteractiveType)
                    {
                        case (int)TeamInteractiveType.ApplyFor:
                            return "已申請加入車隊.";

                        case (int)TeamInteractiveType.Invite:
                            return "車隊已邀請加入.";

                        default:
                            this.logger.LogError($"Apply For Join Team Fail >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID} InteractiveType:{teamInteractiveData.InteractiveType}");
                            return "申請加入車隊失敗.";
                    }
                }

                if (teamData.ExamineStatus == (int)TeamExamineStatusType.Close)
                {
                    return await this.JoinTeam(teamDto, false);
                }

                teamInteractiveData = this.CreateTeamInteractiveData(teamData.TeamID, teamDto.ExecutorID, string.Empty, false);
                bool isSuccess = await this.teamRepository.CreateTeamInteractiveData(teamInteractiveData);
                if (!isSuccess)
                {
                    return "申請加入車隊失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Apply For Join Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return "申請加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 取消申請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        public async Task<string> CancelApplyForJoinTeam(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(teamDto.ExecutorID))
                {
                    return "會員編號無效.";
                }

                TeamInteractiveData teamInteractiveData = await this.teamRepository.GetAppointTeamInteractiveData(teamDto.TeamID, teamDto.ExecutorID);
                if (teamInteractiveData == null)
                {
                    //// 如果沒車隊互動資料了，就當作已經取消申請加入
                    this.logger.LogWarning($"Cancel Apply For Join Team Fail For TeamInteractiveData Not Exist >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}");
                    return string.Empty;
                }

                bool isSuccess = await this.teamRepository.DeleteTeamInteractiveData(teamDto.TeamID, teamDto.ExecutorID);
                if (!isSuccess)
                {
                    return "取消申請加入車隊失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Cancel Apply For Join Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return "取消申請加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 強制離開車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        public async Task<string> ForceLeaveTeam(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(teamDto.ExaminerID))
                {
                    return "無法進行強制離開車隊審核.";
                }

                if (string.IsNullOrEmpty(teamDto.TargetID))
                {
                    return "對方的會員編號無效.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (teamDto.TeamLeaderID.Equals(teamDto.TargetID))
                {
                    return "無法將車隊隊長強制離開車隊.";
                }

                if (!teamDto.TeamMemberIDs.Contains(teamDto.TargetID))
                {
                    return "對方未加入車隊.";
                }

                if (!teamData.TeamLeaderID.Equals(teamDto.ExaminerID) && !teamData.TeamViceLeaderIDs.Contains(teamDto.ExaminerID))
                {
                    return "無強制會員離開車隊權限.";
                }

                bool updateTeamPlayerIDsResult = Utility.UpdateListHandler(teamData.TeamMemberIDs, teamDto.TargetID, false);
                bool updateTeamViceLeaderIDsResult = Utility.UpdateListHandler(teamData.TeamViceLeaderIDs, teamDto.TargetID, false);
                if (updateTeamPlayerIDsResult || updateTeamViceLeaderIDsResult)
                {
                    bool updateTeamDataResult = await this.teamRepository.UpdateTeamData(teamData);
                    if (!updateTeamDataResult)
                    {
                        return "車隊資料更新失敗.";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Force Leave Team Error >>> TeamID:{teamDto.TeamID} ExaminerID:{teamDto.ExaminerID} TargetID:{teamDto.TargetID}\n{ex}");
                return "強制離開車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 取得車隊互動資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamInteractiveDtos, string)</returns>
        public async Task<Tuple<IEnumerable<TeamInteractiveDto>, string>> GetTeamInteractiveDataList(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return Tuple.Create<IEnumerable<TeamInteractiveDto>, string>(null, "車隊編號無效.");
                }

                if (string.IsNullOrEmpty(teamDto.ExecutorID))
                {
                    return Tuple.Create<IEnumerable<TeamInteractiveDto>, string>(null, "無法進行取得車隊互動資料審核.");
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return Tuple.Create<IEnumerable<TeamInteractiveDto>, string>(null, "車隊不存在.");
                }

                if (!teamData.TeamLeaderID.Equals(teamDto.ExecutorID) && !teamData.TeamViceLeaderIDs.Contains(teamDto.ExecutorID))
                {
                    return Tuple.Create<IEnumerable<TeamInteractiveDto>, string>(null, "無取得車隊互動資料權限.");
                }

                IEnumerable<TeamInteractiveData> teamInteractiveDatas = await this.teamRepository.GetTeamInteractiveDataListOfTeam(teamData.TeamID);
                IEnumerable<TeamInteractiveData> waitReviewTeamInteractiveDatas = teamInteractiveDatas.Where(data => data.ReviewFlag == (int)TeamReviewStatusType.Review);
                IEnumerable<TeamInteractiveDto> waitReviewTeamInteractiveDtos = this.mapper.Map<IEnumerable<TeamInteractiveDto>>(waitReviewTeamInteractiveDatas);
                foreach (TeamInteractiveDto waitReviewTeamInteractiveDto in waitReviewTeamInteractiveDtos)
                {
                    this.TeamInteractiveDtoHandler(waitReviewTeamInteractiveDto);
                }
                return Tuple.Create(waitReviewTeamInteractiveDtos, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Interactive Data List Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return Tuple.Create<IEnumerable<TeamInteractiveDto>, string>(null, "取得車隊互動資料列表發生錯誤.");
            }
        }

        /// <summary>
        /// 邀請多人加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        public async Task<string> InviteManyJoinTeam(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(teamDto.ExecutorID))
                {
                    return "會員編號無效.";
                }

                if (teamDto.TargetIDs == null || !teamDto.TargetIDs.Any())
                {
                    return "未邀請會員加入.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool hasMissTarget = false;
                IEnumerable<string> targetIDs = teamDto.TargetIDs;
                List<TeamInteractiveData> newTeamInteractiveDatas = new List<TeamInteractiveData>();
                foreach (string targetID in targetIDs)
                {
                    TeamInteractiveData teamInteractiveData = await this.teamRepository.GetAppointTeamInteractiveData(teamData.TeamID, targetID);
                    if (teamInteractiveData != null)
                    {
                        hasMissTarget = true;
                        this.logger.LogWarning($"Invite Many Join Team Fail For TeamInteractiveData Exist >>> TeamID:{teamDto.TeamID} TargetID:{targetID}");
                        continue;
                    }

                    teamInteractiveData = this.CreateTeamInteractiveData(teamData.TeamID, targetID, teamDto.ExecutorID, true);
                    newTeamInteractiveDatas.Add(teamInteractiveData);
                }

                if (!newTeamInteractiveDatas.Any())
                {
                    return "邀請加入車隊失敗.";
                }

                bool isSuccess = await this.teamRepository.CreateManyTeamInteractiveData(newTeamInteractiveDatas);
                if (!isSuccess)
                {
                    return "邀請加入車隊失敗.";
                }

                if (hasMissTarget)
                {
                    return "部分會員邀請加入車隊失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Invite Many Join Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID} TargetIDs:{JsonConvert.SerializeObject(teamDto.TargetIDs)}\n{ex}");
                return "邀請加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <param name="isInvite">isInvite</param>
        /// <returns>string</returns>
        public async Task<string> JoinTeam(TeamDto teamDto, bool isInvite)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                string verifyJoinTeamResult = await this.VerifyJoinTeam(teamData, teamDto.ExaminerID, teamDto.TargetID, isInvite);
                if (!string.IsNullOrEmpty(verifyJoinTeamResult))
                {
                    return verifyJoinTeamResult;
                }

                bool updateTeamPlayerIDsResult = Utility.UpdateListHandler(teamData.TeamMemberIDs, teamDto.TargetID, true);
                if (updateTeamPlayerIDsResult)
                {
                    bool updateTeamDataResult = await this.teamRepository.UpdateTeamData(teamData);
                    if (!updateTeamDataResult)
                    {
                        return "車隊資料更新失敗.";
                    }
                }

                bool deleteTeamInteractiveDataResult = await this.teamRepository.DeleteTeamInteractiveData(teamData.TeamID, teamDto.TargetID);
                if (!deleteTeamInteractiveDataResult)
                {
                    //// 記下 Log 就好，無須影響加入車隊流程
                    this.logger.LogWarning($"Join Team Fail For Delete Target TeamInteractiveData Fail >>> TeamID:{teamData.TeamID} TargetID:{teamDto.TargetID}");
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Join Team Error >>> TeamID:{teamDto.TeamID} ExaminerID:{teamDto.ExaminerID} TargetID:{teamDto.TargetID} IsInvite:{isInvite}\n{ex}");
                return "加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 離開車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        public async Task<string> LeaveTeam(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(teamDto.ExecutorID))
                {
                    return "會員編號無效.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (teamDto.ExecutorID.Equals(teamData.TeamLeaderID))
                {
                    return "請先移交車隊隊長職務.";
                }

                if (!teamDto.TeamMemberIDs.Contains(teamDto.ExecutorID))
                {
                    return "未加入車隊.";
                }

                bool updateTeamPlayerIDsResult = Utility.UpdateListHandler(teamData.TeamMemberIDs, teamDto.ExecutorID, false);
                bool updateTeamViceLeaderIDsResult = Utility.UpdateListHandler(teamData.TeamViceLeaderIDs, teamDto.ExecutorID, false);
                if (updateTeamPlayerIDsResult || updateTeamViceLeaderIDsResult)
                {
                    bool updateTeamDataResult = await this.teamRepository.UpdateTeamData(teamData);
                    if (!updateTeamDataResult)
                    {
                        return "車隊資料更新失敗.";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Leave Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return "離開車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 拒絕申請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        public async Task<string> RejectApplyForJoinTeam(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(teamDto.ExaminerID))
                {
                    return "無法進行拒絕申請加入車隊審核.";
                }

                if (string.IsNullOrEmpty(teamDto.TargetID))
                {
                    return "對方的會員編號無效.";
                }

                TeamInteractiveData teamInteractiveData = await this.teamRepository.GetAppointTeamInteractiveData(teamDto.TeamID, teamDto.TargetID);
                if (teamInteractiveData == null)
                {
                    //// 如果沒車隊互動資料了，就當作已經拒絕申請加入
                    this.logger.LogWarning($"Reject Apply For Join Team Fail For TeamInteractiveData Not Exist >>> TeamID:{teamDto.TeamID} ExaminerID:{teamDto.ExaminerID} TargetID:{teamDto.TargetID}");
                    return string.Empty;
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (teamData.TeamMemberIDs.Contains(teamDto.TargetID))
                {
                    return "對方已加入車隊.";
                }

                if (!teamData.TeamLeaderID.Equals(teamDto.ExaminerID) && !teamData.TeamViceLeaderIDs.Contains(teamDto.ExaminerID))
                {
                    return "無拒絕會員申請加入車隊權限.";
                }

                bool isSuccess = await this.teamRepository.DeleteTeamInteractiveData(teamData.TeamID, teamDto.TargetID);
                if (!isSuccess)
                {
                    return "拒絕申請加入車隊失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Apply For Join Team Error >>> TeamID:{teamDto.TeamID} ExaminerID:{teamDto.ExaminerID} TargetID:{teamDto.TargetID}\n{ex}");
                return "拒絕申請加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 拒絕邀請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        public async Task<string> RejectInviteJoinTeam(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(teamDto.ExecutorID))
                {
                    return "會員編號無效.";
                }

                TeamInteractiveData teamInteractiveData = await this.teamRepository.GetAppointTeamInteractiveData(teamDto.TeamID, teamDto.ExecutorID);
                if (teamInteractiveData == null)
                {
                    //// 如果沒車隊互動資料了，就當作已經拒絕邀請加入
                    this.logger.LogWarning($"Reject Invite Join Team Fail For TeamInteractiveData Not Exist >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}");
                    return string.Empty;
                }

                bool isSuccess = await this.teamRepository.DeleteTeamInteractiveData(teamDto.TeamID, teamDto.ExecutorID);
                if (!isSuccess)
                {
                    return "拒絕邀請加入車隊失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Invite Join Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return "拒絕邀請加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 更新車隊隊長
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        public async Task<string> UpdateTeamLeader(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(teamDto.ExaminerID))
                {
                    return "無法進行更新車隊隊長審核.";
                }

                if (string.IsNullOrEmpty(teamDto.TargetID))
                {
                    return "對方的會員編號無效.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (!teamData.TeamLeaderID.Equals(teamDto.ExaminerID))
                {
                    return "非車隊隊長無法移交隊長職務.";
                }

                if (teamDto.ExaminerID.Equals(teamDto.TargetID))
                {
                    return "移交隊長職務對象無法為本人.";
                }

                if (!teamData.TeamMemberIDs.Contains(teamDto.TargetID))
                {
                    return "對方未加入車隊.";
                }

                bool isMultipleTeam = await this.teamRepository.VerifyTeamDataByTeamLeaderID(teamDto.TargetID);
                if (isMultipleTeam)
                {
                    return "無法指定其他車隊隊長.";
                }

                teamData.TeamLeaderID = teamDto.TargetID;
                bool updateTeamDataResult = await this.teamRepository.UpdateTeamData(teamData);
                if (!updateTeamDataResult)
                {
                    return "車隊資料更新失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Leader Error >>> TeamID:{teamDto.TeamID} ExaminerID:{teamDto.ExaminerID} TargetID:{teamDto.TargetID}\n{ex}");
                return "更新車隊隊長發生錯誤.";
            }
        }

        /// <summary>
        /// 更新車隊副隊長
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <param name="isAdd">isAdd</param>
        /// <returns>string</returns>
        public async Task<string> UpdateTeamViceLeader(TeamDto teamDto, bool isAdd)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(teamDto.ExaminerID))
                {
                    return "無法進行更新車隊副隊長審核.";
                }

                if (string.IsNullOrEmpty(teamDto.TargetID))
                {
                    return "對方的會員編號無效.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (!teamData.TeamLeaderID.Equals(teamDto.ExaminerID))
                {
                    return $"非車隊隊長無法{(isAdd ? "指定" : "卸除")}車隊副隊長.";
                }

                if (teamDto.ExaminerID.Equals(teamDto.TargetID))
                {
                    return $"{(isAdd ? "指定" : "卸除")}車隊副隊長對象無法為本人.";
                }

                if (isAdd && !teamData.TeamMemberIDs.Contains(teamDto.TargetID))
                {
                    return "對方未加入車隊.";
                }

                bool updateTeamViceLeaderIDsResult = Utility.UpdateListHandler(teamData.TeamViceLeaderIDs, teamDto.TargetID, isAdd);
                if (updateTeamViceLeaderIDsResult)
                {
                    bool result = await this.teamRepository.UpdateTeamData(teamData);
                    if (!result)
                    {
                        return "車隊資料更新失敗.";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Vice Leader Error >>> TeamID:{teamDto.TeamID} ExaminerID:{teamDto.ExaminerID} TargetID:{teamDto.TargetID}\n{ex}");
                return "更新車隊副隊長發生錯誤.";
            }
        }

        /// <summary>
        /// 建立車隊互動資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberID">memberID</param>
        /// <param name="inviteID">inviteID</param>
        /// <param name="interactiveType">interactiveType</param>
        /// <returns>TeamInteractiveData</returns>
        private TeamInteractiveData CreateTeamInteractiveData(string teamID, string memberID, string inviteID, bool isInvite)
        {
            return new TeamInteractiveData()
            {
                CreateDate = DateTime.Now,
                TeamID = teamID,
                MemberID = memberID,
                InviteID = inviteID,
                InteractiveType = isInvite ? (int)TeamInteractiveType.Invite : (int)TeamInteractiveType.ApplyFor,
                ReviewFlag = isInvite ? (int)TeamReviewStatusType.None : (int)TeamReviewStatusType.Review
            };
        }

        /// <summary>
        /// 車隊互動資料處理
        /// </summary>
        /// <param name="teamInteractiveDto">teamInteractiveDto</param>
        private async void TeamInteractiveDtoHandler(TeamInteractiveDto teamInteractiveDto)
        {
            MemberData memberData = await this.memberRepository.GetMemberDataByMemberID(teamInteractiveDto.MemberID);
            if (memberData == null)
            {
                //// 如果沒有會員資料就略過
                this.logger.LogError($"Team Interactive Dto Handler Error For Member Data Not Exist >>> MemberID:{teamInteractiveDto.MemberID}");
                return;
            }
            teamInteractiveDto.Nickname = memberData.Nickname;
            teamInteractiveDto.PhotoUrl = memberData.PhotoUrl;
            if (!string.IsNullOrEmpty(teamInteractiveDto.InviteID))
            {
                MemberData inviteData = await this.memberRepository.GetMemberDataByMemberID(teamInteractiveDto.InviteID);
                teamInteractiveDto.InviteNickname = inviteData.Nickname;
            }
        }

        /// <summary>
        /// 驗證加入車隊資格
        /// </summary>
        /// <param name="teamData">teamData</param>
        /// <param name="examinerID">examinerID</param>
        /// <param name="targetID">targetID</param>
        /// <param name="isInvite">isInvite</param>
        /// <returns>string</returns>
        private async Task<string> VerifyJoinTeam(TeamData teamData, string examinerID, string targetID, bool isInvite)
        {
            if (teamData.TeamMemberIDs.Contains(targetID))
            {
                return "會員已加入車隊.";
            }

            if (isInvite)
            {
                if (string.IsNullOrEmpty(targetID))
                {
                    return "會員編號無效.";
                }

                TeamInteractiveData inviteTeamInteractiveData = await this.teamRepository.GetAppointTeamInteractiveData(teamData.TeamID, targetID);
                if (inviteTeamInteractiveData == null || inviteTeamInteractiveData.InteractiveType != (int)TeamInteractiveType.Invite)
                {
                    return "車隊尚未邀請加入.";
                }
            }
            else
            {
                if (teamData.ExamineStatus == (int)TeamExamineStatusType.Open)
                {
                    if (string.IsNullOrEmpty(examinerID))
                    {
                        return "無法進行車隊審核.";
                    }

                    if (string.IsNullOrEmpty(targetID))
                    {
                        return "申請加入的會員編號無效.";
                    }

                    TeamInteractiveData applyForTeamInteractiveData = await this.teamRepository.GetAppointTeamInteractiveData(teamData.TeamID, targetID);
                    if (applyForTeamInteractiveData == null || applyForTeamInteractiveData.InteractiveType != (int)TeamInteractiveType.ApplyFor)
                    {
                        return "該會員未申請加入車隊.";
                    }

                    if (!teamData.TeamLeaderID.Equals(examinerID) && !teamData.TeamViceLeaderIDs.Contains(examinerID))
                    {
                        return "無車隊審核權限.";
                    }
                }
                else
                {
                    if (string.IsNullOrEmpty(targetID))
                    {
                        return "會員編號無效.";
                    }
                }
            }

            return string.Empty;
        }

        #endregion 車隊互動資料

        #region 車隊公告資料

        /// <summary>
        /// 建立車隊公告資料
        /// </summary>
        /// <param name="teamAnnouncementDto">teamAnnouncementDto</param>
        /// <returns>string</returns>
        public async Task<string> CreateTeamAnnouncementData(TeamAnnouncementDto teamAnnouncementDto)
        {
            try
            {
                string verifyCreateTeamAnnouncementResult = this.VerifyCreateTeamAnnouncement(teamAnnouncementDto);
                if (!string.IsNullOrEmpty(verifyCreateTeamAnnouncementResult))
                {
                    return verifyCreateTeamAnnouncementResult;
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamAnnouncementDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (!teamAnnouncementDto.MemberID.Equals(SYSTEM_FLAG))
                {
                    //// 非系統公告
                    if (!teamData.TeamLeaderID.Equals(teamAnnouncementDto.MemberID) || !teamData.TeamViceLeaderIDs.Contains(teamAnnouncementDto.MemberID))
                    {
                        return "無建立車隊公告資料權限.";
                    }
                }

                DateTime createDate = DateTime.Now;
                TeamAnnouncementData teamAnnouncementData = this.mapper.Map<TeamAnnouncementData>(teamAnnouncementDto);
                teamAnnouncementData.CreateDate = createDate;
                teamAnnouncementData.AnnouncementID = Utility.GetSerialID(createDate);
                teamAnnouncementData.SaveDeadline = createDate.AddDays(teamAnnouncementDto.LimitDate);
                bool isSuccess = await this.teamRepository.CreateTeamAnnouncementData(teamAnnouncementData);
                if (!isSuccess)
                {
                    return "建立車隊公告資料失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Team Announcement Data Error >>> Data:{JsonConvert.SerializeObject(teamAnnouncementDto)}\n{ex}");
                return "建立車隊公告資料發生錯誤.";
            }
        }

        /// <summary>
        /// 刪除車隊公告資料
        /// </summary>
        /// <param name="teamAnnouncementDto">teamAnnouncementDto</param>
        /// <returns>string</returns>
        public async Task<string> DeleteTeamAnnouncementData(TeamAnnouncementDto teamAnnouncementDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamAnnouncementDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(teamAnnouncementDto.AnnouncementID))
                {
                    return "車隊公告編號無效.";
                }

                if (string.IsNullOrEmpty(teamAnnouncementDto.MemberID))
                {
                    return "無法進行刪除車隊公告資料審核.";
                }

                if (teamAnnouncementDto.MemberID.Equals(SYSTEM_FLAG))
                {
                    return "無法刪除系統公告.";
                }

                TeamAnnouncementData teamAnnouncementData = await this.teamRepository.GetTeamAnnouncementData(teamAnnouncementDto.AnnouncementID);
                if (teamAnnouncementData == null)
                {
                    //// 如果沒車隊公告資料了，就當作已經刪除掉
                    return string.Empty;
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamAnnouncementDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (!teamData.TeamLeaderID.Equals(teamAnnouncementDto.MemberID) || !teamData.TeamViceLeaderIDs.Contains(teamAnnouncementDto.MemberID))
                {
                    return "無刪除車隊公告資料權限.";
                }

                bool deleteTeamAnnouncementDataResult = await this.teamRepository.DeleteTeamAnnouncementData(teamAnnouncementDto.AnnouncementID);
                if (!deleteTeamAnnouncementDataResult)
                {
                    return "刪除車隊公告資料失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Team Announcement Data Error >>> TeamID:{teamAnnouncementDto.TeamID} AnnouncementID:{teamAnnouncementDto.AnnouncementID} MemberID:{teamAnnouncementDto.MemberID}\n{ex}");
                return "刪除車隊公告資料發生錯誤.";
            }
        }

        /// <summary>
        /// 編輯車隊公告資料
        /// </summary>
        /// <param name="teamAnnouncementDto">teamAnnouncementDto</param>
        /// <returns>string</returns>
        public async Task<string> EditTeamAnnouncementData(TeamAnnouncementDto teamAnnouncementDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamAnnouncementDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(teamAnnouncementDto.AnnouncementID))
                {
                    return "車隊公告編號無效.";
                }

                if (string.IsNullOrEmpty(teamAnnouncementDto.MemberID))
                {
                    return "無法進行編輯車隊公告資料審核.";
                }

                if (teamAnnouncementDto.MemberID.Equals(SYSTEM_FLAG))
                {
                    return "無法編輯系統公告.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamAnnouncementDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (!teamData.TeamLeaderID.Equals(teamAnnouncementDto.MemberID) || !teamData.TeamViceLeaderIDs.Contains(teamAnnouncementDto.MemberID))
                {
                    return "無編輯車隊公告資料權限.";
                }

                TeamAnnouncementData teamAnnouncementData = await this.teamRepository.GetTeamAnnouncementData(teamAnnouncementDto.AnnouncementID);
                if (teamAnnouncementData == null)
                {
                    return "車隊公告資料不存在.";
                }

                string updateTeamAnnouncementDataHandlerResult = this.UpdateTeamAnnouncementDataHandler(teamAnnouncementDto, teamAnnouncementData);
                if (!string.IsNullOrEmpty(updateTeamAnnouncementDataHandlerResult))
                {
                    return updateTeamAnnouncementDataHandlerResult;
                }

                bool updateTeamAnnouncementDataResult = await this.teamRepository.UpdateTeamAnnouncementData(teamAnnouncementData);
                if (!updateTeamAnnouncementDataResult)
                {
                    return "車隊公告資料更新失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Team Announcement Data Error >>> Data:{JsonConvert.SerializeObject(teamAnnouncementDto)}\n{ex}");
                return "編輯車隊公告資料發生錯誤.";
            }
        }

        /// <summary>
        /// 取得車隊公告資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamAnnouncementDtos, string)</returns>
        public async Task<Tuple<IEnumerable<TeamAnnouncementDto>, string>> GetTeamAnnouncementDataList(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return Tuple.Create<IEnumerable<TeamAnnouncementDto>, string>(null, "車隊編號無效.");
                }

                if (string.IsNullOrEmpty(teamDto.ExecutorID))
                {
                    return Tuple.Create<IEnumerable<TeamAnnouncementDto>, string>(null, "無法進行取得車隊公告資料審核.");
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return Tuple.Create<IEnumerable<TeamAnnouncementDto>, string>(null, "車隊不存在.");
                }

                IEnumerable<TeamAnnouncementData> teamAnnouncementDatas = await this.teamRepository.GetTeamAnnouncementDataListOfTeam(teamData.TeamID);
                IEnumerable<TeamAnnouncementDto> teamAnnouncementDtos = this.mapper.Map<IEnumerable<TeamAnnouncementDto>>(teamAnnouncementDatas);
                foreach (TeamAnnouncementDto teamAnnouncementDto in teamAnnouncementDtos)
                {
                    MemberData memberData = await this.memberRepository.GetMemberDataByMemberID(teamDto.ExecutorID);
                    teamAnnouncementDto.EditType = teamData.TeamLeaderID.Equals(teamDto.ExecutorID) || teamData.TeamViceLeaderIDs.Contains(teamDto.ExecutorID) ? (int)TeamAnnouncementEditType.Edit : (int)TeamAnnouncementEditType.None;
                    teamAnnouncementDto.Nickname = teamAnnouncementDto.MemberID.Equals(SYSTEM_FLAG) ? "系統公告" : memberData != null ? memberData.Nickname : string.Empty;
                }

                return Tuple.Create(teamAnnouncementDtos, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Announcement Data List Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return Tuple.Create<IEnumerable<TeamAnnouncementDto>, string>(null, "取得車隊公告資料列表發生錯誤.");
            }
        }

        /// <summary>
        /// 車隊公告資料更新處理
        /// </summary>
        /// <param name="teamAnnouncementDto">teamAnnouncementDto</param>
        /// <param name="teamAnnouncementData">teamAnnouncementData</param>
        private string UpdateTeamAnnouncementDataHandler(TeamAnnouncementDto teamAnnouncementDto, TeamAnnouncementData teamAnnouncementData)
        {
            if (teamAnnouncementDto.Context.Length > AppSettingHelper.Appsetting.TeamAnnouncementMaxLength)
            {
                return $"公告內容字數不得超過 {AppSettingHelper.Appsetting.TeamAnnouncementMaxLength} 字元.";
            }

            if (!string.IsNullOrEmpty(teamAnnouncementDto.Context))
            {
                teamAnnouncementData.Context = teamAnnouncementDto.Context;
            }

            return string.Empty;
        }

        /// <summary>
        /// 驗證車隊公告建立資料
        /// </summary>
        /// <param name="teamAnnouncementDto">teamAnnouncementDto</param>
        /// <returns>string</returns>
        private string VerifyCreateTeamAnnouncement(TeamAnnouncementDto teamAnnouncementDto)
        {
            if (string.IsNullOrEmpty(teamAnnouncementDto.TeamID))
            {
                return "車隊編號無效.";
            }

            if (string.IsNullOrEmpty(teamAnnouncementDto.MemberID))
            {
                return "無法進行建立車隊公告資料審核.";
            }

            if (string.IsNullOrEmpty(teamAnnouncementDto.Context))
            {
                return "請輸入公告內容.";
            }

            if (teamAnnouncementDto.Context.Length > AppSettingHelper.Appsetting.TeamAnnouncementMaxLength)
            {
                return $"公告內容字數不得超過 {AppSettingHelper.Appsetting.TeamAnnouncementMaxLength} 字元.";
            }

            if (teamAnnouncementDto.LimitDate == 0)
            {
                return "請指定公告天數.";
            }

            return string.Empty;
        }

        #endregion 車隊公告資料

        #region 車隊活動資料

        /// <summary>
        /// 建立車隊活動資料
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>string</returns>
        public async Task<string> CreateTeamEventData(TeamEventDto teamEventDto)
        {
            try
            {
                string verifyCreateTeamEventResult = this.VerifyCreateTeamEvent(teamEventDto);
                if (!string.IsNullOrEmpty(verifyCreateTeamEventResult))
                {
                    return verifyCreateTeamEventResult;
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamEventDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (!teamData.TeamMemberIDs.Contains(teamEventDto.MemberID))
                {
                    return "未加入車隊.";
                }

                DateTime createDate = DateTime.Now;
                TeamEventData teamEventData = this.mapper.Map<TeamEventData>(teamEventDto);
                teamEventData.CreateDate = createDate;
                teamEventData.EventID = Utility.GetSerialID(createDate);
                bool isSuccess = await this.teamRepository.CreateTeamEventData(teamEventData);
                if (!isSuccess)
                {
                    return "建立車隊活動資料失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Team Event Data Error >>> Data:{JsonConvert.SerializeObject(teamEventDto)}\n{ex}");
                return "建立車隊活動資料發生錯誤.";
            }
        }

        /// <summary>
        /// 刪除車隊活動資料
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>string</returns>
        public async Task<string> DeleteTeamEventData(TeamEventDto teamEventDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamEventDto.EventID))
                {
                    return "車隊活動編號無效.";
                }

                if (string.IsNullOrEmpty(teamEventDto.MemberID))
                {
                    return "無法進行刪除車隊活動資料審核.";
                }

                TeamEventData teamEventData = await this.teamRepository.GetTeamEventData(teamEventDto.EventID);
                if (teamEventData == null)
                {
                    //// 如果沒車隊活動資料了，就當作已經刪除掉
                    return string.Empty;
                }

                if (!teamEventData.MemberID.Equals(teamEventDto.MemberID))
                {
                    return "無刪除車隊活動資料權限.";
                }

                bool deleteTeamEventDataResult = await this.teamRepository.DeleteTeamEventData(teamEventData.EventID);
                if (!deleteTeamEventDataResult)
                {
                    return "刪除車隊活動資料失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Team Event Data Error >>> EventID:{teamEventDto.EventID} MemberID:{teamEventDto.MemberID}\n{ex}");
                return "刪除車隊活動資料發生錯誤.";
            }
        }

        /// <summary>
        /// 編輯車隊活動資料
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>string</returns>
        public async Task<string> EditTeamEventData(TeamEventDto teamEventDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamEventDto.EventID))
                {
                    return "車隊活動編號無效.";
                }

                if (string.IsNullOrEmpty(teamEventDto.MemberID))
                {
                    return "無法進行編輯車隊活動資料審核.";
                }

                TeamEventData teamEventData = await this.teamRepository.GetTeamEventData(teamEventDto.EventID);
                if (teamEventData == null)
                {
                    return "車隊活動資料不存在.";
                }

                if (!teamEventData.MemberID.Equals(teamEventDto.MemberID))
                {
                    return "無編輯車隊活動資料權限.";
                }

                string updateTeamEventDataHandlerResult = this.UpdateTeamEventDataHandler(teamEventDto, teamEventData);
                if (!string.IsNullOrEmpty(updateTeamEventDataHandlerResult))
                {
                    return updateTeamEventDataHandlerResult;
                }

                bool updateTeamEventDataResult = await this.teamRepository.UpdateTeamEventData(teamEventData);
                if (!updateTeamEventDataResult)
                {
                    return "車隊活動資料更新失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Team Event Data Error >>> Data:{JsonConvert.SerializeObject(teamEventDto)}\n{ex}");
                return "編輯車隊活動資料發生錯誤.";
            }
        }

        /// <summary>
        /// 取得車隊活動資料
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>Tuple(TeamEventDto, string)</returns>
        public async Task<Tuple<TeamEventDto, string>> GetTeamEventData(TeamEventDto teamEventDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamEventDto.TeamID))
                {
                    return Tuple.Create<TeamEventDto, string>(null, "車隊編號無效.");
                }

                if (string.IsNullOrEmpty(teamEventDto.EventID))
                {
                    return Tuple.Create<TeamEventDto, string>(null, "車隊活動編號無效.");
                }

                if (string.IsNullOrEmpty(teamEventDto.MemberID))
                {
                    return Tuple.Create<TeamEventDto, string>(null, "無法進行取得車隊活動資料審核.");
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamEventDto.TeamID);
                if (teamData == null)
                {
                    return Tuple.Create<TeamEventDto, string>(null, "車隊不存在.");
                }

                if (!teamData.TeamMemberIDs.Contains(teamEventDto.MemberID))
                {
                    return Tuple.Create<TeamEventDto, string>(null, "未加入車隊.");
                }

                TeamEventData teamEventData = await this.teamRepository.GetTeamEventData(teamEventDto.EventID);
                if (teamEventData == null)
                {
                    return Tuple.Create<TeamEventDto, string>(null, "車隊活動不存在.");
                }

                TeamEventDto getTeamEventDto = this.mapper.Map<TeamEventDto>(teamEventData);
                getTeamEventDto.EditType = getTeamEventDto.MemberID.Equals(teamEventDto.MemberID) ? (int)TeamEventEditType.Edit : (int)TeamEventEditType.None;
                return Tuple.Create(getTeamEventDto, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Event Data Error >>> TeamID:{teamEventDto.TeamID} EventID:{teamEventDto.EventID} MemberID:{teamEventDto.MemberID}\n{ex}");
                return Tuple.Create<TeamEventDto, string>(null, "取得車隊活動資料發生錯誤.");
            }
        }

        /// <summary>
        /// 取得車隊活動資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamEventDtos, string)</returns>
        public async Task<Tuple<IEnumerable<TeamEventDto>, string>> GetTeamEventDataList(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return Tuple.Create<IEnumerable<TeamEventDto>, string>(null, "車隊編號無效.");
                }

                if (string.IsNullOrEmpty(teamDto.ExecutorID))
                {
                    return Tuple.Create<IEnumerable<TeamEventDto>, string>(null, "無法進行取得車隊活動資料審核.");
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return Tuple.Create<IEnumerable<TeamEventDto>, string>(null, "車隊不存在.");
                }

                if (!teamData.TeamMemberIDs.Contains(teamDto.ExecutorID))
                {
                    return Tuple.Create<IEnumerable<TeamEventDto>, string>(null, "未加入車隊.");
                }

                IEnumerable<TeamEventData> teamEventDatas = await this.teamRepository.GetTeamEventDataListOfTeam(teamData.TeamID);
                IEnumerable<TeamEventDto> teamEventDtos = this.mapper.Map<IEnumerable<TeamEventDto>>(teamEventDatas);
                foreach (TeamEventDto teamEventDto in teamEventDtos)
                {
                    teamEventDto.EditType = teamEventDto.MemberID.Equals(teamDto.ExecutorID) ? (int)TeamEventEditType.Edit : (int)TeamEventEditType.None;
                }
                return Tuple.Create(teamEventDtos, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Event Data List Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return Tuple.Create<IEnumerable<TeamEventDto>, string>(null, "取得車隊活動資料列表發生錯誤.");
            }
        }

        /// <summary>
        /// 加入車隊活動
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>string</returns>
        public async Task<string> JoinTeamEvent(TeamEventDto teamEventDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamEventDto.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(teamEventDto.EventID))
                {
                    return "車隊活動編號無效.";
                }

                if (string.IsNullOrEmpty(teamEventDto.MemberID))
                {
                    return "無法進行加入車隊活動資料審核.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamEventDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (!teamData.TeamMemberIDs.Contains(teamEventDto.MemberID))
                {
                    return "未加入車隊.";
                }

                TeamEventData teamEventData = await this.teamRepository.GetTeamEventData(teamEventDto.EventID);
                if (teamEventData == null)
                {
                    return "車隊活動資料不存在.";
                }

                bool updateJoinMemberIDsResult = Utility.UpdateListHandler(teamEventData.JoinMemberIDs, teamEventDto.MemberID, true);
                if (updateJoinMemberIDsResult)
                {
                    bool updateTeamEventDataResult = await this.teamRepository.UpdateTeamEventData(teamEventData);
                    if (!updateTeamEventDataResult)
                    {
                        return "車隊活動資料更新失敗.";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Join Team Event Error >>> TeamID:{teamEventDto.TeamID} EventID:{teamEventDto.EventID} MemberID:{teamEventDto.MemberID}\n{ex}");
                return "加入車隊活動發生錯誤.";
            }
        }

        /// <summary>
        /// 離開車隊活動
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>string</returns>
        public async Task<string> LeaveTeamEvent(TeamEventDto teamEventDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamEventDto.EventID))
                {
                    return "車隊活動編號無效.";
                }

                if (string.IsNullOrEmpty(teamEventDto.MemberID))
                {
                    return "無法進行離開車隊活動資料審核.";
                }

                TeamEventData teamEventData = await this.teamRepository.GetTeamEventData(teamEventDto.EventID);
                if (teamEventData == null)
                {
                    //// 如果沒車隊活動資料了，就當作已經離開
                    return string.Empty;
                }

                bool updateJoinMemberIDsResult = Utility.UpdateListHandler(teamEventData.JoinMemberIDs, teamEventDto.MemberID, false);
                if (updateJoinMemberIDsResult)
                {
                    bool updateTeamEventDataResult = await this.teamRepository.UpdateTeamEventData(teamEventData);
                    if (!updateTeamEventDataResult)
                    {
                        return "車隊活動資料更新失敗.";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Leave Team Event Error >>> EventID:{teamEventDto.EventID} MemberID:{teamEventDto.MemberID}\n{ex}");
                return "離開車隊活動發生錯誤.";
            }
        }

        /// <summary>
        /// 車隊活動資料更新處理
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <param name="teamEventData">teamEventData</param>
        private string UpdateTeamEventDataHandler(TeamEventDto teamEventDto, TeamEventData teamEventData)
        {
            //// TODO
            if (teamEventDto.EventDate != null)
            {
                teamEventData.EventDate = teamEventDto.EventDate;
            }

            if (teamEventDto.Distance > 0)
            {
                teamEventData.Distance = teamEventDto.Distance;
            }

            if (teamEventDto.Altitude > 0)
            {
                teamEventData.Altitude = teamEventDto.Altitude;
            }

            return string.Empty;
        }

        /// <summary>
        /// 驗證車隊活動建立資料
        /// </summary>
        /// <param name="teamEventDto">teamEventDto</param>
        /// <returns>string</returns>
        private string VerifyCreateTeamEvent(TeamEventDto teamEventDto)
        {
            if (string.IsNullOrEmpty(teamEventDto.TeamID))
            {
                return "車隊編號無效.";
            }

            if (string.IsNullOrEmpty(teamEventDto.MemberID))
            {
                return "無法進行建立車隊活動資料審核.";
            }

            if (teamEventDto.EventDate != null)
            {
                if (teamEventDto.EventDate < DateTime.Now.AddDays(AppSettingHelper.Appsetting.MinDaysOfEvent))
                {
                    return "車隊活動時間需大於90天後.";
                }
            }
            else
            {
                return "請輸入活動時間.";
            }

            if (teamEventDto.Distance <= 0)
            {
                return "請輸入總距離.";
            }

            if (teamEventDto.Altitude == 0)
            {
                return "請輸入最高海拔.";
            }

            //// TODO
            //teamEventDto.RoadLines
            //teamEventDto.RoadRemarks
            return string.Empty;
        }

        #endregion 車隊活動資料
    }
}