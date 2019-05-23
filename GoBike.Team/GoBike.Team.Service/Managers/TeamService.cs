using AutoMapper;
using GoBike.Team.Core.Resource;
using GoBike.Team.Repository.Interface;
using GoBike.Team.Repository.Models;
using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using GoBike.Team.Service.Models.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoBike.Team.Service.Managers
{
    /// <summary>
    /// 車隊服務
    /// </summary>
    public class TeamService : ITeamService
    {
        /// <summary>
        /// eventRepository
        /// </summary>
        private readonly IEventRepository eventRepository;

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
        /// <param name="eventRepository">eventRepository</param>
        public TeamService(ILogger<TeamService> logger, IMapper mapper, ITeamRepository teamRepository, IEventRepository eventRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.teamRepository = teamRepository;
            this.eventRepository = eventRepository;
        }

        #region 車隊資料

        /// <summary>
        /// 車隊編輯
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(TeamInfoDto, string)</returns>
        public async Task<Tuple<TeamInfoDto, string>> EditData(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, false, false, true);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Edit Data Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} Data:{JsonConvert.SerializeObject(teamCommand.Data)}");
                    return Tuple.Create<TeamInfoDto, string>(null, "車隊編輯失敗.");
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return Tuple.Create<TeamInfoDto, string>(null, "車隊不存在.");
                }

                bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, false, string.Empty);
                if (!verifyTeamExaminerAuthorityResult)
                {
                    this.logger.LogError($"Edit Data Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID}  ExaminerID:{teamCommand.ExaminerID}");
                    return Tuple.Create<TeamInfoDto, string>(null, "車隊編輯失敗.");
                }

                string updateTeamDataHandlerReault = await this.UpdateTeamDataHandler(teamCommand.Data, teamData);
                if (!string.IsNullOrEmpty(updateTeamDataHandlerReault))
                {
                    return Tuple.Create<TeamInfoDto, string>(null, updateTeamDataHandlerReault);
                }

                bool updateTeamDataResult = await this.teamRepository.UpdateTeamData(teamData);
                if (!updateTeamDataResult)
                {
                    return Tuple.Create<TeamInfoDto, string>(null, "車隊資料更新失敗.");
                }

                return Tuple.Create(this.mapper.Map<TeamInfoDto>(teamData), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Data Error >>> Data:{JsonConvert.SerializeObject(teamCommand)}\n{ex}");
                return Tuple.Create<TeamInfoDto, string>(null, "車隊編輯發生錯誤.");
            }
        }

        /// <summary>
        /// 取得車隊資訊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(TeamInfoDto, string)</returns>
        public async Task<Tuple<TeamInfoDto, string>> GetTeamInfo(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Get Team Info Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}");
                    return Tuple.Create<TeamInfoDto, string>(null, "取得車隊資訊失敗.");
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return Tuple.Create<TeamInfoDto, string>(null, "車隊不存在.");
                }

                if (teamData.TeamBlacklistIDs.Contains(teamCommand.TargetID))
                {
                    return Tuple.Create<TeamInfoDto, string>(null, "會員已被車隊設為黑名單.");
                }

                return Tuple.Create(this.mapper.Map<TeamInfoDto>(teamData), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Info Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return Tuple.Create<TeamInfoDto, string>(null, "取得車隊資訊發生錯誤.");
            }
        }

        /// <summary>
        /// 取得會員的車隊資訊列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(TeamInfoArray, string)</returns>
        public async Task<Tuple<dynamic[], string>> GetTeamInfoListOfMember(TeamCommandDto teamCommand)
        {
            try
            {
                string memberID = teamCommand.TargetID;
                if (string.IsNullOrEmpty(memberID))
                {
                    return Tuple.Create<dynamic[], string>(null, "會員編號無效.");
                }

                IEnumerable<TeamData> teamDatas = await this.teamRepository.GetTeamDataListOfMember(memberID);
                TeamData leaderTeamData = teamDatas.Where(data => data.TeamLeaderID.Equals(memberID)).FirstOrDefault();
                if (leaderTeamData != null)
                    (teamDatas as List<TeamData>).Remove(leaderTeamData);

                dynamic[] myTeamInfo = new dynamic[] { this.mapper.Map<TeamInfoDto>(leaderTeamData), this.mapper.Map<IEnumerable<TeamInfoDto>>(teamDatas) };
                return Tuple.Create(myTeamInfo, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Info List Of Member Error >>> MemberID:{teamCommand.TargetID}\n{ex}");
                return Tuple.Create<dynamic[], string>(null, "取得會員的車隊資訊列表發生錯誤.");
            }
        }

        /// <summary>
        /// 建立車隊
        /// </summary>
        /// <param name="teamInfo">teamInfo</param>
        /// <returns>string</returns>
        public async Task<string> Register(TeamInfoDto teamInfo)
        {
            try
            {
                Tuple<TeamData, string> createTeamDataResult = await this.CreateTeamData(teamInfo);
                if (!string.IsNullOrEmpty(createTeamDataResult.Item2))
                {
                    return createTeamDataResult.Item2;
                }

                TeamData teamData = createTeamDataResult.Item1;
                bool isSuccess = await this.teamRepository.CreateTeamData(teamData);
                if (!isSuccess)
                {
                    return "建立車隊失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Register Error >>> Data:{JsonConvert.SerializeObject(teamInfo)}\n{ex}");
                return "建立車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 搜尋車隊
        /// </summary>
        /// <param name="teamSearchCommand">teamSearchCommand</param>
        /// <returns>Tuple(TeamInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<TeamInfoDto>, string>> SearchTeam(TeamSearchCommandDto teamSearchCommand)
        {
            try
            {
                bool verifyTeamSearchCommandResult = this.VerifyTeamSearchCommand(teamSearchCommand);
                if (!verifyTeamSearchCommandResult)
                {
                    this.logger.LogError($"Search Team Fail For Verify TeamSearchCommand >>> SearcherID:{teamSearchCommand.SearcherID} SearchKey:{teamSearchCommand.SearchKey}");
                    return Tuple.Create<IEnumerable<TeamInfoDto>, string>(null, "搜尋車隊失敗");
                }

                int searchOpenStatus = (int)TeamSearchStatusType.Open;
                IEnumerable<TeamData> teamDatas = await this.teamRepository.GetTeamDataListByTeamName(teamSearchCommand.SearchKey, false);
                IEnumerable<TeamData> allowTeamDatas = teamDatas.Where(data => !data.TeamBlacklistIDs.Contains(teamSearchCommand.SearcherID) && data.TeamSearchStatus == searchOpenStatus);
                return Tuple.Create(this.mapper.Map<IEnumerable<TeamInfoDto>>(allowTeamDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Team Error >>> SearcherID:{teamSearchCommand.SearcherID} SearchKey:{teamSearchCommand.SearchKey}\n{ex}");
                return Tuple.Create<IEnumerable<TeamInfoDto>, string>(null, "搜尋車隊發生錯誤.");
            }
        }

        /// <summary>
        /// 創建新車隊資料
        /// </summary>
        /// <param name="teamInfo">teamInfo</param>
        /// <returns>Tuple(TeamData, string)</returns>
        private async Task<Tuple<TeamData, string>> CreateTeamData(TeamInfoDto teamInfo)
        {
            if (string.IsNullOrEmpty(teamInfo.TeamLeaderID))
            {
                return Tuple.Create<TeamData, string>(null, "創建人會員編號無效.");
            }
            else
            {
                bool isMultipleTeam = await this.teamRepository.VerifyTeamDataByTeamLeaderID(teamInfo.TeamLeaderID);
                if (isMultipleTeam)
                {
                    return Tuple.Create<TeamData, string>(null, "無法建立多個車隊.");
                }
            }

            if (string.IsNullOrEmpty(teamInfo.TeamName))
            {
                return Tuple.Create<TeamData, string>(null, "車隊名稱無效.");
            }
            else
            {
                bool isRepeatTeamName = await this.teamRepository.VerifyTeamDataByTeamName(teamInfo.TeamName);
                if (isRepeatTeamName)
                {
                    return Tuple.Create<TeamData, string>(null, "車隊名稱重複.");
                }
            }

            if (string.IsNullOrEmpty(teamInfo.TeamLocation))
            {
                return Tuple.Create<TeamData, string>(null, "車隊所在地無效.");
            }

            if (string.IsNullOrEmpty(teamInfo.TeamInfo))
            {
                return Tuple.Create<TeamData, string>(null, "車隊簡介無效.");
            }

            if (string.IsNullOrEmpty(teamInfo.TeamPhoto))
            {
                return Tuple.Create<TeamData, string>(null, "未上傳車隊頭像.");
            }

            if (string.IsNullOrEmpty(teamInfo.TeamCoverPhoto))
            {
                return Tuple.Create<TeamData, string>(null, "未上傳車隊封面.");
            }

            if (teamInfo.TeamSearchStatus == (int)TeamSearchStatusType.None)
            {
                return Tuple.Create<TeamData, string>(null, "未設定搜尋狀態.");
            }

            if (teamInfo.TeamExamineStatus == (int)TeamExamineStatusType.None)
            {
                return Tuple.Create<TeamData, string>(null, "未設定審核狀態.");
            }

            DateTime createDate = DateTime.Now;
            TeamData teamData = this.mapper.Map<TeamData>(teamInfo);
            teamData.TeamID = $"{Guid.NewGuid().ToString().Substring(0, 6)}-{createDate:yyyy}-{createDate:MMdd}";
            teamData.TeamCreateDate = createDate;
            teamData.TeamSaveDeadline = createDate.AddDays(60);
            teamData.TeamViceLeaderIDs = new List<string>();
            teamData.TeamPlayerIDs = new List<string>();
            teamData.TeamApplyForJoinIDs = new List<string>();
            teamData.TeamInviteJoinIDs = new List<string>();
            teamData.TeamBlacklistIDs = new List<string>();
            teamData.TeamBlacklistedIDs = new List<string>();
            teamData.TeamEventIDs = new List<string>();
            return Tuple.Create(teamData, string.Empty);
        }

        /// <summary>
        /// 車隊資料更新處理
        /// </summary>
        /// <param name="teamInfo">teamInfo</param>
        /// <param name="teamData">teamData</param>
        /// <returns>string</returns>
        private async Task<string> UpdateTeamDataHandler(TeamInfoDto teamInfo, TeamData teamData)
        {
            if (!string.IsNullOrEmpty(teamInfo.TeamName))
            {
                bool isRepeatTeamName = await this.teamRepository.VerifyTeamDataByTeamName(teamInfo.TeamName);
                if (isRepeatTeamName)
                {
                    return "車隊名稱重複.";
                }

                teamData.TeamName = teamInfo.TeamName;
            }

            if (!string.IsNullOrEmpty(teamInfo.TeamLocation))
                teamData.TeamLocation = teamInfo.TeamLocation;

            if (!string.IsNullOrEmpty(teamInfo.TeamInfo))
                teamData.TeamInfo = teamInfo.TeamInfo;

            if (!string.IsNullOrEmpty(teamInfo.TeamPhoto))
                teamData.TeamPhoto = teamInfo.TeamPhoto;

            if (!string.IsNullOrEmpty(teamInfo.TeamCoverPhoto))
                teamData.TeamCoverPhoto = teamInfo.TeamCoverPhoto;

            if (teamInfo.TeamSearchStatus != (int)TeamSearchStatusType.None)
                teamData.TeamSearchStatus = teamInfo.TeamSearchStatus;

            if (teamInfo.TeamExamineStatus != (int)TeamExamineStatusType.None)
                teamData.TeamExamineStatus = teamInfo.TeamExamineStatus;

            return string.Empty;
        }

        /// <summary>
        /// 驗證車隊指令資料
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <param name="isVerifyExaminer">isVerifyExaminer</param>
        /// <param name="isVerifyTarget">isVerifyTarget</param>
        /// <param name="isVerifyTargets">isVerifyTargets</param>
        /// <param name="isVerifyData">isVerifyData</param>
        /// <returns>bool</returns>
        private bool VerifyTeamCommand(TeamCommandDto teamCommand, bool isVerifyExaminer, bool isVerifyTarget, bool isVerifyTargets, bool isVerifyData)
        {
            if (teamCommand == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(teamCommand.TeamID))
            {
                return false;
            }

            if (isVerifyExaminer)
            {
                if (string.IsNullOrEmpty(teamCommand.ExaminerID))
                {
                    return false;
                }
            }

            if (isVerifyTarget)
            {
                if (string.IsNullOrEmpty(teamCommand.TargetID))
                {
                    return false;
                }
            }

            if (isVerifyTargets)
            {
                if (teamCommand.TargetIDs == null || teamCommand.TargetIDs.Count() == 0)
                {
                    return false;
                }
            }

            if (isVerifyData)
            {
                if (teamCommand.Data == null)
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 驗證車隊審查者權限
        /// </summary>
        /// <param name="teamData">teamData</param>
        /// <param name="examinerID">examinerID</param>
        /// <param name="isSupreme">isSupreme</param>
        /// <param name="isVerifyTarget">isVerifyTarget</param>
        /// <param name="targetID">targetID</param>
        /// <returns>bool</returns>
        private bool VerifyTeamExaminerAuthority(TeamData teamData, string examinerID, bool isSupreme, bool isVerifyTarget, string targetID)
        {
            if (string.IsNullOrEmpty(examinerID))
            {
                return false;
            }

            if (isSupreme)
            {
                if (!examinerID.Equals(teamData.TeamLeaderID))
                {
                    return false;
                }
            }

            if (!teamData.TeamLeaderID.Equals(examinerID) && !teamData.TeamViceLeaderIDs.Contains(examinerID))
            {
                return false;
            }

            if (isVerifyTarget)
            {
                if (string.IsNullOrEmpty(targetID))
                {
                    return false;
                }

                if (examinerID.Equals(targetID))
                {
                    return false;
                }
                if (targetID.Equals(teamData.TeamLeaderID))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 驗證車隊搜尋指令資料
        /// </summary>
        /// <param name="teamSearchCommand">teamSearchCommand</param>
        /// <returns>bool</returns>
        private bool VerifyTeamSearchCommand(TeamSearchCommandDto teamSearchCommand)
        {
            if (teamSearchCommand == null)
            {
                return false;
            }

            if (string.IsNullOrEmpty(teamSearchCommand.SearcherID))
            {
                return false;
            }

            if (string.IsNullOrEmpty(teamSearchCommand.SearchKey))
            {
                return false;
            }

            return true;
        }

        #endregion 車隊資料

        #region 車隊互動資料

        /// <summary>
        /// 申請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> ApplyForJoinTeam(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Apply For Join Team Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}");
                    return "申請加入車隊失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (teamData.TeamExamineStatus == (int)TeamExamineStatusType.Close)
                {
                    return await this.JoinTeam(teamCommand, false);
                }

                bool verifyJoinTeamQualificationResult = this.VerifyJoinTeamQualification(teamData, teamCommand.TargetID);
                if (!verifyJoinTeamQualificationResult)
                {
                    return "申請加入車隊失敗.";
                }

                bool updateTeamPlayerIDsResult = Utility.UpdateListHandler(teamData.TeamApplyForJoinIDs, teamCommand.TargetID, true);
                bool updateTeamInviteJoinIDsResult = Utility.UpdateListHandler(teamData.TeamInviteJoinIDs, teamCommand.TargetID, false);
                if (updateTeamPlayerIDsResult || updateTeamInviteJoinIDsResult)
                {
                    teamData.TeamNewsDate = DateTime.Now;
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
                this.logger.LogError($"Apply For Join Team Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return "申請加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 取消申請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> CancelApplyForJoinTeam(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Cancel Apply For Join Team Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}");
                    return "取消申請加入車隊失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool updateTeamApplyForJoinIDsResult = Utility.UpdateListHandler(teamData.TeamApplyForJoinIDs, teamCommand.TargetID, false);
                if (updateTeamApplyForJoinIDsResult)
                {
                    bool result = await this.teamRepository.UpdateTeamApplyForJoinIDs(teamData.TeamID, teamData.TeamApplyForJoinIDs);
                    if (!result)
                    {
                        return "申請加入名單資料更新失敗.";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Cancel Apply For Join Team Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return "取消申請加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 取消邀請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> CancelInviteJoinTeam(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Cancel Invite Join Team Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}");
                    return "取消邀請加入車隊失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, true, teamCommand.TargetID);
                if (!verifyTeamExaminerAuthorityResult)
                {
                    this.logger.LogError($"Cancel Invite Join Team Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}");
                    return "邀請加入車隊失敗.";
                }

                if (teamData.TeamPlayerIDs.Contains(teamCommand.TargetID))
                {
                    return "會員已加入車隊.";
                }

                bool updateTeamInviteJoinIDsResult = Utility.UpdateListHandler(teamData.TeamInviteJoinIDs, teamCommand.TargetID, false);
                if (updateTeamInviteJoinIDsResult)
                {
                    bool result = await this.teamRepository.UpdateTeamInviteJoinIDs(teamData.TeamID, teamData.TeamInviteJoinIDs);
                    if (!result)
                    {
                        return "邀請加入名單資料更新失敗.";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Cancel Invite Join Team Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return "取消邀請加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 強制離開車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> ForceLeaveTeam(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Force Leave Team Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}");
                    return "強制離開車隊失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, true, teamCommand.TargetID);
                if (!verifyTeamExaminerAuthorityResult)
                {
                    this.logger.LogError($"Force Leave Team Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID}  ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}");
                    return "強制離開車隊失敗.";
                }

                bool updateTeamPlayerIDsResult = Utility.UpdateListHandler(teamData.TeamPlayerIDs, teamCommand.TargetID, false);
                bool updateTeamViceLeaderIDsResult = Utility.UpdateListHandler(teamData.TeamViceLeaderIDs, teamCommand.TargetID, false);
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
                this.logger.LogError($"Force Leave Team Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}\n{ex}");
                return "強制離開車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 取得申請加入名單
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(strings, string)</returns>
        public async Task<Tuple<IEnumerable<string>, string>> GetApplyForRequestList(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, false, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Get Apply For Request List Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID}");
                    return Tuple.Create<IEnumerable<string>, string>(null, "取得申請加入名單失敗.");
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return Tuple.Create<IEnumerable<string>, string>(null, "車隊不存在.");
                }

                bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, false, string.Empty);
                if (!verifyTeamExaminerAuthorityResult)
                {
                    this.logger.LogError($"Get Apply For Request List Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID}");
                    return Tuple.Create<IEnumerable<string>, string>(null, "取得申請加入名單失敗.");
                }

                return Tuple.Create(teamData.TeamApplyForJoinIDs, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Apply For Request List Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID}\n{ex}");
                return Tuple.Create<IEnumerable<string>, string>(null, "取得申請加入名單發生錯誤.");
            }
        }

        /// <summary>
        /// 取得邀請加入名單
        /// </summary>
        /// <param name="memberCommand">memberCommand</param>
        /// <returns>Tuple(TeamInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<TeamInfoDto>, string>> GetInviteRequestList(TeamCommandDto teamCommand)
        {
            try
            {
                string memberID = teamCommand.TargetID;
                if (string.IsNullOrEmpty(memberID))
                {
                    return Tuple.Create<IEnumerable<TeamInfoDto>, string>(null, "會員編號無效.");
                }

                IEnumerable<TeamData> teamDatas = await this.teamRepository.GetTeamDataListOfInviteJoin(teamCommand.TargetID);
                return Tuple.Create(this.mapper.Map<IEnumerable<TeamInfoDto>>(teamDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Invite Request List Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return Tuple.Create<IEnumerable<TeamInfoDto>, string>(null, "取得邀請加入名單發生錯誤.");
            }
        }

        /// <summary>
        /// 邀請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> InviteJoinTeam(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Invite Join Team Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}");
                    return "邀請加入車隊失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (!teamData.TeamApplyForJoinIDs.Contains(teamCommand.TargetID))
                {
                    return "該會員已提出申請加入請求.";
                }

                bool verifyJoinTeamQualificationResult = this.VerifyJoinTeamQualification(teamData, teamCommand.TargetID);
                if (!verifyJoinTeamQualificationResult)
                {
                    return "邀請加入車隊失敗.";
                }

                bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, true, teamCommand.TargetID);
                if (!verifyTeamExaminerAuthorityResult)
                {
                    this.logger.LogError($"Invite Join Team Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}");
                    return "邀請加入車隊失敗.";
                }

                bool updateTeamInviteJoinIDsResult = Utility.UpdateListHandler(teamData.TeamInviteJoinIDs, teamCommand.TargetID, true);
                if (updateTeamInviteJoinIDsResult)
                {
                    bool result = await this.teamRepository.UpdateTeamInviteJoinIDs(teamData.TeamID, teamData.TeamInviteJoinIDs);
                    if (!result)
                    {
                        return "邀請加入名單資料更新失敗.";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Invite Join Team Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}\n{ex}");
                return "邀請加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 邀請多人加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> InviteManyJoinTeam(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, false, true, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Invite Many Join Team Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetIDs:{JsonConvert.SerializeObject(teamCommand.TargetIDs)}");
                    return "邀請多人加入車隊失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, false, string.Empty);
                if (!verifyTeamExaminerAuthorityResult)
                {
                    this.logger.LogError($"Invite Many Join Team Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID}");
                    return "邀請多人加入車隊失敗.";
                }

                IEnumerable<string> teamApplyForJoinIDs = teamData.TeamApplyForJoinIDs;
                IEnumerable<string> targetIDs = teamCommand.TargetIDs;
                IEnumerable<string> validTargetIDs = targetIDs.Where(targetID => this.VerifyJoinTeamQualification(teamData, targetID) && !teamApplyForJoinIDs.Contains(targetID));
                if (!validTargetIDs.Any())
                {
                    return "無會員名單可邀請加入車隊.";
                }

                bool updateTeamInviteJoinIDsResult = Utility.UpdateListHandler(teamData.TeamInviteJoinIDs, validTargetIDs, true);
                if (updateTeamInviteJoinIDsResult)
                {
                    bool result = await this.teamRepository.UpdateTeamInviteJoinIDs(teamData.TeamID, teamData.TeamInviteJoinIDs);
                    if (!result)
                    {
                        return "邀請加入名單資料更新失敗.";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Invite Many Join Team Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetIDs:{JsonConvert.SerializeObject(teamCommand.TargetIDs)}\n{ex}");
                return "邀請多人加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <param name="isInvite">isInvite</param>
        /// <returns>string</returns>
        public async Task<string> JoinTeam(TeamCommandDto teamCommand, bool isInvite)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, !isInvite, true, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Join Team Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID} IsInvite:{isInvite}");
                    return "加入車隊失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool verifyJoinTeamQualificationResult = this.VerifyJoinTeamQualification(teamData, teamCommand.TargetID);
                if (!verifyJoinTeamQualificationResult)
                {
                    return "加入車隊失敗.";
                }

                if (isInvite)
                {
                    if (!teamData.TeamInviteJoinIDs.Contains(teamCommand.TargetID))
                    {
                        return "未邀請該會員加入車隊.";
                    }
                }
                else
                {
                    if (teamData.TeamExamineStatus == (int)TeamExamineStatusType.Open)
                    {
                        if (!teamData.TeamApplyForJoinIDs.Contains(teamCommand.TargetID))
                        {
                            return "該會員未申請加入車隊.";
                        }

                        bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, true, teamCommand.TargetID);
                        if (!verifyTeamExaminerAuthorityResult)
                        {
                            this.logger.LogError($"Join Team Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}");
                            return "加入車隊失敗.";
                        }
                    }
                }

                bool updateTeamPlayerIDsResult = Utility.UpdateListHandler(teamData.TeamPlayerIDs, teamCommand.TargetID, true);
                bool updateTeamInviteJoinIDsResult = Utility.UpdateListHandler(teamData.TeamInviteJoinIDs, teamCommand.TargetID, false);
                bool updateTeamApplyForJoinIDsResult = Utility.UpdateListHandler(teamData.TeamApplyForJoinIDs, teamCommand.TargetID, false);
                if (updateTeamPlayerIDsResult || updateTeamInviteJoinIDsResult || updateTeamApplyForJoinIDsResult)
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
                this.logger.LogError($"Join Team Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID} IsInvite:{isInvite}\n{ex}");
                return "加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 離開車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> LeaveTeam(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Leave Team Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}");
                    return "離開車隊失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (teamCommand.TargetID.Equals(teamData.TeamLeaderID))
                {
                    return "請先移交隊長職務.";
                }

                if (!teamData.TeamPlayerIDs.Contains(teamCommand.TargetID))
                {
                    return "會員未加入車隊.";
                }

                bool updateTeamPlayerIDsResult = Utility.UpdateListHandler(teamData.TeamPlayerIDs, teamCommand.TargetID, false);
                bool updateTeamViceLeaderIDsResult = Utility.UpdateListHandler(teamData.TeamViceLeaderIDs, teamCommand.TargetID, false);
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
                this.logger.LogError($"Leave Team Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return "離開車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 拒絕申請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> RejectApplyForJoinTeam(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Reject Apply For Join Team Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}");
                    return "拒絕申請加入車隊失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, true, teamCommand.TargetID);
                if (!verifyTeamExaminerAuthorityResult)
                {
                    this.logger.LogError($"Reject Apply For Join Team Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}");
                    return "拒絕申請加入車隊失敗.";
                }

                if (teamData.TeamPlayerIDs.Contains(teamCommand.TargetID))
                {
                    return "會員已加入車隊.";
                }

                bool updateTeamApplyForJoinIDsResult = Utility.UpdateListHandler(teamData.TeamApplyForJoinIDs, teamCommand.TargetID, false);
                if (updateTeamApplyForJoinIDsResult)
                {
                    bool result = await this.teamRepository.UpdateTeamApplyForJoinIDs(teamData.TeamID, teamData.TeamApplyForJoinIDs);
                    if (!result)
                    {
                        return "申請加入名單資料更新失敗.";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Apply For Join Team Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}\n{ex}");
                return "拒絕申請加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 拒絕邀請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> RejectInviteJoinTeam(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Reject Invite Join Team Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}");
                    return "拒絕邀請加入車隊失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool updateTeamInviteJoinIDsResult = Utility.UpdateListHandler(teamData.TeamInviteJoinIDs, teamCommand.TargetID, false);
                if (updateTeamInviteJoinIDsResult)
                {
                    bool result = await this.teamRepository.UpdateTeamInviteJoinIDs(teamData.TeamID, teamData.TeamInviteJoinIDs);
                    if (!result)
                    {
                        return "邀請加入名單資料更新失敗.";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Invite Join Team Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return "拒絕邀請加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 更新車隊隊長
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> UpdateTeamLeader(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Update Team Leader Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}");
                    return "更新車隊隊長失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, true, true, teamCommand.TargetID);
                if (!verifyTeamExaminerAuthorityResult)
                {
                    this.logger.LogError($"Update Team Leader Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID}  ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}");
                    return "更新車隊隊長失敗.";
                }

                if (!teamData.TeamPlayerIDs.Contains(teamCommand.TargetID))
                {
                    return "會員未加入車隊.";
                }

                bool isMultipleTeam = await this.teamRepository.VerifyTeamDataByTeamLeaderID(teamCommand.TargetID);
                if (isMultipleTeam)
                {
                    return "無法指定其他車隊隊長.";
                }

                Utility.UpdateListHandler(teamData.TeamPlayerIDs, teamData.TeamLeaderID, true);
                Utility.UpdateListHandler(teamData.TeamPlayerIDs, teamCommand.TargetID, false);
                teamData.TeamLeaderID = teamCommand.TargetID;
                bool updateTeamDataResult = await this.teamRepository.UpdateTeamData(teamData);
                if (!updateTeamDataResult)
                {
                    return "車隊資料更新失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Leader Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}\n{ex}");
                return "更新車隊隊長發生錯誤.";
            }
        }

        /// <summary>
        /// 更新車隊副隊長
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <param name="isAdd">isAdd</param>
        /// <returns>string</returns>
        public async Task<string> UpdateTeamViceLeader(TeamCommandDto teamCommand, bool isAdd)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Update Team Vice Leader Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}");
                    return "更新車隊副隊長失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, true, true, teamCommand.TargetID);
                if (!verifyTeamExaminerAuthorityResult)
                {
                    this.logger.LogError($"Update Team Vice Leader Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID}  ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}");
                    return "更新車隊副隊長失敗.";
                }

                if (isAdd && !teamData.TeamPlayerIDs.Contains(teamCommand.TargetID))
                {
                    return "會員未加入車隊.";
                }

                bool updateTeamViceLeaderIDsResult = Utility.UpdateListHandler(teamData.TeamViceLeaderIDs, teamCommand.TargetID, isAdd);
                if (updateTeamViceLeaderIDsResult)
                {
                    bool result = await this.teamRepository.UpdateTeamViceLeaders(teamData.TeamID, teamData.TeamViceLeaderIDs);
                    if (!result)
                    {
                        return "車隊副隊長名單資料更新失敗.";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Vice Leader Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}\n{ex}");
                return "更新車隊副隊長發生錯誤.";
            }
        }

        /// <summary>
        /// 驗證加入車隊資格
        /// </summary>
        /// <param name="teamData">teamData</param>
        /// <param name="memberID">memberID</param>
        /// <returns>bool</returns>
        private bool VerifyJoinTeamQualification(TeamData teamData, string memberID)
        {
            if (teamData.TeamLeaderID.Equals(memberID) || teamData.TeamPlayerIDs.Contains(memberID))
            {
                this.logger.LogError($"Verify Join Team Qualification Error For Member Exist >>> TeamID:{teamData.TeamID} MemberID:{memberID}");
                return false;
            }

            if (teamData.TeamBlacklistIDs.Contains(memberID))
            {
                this.logger.LogError($"Verify Join Team Qualification Error For Team Blacklist IDs >>> TeamID:{teamData.TeamID} MemberID:{memberID}");
                return false;
            }

            if (teamData.TeamBlacklistedIDs.Contains(memberID))
            {
                this.logger.LogError($"Verify Join Team Qualification Error For Team Blacklisted IDs >>> TeamID:{teamData.TeamID} MemberID:{memberID}");
                return false;
            }

            return true;
        }

        #endregion 車隊互動資料
    }
}