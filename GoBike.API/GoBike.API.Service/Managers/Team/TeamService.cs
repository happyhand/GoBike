using AutoMapper;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Core.Resource.Enum;
using GoBike.API.Repository.Interface;
using GoBike.API.Service.Interface.Team;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Team.Data;
using GoBike.API.Service.Models.Team.View;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoBike.API.Service.Managers.Team
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
        /// redisRepository
        /// </summary>
        private readonly IRedisRepository redisRepository;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        /// <param name="redisRepository">redisRepository</param>
        public TeamService(ILogger<TeamService> logger, IMapper mapper, IRedisRepository redisRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.redisRepository = redisRepository;
        }

        #region 車隊資訊

        /// <summary>
        /// 建立車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> CreateTeam(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/CreateTeam", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Team Error >>> Data:{JsonConvert.SerializeObject(teamDto)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "建立車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 編輯車隊資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> EditTeamData(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/EditTeamData", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Team Data Error >>> Data:{JsonConvert.SerializeObject(teamDto)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "編輯車隊資料發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得附近車隊資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetNearbyTeamDataList(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/GetNearbyTeamDataList", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<TeamSimpleInfoView>>()
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Nearby Team Data List Error >>> CityID:{teamDto.CityID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得附近車隊資料列表發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得新創車隊資料列表
        /// </summary>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetNewCreationTeamDataList()
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await Utility.ApiGet(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/GetNewCreationTeamDataList");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<TeamSimpleInfoView>>()
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get New Creation Team Data List Error\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得新創車隊資料列表發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得推薦車隊資料列表
        /// </summary>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetRecommendationTeamDataList()
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await Utility.ApiGet(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/GetRecommendationTeamDataList");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<TeamSimpleInfoView>>()
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Recommendation Team Data List Error\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得推薦車隊資料列表發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得車隊資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetTeamData(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/GetTeamData", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    TeamDto getTeamDto = await httpResponseMessage.Content.ReadAsAsync<TeamDto>();
                    bool isJoinTeam = getTeamDto.TeamMemberIDs.Contains(teamDto.ExecutorID);
                    //// 取得車隊會員列表
                    IEnumerable<string> memberIDs = getTeamDto.TeamMemberIDs;
                    postData = JsonConvert.SerializeObject(memberIDs);
                    httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Member/SearchMember/List", postData);
                    IEnumerable<TeamMemberInfoView> teamMemberList = httpResponseMessage.IsSuccessStatusCode ? await httpResponseMessage.Content.ReadAsAsync<IEnumerable<TeamMemberInfoView>>() : new List<TeamMemberInfoView>();
                    foreach (TeamMemberInfoView teamMember in teamMemberList)
                    {
                        string fuzzyCacheKey = $"{CommonFlagHelper.CommonFlag.RedisFlag.Session}-*-{teamMember.MemberID}";
                        string cacheKey = this.redisRepository.GetRedisKeys(fuzzyCacheKey).FirstOrDefault();
                        teamMember.OnlineType = isJoinTeam ? string.IsNullOrEmpty(cacheKey) ? (int)OnlineStatusType.Offline : (int)OnlineStatusType.Online : (int)OnlineStatusType.None;
                        teamMember.TeamIdentity = this.GetTeamIdentity(getTeamDto.TeamLeaderID, getTeamDto.TeamViceLeaderIDs, getTeamDto.TeamMemberIDs, teamMember.MemberID);
                    }

                    if (getTeamDto.TeamMemberIDs.Contains(teamDto.ExecutorID))
                    {
                        TeamDetailInfoViewDto teamDetailInfoView = this.mapper.Map<TeamDetailInfoViewDto>(getTeamDto);
                        teamDetailInfoView.MemberList = teamMemberList;
                        teamDetailInfoView.TeamIdentity = this.GetTeamIdentity(getTeamDto.TeamLeaderID, getTeamDto.TeamViceLeaderIDs, getTeamDto.TeamMemberIDs, teamDto.ExecutorID);

                        postData = JsonConvert.SerializeObject(teamDto);
                        httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/GetTeamInteractiveDataList", postData);
                        IEnumerable<TeamInteractiveInfoViewDto> teamInteractiveList = httpResponseMessage.IsSuccessStatusCode ? await httpResponseMessage.Content.ReadAsAsync<IEnumerable<TeamInteractiveInfoViewDto>>() : new List<TeamInteractiveInfoViewDto>();
                        teamDetailInfoView.ApplyForList = teamInteractiveList.Where(data => data.InteractiveType == (int)TeamInteractiveType.ApplyFor);
                        teamDetailInfoView.InviteList = teamInteractiveList.Where(data => data.InteractiveType == (int)TeamInteractiveType.Invite);

                        postData = JsonConvert.SerializeObject(teamDto);
                        httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/Announcement/Get", postData);
                        IEnumerable<TeamAnnouncementInfoViewDto> teamAnnouncementList = httpResponseMessage.IsSuccessStatusCode ? await httpResponseMessage.Content.ReadAsAsync<IEnumerable<TeamAnnouncementInfoViewDto>>() : new List<TeamAnnouncementInfoViewDto>();
                        teamDetailInfoView.AnnouncementList = teamAnnouncementList;

                        //// TODO 車隊活動

                        return new ResponseResultDto()
                        {
                            Ok = true,
                            Data = teamDetailInfoView
                        };
                    }
                    else
                    {
                        TeamNoJoinInfoView teamNoJoinInfoView = this.mapper.Map<TeamNoJoinInfoView>(getTeamDto);
                        teamNoJoinInfoView.MemberList = teamMemberList;
                        teamNoJoinInfoView.JoinStatus = getTeamDto.JoinStatus;
                        return new ResponseResultDto()
                        {
                            Ok = true,
                            Data = teamNoJoinInfoView
                        };
                    }
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Data Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得車隊資料發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得會員的車隊資料列表
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetTeamDataListOfMember(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/GetTeamDataListOfMember", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<IEnumerable<TeamSimpleInfoView>>>()
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team List Of Member Error >>> ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得會員的車隊資料列表發生錯誤."
                };
            }
        }

        /// <summary>
        /// 搜尋車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> SearchTeam(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/SearchTeam", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<TeamSimpleInfoView>>()
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Team Error >>> ExecutorID:{teamDto.ExecutorID} SearchKey:{teamDto.SearchKey}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "搜尋車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得車隊身分
        /// </summary>
        /// <param name="teamLeaderID">teamLeaderID</param>
        /// <param name="teamViceLeaderIDs">teamViceLeaderIDs</param>
        /// <param name="teamMemberIDs">teamMemberIDs</param>
        /// <param name="memberID">memberID</param>
        /// <returns>int</returns>
        private int GetTeamIdentity(string teamLeaderID, IEnumerable<string> teamViceLeaderIDs, IEnumerable<string> teamMemberIDs, string memberID)
        {
            if (teamLeaderID.Equals(memberID))
            {
                return (int)TeamIdentityType.Leader;
            }

            if (teamViceLeaderIDs.Contains(memberID))
            {
                return (int)TeamIdentityType.ViceLeader;
            }

            if (teamMemberIDs.Contains(memberID))
            {
                return (int)TeamIdentityType.Normal;
            }

            return (int)TeamIdentityType.None;
        }

        #endregion 車隊資訊

        #region 車隊互動

        /// <summary>
        /// 同意邀請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> AgreeInviteJoinTeam(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/Invite/AgreeInvite", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Agree Invite Join Team Error >>> Data:{JsonConvert.SerializeObject(teamDto)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "同意邀請加入車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 允許申請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> AllowApplyForJoinTeam(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/ApplyFor/AllowJoin", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Allow Apply For Join Team Error >>> TeamID:{teamDto.TeamID} ExaminerID:{teamDto.ExaminerID} TargetID:{teamDto.TargetID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "加入車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 允許邀請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> AllowInviteJoinTeam(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/Invite/AllowJoin", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Allow Invite Join Team Error >>> TeamID:{teamDto.TeamID} ExaminerID:{teamDto.ExaminerID} TargetID:{teamDto.TargetID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "加入車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 申請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> ApplyForJoinTeam(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/ApplyFor/RequestJoin", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Apply For Join Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "加入車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 邀請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> InviteJoinTeam(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/Invite/RequestJoin", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Invite Join Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID} TargetIDs:{JsonConvert.SerializeObject(teamDto.TargetIDs)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "邀請加入車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 離開車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> LeaveTeam(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/LeaveTeam", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Leave Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "離開車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 拒絕申請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> RejectApplyForJoinTeam(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/ApplyFor/RejectJoin", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Apply For Join Team Error >>> TeamID:{teamDto.TeamID} ExaminerID:{teamDto.ExaminerID} TargetID:{teamDto.TargetID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "拒絕申請加入車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 拒絕邀請加入車隊
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> RejectInviteJoinTeam(TeamDto teamDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/Invite/RejectJoin", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Invite For Join Team Error >>> TeamID:{teamDto.TeamID} ExecutorID:{teamDto.ExecutorID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "拒絕邀請加入車隊發生錯誤."
                };
            }
        }

        #endregion 車隊互動

        #region 車隊公告

        /// <summary>
        /// 建立車隊公告資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> CreateTeamAnnouncementData(TeamAnnouncementDto teamAnnouncementDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamAnnouncementDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/Announcement/Create", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Team Announcement Data Error >>> Data:{JsonConvert.SerializeObject(teamAnnouncementDto)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "建立車隊公告資料發生錯誤."
                };
            }
        }

        /// <summary>
        /// 刪除車隊公告資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> DeleteTeamAnnouncementData(TeamAnnouncementDto teamAnnouncementDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamAnnouncementDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/Announcement/Delete", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Team Announcement Data Error >>> TeamID:{teamAnnouncementDto.TeamID} AnnouncementID:{teamAnnouncementDto.AnnouncementID} MemberID:{teamAnnouncementDto.MemberID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "刪除車隊公告資料發生錯誤."
                };
            }
        }

        /// <summary>
        /// 編輯車隊公告資料
        /// </summary>
        /// <param name="teamDto">teamDto</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> EditTeamAnnouncementData(TeamAnnouncementDto teamAnnouncementDto)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamAnnouncementDto);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Team/Announcement/Edit", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Edit Team Announcement Data Error >>> Data:{JsonConvert.SerializeObject(teamAnnouncementDto)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "編輯車隊公告資料發生錯誤."
                };
            }
        }

        #endregion 車隊公告
    }
}