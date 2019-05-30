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
    /// 互動服務
    /// </summary>
    public class InteractiveService : TeamCommonService, IInteractiveService
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<InteractiveService> logger;

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
        public InteractiveService(ILogger<InteractiveService> logger, IMapper mapper, ITeamRepository teamRepository) : base(logger)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.teamRepository = teamRepository;
        }

        /// <summary>
        /// 申請加入車隊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> ApplyForJoinTeam(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false, false);
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
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false, false);
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
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false, false);
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
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false, false);
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
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, false, false, false, false);
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
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false, false);
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
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, false, true, false, false);
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
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, !isInvite, true, false, false, false);
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
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false, false);
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
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false, false);
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
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false, false);
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
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false, false);
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
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, true, false, false, false);
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
    }
}