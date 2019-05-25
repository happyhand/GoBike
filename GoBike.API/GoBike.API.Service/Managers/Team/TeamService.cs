using AutoMapper;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Core.Resource.Enum;
using GoBike.API.Service.Interface.Team;
using GoBike.API.Service.Models.Member.View;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Team.Command;
using GoBike.API.Service.Models.Team.Command.Data;
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
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        public TeamService(ILogger<TeamService> logger, IMapper mapper)
        {
            this.logger = logger;
            this.mapper = mapper;
        }

        #region 車隊資料

        /// <summary>
        /// 車隊編輯
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> EditData(TeamCommandDto teamCommand)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/EditData", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<TeamInfoViewDto>()
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
                this.logger.LogError($"Edit Data Error >>> Data:{JsonConvert.SerializeObject(teamCommand)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "車隊編輯發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得我的車隊資訊
        /// </summary>
        /// <param name="membrID">membrID</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetMyTeamInfo(string membrID)
        {
            try
            {
                MemberTeamInfoViewDto memberTeamInfoView = new MemberTeamInfoViewDto();
                string postData = JsonConvert.SerializeObject(new TeamCommandDto() { TargetID = membrID });
                Task<HttpResponseMessage> getTeamInfoListOfMemberHttpResponseMessage = Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/GetTeamInfoListOfMember", postData);
                Task<HttpResponseMessage> getInviteListHttpResponseMessage = Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/Invite/Get", postData);
                //// 取得車隊列表
                HttpResponseMessage getTeamInfoListOfMemberHttpResponseMessageResult = await getTeamInfoListOfMemberHttpResponseMessage;
                if (getTeamInfoListOfMemberHttpResponseMessageResult.IsSuccessStatusCode)
                {
                    dynamic[] datas = await getTeamInfoListOfMemberHttpResponseMessageResult.Content.ReadAsAsync<dynamic[]>();
                    memberTeamInfoView.LeaderTeam = this.mapper.Map<TeamSimpleInfoViewDto>(datas[0]);
                    memberTeamInfoView.TeamList = this.mapper.Map<IEnumerable<TeamSimpleInfoViewDto>>(datas[1]);
                    //// TODO 車隊活動
                    memberTeamInfoView.JoinedEventList = new List<dynamic>();
                    memberTeamInfoView.notYetJoinEventList = new List<dynamic>();
                }
                else
                {
                    this.logger.LogError($"Get My Team Info Fail For Get Team Info List Of Member >>> MemberID:{membrID} Message:{getTeamInfoListOfMemberHttpResponseMessageResult.Content.ReadAsAsync<string>()}");
                    memberTeamInfoView.LeaderTeam = null;
                    memberTeamInfoView.TeamList = new List<TeamSimpleInfoViewDto>();
                    memberTeamInfoView.JoinedEventList = new List<dynamic>();
                    memberTeamInfoView.notYetJoinEventList = new List<dynamic>();
                }

                //// 取得邀請加入列表
                HttpResponseMessage getInviteListHttpResponseMessageResult = await getInviteListHttpResponseMessage;
                if (getTeamInfoListOfMemberHttpResponseMessageResult.IsSuccessStatusCode)
                {
                    memberTeamInfoView.InviteJoinList = await getTeamInfoListOfMemberHttpResponseMessageResult.Content.ReadAsAsync<IEnumerable<TeamSimpleInfoViewDto>>();
                }
                else
                {
                    this.logger.LogError($"Get My Team Info Fail For Get Invite List >>> MemberID:{membrID} Message:{getTeamInfoListOfMemberHttpResponseMessageResult.Content.ReadAsAsync<string>()}");
                    memberTeamInfoView.InviteJoinList = new List<TeamSimpleInfoViewDto>();
                }

                return new ResponseResultDto()
                {
                    Ok = true,
                    Data = memberTeamInfoView
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get My Team Info Error >>> MemberID:{membrID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得我的車隊資訊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得車隊明細資訊
        /// </summary>
        /// <param name="teamCommand">teamCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetTeamDetailInfo(TeamCommandDto teamCommand)
        {
            try
            {
                //// 取得車隊資訊
                string postData = JsonConvert.SerializeObject(teamCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/GetTeamInfo", postData);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                    };
                }

                TeamInfoDto teamInfo = await httpResponseMessage.Content.ReadAsAsync<TeamInfoDto>();
                TeamDetailInfoViewDto teamDetailInfoView = this.mapper.Map<TeamDetailInfoViewDto>(teamInfo);
                //// 車隊身分行為設定
                teamDetailInfoView.TeamActionSetting = this.GetTeamActionFlag(teamInfo, teamCommand.TargetID);
                //// 取得車隊隊員會員資訊列表
                postData = JsonConvert.SerializeObject(teamInfo.TeamPlayerIDs);
                httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/GetMemberInfo/List", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    IEnumerable<TeamMemberInfoViewDto> teamMemberInfoViews = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<TeamMemberInfoViewDto>>();
                    this.TeamMemberIdentityAuthorityHandler(teamInfo, teamCommand.TargetID, teamMemberInfoViews);
                    teamDetailInfoView.TeamMemberList = teamMemberInfoViews;
                }
                else
                {
                    this.logger.LogError($"Get Team Detail Info Fail For Get Member Info List >>> MemberIDs:{JsonConvert.SerializeObject(teamInfo.TeamPlayerIDs)} Message:{httpResponseMessage.Content.ReadAsAsync<string>()}");
                    teamDetailInfoView.TeamMemberList = new List<TeamMemberInfoViewDto>();
                }

                //// 取得申請加入車隊會員資訊列表
                if (teamInfo.TeamLeaderID.Equals(teamCommand.TargetID) || teamInfo.TeamViceLeaderIDs.Contains(teamCommand.TargetID))
                {
                    postData = JsonConvert.SerializeObject(teamInfo.TeamApplyForJoinIDs);
                    httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/GetMemberInfo/List", postData);
                    if (httpResponseMessage.IsSuccessStatusCode)
                    {
                        teamDetailInfoView.ApplyForRequestList = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<MemberSimpleInfoViewDto>>();
                    }
                    else
                    {
                        this.logger.LogError($"Get Team Detail Info Fail For Get Apply For Member Info >>> MemberIDs:{JsonConvert.SerializeObject(teamInfo.TeamApplyForJoinIDs)} Message:{httpResponseMessage.Content.ReadAsAsync<string>()}");
                        teamDetailInfoView.ApplyForRequestList = new List<MemberSimpleInfoViewDto>();
                    }
                }
                else
                {
                    teamDetailInfoView.ApplyForRequestList = new List<MemberSimpleInfoViewDto>();
                }

                //// TODO 取得最新公告
                teamDetailInfoView.NewAnnouncement = new { };
                //// TODO 取得活動列表
                teamDetailInfoView.EventList = new List<string>();
                return new ResponseResultDto()
                {
                    Ok = true,
                    Data = teamDetailInfoView
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Team Detail Info Error >>> TeamID:{teamCommand.TeamID} TargetID:{teamCommand.TargetID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得車隊明細資訊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 建立車隊
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="teamInfo">teamInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> Register(string memberID, TeamInfoDto teamInfo)
        {
            try
            {
                teamInfo.TeamLeaderID = memberID;
                string postData = JsonConvert.SerializeObject(teamInfo);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/Register", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Register Error >>> MemberID:{memberID} Data:{JsonConvert.SerializeObject(teamInfo)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "建立車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 搜尋車隊
        /// </summary>
        /// <param name="teamSearchCommand">teamSearchCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> SearchTeam(TeamSearchCommandDto teamSearchCommand)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(teamSearchCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/SearchTeam", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<TeamSimpleInfoViewDto>>()
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
                this.logger.LogError($"Search Team Error >>> SearcherID:{teamSearchCommand.SearcherID} SearchKey:{teamSearchCommand.SearchKey}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "搜尋車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得車隊可執行命令 Flag
        /// </summary>
        /// <param name="teamInfo">teamInfo</param>
        /// <param name="memberID">memberID</param>
        /// <returns></returns>
        private int GetTeamActionFlag(TeamInfoDto teamInfo, string memberID)
        {
            int teamActionFlag = 0;
            if (teamInfo.TeamLeaderID.Equals(memberID))
            {
                teamActionFlag = (int)TeamActionSettingType.HistoricalAnnouncement
                                + (int)TeamActionSettingType.HoldEvent
                                + (int)TeamActionSettingType.EditData
                                + (int)TeamActionSettingType.InviteFriend
                                + (int)TeamActionSettingType.PublishAnnouncement
                                + (int)TeamActionSettingType.Transfer
                                + (int)TeamActionSettingType.Disband;
            }
            else if (teamInfo.TeamViceLeaderIDs.Contains(memberID))
            {
                teamActionFlag = (int)TeamActionSettingType.HistoricalAnnouncement
                               + (int)TeamActionSettingType.HoldEvent
                               + (int)TeamActionSettingType.Leave
                               + (int)TeamActionSettingType.EditData
                               + (int)TeamActionSettingType.InviteFriend
                               + (int)TeamActionSettingType.PublishAnnouncement;
            }
            else if (teamInfo.TeamPlayerIDs.Contains(memberID))
            {
                teamActionFlag = (int)TeamActionSettingType.HistoricalAnnouncement
                              + (int)TeamActionSettingType.HoldEvent
                              + (int)TeamActionSettingType.Leave;
            }
            else
            {
                teamActionFlag = (int)TeamActionSettingType.ApplyForJoin;
            }

            return teamActionFlag;
        }

        /// <summary>
        /// 取得車隊身分
        /// </summary>
        /// <param name="teamInfo">teamInfo</param>
        /// <param name="memberID">memberID</param>
        /// <returns></returns>
        private int GetTeamIndetity(TeamInfoDto teamInfo, string memberID)
        {
            if (teamInfo.TeamLeaderID.Equals(memberID))
            {
                return (int)TeamIdentityType.Leader;
            }
            else if (teamInfo.TeamViceLeaderIDs.Contains(memberID))
            {
                return (int)TeamIdentityType.ViceLeader;
            }
            else if (teamInfo.TeamPlayerIDs.Contains(memberID))
            {
                return (int)TeamIdentityType.Normal;
            }

            return (int)TeamIdentityType.None;
        }

        /// <summary>
        /// 取得車隊踢除設定
        /// </summary>
        /// <param name="examinerIdentity">examinerIdentity</param>
        /// <param name="targetIdentity">targetIdentity</param>
        /// <returns>int</returns>
        private int GetTeamKickOutSetFlag(int examinerIdentity, int targetIdentity)
        {
            switch (examinerIdentity)
            {
                case (int)TeamIdentityType.Leader:
                case (int)TeamIdentityType.ViceLeader:
                    switch (targetIdentity)
                    {
                        case (int)TeamIdentityType.Leader:
                            return (int)TeamKickOutSettingType.None;

                        default:
                            return (int)TeamKickOutSettingType.KickOut;
                    }
                default:
                    return (int)TeamKickOutSettingType.None;
            }
        }

        /// <summary>
        /// 取得車隊副隊長委任設定
        /// </summary>
        /// <param name="examinerIdentity">examinerIdentity</param>
        /// <param name="targetIdentity">targetIdentity</param>
        /// <returns>int</returns>
        private int GetTeamViceLeaderSetFlag(int examinerIdentity, int targetIdentity)
        {
            switch (examinerIdentity)
            {
                case (int)TeamIdentityType.Leader:
                    switch (targetIdentity)
                    {
                        case (int)TeamIdentityType.Leader:
                            return (int)TeamViceLeaderSettingType.None;

                        case (int)TeamIdentityType.ViceLeader:
                            return (int)TeamViceLeaderSettingType.Cancel;

                        default:
                            return (int)TeamViceLeaderSettingType.Appoint;
                    }
                default:
                    return (int)TeamViceLeaderSettingType.None;
            }
        }

        /// <summary>
        /// 車隊隊員身分權限處理
        /// </summary>
        /// <param name="teamInfo">teamInfo</param>
        /// <param name="targetID">targetID</param>
        /// <param name="teamMemberInfoViews">teamMemberInfoViews</param>
        private void TeamMemberIdentityAuthorityHandler(TeamInfoDto teamInfo, string targetID, IEnumerable<TeamMemberInfoViewDto> teamMemberInfoViews)
        {
            string teamLeaderID = teamInfo.TeamLeaderID;
            IEnumerable<string> teamViceLeaderIDs = teamInfo.TeamViceLeaderIDs;
            IEnumerable<string> teamPlayerIDs = teamInfo.TeamPlayerIDs;
            int targetIdentity = this.GetTeamIndetity(teamInfo, targetID);
            foreach (TeamMemberInfoViewDto teamMemberInfoView in teamMemberInfoViews)
            {
                teamMemberInfoView.TeamIdentity = this.GetTeamIndetity(teamInfo, teamMemberInfoView.MemberID);
                teamMemberInfoView.TeamKickOutSetting = this.GetTeamKickOutSetFlag(targetIdentity, teamMemberInfoView.TeamIdentity);
                teamMemberInfoView.TeamViceLeaderSetting = this.GetTeamViceLeaderSetFlag(targetIdentity, teamMemberInfoView.TeamIdentity);
            }
        }

        #endregion 車隊資料

        #region 車隊互動資料

        /// <summary>
        /// 申請加入車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> ApplyForJoinTeam(TeamInteractiveCommandDto teamInteractiveCommand)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new TeamCommandDto() { TeamID = teamInteractiveCommand.TeamID, TargetID = teamInteractiveCommand.MemberID });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/ApplyFor/Join", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Apply For Join Team Error >>> TemaID:{teamInteractiveCommand.TeamID} TargetID:{teamInteractiveCommand.MemberID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "申請加入車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取消申請加入車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> CancelApplyForJoinTeam(TeamInteractiveCommandDto teamInteractiveCommand)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new TeamCommandDto() { TeamID = teamInteractiveCommand.TeamID, TargetID = teamInteractiveCommand.MemberID });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/ApplyFor/Cancel", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Cancel Apply For Join Team Error >>> TemaID:{teamInteractiveCommand.TeamID} TargetID:{teamInteractiveCommand.MemberID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取消申請加入車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取消邀請加入車隊
        /// </summary>
        /// <param name="inviter">inviter</param>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> CancelInviteJoinTeam(string inviter, TeamInteractiveCommandDto teamInteractiveCommand)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new TeamCommandDto() { TeamID = teamInteractiveCommand.TeamID, ExaminerID = inviter, TargetID = teamInteractiveCommand.MemberID });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/Invite/Cancel", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Cancel Invite Join Team Error >>> TemaID:{teamInteractiveCommand.TeamID} ExaminerID:{inviter} TargetID:{teamInteractiveCommand.MemberID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取消邀請加入車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 強制離開車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> ForceLeaveTeam(string examinerID, TeamInteractiveCommandDto teamInteractiveCommand)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new TeamCommandDto() { TeamID = teamInteractiveCommand.TeamID, ExaminerID = examinerID, TargetID = teamInteractiveCommand.MemberID });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/ForceLeaveTeam", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Force Leave Team Error >>> TeamID:{teamInteractiveCommand.TeamID} ExaminerID:{examinerID} TargetID:{teamInteractiveCommand.MemberID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "強制離開車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 邀請加入車隊
        /// </summary>
        /// <param name="inviter">inviter</param>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> InviteJoinTeam(string inviter, TeamInteractiveCommandDto teamInteractiveCommand)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new TeamCommandDto() { TeamID = teamInteractiveCommand.TeamID, ExaminerID = inviter, TargetID = teamInteractiveCommand.MemberID });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/Invite/Join", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Invite Join Team Error >>> TemaID:{teamInteractiveCommand.TeamID} ExaminerID:{inviter} TargetID:{teamInteractiveCommand.MemberID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "邀請加入車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 邀請多人加入車隊
        /// </summary>
        /// <param name="inviter">inviter</param>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> InviteManyJoinTeam(string inviter, TeamInteractiveCommandDto teamInteractiveCommand)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new TeamCommandDto() { TeamID = teamInteractiveCommand.TeamID, ExaminerID = inviter, TargetIDs = teamInteractiveCommand.MemberList });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/Invite/ManyJoin", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Invite Many Join Team Error >>> TemaID:{teamInteractiveCommand.TeamID} ExaminerID:{inviter} TargetIDs:{JsonConvert.SerializeObject(teamInteractiveCommand.MemberList)}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "邀請多人加入車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 加入車隊
        /// </summary>
        /// <param name="examinerID">examinerID</param>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <param name="isInvite">isInvite</param>
        /// <returns>string</returns>
        public async Task<ResponseResultDto> JoinTeam(string examinerID, TeamInteractiveCommandDto teamInteractiveCommand, bool isInvite)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new TeamCommandDto() { TeamID = teamInteractiveCommand.TeamID, ExaminerID = examinerID, TargetID = teamInteractiveCommand.MemberID });
                string apiUrl = isInvite ? "api/Team/JoinTeam/InviteJoin" : "api/Team/JoinTeam/AllowJoin";
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, apiUrl, postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Join Team Error >>> TeamID:{teamInteractiveCommand.TeamID} ExaminerID:{examinerID} TargetID:{teamInteractiveCommand.MemberID} IsInvite:{isInvite}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "加入車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 離開車隊
        /// </summary>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>string</returns>
        public async Task<ResponseResultDto> LeaveTeam(TeamInteractiveCommandDto teamInteractiveCommand)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new TeamCommandDto() { TeamID = teamInteractiveCommand.TeamID, TargetID = teamInteractiveCommand.MemberID });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/LeaveTeam", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Leave Team Error >>> TeamID:{teamInteractiveCommand.TeamID} TargetID:{teamInteractiveCommand.MemberID}\n{ex}");
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
        /// <param name="examinerID">examinerID</param>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> RejectApplyForJoinTeam(string examinerID, TeamInteractiveCommandDto teamInteractiveCommand)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new TeamCommandDto() { TeamID = teamInteractiveCommand.TeamID, ExaminerID = examinerID, TargetID = teamInteractiveCommand.MemberID });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/ApplyFor/Reject", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Apply For Join Team Error >>> TemaID:{teamInteractiveCommand.TeamID} ExaminerID:{examinerID} TargetID:{teamInteractiveCommand.MemberID}\n{ex}");
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
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> RejectInviteJoinTeam(TeamInteractiveCommandDto teamInteractiveCommand)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new TeamCommandDto() { TeamID = teamInteractiveCommand.TeamID, TargetID = teamInteractiveCommand.MemberID });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/Invite/Reject", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Invite Join Team Error >>> TemaID:{teamInteractiveCommand.TeamID} TargetID:{teamInteractiveCommand.MemberID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "拒絕邀請加入車隊發生錯誤."
                };
            }
        }

        /// <summary>
        /// 更新車隊隊長
        /// </summary>
        /// <param name="examinerID">examinerID</param>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> UpdateTeamLeader(string examinerID, TeamInteractiveCommandDto teamInteractiveCommand)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new TeamCommandDto() { TeamID = teamInteractiveCommand.TeamID, ExaminerID = examinerID, TargetID = teamInteractiveCommand.MemberID });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, "api/Team/UpdateTeamLeader", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Leader Error >>> TemaID:{teamInteractiveCommand.TeamID} ExaminerID:{examinerID} TargetID:{teamInteractiveCommand.MemberID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "更新車隊隊長發生錯誤."
                };
            }
        }

        /// <summary>
        /// 更新車隊副隊長
        /// </summary>
        /// <param name="examinerID">examinerID</param>
        /// <param name="teamInteractiveCommand">teamInteractiveCommand</param>
        /// <param name="isAdd">isAdd</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> UpdateTeamViceLeader(string examinerID, TeamInteractiveCommandDto teamInteractiveCommand, bool isAdd)
        {
            try
            {
                string postData = JsonConvert.SerializeObject(new TeamCommandDto() { TeamID = teamInteractiveCommand.TeamID, ExaminerID = examinerID, TargetID = teamInteractiveCommand.MemberID });
                string apiUrl = isAdd ? "api/Team/UpdateTeamViceLeader/Add" : "api/Team/UpdateTeamViceLeader/Remove";
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.TeamService, apiUrl, postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Team Vice Leader Error >>> TemaID:{teamInteractiveCommand.TeamID} ExaminerID:{examinerID} TargetID:{teamInteractiveCommand.MemberID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "更新車隊副隊長發生錯誤."
                };
            }
        }

        #endregion 車隊互動資料
    }
}