using AutoMapper;
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
    public class TeamService : TeamCommonService, ITeamService
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
        public TeamService(ILogger<TeamService> logger, IMapper mapper, ITeamRepository teamRepository, IEventRepository eventRepository) : base(logger)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.teamRepository = teamRepository;
            this.eventRepository = eventRepository;
        }

        /// <summary>
        /// 解散車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> DisbandTeam(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, false, false, false, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Disband Team Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID}");
                    return "解散車隊失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, true, false, string.Empty);
                if (!verifyTeamExaminerAuthorityResult)
                {
                    this.logger.LogError($"Disband Team Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID}  ExaminerID:{teamCommand.ExaminerID}");
                    return "解散車隊失敗.";
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
                this.logger.LogError($"Disband Team Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID}\n{ex}");
                return "解散車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 車隊編輯
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(TeamInfoDto, string)</returns>
        public async Task<Tuple<TeamInfoDto, string>> EditData(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, false, false, true, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Edit Data Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TeamInfo:{JsonConvert.SerializeObject(teamCommand.TeamInfo)}");
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

                string updateTeamDataHandlerReault = await this.UpdateTeamDataHandler(teamCommand.TeamInfo, teamData);
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
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false, false, false);
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
            teamData.TeamID = this.GetSerialID(createDate);
            teamData.TeamCreateDate = createDate;
            teamData.TeamSaveDeadline = createDate.AddDays(60);
            teamData.TeamViceLeaderIDs = new List<string>();
            teamData.TeamPlayerIDs = new List<string>();
            teamData.TeamApplyForJoinIDs = new List<string>();
            teamData.TeamInviteJoinIDs = new List<string>();
            teamData.TeamBlacklistIDs = new List<string>();
            teamData.TeamBlacklistedIDs = new List<string>();
            teamData.TeamEventIDs = new List<string>();
            teamData.HaveSeenAnnouncementPlayerIDs = new List<string>();
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
    }
}