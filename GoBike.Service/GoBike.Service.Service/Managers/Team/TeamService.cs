﻿using AutoMapper;
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
        /// 編輯車隊
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
                this.logger.LogError($"Edit Data Error >>> Data:{JsonConvert.SerializeObject(teamDto)}\n{ex}");
                return "車隊編輯發生錯誤.";
            }
        }

        /// <summary>
        /// 取得附近車隊資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<TeamDto>, string>> GetNearbyTeamDataList(TeamDto teamDto)
        {
            try
            {
                if (teamDto.CityID == (int)CityType.None)
                {
                    return Tuple.Create<IEnumerable<TeamDto>, string>(null, "市區編號無效.");
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
        /// <returns>Tuple(TeamInfoDtos, string)</returns>
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
        /// 取得會員的車隊列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<TeamDto>, string>> GetTeamDataListOfMember(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.ExecutorID))
                {
                    return Tuple.Create<IEnumerable<TeamDto>, string>(null, "會員編號無效.");
                }

                IEnumerable<TeamData> teamDatas = await this.teamRepository.GetTeamDataListOfMember(teamDto.ExecutorID);
                return Tuple.Create(this.mapper.Map<IEnumerable<TeamDto>>(teamDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data List Of Member Error >>> ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return Tuple.Create<IEnumerable<TeamDto>, string>(null, "取得會員的車隊列表發生錯誤.");
            }
        }

        /// <summary>
        /// 搜尋車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>Tuple(TeamInfoDtos, string)</returns>
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
                TeamMemberIDs = new List<string>(),
                TeamApplyForJoinIDs = new List<string>(),
                TeamInviteJoinIDs = new List<string>(),
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

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (teamData.ExamineStatus == (int)TeamExamineStatusType.Close)
                {
                    return await this.JoinTeam(teamDto, false);
                }

                if (teamData.TeamLeaderID.Equals(teamDto.TargetID) || teamData.TeamMemberIDs.Contains(teamDto.TargetID))
                {
                    return "會員已加入車隊.";
                }

                bool updateTeamPlayerIDsResult = Utility.UpdateListHandler(teamData.TeamApplyForJoinIDs, teamDto.TargetID, true);
                bool updateTeamInviteJoinIDsResult = Utility.UpdateListHandler(teamData.TeamInviteJoinIDs, teamDto.TargetID, false);
                if (updateTeamPlayerIDsResult || updateTeamInviteJoinIDsResult)
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
                this.logger.LogError($"Apply For Join Team Error >>> TeamID:{teamDto.TeamID} TargetID:{teamDto.TargetID}\n{ex}");
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

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool updateTeamApplyForJoinIDsResult = Utility.UpdateListHandler(teamData.TeamApplyForJoinIDs, teamDto.ExecutorID, false);
                if (updateTeamApplyForJoinIDsResult)
                {
                    bool result = await this.teamRepository.UpdateTeamApplyForJoinIDs(teamData.TeamID, teamData.TeamApplyForJoinIDs);
                    if (!result)
                    {
                        return "車隊資料更新失敗.";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Cancel Apply For Join Team Error >>> TeamID:{teamDto.TeamID} TargetID:{teamDto.TargetID}\n{ex}");
                return "取消申請加入車隊發生錯誤.";
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

                string verifyJoinTeamResult = this.VerifyJoinTeam(teamData, teamDto.ExaminerID, teamDto.TargetID, isInvite);
                if (!string.IsNullOrEmpty(verifyJoinTeamResult))
                {
                    return verifyJoinTeamResult;
                }

                bool updateTeamPlayerIDsResult = Utility.UpdateListHandler(teamData.TeamMemberIDs, teamDto.TargetID, true);
                bool updateTeamInviteJoinIDsResult = Utility.UpdateListHandler(teamData.TeamInviteJoinIDs, teamDto.TargetID, false);
                bool updateTeamApplyForJoinIDsResult = Utility.UpdateListHandler(teamData.TeamApplyForJoinIDs, teamDto.TargetID, false);
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
                this.logger.LogError($"Join Team Error >>> TeamID:{teamDto.TeamID} ExaminerID:{teamDto.ExaminerID} TargetID:{teamDto.TargetID} IsInvite:{isInvite}\n{ex}");
                return "加入車隊發生錯誤.";
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
        private string VerifyJoinTeam(TeamData teamData, string examinerID, string targetID, bool isInvite)
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

                if (!teamData.TeamInviteJoinIDs.Contains(targetID))
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

                    if (!teamData.TeamApplyForJoinIDs.Contains(targetID))
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