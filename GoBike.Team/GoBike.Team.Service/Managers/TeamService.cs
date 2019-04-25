using AutoMapper;
using GoBike.Team.Repository.Interface;
using GoBike.Team.Repository.Models;
using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models;
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
        /// <param name="memberRepository">memberRepository</param>
        /// <param name="eventRepository">eventRepository</param>
        public TeamService(ILogger<TeamService> logger, IMapper mapper, ITeamRepository teamRepository, IMemberRepository memberRepository, IEventRepository eventRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.teamRepository = teamRepository;
            this.memberRepository = memberRepository;
            this.eventRepository = eventRepository;
        }

        #region 車隊資料

        /// <summary>
        /// 車隊編輯
        /// </summary>
        /// <param name="teamInfo">teamInfo</param>
        /// <returns>Tuple(TeamInfoDto, string)</returns>
        public async Task<Tuple<TeamInfoDto, string>> EditData(TeamInfoDto teamInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(teamInfo.TeamID))
                {
                    return Tuple.Create<TeamInfoDto, string>(null, "車隊編號無效.");
                }

                TeamData teamData = await this.teamRepository.GetTeamDataByTeamID(teamInfo.TeamID);
                if (teamData == null)
                {
                    return Tuple.Create<TeamInfoDto, string>(null, "車隊不存在.");
                }

                this.UpdateTeamDataHandler(teamInfo, ref teamData);
                Tuple<bool, string> result = await this.teamRepository.UpdateTeamData(teamData);
                if (!result.Item1)
                {
                    this.logger.LogError($"Edit Data Fail >>> Data:{JsonConvert.SerializeObject(teamData)}");
                    return Tuple.Create<TeamInfoDto, string>(null, result.Item2);
                }

                return Tuple.Create(this.mapper.Map<TeamInfoDto>(teamData), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Data Error >>> Data:{JsonConvert.SerializeObject(teamInfo)}\n{ex}");
                return Tuple.Create<TeamInfoDto, string>(null, "車隊編輯發生錯誤.");
            }
        }

        /// <summary>
        /// 取得我的車隊資訊列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>Tuple(TeamInfoDto, TeamInfoDtos, string)</returns>
        public async Task<Tuple<TeamInfoDto, IEnumerable<TeamInfoDto>, string>> GetMyTeamInfoList(string memberID)
        {
            try
            {
                if (string.IsNullOrEmpty(memberID))
                {
                    return Tuple.Create<TeamInfoDto, IEnumerable<TeamInfoDto>, string>(null, null, "會員編號無效.");
                }

                IEnumerable<TeamData> teamDatas = await this.teamRepository.GetTeamDataListOfMember(memberID);
                TeamData creatorTeamData = teamDatas.Where(x => x.TeamCreatorID.Equals(memberID)).FirstOrDefault();
                IEnumerable<TeamData> joinTeamDatas = teamDatas.SkipWhile(x => x.TeamCreatorID.Equals(memberID));
                return Tuple.Create(this.mapper.Map<TeamInfoDto>(creatorTeamData), this.mapper.Map<IEnumerable<TeamInfoDto>>(joinTeamDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get My Team Info List Error >>> MemberID:{memberID}\n{ex}");
                return Tuple.Create<TeamInfoDto, IEnumerable<TeamInfoDto>, string>(null, null, "取得我的車隊資訊列表發生錯誤.");
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
                if (string.IsNullOrEmpty(teamInfo.TeamName))
                {
                    return "車隊名稱無效.";
                }

                if (string.IsNullOrEmpty(teamInfo.TeamLocation))
                {
                    return "車隊所在地無效.";
                }

                if (string.IsNullOrEmpty(teamInfo.TeamInfo))
                {
                    return "車隊簡介無效.";
                }

                if (string.IsNullOrEmpty(teamInfo.TeamPhoto))
                {
                    return "未上傳車隊頭像.";
                }

                if (string.IsNullOrEmpty(teamInfo.TeamCoverPhoto))
                {
                    return "未上傳車隊封面.";
                }

                if (string.IsNullOrEmpty(teamInfo.TeamCreatorID))
                {
                    return "創建人會員編號無效.";
                }

                MemberData memberData = await this.memberRepository.GetMemebrData(teamInfo.TeamCreatorID);
                if (memberData == null)
                {
                    return "創建人不存在.";
                }

                bool isMultipleTeam = await this.teamRepository.GetTeamDataByTeamCreatorID(teamInfo.TeamCreatorID) != null;
                if (isMultipleTeam)
                {
                    return "無法創建多個車隊.";
                }

                TeamData teamData = this.CreateTeamData(teamInfo);
                bool isSuccess = await this.teamRepository.CreateTeamData(teamData);
                if (!isSuccess)
                {
                    this.logger.LogError($"Register Fail >>> Data:{JsonConvert.SerializeObject(teamData)}");
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
        /// 創建新車隊資料
        /// </summary>
        /// <param name="teamInfoDto">teamInfoDto</param>
        /// <returns>MemberData</returns>
        private TeamData CreateTeamData(TeamInfoDto teamInfo)
        {
            DateTime createDate = DateTime.Now;
            string teamID = $"{Guid.NewGuid().ToString().Substring(0, 6)}-{createDate:yyyy}-{createDate:MMdd}";
            TeamData teamData = this.mapper.Map<TeamData>(teamInfo);
            teamData.TeamID = teamID;
            teamData.TeamCreateDate = createDate;
            //teamData.TeamSaveDeadline = createDate.AddDays(60);
            teamData.TeamSaveDeadline = createDate.AddMinutes(30);
            teamData.TeamPlayerIDs = new List<string>();
            teamData.TeamBlacklistIDs = new List<string>();
            teamData.TeamBlacklistedIDs = new List<string>();
            teamData.TeamEventIDs = new List<string>();
            return teamData;
        }

        /// <summary>
        /// 車隊資料更新處理
        /// </summary>
        /// <param name="teamInfo">teamInfo</param>
        /// <param name="teamData">teamData</param>
        private void UpdateTeamDataHandler(TeamInfoDto teamInfo, ref TeamData teamData)
        {
            if (!string.IsNullOrEmpty(teamInfo.TeamName))
                teamData.TeamName = teamInfo.TeamName;

            if (!string.IsNullOrEmpty(teamInfo.TeamLocation))
                teamData.TeamLocation = teamInfo.TeamLocation;

            if (!string.IsNullOrEmpty(teamInfo.TeamInfo))
                teamData.TeamInfo = teamInfo.TeamInfo;

            if (!string.IsNullOrEmpty(teamInfo.TeamPhoto))
                teamData.TeamPhoto = teamInfo.TeamPhoto;

            if (!string.IsNullOrEmpty(teamInfo.TeamCoverPhoto))
                teamData.TeamCoverPhoto = teamInfo.TeamCoverPhoto;

            teamData.TeamSearchStatus = teamInfo.TeamSearchStatus;
            teamData.TeamExamineStatus = teamInfo.TeamExamineStatus;
        }

        #endregion 車隊資料

        #region 車隊互動資料

        /// <summary>
        /// 申請加入車隊
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        public async Task<string> ApplyForJoinTeam(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                return await this.CreateInteractiveData(interactiveInfo.TeamID, interactiveInfo.MemberID, (int)InteractiveStatusType.ApplyForJoin);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Apply For Join Team Error >>> TemaID:{interactiveInfo.TeamID} MemberID:{interactiveInfo.MemberID}\n{ex}");
                return "申請加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 強制離開車隊
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        public async Task<string> ForceLeaveTeam(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(interactiveInfo.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(interactiveInfo.MemberID))
                {
                    return "會員編號無效.";
                }

                if (string.IsNullOrEmpty(interactiveInfo.ActionID))
                {
                    return "執行者編號無效.";
                }

                TeamData teamData = await this.teamRepository.GetTeamDataByTeamID(interactiveInfo.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (interactiveInfo.MemberID.Equals(teamData.TeamCreatorID))
                {
                    return "車隊隊長無法被強制離開車隊.";
                }

                if (!teamData.TeamPlayerIDs.Contains(interactiveInfo.MemberID))
                {
                    return "會員未加入車隊.";
                }

                if (interactiveInfo.MemberID.Equals(teamData.TeamViceLeaderID))
                {
                    if (!interactiveInfo.ActionID.Equals(teamData.TeamCreatorID))
                    {
                        return "車隊副隊長須由車隊隊長強制離開車隊.";
                    }

                    teamData.TeamViceLeaderID = null;
                }

                if (!interactiveInfo.ActionID.Equals(teamData.TeamCreatorID) && !interactiveInfo.ActionID.Equals(teamData.TeamViceLeaderID))
                {
                    return "車隊隊員須由車隊隊長或副隊長強制離開車隊.";
                }

                (teamData.TeamPlayerIDs as List<string>).Remove(interactiveInfo.MemberID);
                Tuple<bool, string> updateTeamResult = await this.teamRepository.UpdateTeamData(teamData);
                if (!updateTeamResult.Item1)
                {
                    this.logger.LogError($"Force Leave Team Fail >>> Data:{JsonConvert.SerializeObject(teamData)}");
                    return updateTeamResult.Item2;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Force Leave Team Error >>> TemaID:{interactiveInfo.TeamID} MemberID:{interactiveInfo.MemberID}\n{ex}");
                return "強制離開車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 取得申請請求列表
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>Tuple(MemberInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<MemberInfoDto>, string>> GetApplyForRequestList(string teamID)
        {
            try
            {
                if (string.IsNullOrEmpty(teamID))
                {
                    return Tuple.Create<IEnumerable<MemberInfoDto>, string>(null, "車隊編號無效.");
                }

                IEnumerable<InteractiveData> interactiveDatas = await this.teamRepository.GetTeamInteractiveDataListForApplyForJoin(teamID);
                IEnumerable<string> memberIDs = interactiveDatas.Select(x => x.MemberID);
                IEnumerable<MemberData> memberDatas = await this.memberRepository.GetMemebrDataList(memberIDs);
                return Tuple.Create(this.mapper.Map<IEnumerable<MemberInfoDto>>(memberDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Apply For Request List Error >>> TemaID:{teamID}\n{ex}");
                return Tuple.Create<IEnumerable<MemberInfoDto>, string>(null, "取得申請請求列表發生錯誤.");
            }
        }

        /// <summary>
        /// 取得邀請請求列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>Tuple(TeamInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<TeamInfoDto>, string>> GetInviteRequestList(string memberID)
        {
            try
            {
                if (string.IsNullOrEmpty(memberID))
                {
                    return Tuple.Create<IEnumerable<TeamInfoDto>, string>(null, "會員編號無效.");
                }

                IEnumerable<InteractiveData> interactiveDatas = await this.teamRepository.GetTeamInteractiveDataListForInviteJoin(memberID);
                IEnumerable<string> teamIDs = interactiveDatas.Select(x => x.TeamID);
                IEnumerable<TeamData> teamDatas = await this.teamRepository.GetTeamDataList(teamIDs);
                return Tuple.Create(this.mapper.Map<IEnumerable<TeamInfoDto>>(teamDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Invite Request List Error >>> MemberID:{memberID}\n{ex}");
                return Tuple.Create<IEnumerable<TeamInfoDto>, string>(null, "取得邀請請求列表發生錯誤.");
            }
        }

        /// <summary>
        /// 邀請加入車隊
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        public async Task<string> InviteJoinTeam(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                return await this.CreateInteractiveData(interactiveInfo.TeamID, interactiveInfo.MemberID, (int)InteractiveStatusType.InviteJoin);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Invite Join Team Error >>> TemaID:{interactiveInfo.TeamID} MemberID:{interactiveInfo.MemberID}\n{ex}");
                return "邀請加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 加入車隊
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        public async Task<string> JoinTeam(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                string verifyResult = await this.VerifyJoinTeamQualification(interactiveInfo.TeamID, interactiveInfo.MemberID);
                if (!string.IsNullOrEmpty(verifyResult))
                {
                    return verifyResult;
                }

                InteractiveData interactiveData = await this.teamRepository.GetTeamInteractiveData(interactiveInfo.TeamID, interactiveInfo.MemberID);
                if (interactiveData == null)
                {
                    return "無車隊互動資料.";
                }

                TeamData teamData = await this.teamRepository.GetTeamDataByTeamID(interactiveInfo.TeamID);
                (teamData.TeamPlayerIDs as List<string>).Add(interactiveInfo.MemberID);
                Tuple<bool, string> UpdateTeamResult = await this.teamRepository.UpdateTeamData(teamData);
                if (!UpdateTeamResult.Item1)
                {
                    this.logger.LogError($"Join Team Fail >>> Data:{JsonConvert.SerializeObject(teamData)}");
                    return UpdateTeamResult.Item2;
                }

                //// 刪除車隊互動資料，由於時間到也會自動刪除，所以就不用等刪除動作結束
                this.teamRepository.DeleteTeamInteractiveData(interactiveData.TeamID, interactiveData.MemberID);
                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Join Team Error >>> TemaID:{interactiveInfo.TeamID} MemberID:{interactiveInfo.MemberID}\n{ex}");
                return "加入車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 離開車隊
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        public async Task<string> LeaveTeam(InteractiveInfoDto interactiveInfo)
        {
            try
            {
                if (string.IsNullOrEmpty(interactiveInfo.TeamID))
                {
                    return "車隊編號無效.";
                }

                if (string.IsNullOrEmpty(interactiveInfo.MemberID))
                {
                    return "會員編號無效.";
                }

                TeamData teamData = await this.teamRepository.GetTeamDataByTeamID(interactiveInfo.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                //// TODO 確認隊長離開後，是刪除車隊或是將車隊移交給副隊長 (目前已刪除為主，待確認)
                if (teamData.TeamCreatorID.Equals(interactiveInfo.MemberID))
                {
                    bool isDeleteSuccess = await this.teamRepository.DeleteTeamData(interactiveInfo.TeamID);
                    if (!isDeleteSuccess)
                    {
                        this.logger.LogError($"Leave Team Fail For Delete Team Data >>> Data:{JsonConvert.SerializeObject(teamData)}");
                        return "離開車隊失敗.";
                    }

                    return string.Empty;
                }
                else
                {
                    if (!teamData.TeamPlayerIDs.Contains(interactiveInfo.MemberID))
                    {
                        return "會員未加入車隊.";
                    }
                }

                (teamData.TeamPlayerIDs as List<string>).Remove(interactiveInfo.MemberID);
                Tuple<bool, string> updateTeamResult = await this.teamRepository.UpdateTeamData(teamData);
                if (!updateTeamResult.Item1)
                {
                    this.logger.LogError($"Leave Team Fail For Update Team Data >>> Data:{JsonConvert.SerializeObject(teamData)}");
                    return updateTeamResult.Item2;
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Leave Team Error >>> TemaID:{interactiveInfo.TeamID} MemberID:{interactiveInfo.MemberID}\n{ex}");
                return "離開車隊發生錯誤.";
            }
        }

        /// <summary>
        /// 建立互動資料
        /// </summary>
        /// <param name="temaID">temaID</param>
        /// <param name="memberID">memberID</param>
        /// <param name="status">status</param>
        /// <returns>string</returns>
        private async Task<string> CreateInteractiveData(string teamID, string memberID, int status)
        {
            string verifyResult = await this.VerifyJoinTeamQualification(teamID, memberID);
            if (!string.IsNullOrEmpty(verifyResult))
            {
                return verifyResult;
            }

            InteractiveData interactiveData = await this.teamRepository.GetTeamInteractiveData(teamID, memberID);
            if (interactiveData != null)
            {
                return "車隊互動資料已存在.";
            }

            interactiveData = new InteractiveData()
            {
                MemberID = memberID,
                TeamID = teamID,
                //SaveDeadline = DateTime.Now.AddDays(10),
                SaveDeadline = DateTime.Now.AddMinutes(10),
                Status = status
            };

            bool isSuccess = await this.teamRepository.CreateTeamInteractiveData(interactiveData);
            if (!isSuccess)
            {
                this.logger.LogError($"Create Interactive Data Fail>>> TeamID:{teamID} MemberID:{memberID} Status:{status} Data:{JsonConvert.SerializeObject(interactiveData)}");
                return "建立車隊互動資料失敗.";
            }

            return string.Empty;
        }

        /// <summary>
        /// 驗證加入車隊資格
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="memberID">memberID</param>
        /// <returns>string</returns>
        private async Task<string> VerifyJoinTeamQualification(string teamID, string memberID)
        {
            if (string.IsNullOrEmpty(teamID))
            {
                return "車隊編號無效.";
            }

            if (string.IsNullOrEmpty(memberID))
            {
                return "會員編號無效.";
            }

            TeamData teamData = await this.teamRepository.GetTeamDataByTeamID(teamID);
            if (teamData == null)
            {
                return "車隊不存在.";
            }

            MemberData memberData = await this.memberRepository.GetMemebrData(memberID);
            if (memberData == null)
            {
                return "會員不存在.";
            }

            if (teamData.TeamCreatorID.Equals(memberID) || teamData.TeamPlayerIDs.Contains(memberID))
            {
                return "會員已加入車隊.";
            }

            if (teamData.TeamBlacklistIDs != null && teamData.TeamBlacklistIDs.Contains(memberID))
            {
                return "會員已被列入黑名單.";
            }

            if (teamData.TeamBlacklistedIDs != null && teamData.TeamBlacklistedIDs.Contains(memberID))
            {
                return "車隊已被列入黑名單.";
            }

            return string.Empty;
        }

        #endregion 車隊互動資料
    }
}