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
    public class AnnouncementService : TeamCommonService, IAnnouncementService
    {
        /// <summary>
        /// announcementRepository
        /// </summary>
        private readonly IAnnouncementRepository announcementRepository;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<AnnouncementService> logger;

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
        /// <param name="announcementRepository">announcementRepository</param>
        public AnnouncementService(ILogger<AnnouncementService> logger, IMapper mapper, ITeamRepository teamRepository, IAnnouncementRepository announcementRepository) : base(logger)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.teamRepository = teamRepository;
            this.announcementRepository = announcementRepository;
        }

        /// <summary>
        /// 刪除公告
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> DeleteAnnouncement(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, false, false, false, true);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Delete Announcement Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} AnnouncementID:{(teamCommand.AnnouncementInfo != null ? teamCommand.AnnouncementInfo.AnnouncementID : "Null")}");
                    return "刪除公告失敗.";
                }

                if (string.IsNullOrEmpty(teamCommand.AnnouncementInfo.AnnouncementID))
                {
                    return "公告編號無效.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, false, string.Empty);
                if (!verifyTeamExaminerAuthorityResult)
                {
                    this.logger.LogError($"Delete Announcement Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} AnnouncementID:{teamCommand.AnnouncementInfo.AnnouncementID}");
                    return "刪除公告失敗.";
                }

                bool deleteAnnouncementDataResult = await this.announcementRepository.DeleteAnnouncementData(teamCommand.AnnouncementInfo.AnnouncementID);
                if (!deleteAnnouncementDataResult)
                {
                    return "刪除公告失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Announcement Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} AnnouncementID:{(teamCommand.AnnouncementInfo != null ? teamCommand.AnnouncementInfo.AnnouncementID : "Null")}\n{ex}");
                return "刪除公告發生錯誤.";
            }
        }

        /// <summary>
        /// 刪除車隊所有公告
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> DeleteAnnouncementListOfTeam(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, false, false, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Delete Announcement List Of Team Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID}");
                    return "刪除車隊所有公告失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, true, false, string.Empty);
                if (!verifyTeamExaminerAuthorityResult)
                {
                    this.logger.LogError($"Delete Announcement List Of Team Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID}");
                    return "刪除車隊所有公告失敗.";
                }

                bool deleteAnnouncementDataListOfTeamResult = await this.announcementRepository.DeleteAnnouncementDataListOfTeam(teamCommand.TeamID);
                if (!deleteAnnouncementDataListOfTeamResult)
                {
                    return "刪除車隊所有公告失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Announcement List Of Team Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID}\n{ex}");
                return "刪除車隊所有公告發生錯誤.";
            }
        }

        /// <summary>
        /// 編輯公告
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> EditAnnouncement(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, false, false, false, true);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Edit Announcement Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} AnnouncementInfo:{JsonConvert.SerializeObject(teamCommand.AnnouncementInfo)}");
                    return "編輯公告失敗.";
                }

                if (string.IsNullOrEmpty(teamCommand.AnnouncementInfo.AnnouncementID))
                {
                    return "公告編號無效.";
                }

                AnnouncementData announcementData = await this.announcementRepository.GetAnnouncementData(teamCommand.AnnouncementInfo.AnnouncementID);
                if (announcementData == null)
                {
                    return "公告不存在.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, false, string.Empty);
                if (!verifyTeamExaminerAuthorityResult)
                {
                    this.logger.LogError($"Edit Announcement Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} AnnouncementInfo:{JsonConvert.SerializeObject(teamCommand.AnnouncementInfo)}");
                    return "編輯公告失敗.";
                }

                string updateTeamDataHandlerReault = this.UpdateTeamDataHandler(teamCommand.AnnouncementInfo, announcementData);
                if (!string.IsNullOrEmpty(updateTeamDataHandlerReault))
                {
                    return updateTeamDataHandlerReault;
                }

                bool updateAnnouncementDataResult = await this.announcementRepository.UpdateAnnouncementData(announcementData);
                if (!updateAnnouncementDataResult)
                {
                    return "公告資料更新失敗.";
                }

                //// 無須對【已閱公告名單資料】作審查，不應影響原功能
                bool updateHaveSeenAnnouncementPlayerIDsResult = await this.teamRepository.UpdateHaveSeenAnnouncementPlayerIDs(teamData.TeamID, new List<string>());
                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Announcement Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} AnnouncementInfo:{JsonConvert.SerializeObject(teamCommand.AnnouncementInfo)}\n{ex}");
                return "編輯公告發生錯誤.";
            }
        }

        /// <summary>
        /// 取得公告列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(AnnouncementInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<AnnouncementInfoDto>, string>> GetAnnouncementList(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Get Announcement List Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}");
                    return Tuple.Create<IEnumerable<AnnouncementInfoDto>, string>(null, "取得公告列表失敗.");
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return Tuple.Create<IEnumerable<AnnouncementInfoDto>, string>(null, "車隊不存在.");
                }

                if (!teamData.TeamLeaderID.Equals(teamCommand.TargetID) && !teamData.TeamPlayerIDs.Contains(teamCommand.TargetID))
                {
                    return Tuple.Create<IEnumerable<AnnouncementInfoDto>, string>(null, "該會員非車隊隊員.");
                }

                IEnumerable<AnnouncementData> announcementDatas = await this.announcementRepository.GetAnnouncementDataListOfTeam(teamData.TeamID);
                bool updateHaveSeenAnnouncementPlayerIDsResult = Utility.UpdateListHandler(teamData.HaveSeenAnnouncementPlayerIDs, teamCommand.TargetID, true);
                if (updateHaveSeenAnnouncementPlayerIDsResult)
                {
                    //// 無須對【已閱公告名單資料】作審查，不應影響原功能
                    bool result = await this.teamRepository.UpdateHaveSeenAnnouncementPlayerIDs(teamData.TeamID, teamData.HaveSeenAnnouncementPlayerIDs);
                }

                return Tuple.Create(this.mapper.Map<IEnumerable<AnnouncementInfoDto>>(announcementDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Announcement List Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return Tuple.Create<IEnumerable<AnnouncementInfoDto>, string>(null, "取得公告列表發生錯誤.");
            }
        }

        /// <summary>
        /// 發佈公告
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> PublishAnnouncement(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, true, false, false, false, true);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Publish Announcement Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} AnnouncementInfo:{JsonConvert.SerializeObject(teamCommand.AnnouncementInfo)}");
                    return "發佈公告失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.ExaminerID, false, false, string.Empty);
                if (!verifyTeamExaminerAuthorityResult)
                {
                    this.logger.LogError($"Publish Announcement Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} TargetID:{teamCommand.TargetID}");
                    return "發佈公告失敗.";
                }

                Tuple<AnnouncementData, string> createAnnouncementDataResult = this.CreateAnnouncementData(teamCommand.TeamID, teamCommand.ExaminerID, teamCommand.AnnouncementInfo);
                if (!string.IsNullOrEmpty(createAnnouncementDataResult.Item2))
                {
                    return createAnnouncementDataResult.Item2;
                }

                AnnouncementData announcementData = createAnnouncementDataResult.Item1;
                bool isSuccess = await this.announcementRepository.CreateAnnouncementData(announcementData);
                if (!isSuccess)
                {
                    return "發佈公告失敗.";
                }

                //// 無須對【已閱公告名單資料】作審查，不應影響原功能
                bool updateHaveSeenAnnouncementPlayerIDsResult = await this.teamRepository.UpdateHaveSeenAnnouncementPlayerIDs(teamData.TeamID, new List<string>());
                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Publish Announcement Error >>> TeamID:{teamCommand.TeamID} ExaminerID:{teamCommand.ExaminerID} AnnouncementInfo:{JsonConvert.SerializeObject(teamCommand.AnnouncementInfo)}\n{ex}");
                return "發佈公告發生錯誤.";
            }
        }

        /// <summary>
        /// 創建新公告資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="publisherID">publisherID</param>
        /// <param name="announcementInfo">announcementInfo</param>
        /// <returns>Tuple(AnnouncementData, string)</returns>
        private Tuple<AnnouncementData, string> CreateAnnouncementData(string teamID, string publisherID, AnnouncementInfoDto announcementInfo)
        {
            if (string.IsNullOrEmpty(announcementInfo.Context))
            {
                return Tuple.Create<AnnouncementData, string>(null, "無公告內容.");
            }

            if (announcementInfo.LimitDate == 0)
            {
                return Tuple.Create<AnnouncementData, string>(null, "公告天數無效.");
            }

            DateTime createDate = DateTime.Now;
            AnnouncementData announcementData = this.mapper.Map<AnnouncementData>(announcementInfo);
            announcementData.AnnouncementID = this.GetSerialID(createDate);
            announcementData.TeamID = teamID;
            announcementData.MemberID = publisherID;
            announcementData.CreateDate = createDate;
            announcementData.SaveDeadline = createDate.AddDays(announcementInfo.LimitDate); //// TODO 公告存在天數更新
            announcementData.HaveSeenPlayerIDs = new List<string>();
            return Tuple.Create<AnnouncementData, string>(announcementData, string.Empty);
        }

        /// <summary>
        /// 公告資料更新處理
        /// </summary>
        /// <param name="announcementInfo">announcementInfo</param>
        /// <param name="announcementData">announcementData</param>
        /// <returns>string</returns>
        private string UpdateTeamDataHandler(AnnouncementInfoDto announcementInfo, AnnouncementData announcementData)
        {
            if (string.IsNullOrEmpty(announcementInfo.Context))
                return "無公告內容.";

            if (announcementInfo.LimitDate == 0)
                return "公告天數無效.";

            announcementData.Context = announcementInfo.Context;
            announcementData.LimitDate = announcementInfo.LimitDate;
            (announcementData.HaveSeenPlayerIDs as List<string>).Clear();
            //// TODO 公告存在天數更新
            return string.Empty;
        }
    }
}