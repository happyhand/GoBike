using AutoMapper;
using GoBike.Team.Core.Applibs;
using GoBike.Team.Repository.Interface;
using GoBike.Team.Repository.Models;
using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Models.Command;
using GoBike.Team.Service.Models.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.Service.Managers
{
    /// <summary>
    /// 活動服務
    /// </summary>
    public class EventService : TeamCommonService, IEventService
    {
        /// <summary>
        /// eventRepository
        /// </summary>
        private readonly IEventRepository eventRepository;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<EventService> logger;

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
        public EventService(ILogger<EventService> logger, IMapper mapper, ITeamRepository teamRepository, IEventRepository eventRepository) : base(logger)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.teamRepository = teamRepository;
            this.eventRepository = eventRepository;
        }

        /// <summary>
        /// 取消加入活動
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> CancelJoinEvent(TeamCommandDto teamCommand)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 建立活動
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> CreateEvent(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false, false, true);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Create Event Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventInfo:{JsonConvert.SerializeObject(teamCommand.EventInfo)}");
                    return "建立活動失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                Tuple<EventData, string> createEventDataResult = this.CreateEventData(teamCommand.TeamID, teamCommand.TargetID, teamCommand.EventInfo);
                if (!string.IsNullOrEmpty(createEventDataResult.Item2))
                {
                    return createEventDataResult.Item2;
                }

                EventData eventData = createEventDataResult.Item1;
                bool isSuccess = await this.eventRepository.CreateEventData(eventData);
                if (!isSuccess)
                {
                    return "建立活動失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Event Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventInfo:{JsonConvert.SerializeObject(teamCommand.EventInfo)}\n{ex}");
                return "建立活動發生錯誤.";
            }
        }

        /// <summary>
        /// 刪除活動
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> DeleteEvent(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false, false, true);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Delete Event Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventID:{(teamCommand.EventInfo != null ? teamCommand.EventInfo.EventID : "Null")}");
                    return "刪除活動失敗.";
                }

                if (string.IsNullOrEmpty(teamCommand.EventInfo.EventID))
                {
                    return "活動編號無效.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                EventData eventData = await this.eventRepository.GetEventData(teamCommand.EventInfo.EventID);
                if (eventData == null)
                {
                    return "活動不存在.";
                }

                bool verifyEventExaminerAuthorityResult = this.VerifyEventExaminerAuthority(teamData, eventData, teamCommand.TargetID);
                if (!verifyEventExaminerAuthorityResult)
                {
                    this.logger.LogError($"Delete Event Fail For Verify Event Examiner Authority >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventID:{teamCommand.EventInfo.EventID}");
                    return "刪除活動失敗.";
                }

                bool deleteEventDataResult = await this.eventRepository.DeleteEventData(teamCommand.EventInfo.EventID);
                if (!deleteEventDataResult)
                {
                    return "刪除活動失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Event Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventID:{(teamCommand.EventInfo != null ? teamCommand.EventInfo.EventID : "Null")}\n{ex}");
                return "刪除活動發生錯誤.";
            }
        }

        /// <summary>
        /// 刪除車隊所有活動
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> DeleteEventListOfTeam(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Delete Event List Of Team Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}");
                    return "刪除車隊所有活動失敗.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool verifyTeamExaminerAuthorityResult = this.VerifyTeamExaminerAuthority(teamData, teamCommand.TargetID, true, false, string.Empty);
                if (!verifyTeamExaminerAuthorityResult)
                {
                    this.logger.LogError($"Delete Event List Of Team Fail For Verify Team Examiner Authority >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}");
                    return "刪除車隊所有活動失敗.";
                }

                bool deleteEventDataListOfTeamResult = await this.eventRepository.DeleteEventDataListOfTeam(teamCommand.TeamID);
                if (!deleteEventDataListOfTeamResult)
                {
                    return "刪除車隊所有活動失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Event List Of Team Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return "刪除車隊所有活動發生錯誤.";
            }
        }

        /// <summary>
        /// 編輯活動
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> EditEvent(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false, false, true);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Edit Event Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventID:{(teamCommand.EventInfo != null ? teamCommand.EventInfo.EventID : "Null")}");
                    return "編輯活動失敗.";
                }

                if (string.IsNullOrEmpty(teamCommand.EventInfo.EventID))
                {
                    return "活動編號無效.";
                }

                EventData eventData = await this.eventRepository.GetEventData(teamCommand.EventInfo.EventID);
                if (eventData == null)
                {
                    return "活動不存在.";
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return "車隊不存在.";
                }

                bool verifyEventExaminerAuthorityResult = this.VerifyEventExaminerAuthority(teamData, eventData, teamCommand.TargetID);
                if (!verifyEventExaminerAuthorityResult)
                {
                    this.logger.LogError($"Edit Event Fail For Verify Event Examiner Authority >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventID:{teamCommand.EventInfo.EventID}");
                    return "編輯活動失敗.";
                }

                string updateEventDataHandlerReault = this.UpdateEventDataHandler(teamCommand.EventInfo, eventData);
                if (!string.IsNullOrEmpty(updateEventDataHandlerReault))
                {
                    return updateEventDataHandlerReault;
                }

                bool updateEventDataResult = await this.eventRepository.UpdateEventData(eventData);
                if (!updateEventDataResult)
                {
                    return "活動資料更新失敗.";
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Event Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventID:{(teamCommand.EventInfo != null ? teamCommand.EventInfo.EventID : "Null")}\n{ex}");
                return "編輯活動發生錯誤.";
            }
        }

        /// <summary>
        /// 取得會員活動列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(EventInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<EventInfoDto>, string>> GetEventListOfMember(TeamCommandDto teamCommand)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 取得車隊活動列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(EventInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<EventInfoDto>, string>> GetEventListOfTeam(TeamCommandDto teamCommand)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 加入活動
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> JoinEvent(TeamCommandDto teamCommand)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 創建新活動資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="creatorID">publisherID</param>
        /// <param name="eventInfo">eventInfo</param>
        /// <returns>Tuple(EventData, string)</returns>
        private Tuple<EventData, string> CreateEventData(string teamID, string creatorID, EventInfoDto eventInfo)
        {
            if (eventInfo.EventDate == null)
            {
                return Tuple.Create<EventData, string>(null, "無活動日期.");
            }

            if (string.IsNullOrEmpty(eventInfo.Site))
            {
                return Tuple.Create<EventData, string>(null, "無集合地點.");
            }

            if (string.IsNullOrEmpty(eventInfo.Description))
            {
                return Tuple.Create<EventData, string>(null, "無路線描述.");
            }

            DateTime createDate = DateTime.Now;
            if (eventInfo.EventDate.CompareTo(createDate) < 0)
            {
                return Tuple.Create<EventData, string>(null, "活動日期需大於現在時間.");
            }

            int countDays = new TimeSpan((eventInfo.EventDate - createDate).Ticks).Days;
            if (countDays > AppSettingHelper.Appsetting.EventMaxDate)
            {
                return Tuple.Create<EventData, string>(null, "活動預約請設定於90天內.");
            }

            EventData eventData = this.mapper.Map<EventData>(eventInfo);
            eventData.EventID = this.GetSerialID(createDate);
            eventData.TeamID = teamID;
            eventData.MemberID = creatorID;
            eventData.CreateDate = createDate;
            eventData.RoutePoints = new List<string>();
            eventData.JoinMemberList = new List<string>();
            return Tuple.Create(eventData, string.Empty);
        }

        /// <summary>
        /// 活動資料更新處理
        /// </summary>
        /// <param name="eventInfo">eventInfo</param>
        /// <param name="eventData">eventData</param>
        /// <returns>string</returns>
        private string UpdateEventDataHandler(EventInfoDto eventInfo, EventData eventData)
        {
            if (eventInfo.EventDate == null)
            {
                return "無活動日期.";
            }

            if (string.IsNullOrEmpty(eventInfo.Site))
            {
                return "無集合地點.";
            }

            if (string.IsNullOrEmpty(eventInfo.Description))
            {
                return "無路線描述.";
            }

            DateTime createDate = eventData.CreateDate;

            if (eventInfo.EventDate.CompareTo(createDate) < 0)
            {
                return "活動日期需大於現在時間.";
            }

            int countDays = new TimeSpan((eventInfo.EventDate - createDate).Ticks).Days;
            if (countDays > AppSettingHelper.Appsetting.EventMaxDate)
            {
                return "活動預約請設定於90天內.";
            }

            eventData.EventDate = eventInfo.EventDate;
            eventData.Site = eventInfo.Site;
            eventData.Description = eventInfo.Description;
            return string.Empty;
        }

        /// <summary>
        /// 驗證活動審查者權限
        /// </summary>
        /// <param name="teamData">teamData</param>
        /// <param name="eventData">eventData</param>
        /// <param name="examinerID">examinerID</param>
        /// <returns>bool</returns>
        private bool VerifyEventExaminerAuthority(TeamData teamData, EventData eventData, string examinerID)
        {
            if (string.IsNullOrEmpty(examinerID))
            {
                return false;
            }

            if (!teamData.TeamLeaderID.Equals(examinerID))
            {
                if (!teamData.TeamViceLeaderIDs.Equals(examinerID))
                {
                    if (!eventData.MemberID.Equals(examinerID))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}