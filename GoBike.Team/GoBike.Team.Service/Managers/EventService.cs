using AutoMapper;
using GoBike.Team.Core.Applibs;
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
using static GoBike.Team.Service.Models.Data.EventSimpleInfoDto;

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
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false, false, true);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Cancel Join Event Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventID:{(teamCommand.EventInfo != null ? teamCommand.EventInfo.EventID : "Null")}");
                    return "取消加入活動失敗.";
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

                bool updateJoinMemberListResult = Utility.UpdateListHandler(eventData.JoinMemberList, teamCommand.TargetID, false);
                if (updateJoinMemberListResult)
                {
                    bool result = await this.eventRepository.UpdateJoinMemberList(eventData.EventID, eventData.JoinMemberList);
                    if (!result)
                    {
                        return "活動參加名單資料更新失敗.";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Cancel Join Event Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventID:{(teamCommand.EventInfo != null ? teamCommand.EventInfo.EventID : "Null")}\n{ex}");
                return "取消加入活動發生錯誤.";
            }
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

                if (!teamData.TeamLeaderID.Equals(teamCommand.TargetID) && !teamData.TeamPlayerIDs.Contains(teamCommand.TargetID))
                {
                    return "該會員非車隊隊員.";
                }

                Tuple<EventData, string> createEventDataResult = await this.CreateEventData(teamData.TeamID, teamData.TeamName, teamCommand.TargetID, teamCommand.EventInfo);
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
                    this.logger.LogError($"Edit Event Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventInfo:{JsonConvert.SerializeObject(teamCommand.EventInfo)}");
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
                this.logger.LogError($"Edit Event Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventInfo:{JsonConvert.SerializeObject(teamCommand.EventInfo)}\n{ex}");
                return "編輯活動發生錯誤.";
            }
        }

        /// <summary>
        /// 取得活動詳細資訊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(EventDetailInfoDto, string)</returns>
        public async Task<Tuple<EventDetailInfoDto, string>> GetEventDetailInfo(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false, false, true);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Get Event Detail Info Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventID:{(teamCommand.EventInfo != null ? teamCommand.EventInfo.EventID : "Null")}");
                    return Tuple.Create<EventDetailInfoDto, string>(null, "取得活動詳細資訊失敗.");
                }

                if (string.IsNullOrEmpty(teamCommand.EventInfo.EventID))
                {
                    return Tuple.Create<EventDetailInfoDto, string>(null, "活動編號無效.");
                }

                EventData eventData = await this.eventRepository.GetEventData(teamCommand.EventInfo.EventID);
                if (eventData == null)
                {
                    return Tuple.Create<EventDetailInfoDto, string>(null, "活動不存在.");
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return Tuple.Create<EventDetailInfoDto, string>(null, "車隊不存在.");
                }

                if (!teamData.TeamLeaderID.Equals(teamCommand.TargetID) && !teamData.TeamPlayerIDs.Contains(teamCommand.TargetID))
                {
                    return Tuple.Create<EventDetailInfoDto, string>(null, "該會員非車隊隊員.");
                }

                bool updateHaveSeenMemberIDsResult = Utility.UpdateListHandler(eventData.HaveSeenMemberIDs, teamCommand.TargetID, true);
                if (updateHaveSeenMemberIDsResult)
                {
                    //// 無須對【已閱活動名單資料】作審查，不應影響原功能
                    bool result = await this.eventRepository.UpdateHaveSeenMemberIDs(teamData.TeamID, eventData.HaveSeenMemberIDs);
                }

                EventDetailInfoDto eventDetailInfo = this.mapper.Map<EventDetailInfoDto>(eventData);
                eventDetailInfo.TeamName = teamData.TeamName;
                eventDetailInfo.TeamPhoto = teamData.TeamPhoto;
                return Tuple.Create(eventDetailInfo, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Event Detail Info Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventID:{(teamCommand.EventInfo != null ? teamCommand.EventInfo.EventID : "Null")}\n{ex}");
                return Tuple.Create<EventDetailInfoDto, string>(null, "取得活動詳細資訊發生錯誤.");
            }
        }

        /// <summary>
        /// 取得會員活動列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(EventInfoArray, string)</returns>
        public async Task<Tuple<List<EventSimpleInfoDto>[], string>> GetEventListOfMember(TeamCommandDto teamCommand)
        {
            try
            {
                if (teamCommand == null)
                {
                    this.logger.LogError($"Get Event List Of Member Fail For TeamCommand Null");
                    return Tuple.Create<List<EventSimpleInfoDto>[], string>(null, "取得會員活動列表失敗.");
                }

                if (string.IsNullOrEmpty(teamCommand.TargetID))
                {
                    this.logger.LogError($"Get Event List Of Member Fail For TargetID Null");
                    return Tuple.Create<List<EventSimpleInfoDto>[], string>(null, "取得會員活動列表失敗.");
                }

                string memberID = teamCommand.TargetID;
                IEnumerable<TeamData> teamDatas = await this.teamRepository.GetTeamDataListOfMember(memberID);
                List<EventSimpleInfoDto> joinEventDatas = new List<EventSimpleInfoDto>();
                List<EventSimpleInfoDto> notYetJoinEventDatas = new List<EventSimpleInfoDto>();
                foreach (TeamData teamData in teamDatas)
                {
                    bool isExaminer = teamData.TeamLeaderID.Equals(memberID) || teamData.TeamViceLeaderIDs.Contains(memberID);
                    IEnumerable<EventData> eventDatas = await this.eventRepository.GetEventDataListOfTeam(teamData.TeamID);
                    IEnumerable<EventSimpleInfoDto> eventInfos = this.mapper.Map<IEnumerable<EventSimpleInfoDto>>(eventDatas);
                    foreach (EventSimpleInfoDto eventInfo in eventInfos)
                    {
                        eventInfo.TeamName = teamData.TeamName;
                        eventInfo.TeamPhoto = teamData.TeamPhoto;
                        eventInfo.EventSettingType = (isExaminer || eventInfo.CreatorID.Equals(memberID)) ? (int)TeamEventSettingType.Edit : (int)TeamEventSettingType.None;
                        if (eventInfo.JoinMemberList.Contains(memberID))
                        {
                            joinEventDatas.Add(eventInfo);
                        }
                        else
                        {
                            notYetJoinEventDatas.Add(eventInfo);
                        }
                    }
                }

                return Tuple.Create(new List<EventSimpleInfoDto>[] { joinEventDatas, notYetJoinEventDatas }, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Event List Of Member Error >>> TargetID:{(teamCommand != null ? teamCommand.TargetID : "Null")}\n{ex}");
                return Tuple.Create<List<EventSimpleInfoDto>[], string>(null, "取得會員活動列表發生錯誤.");
            }
        }

        /// <summary>
        /// 取得車隊活動列表
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>Tuple(EventInfoDtos, string)</returns>
        public async Task<Tuple<IEnumerable<EventSimpleInfoDto>, string>> GetEventListOfTeam(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false, false, false);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Get Event List Of Team Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}");
                    return Tuple.Create<IEnumerable<EventSimpleInfoDto>, string>(null, "取得車隊活動列表失敗.");
                }

                TeamData teamData = await this.teamRepository.GetTeamData(teamCommand.TeamID);
                if (teamData == null)
                {
                    return Tuple.Create<IEnumerable<EventSimpleInfoDto>, string>(null, "車隊不存在.");
                }

                if (!teamData.TeamLeaderID.Equals(teamCommand.TargetID) && !teamData.TeamPlayerIDs.Contains(teamCommand.TargetID))
                {
                    return Tuple.Create<IEnumerable<EventSimpleInfoDto>, string>(null, "該會員非車隊隊員.");
                }

                string memberID = teamCommand.TargetID;
                IEnumerable<EventData> eventDatas = await this.eventRepository.GetEventDataListOfTeam(teamCommand.TeamID);
                IEnumerable<EventSimpleInfoDto> eventInfos = this.mapper.Map<IEnumerable<EventSimpleInfoDto>>(eventDatas);
                bool isExaminer = teamData.TeamLeaderID.Equals(memberID) || teamData.TeamViceLeaderIDs.Contains(memberID);
                foreach (EventSimpleInfoDto eventInfo in eventInfos)
                {
                    eventInfo.TeamName = teamData.TeamName;
                    eventInfo.TeamPhoto = teamData.TeamPhoto;
                    eventInfo.EventSettingType = (isExaminer || eventInfo.CreatorID.Equals(memberID)) ? (int)TeamEventSettingType.Edit : (int)TeamEventSettingType.None;
                }

                return Tuple.Create(eventInfos, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Event List Of Team Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return Tuple.Create<IEnumerable<EventSimpleInfoDto>, string>(null, "取得車隊活動列表發生錯誤.");
            }
        }

        /// <summary>
        /// 加入活動
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>string</returns>
        public async Task<string> JoinEvent(TeamCommandDto teamCommand)
        {
            try
            {
                bool verifyTeamCommandResult = this.VerifyTeamCommand(teamCommand, false, true, false, false, false, true);
                if (!verifyTeamCommandResult)
                {
                    this.logger.LogError($"Join Event Fail For Verify TeamCommand >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventID:{(teamCommand.EventInfo != null ? teamCommand.EventInfo.EventID : "Null")}");
                    return "加入活動失敗.";
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

                if (!teamData.TeamLeaderID.Equals(teamCommand.TargetID) && !teamData.TeamPlayerIDs.Contains(teamCommand.TargetID))
                {
                    return "該會員非車隊隊員.";
                }

                bool updateJoinMemberListResult = Utility.UpdateListHandler(eventData.JoinMemberList, teamCommand.TargetID, true);
                if (updateJoinMemberListResult)
                {
                    bool result = await this.eventRepository.UpdateJoinMemberList(eventData.EventID, eventData.JoinMemberList);
                    if (!result)
                    {
                        return "活動參加名單資料更新失敗.";
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Join Event Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID} EventID:{(teamCommand.EventInfo != null ? teamCommand.EventInfo.EventID : "Null")}\n{ex}");
                return "加入活動發生錯誤.";
            }
        }

        /// <summary>
        /// 創建新活動資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <param name="teamName">teamName</param>
        /// <param name="creatorID">publisherID</param>
        /// <param name="eventInfo">eventInfo</param>
        /// <returns>Tuple(EventData, string)</returns>
        private async Task<Tuple<EventData, string>> CreateEventData(string teamID, string teamName, string creatorID, EventDetailInfoDto eventInfo)
        {
            if (eventInfo.EventDate == null)
            {
                return Tuple.Create<EventData, string>(null, "無活動日期.");
            }

            if (string.IsNullOrEmpty(eventInfo.EventTitle))
            {
                return Tuple.Create<EventData, string>(null, "無活動標題.");
            }

            if (string.IsNullOrEmpty(eventInfo.Site))
            {
                return Tuple.Create<EventData, string>(null, "無集合地點.");
            }

            if (string.IsNullOrEmpty(eventInfo.Description))
            {
                return Tuple.Create<EventData, string>(null, "無路線描述.");
            }

            bool verifyEventDataByCreatorIDResult = await this.eventRepository.VerifyEventDataByCreatorID(creatorID);
            if (verifyEventDataByCreatorIDResult)
            {
                return Tuple.Create<EventData, string>(null, "無法建立兩個以上的活動.");
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
            eventData.CreatorID = creatorID;
            eventData.CreateDate = createDate;
            eventData.RoutePoints = new List<string>();
            eventData.JoinMemberList = new List<string>() { creatorID };
            eventData.HaveSeenMemberIDs = new List<string>();
            eventData.SaveDeadline = new DateTime(eventInfo.EventDate.Year, eventInfo.EventDate.Month, eventInfo.EventDate.Day, 23, 59, 59);
            return Tuple.Create(eventData, string.Empty);
        }

        /// <summary>
        /// 活動資料更新處理
        /// </summary>
        /// <param name="eventInfo">eventInfo</param>
        /// <param name="eventData">eventData</param>
        /// <returns>string</returns>
        private string UpdateEventDataHandler(EventDetailInfoDto eventInfo, EventData eventData)
        {
            if (eventInfo.EventDate == null)
            {
                return "無活動日期.";
            }

            if (string.IsNullOrEmpty(eventInfo.EventTitle))
            {
                return "無活動標題.";
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
            eventData.HaveSeenMemberIDs = new List<string>();
            eventData.SaveDeadline = new DateTime(eventInfo.EventDate.Year, eventInfo.EventDate.Month, eventInfo.EventDate.Day, 23, 59, 59);
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
                    if (!eventData.CreatorID.Equals(examinerID))
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}