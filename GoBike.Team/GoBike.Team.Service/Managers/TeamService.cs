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
                string verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, false, false, true);
                if (!string.IsNullOrEmpty(verifyTeamCommandResult))
                {
                    return Tuple.Create<TeamInfoDto, string>(null, verifyTeamCommandResult);
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                string verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, false, string.Empty);
                if (!string.IsNullOrEmpty(verifyTeamExaminerAuthorityResult))
                {
                    return Tuple.Create<TeamInfoDto, string>(null, verifyTeamExaminerAuthorityResult);
                }

                string updateTeamDataHandlerReault = await this.UpdateTeamDataHandler(teamCommand.Data, teamData);
                if (!string.IsNullOrEmpty(updateTeamDataHandlerReault))
                {
                    return Tuple.Create<TeamInfoDto, string>(null, updateTeamDataHandlerReault);
                }

                Tuple<bool, string> updateTeamDataResult = await this.teamRepository.UpdateTeamData(teamData);
                if (!updateTeamDataResult.Item1)
                {
                    this.logger.LogError($"Edit Data Fail For Update Team Data >>> Data:{JsonConvert.SerializeObject(teamData)}");
                    return Tuple.Create<TeamInfoDto, string>(null, updateTeamDataResult.Item2);
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
        /// 強制離開車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> ForceLeaveTeam(TeamCommandDto teamCommand)
        {
            try
            {
                string verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false);
                if (!string.IsNullOrEmpty(verifyTeamCommandResult))
                {
                    return verifyTeamCommandResult;
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                string verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, true, teamCommand.TargetID);
                if (!string.IsNullOrEmpty(verifyTeamExaminerAuthorityResult))
                {
                    return verifyTeamExaminerAuthorityResult;
                }

                bool updateTeamPlayerIDsResult = Utility.UpdateListHandler(teamData.TeamPlayerIDs, teamCommand.TargetID, false);
                bool updateTeamViceLeaderIDsResult = Utility.UpdateListHandler(teamData.TeamViceLeaderIDs, teamCommand.TargetID, false);
                if (updateTeamPlayerIDsResult || updateTeamViceLeaderIDsResult)
                {
                    Tuple<bool, string> updateTeamDataResult = await this.teamRepository.UpdateTeamData(teamData);
                    if (!updateTeamDataResult.Item1)
                    {
                        this.logger.LogError($"Force Leave Team Fail For Update Team Data >>> Data:{JsonConvert.SerializeObject(teamData)}");
                        return updateTeamDataResult.Item2;
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
        /// 取得我的車隊資訊列表
        /// </summary>
        /// <param name="memberCommand">memberCommand</param>
        /// <returns>Tuple(TeamInfoDto, TeamInfoDtos, string)</returns>
        public async Task<Tuple<TeamInfoDto, IEnumerable<TeamInfoDto>, string>> GetMyTeamInfoList(MemberCommandDto memberCommand)
        {
            try
            {
                if (memberCommand == null)
                {
                    return Tuple.Create<TeamInfoDto, IEnumerable<TeamInfoDto>, string>(null, null, "會員指令資料不存在.");
                }

                if (string.IsNullOrEmpty(memberCommand.MemberID))
                {
                    return Tuple.Create<TeamInfoDto, IEnumerable<TeamInfoDto>, string>(null, null, "會員編號無效.");
                }

                string memberID = memberCommand.MemberID;
                IEnumerable<TeamData> teamDatas = await this.teamRepository.GetTeamDataListOfMember(memberID);
                TeamData leaderTeamData = teamDatas.Where(data => data.TeamLeaderID.Equals(memberID)).FirstOrDefault();
                if (leaderTeamData != null)
                    (teamDatas as List<TeamData>).Remove(leaderTeamData);

                return Tuple.Create(this.mapper.Map<TeamInfoDto>(leaderTeamData), this.mapper.Map<IEnumerable<TeamInfoDto>>(teamDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get My Team Info List Error >>> MemberID:{memberCommand.MemberID}\n{ex}");
                return Tuple.Create<TeamInfoDto, IEnumerable<TeamInfoDto>, string>(null, null, "取得我的車隊資訊列表發生錯誤.");
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
                string verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, false, false, false);
                if (!string.IsNullOrEmpty(verifyTeamCommandResult))
                {
                    return Tuple.Create<TeamInfoDto, string>(null, verifyTeamCommandResult);
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return Tuple.Create<TeamInfoDto, string>(null, "車隊不存在.");
                }

                return Tuple.Create(this.mapper.Map<TeamInfoDto>(teamData), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Info Error >>> TeamID:{teamCommand.TeamID}\n{ex}");
                return Tuple.Create<TeamInfoDto, string>(null, "取得車隊資訊發生錯誤.");
            }
        }

        /// <summary>
        /// 加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <param name="isExamine">isExamine</param>
        /// <returns>string</returns>
        public async Task<string> JoinTeam(TeamCommandDto teamCommand, bool isExamine)
        {
            try
            {
                string verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, isExamine, true, false, false);
                if (!string.IsNullOrEmpty(verifyTeamCommandResult))
                {
                    return verifyTeamCommandResult;
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                string verifyJoinTeamQualificationResult = this.VerifyJoinTeamQualification(teamData, teamCommand.TargetID);
                if (!string.IsNullOrEmpty(verifyJoinTeamQualificationResult))
                {
                    return verifyJoinTeamQualificationResult;
                }

                if (isExamine)
                {
                    if (!teamData.TeamApplyForJoinIDs.Contains(teamCommand.TargetID))
                    {
                        return "該會員未申請加入車隊.";
                    }

                    string verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, true, teamCommand.TargetID);
                    if (!string.IsNullOrEmpty(verifyTeamExaminerAuthorityResult))
                    {
                        return verifyTeamExaminerAuthorityResult;
                    }
                }

                bool updateTeamPlayerIDsResult = Utility.UpdateListHandler(teamData.TeamPlayerIDs, teamCommand.TargetID, true);
                bool updateTeamInviteJoinIDsResult = Utility.UpdateListHandler(teamData.TeamInviteJoinIDs, teamCommand.TargetID, false);
                bool updateTeamApplyForJoinIDsResult = Utility.UpdateListHandler(teamData.TeamApplyForJoinIDs, teamCommand.TargetID, false);
                if (updateTeamPlayerIDsResult || updateTeamInviteJoinIDsResult || updateTeamApplyForJoinIDsResult)
                {
                    Tuple<bool, string> updateTeamDataResult = await this.teamRepository.UpdateTeamData(teamData);
                    if (!updateTeamDataResult.Item1)
                    {
                        this.logger.LogError($"Join Team Fail For Update Team Data >>> Data:{JsonConvert.SerializeObject(teamData)}");
                        return updateTeamDataResult.Item2;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Join Team Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID} IsExamine:{isExamine}\n{ex}");
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
                string verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false);
                if (!string.IsNullOrEmpty(verifyTeamCommandResult))
                {
                    return verifyTeamCommandResult;
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
                    Tuple<bool, string> updateTeamDataResult = await this.teamRepository.UpdateTeamData(teamData);
                    if (!updateTeamDataResult.Item1)
                    {
                        this.logger.LogError($"Leave Team Fail For Update Team Data >>> Data:{JsonConvert.SerializeObject(teamData)}");
                        return updateTeamDataResult.Item2;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Leave Team Error >>> TemaID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return "離開車隊發生錯誤.";
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
                    this.logger.LogError($"Register Fail For Create Team Data >>> Data:{JsonConvert.SerializeObject(teamData)}");
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
        /// 拒絕加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> RejectJoinTeam(TeamCommandDto teamCommand)
        {
            try
            {
                string verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false);
                if (!string.IsNullOrEmpty(verifyTeamCommandResult))
                {
                    return verifyTeamCommandResult;
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                string verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, true, teamCommand.TargetID); //// 不需要對目標做審核，只要確定目前隊長的身分
                if (!string.IsNullOrEmpty(verifyTeamExaminerAuthorityResult))
                {
                    return verifyTeamExaminerAuthorityResult;
                }

                if (teamData.TeamPlayerIDs.Contains(teamCommand.TargetID))
                {
                    return "會員已加入車隊.";
                }

                bool updateTeamApplyForJoinIDsResult = Utility.UpdateListHandler(teamData.TeamApplyForJoinIDs, teamCommand.TargetID, false);
                if (updateTeamApplyForJoinIDsResult)
                {
                    Tuple<bool, string> result = await this.teamRepository.UpdateTeamApplyForJoinIDs(teamData.TeamID, teamData.TeamApplyForJoinIDs);
                    if (!result.Item1)
                    {
                        this.logger.LogError($"Reject Join Team Fail For Update Team Apply For Join IDs >>> Data:{JsonConvert.SerializeObject(teamData)}");
                        return result.Item2;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Join Team Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}\n{ex}");
                return "拒絕加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 搜尋車隊資訊列表
        /// </summary>
        /// <param name="teamSearchCommand">teamSearchCommand</param>
        /// <returns>Tuple(TeamInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<TeamInfoDto>, string>> SearchTeamInfoList(TeamSearchCommandDto teamSearchCommand)
        {
            try
            {
                string verifyTeamSearchCommandResult = this.VerifyTeamSearchCommand(teamSearchCommand);
                if (!string.IsNullOrEmpty(verifyTeamSearchCommandResult))
                {
                    return Tuple.Create<IEnumerable<TeamInfoDto>, string>(null, verifyTeamSearchCommandResult);
                }

                int searchOpenStatus = (int)TeamSearchStatusType.Open;
                IEnumerable<TeamData> teamDatas = await this.teamRepository.GetTeamDataListByTeamName(teamSearchCommand.SearchKey, false);
                IEnumerable<TeamData> allowTeamDatas = teamDatas.Where(data => !data.TeamBlacklistIDs.Contains(teamSearchCommand.SearcherID) && data.TeamSearchStatus == searchOpenStatus);
                return Tuple.Create(this.mapper.Map<IEnumerable<TeamInfoDto>>(allowTeamDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Team Info List Error >>> SearcherID:{teamSearchCommand.SearcherID} SearchKey:{teamSearchCommand.SearchKey}\n{ex}");
                return Tuple.Create<IEnumerable<TeamInfoDto>, string>(null, "搜尋車隊資訊列表發生錯誤.");
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
                string verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false);
                if (!string.IsNullOrEmpty(verifyTeamCommandResult))
                {
                    return verifyTeamCommandResult;
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                string verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, true, false, string.Empty); //// 不需要對目標做審核，只要確定目前隊長的身分
                if (!string.IsNullOrEmpty(verifyTeamExaminerAuthorityResult))
                {
                    return verifyTeamExaminerAuthorityResult;
                }

                if (teamCommand.TargetID.Equals(teamData.TeamLeaderID))
                {
                    return "該會員已是車隊隊長.";
                }

                bool isMultipleTeam = await this.teamRepository.VerifyTeamDataByTeamLeaderID(teamCommand.TargetID);
                if (isMultipleTeam)
                {
                    return "無法指定其他車隊隊長.";
                }

                Utility.UpdateListHandler(teamData.TeamPlayerIDs, teamData.TeamLeaderID, true);
                Utility.UpdateListHandler(teamData.TeamPlayerIDs, teamCommand.TargetID, false);
                teamData.TeamLeaderID = teamCommand.TargetID;
                Tuple<bool, string> updateTeamDataResult = await this.teamRepository.UpdateTeamData(teamData);
                if (!updateTeamDataResult.Item1)
                {
                    this.logger.LogError($"Update Team Leader Fail For update Team Data >>> Data:{JsonConvert.SerializeObject(teamData)}");
                    return updateTeamDataResult.Item2;
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
                string verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false);
                if (!string.IsNullOrEmpty(verifyTeamCommandResult))
                {
                    return verifyTeamCommandResult;
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                string verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, true, true, teamCommand.TargetID);
                if (!string.IsNullOrEmpty(verifyTeamExaminerAuthorityResult))
                {
                    return verifyTeamExaminerAuthorityResult;
                }

                if (!teamData.TeamPlayerIDs.Contains(teamCommand.TargetID))
                {
                    return "會員未加入車隊.";
                }

                bool updateTeamViceLeaderIDsResult = Utility.UpdateListHandler(teamData.TeamViceLeaderIDs, teamCommand.TargetID, isAdd);
                if (updateTeamViceLeaderIDsResult)
                {
                    Tuple<bool, string> result = await this.teamRepository.UpdateTeamViceLeaders(teamData.TeamID, teamData.TeamViceLeaderIDs);
                    if (!result.Item1)
                    {
                        this.logger.LogError($"Update Team Vice Leader Fail For Update Team Vice Leaders >>> TeamID:{teamData.TeamID} TeamViceLeaderIDs:{JsonConvert.SerializeObject(teamData.TeamViceLeaderIDs)}");
                        return result.Item2;
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
        /// 創建新車隊資料
        /// </summary>
        /// <param name="teamInfoDto">teamInfoDto</param>
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

            DateTime createDate = DateTime.Now;
            string teamID = $"{Guid.NewGuid().ToString().Substring(0, 6)}-{createDate:yyyy}-{createDate:MMdd}";
            TeamData teamData = this.mapper.Map<TeamData>(teamInfo);
            teamData.TeamID = teamID;
            teamData.TeamCreateDate = createDate;
            //teamData.TeamSaveDeadline = createDate.AddDays(60);
            teamData.TeamSaveDeadline = createDate.AddMinutes(30);
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
        /// 驗證加入車隊資格
        /// </summary>
        /// <param name="teamData">teamData</param>
        /// <param name="memberID">memberID</param>
        /// <returns>string</returns>
        private string VerifyJoinTeamQualification(TeamData teamData, string memberID)
        {
            if (teamData == null)
            {
                return "車隊不存在.";
            }

            if (string.IsNullOrEmpty(memberID))
            {
                return "會員編號無效.";
            }

            if (teamData.TeamLeaderID.Equals(memberID) || teamData.TeamPlayerIDs.Contains(memberID))
            {
                return "會員已加入車隊.";
            }

            if (teamData.TeamBlacklistIDs.Contains(memberID))
            {
                return "會員已被列入黑名單.";
            }

            if (teamData.TeamBlacklistedIDs.Contains(memberID))
            {
                return "車隊已被列入黑名單.";
            }

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
        /// <returns>string</returns>
        private string VerifyTeamCommand(TeamCommandDto teamCommand, bool isVerifyExaminer, bool isVerifyTarget, bool isVerifyTargets, bool isVerifyData)
        {
            if (teamCommand == null)
            {
                return "車隊指令資料不存在.";
            }

            if (string.IsNullOrEmpty(teamCommand.TeamID))
            {
                return "車隊編號無效.";
            }

            if (isVerifyExaminer)
            {
                if (string.IsNullOrEmpty(teamCommand.ExaminerID))
                {
                    return "審查者會員編號無效.";
                }
            }

            if (isVerifyTarget)
            {
                if (string.IsNullOrEmpty(teamCommand.TargetID))
                {
                    return "目標者會員編號無效.";
                }
            }

            if (isVerifyTargets)
            {
                if (teamCommand.TargetIDs == null || teamCommand.TargetIDs.Count() == 0)
                {
                    return "無目標者會員編號列表.";
                }
            }

            if (isVerifyData)
            {
                if (teamCommand.Data == null)
                {
                    return "無車隊資訊.";
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 驗證車隊審查者權限
        /// </summary>
        /// <param name="teamData">teamData</param>
        /// <param name="examinerID">examinerID</param>
        /// <param name="isSupreme">isSupreme</param>
        /// <param name="isVerifyTarget">isVerifyTarget</param>
        /// <param name="targetID">targetID</param>
        /// <returns>string</returns>
        private string VerifyTeamExaminerAuthority(TeamData teamData, string examinerID, bool isSupreme, bool isVerifyTarget, string targetID)
        {
            if (teamData == null)
            {
                return "車隊不存在.";
            }

            if (string.IsNullOrEmpty(examinerID))
            {
                return "審查者會員編號無效.";
            }

            if (isSupreme)
            {
                if (!examinerID.Equals(teamData.TeamLeaderID))
                {
                    return "審查者無最高權限.";
                }
            }

            if (!teamData.TeamLeaderID.Equals(examinerID) && !teamData.TeamViceLeaderIDs.Contains(examinerID))
            {
                return "車隊隊員無審查權限.";
            }

            if (isVerifyTarget)
            {
                if (string.IsNullOrEmpty(targetID))
                {
                    return "目標者會員編號無效.";
                }

                if (examinerID.Equals(targetID))
                {
                    return "無法對本身執行車隊指令.";
                }
                if (targetID.Equals(teamData.TeamLeaderID))
                {
                    return "無法對最高權限的車隊隊長執行動作.";
                }
            }

            return string.Empty;
        }

        /// <summary>
        /// 驗證車隊搜尋指令資料
        /// </summary>
        /// <param name="teamSearchCommand">teamSearchCommand</param>
        /// <returns>string</returns>
        private string VerifyTeamSearchCommand(TeamSearchCommandDto teamSearchCommand)
        {
            if (teamSearchCommand == null)
            {
                return "車隊搜尋指令資料不存在.";
            }

            if (string.IsNullOrEmpty(teamSearchCommand.SearcherID))
            {
                return "搜尋者會員編號無效.";
            }

            if (string.IsNullOrEmpty(teamSearchCommand.SearchKey))
            {
                return "無車隊搜尋關鍵字.";
            }

            return string.Empty;
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
                string verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false);
                if (!string.IsNullOrEmpty(verifyTeamCommandResult))
                {
                    return verifyTeamCommandResult;
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

                string verifyResult = this.VerifyJoinTeamQualification(teamData, teamCommand.TargetID);
                if (!string.IsNullOrEmpty(verifyResult))
                {
                    return verifyResult;
                }

                bool updateTeamPlayerIDsResult = Utility.UpdateListHandler(teamData.TeamApplyForJoinIDs, teamCommand.TargetID, true);
                bool updateTeamInviteJoinIDsResult = Utility.UpdateListHandler(teamData.TeamInviteJoinIDs, teamCommand.TargetID, false);
                if (updateTeamPlayerIDsResult || updateTeamInviteJoinIDsResult)
                {
                    Tuple<bool, string> updateTeamDataResult = await this.teamRepository.UpdateTeamData(teamData);
                    if (!updateTeamDataResult.Item1)
                    {
                        this.logger.LogError($"Apply For Join Team Fail For Update Team Data >>> Data:{JsonConvert.SerializeObject(teamData)}");
                        return updateTeamDataResult.Item2;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Apply For Join Team Error >>> TemaID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
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
                string verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false);
                if (!string.IsNullOrEmpty(verifyTeamCommandResult))
                {
                    return verifyTeamCommandResult;
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool updateTeamApplyForJoinIDsResult = Utility.UpdateListHandler(teamData.TeamApplyForJoinIDs, teamCommand.TargetID, false);
                if (updateTeamApplyForJoinIDsResult)
                {
                    Tuple<bool, string> result = await this.teamRepository.UpdateTeamApplyForJoinIDs(teamData.TeamID, teamData.TeamApplyForJoinIDs);
                    if (!result.Item1)
                    {
                        this.logger.LogError($"Cancel Apply For Join Team Fail For Update Team Invite Join IDs >>> TeamID:{teamData.TeamID} TeamApplyForJoinIDs:{JsonConvert.SerializeObject(teamData.TeamApplyForJoinIDs)}");
                        return result.Item2;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Cancel Apply For Join Team Error >>> TemaID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
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
                string verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false);
                if (!string.IsNullOrEmpty(verifyTeamCommandResult))
                {
                    return verifyTeamCommandResult;
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool updateTeamInviteJoinIDsResult = Utility.UpdateListHandler(teamData.TeamInviteJoinIDs, teamCommand.TargetID, false);
                if (updateTeamInviteJoinIDsResult)
                {
                    Tuple<bool, string> result = await this.teamRepository.UpdateTeamInviteJoinIDs(teamData.TeamID, teamData.TeamInviteJoinIDs);
                    if (!result.Item1)
                    {
                        this.logger.LogError($"Cancel Invite Join Team Fail For Update Team Invite Join IDs >>> TeamID:{teamData.TeamID} TeamInviteJoinIDs:{JsonConvert.SerializeObject(teamData.TeamInviteJoinIDs)}");
                        return result.Item2;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Cancel Invite Join Team Error >>> TemaID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return "取消邀請加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 取得申請請求列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(strings, string)</returns>
        public async Task<Tuple<IEnumerable<string>, string>> GetApplyForRequestList(TeamCommandDto teamCommand)
        {
            try
            {
                string verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, false, false, false);
                if (!string.IsNullOrEmpty(verifyTeamCommandResult))
                {
                    return Tuple.Create<IEnumerable<string>, string>(null, verifyTeamCommandResult);
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                string verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, false, string.Empty);
                if (!string.IsNullOrEmpty(verifyTeamExaminerAuthorityResult))
                {
                    return Tuple.Create<IEnumerable<string>, string>(null, verifyTeamExaminerAuthorityResult);
                }

                return Tuple.Create(teamData.TeamApplyForJoinIDs, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Apply For Request List Error >>> TemaID:{teamCommand.TeamID}\n{ex}");
                return Tuple.Create<IEnumerable<string>, string>(null, "取得申請請求列表發生錯誤.");
            }
        }

        /// <summary>
        /// 取得邀請請求列表
        /// </summary>
        /// <param name="memberCommand">memberCommand</param>
        /// <returns>Tuple(TeamInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<TeamInfoDto>, string>> GetInviteRequestList(MemberCommandDto memberCommand)
        {
            try
            {
                if (memberCommand == null)
                {
                    return Tuple.Create<IEnumerable<TeamInfoDto>, string>(null, "會員指令資料不存在.");
                }

                if (string.IsNullOrEmpty(memberCommand.MemberID))
                {
                    return Tuple.Create<IEnumerable<TeamInfoDto>, string>(null, "會員編號無效.");
                }

                IEnumerable<TeamData> teamDatas = await this.teamRepository.GetTeamDataListOfInviteJoin(memberCommand.MemberID);
                return Tuple.Create(this.mapper.Map<IEnumerable<TeamInfoDto>>(teamDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Invite Request List Error >>> MemberID:{memberCommand.MemberID}\n{ex}");
                return Tuple.Create<IEnumerable<TeamInfoDto>, string>(null, "取得邀請請求列表發生錯誤.");
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
                string verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, false, true, false);
                if (!string.IsNullOrEmpty(verifyTeamCommandResult))
                {
                    return verifyTeamCommandResult;
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                IEnumerable<string> teamApplyForJoinIDs = teamData.TeamApplyForJoinIDs;
                IEnumerable<string> targetIDs = teamCommand.TargetIDs;
                List<string> validTargetIDs = new List<string>();
                foreach (string targetID in targetIDs)
                {
                    string verifyJoinTeamQualificationResult = this.VerifyJoinTeamQualification(teamData, targetID);
                    if (string.IsNullOrEmpty(verifyJoinTeamQualificationResult))
                    {
                        if (!teamApplyForJoinIDs.Contains(targetID))
                        {
                            validTargetIDs.Add(targetID);
                        }
                    }
                }

                if (validTargetIDs.Count == 0)
                {
                    return "無會員名單可邀請加入車隊.";
                }

                bool updateTeamInviteJoinIDsResult = Utility.UpdateListHandler(teamData.TeamInviteJoinIDs, validTargetIDs, true);
                if (updateTeamInviteJoinIDsResult)
                {
                    Tuple<bool, string> result = await this.teamRepository.UpdateTeamInviteJoinIDs(teamData.TeamID, teamData.TeamInviteJoinIDs);
                    if (!result.Item1)
                    {
                        this.logger.LogError($"Invite Join Team Fail For Update Team Invite Join IDs >>> TeamID:{teamData.TeamID} TeamInviteJoinIDs:{JsonConvert.SerializeObject(teamData.TeamInviteJoinIDs)}");
                        return result.Item2;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Invite Join Team Error >>> TemaID:{teamCommand.TeamID} TargetIDs:{JsonConvert.SerializeObject(teamCommand.TargetIDs)}\n{ex}");
                return "邀請加入車隊發生錯誤.";
            }
        }

        #endregion 車隊互動資料
    }
}