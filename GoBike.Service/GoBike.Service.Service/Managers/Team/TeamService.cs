using AutoMapper;
using GoBike.Service.Core.Resource;
using GoBike.Service.Core.Resource.Enum;
using GoBike.Service.Repository.Interface.Team;
using GoBike.Service.Repository.Models.Team;
using GoBike.Service.Service.Interface.Team;
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
        /// logger
        /// </summary>
        private readonly ILogger<TeamService> logger;

        /// <summary>
        /// mapper
        /// </summary>
        private readonly IMapper mapper;

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
        public TeamService(ILogger<TeamService> logger, IMapper mapper, ITeamRepository teamRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
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
                    return "會員編號無效.";
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
                    return "會員編號無效.";
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
                TimeSpan timeSpan = new TimeSpan(30, 0, 0, 0, 0);
                int searchOpenStatus = (int)TeamSearchStatusType.Open;
                IEnumerable<TeamData> teamDatas = await this.teamRepository.GetTeamDataListByCreateDate(timeSpan);
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
                return Tuple.Create(this.mapper.Map<TeamDto>(teamData), string.Empty);
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
                IEnumerable<TeamInteractiveData> teamInteractiveDatas = await this.teamRepository.GetTeamInteractiveDataListOfMember(teamDto.ExecutorID);
                IEnumerable<string> inviteTeamIDs = teamInteractiveDatas.Select(data => data.TeamID).Distinct();
                IEnumerable<TeamData> invietTeamDatas = await this.teamRepository.GetTeamDataListByTeamID(inviteTeamIDs);
                IEnumerable<IEnumerable<TeamDto>> teamDtos = new List<IEnumerable<TeamDto>>()
                {
                    this.mapper.Map<IEnumerable<TeamDto>>(teamDatas),
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
                TeamMemberIDs = new List<string>()
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

        #region 互動資料

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

                if (teamData.TeamLeaderID.Equals(teamDto.ExecutorID) || teamData.TeamMemberIDs.Contains(teamDto.ExecutorID))
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

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool isSuccess = await this.teamRepository.DeleteTeamInteractiveData(teamData.TeamID, teamDto.ExecutorID);
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

                Utility.UpdateListHandler(teamDto.TeamMemberIDs, teamData.TeamLeaderID, true);
                Utility.UpdateListHandler(teamDto.TeamMemberIDs, teamDto.TargetID, false);
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
        /// 驗證加入車隊資格
        /// </summary>
        /// <param name="teamData">teamData</param>
        /// <param name="examinerID">examinerID</param>
        /// <param name="targetID">targetID</param>
        /// <param name="isInvite">isInvite</param>
        /// <returns>string</returns>
        private async Task<string> VerifyJoinTeam(TeamData teamData, string examinerID, string targetID, bool isInvite)
        {
            if (teamData.TeamLeaderID.Equals(targetID) || teamData.TeamMemberIDs.Contains(targetID))
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

                    if (!teamData.TeamLeaderID.Equals(examinerID) && !teamData.TeamMemberIDs.Contains(examinerID))
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

        #endregion 互動資料
    }
}