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
                    return "無效的車隊編號.";
                }

                if (string.IsNullOrEmpty(teamDto.ExecutorID))
                {
                    return "無效的執行者編號.";
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
        /// 車隊編輯
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>string</returns>
        public async Task<string> EditData(TeamDto teamDto)
        {
            try
            {
                if (string.IsNullOrEmpty(teamDto.TeamID))
                {
                    return "無效的車隊編號.";
                }

                if (string.IsNullOrEmpty(teamDto.ExecutorID))
                {
                    return "無效的執行者編號.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamDto.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                if (!teamData.TeamLeaderID.Equals(teamDto.ExecutorID) && !teamData.TeamViceLeaderIDs.Contains(teamDto.ExecutorID))
                {
                    return "一般車隊隊員無法編輯車隊資料.";
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
                return "創建人會員編號無效.";
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

            if (teamDto.CityID == (int)TeamCityID.None)
            {
                return "車隊所在地無效.";
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
    }
}