using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interactive;
using GoBike.API.Service.Interface.Interactive;
using GoBike.API.Service.Models.Member;
using GoBike.API.Service.Models.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoBike.API.Service.Managers.Interactive
{
    /// <summary>
    /// 好友服務
    /// </summary>
    public class InteractiveService : IInteractiveService
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<InteractiveService> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public InteractiveService(ILogger<InteractiveService> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 加入黑名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> AddBlacklist(InteractiveInfoDto interactiveInfo)
        {
            if (string.IsNullOrEmpty(interactiveInfo.InitiatorID) || string.IsNullOrEmpty(interactiveInfo.PassiveID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編號無效."
                };
            }

            if (interactiveInfo.InitiatorID.Equals(interactiveInfo.PassiveID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "無法設定本人為黑名單."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(interactiveInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Blacklist/add", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.StatusCode == HttpStatusCode.OK,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Blacklist Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "加入黑名單發生錯誤."
                };
            }
        }

        /// <summary>
        /// 加入好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> AddFriend(InteractiveInfoDto interactiveInfo)
        {
            if (string.IsNullOrEmpty(interactiveInfo.InitiatorID) || string.IsNullOrEmpty(interactiveInfo.PassiveID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編號無效."
                };
            }

            if (interactiveInfo.InitiatorID.Equals(interactiveInfo.PassiveID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "無法設定本人為好友."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(interactiveInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/add", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.StatusCode == HttpStatusCode.OK,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Friend Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "加入好友發生錯誤."
                };
            }
        }

        /// <summary>
        /// 加入好友請求
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> AddFriendRequest(InteractiveInfoDto interactiveInfo)
        {
            if (string.IsNullOrEmpty(interactiveInfo.InitiatorID) || string.IsNullOrEmpty(interactiveInfo.PassiveID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編號無效."
                };
            }

            if (interactiveInfo.InitiatorID.Equals(interactiveInfo.PassiveID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "無法對本人發送加入好友請求."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(interactiveInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/request", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.StatusCode == HttpStatusCode.OK,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Friend Request Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "加入好友請求發生錯誤."
                };
            }
        }

        /// <summary>
        /// 刪除黑名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> DeleteBlacklist(InteractiveInfoDto interactiveInfo)
        {
            if (string.IsNullOrEmpty(interactiveInfo.InitiatorID) || string.IsNullOrEmpty(interactiveInfo.PassiveID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編號無效."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(interactiveInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Blacklist/delete", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.StatusCode == HttpStatusCode.OK,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Blacklist Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "刪除黑名單發生錯誤."
                };
            }
        }

        /// <summary>
        /// 刪除好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> DeleteFriend(InteractiveInfoDto interactiveInfo)
        {
            if (string.IsNullOrEmpty(interactiveInfo.InitiatorID) || string.IsNullOrEmpty(interactiveInfo.PassiveID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編號無效."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(interactiveInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/delete", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.StatusCode == HttpStatusCode.OK,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Friend Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "刪除好友發生錯誤."
                };
            }
        }

        /// <summary>
        /// 刪除加入好友請求
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> DeleteRequestForAddFriend(InteractiveInfoDto interactiveInfo)
        {
            if (string.IsNullOrEmpty(interactiveInfo.InitiatorID) || string.IsNullOrEmpty(interactiveInfo.PassiveID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編號無效."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(interactiveInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/deleteRequest", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.StatusCode == HttpStatusCode.OK,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Request For Add Friend Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "刪除加入好友請求發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得加入好友請求名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetAddFriendRequestList(InteractiveInfoDto interactiveInfo)
        {
            if (string.IsNullOrEmpty(interactiveInfo.InitiatorID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編號無效."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(interactiveInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/requestList", postData);
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<MemberInfoDto>>()
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
                this.logger.LogError($"Get Add Friend Request List Error >>> InitiatorID:{interactiveInfo.InitiatorID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得加入好友請求名單發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得黑名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetBlacklist(InteractiveInfoDto interactiveInfo)
        {
            if (string.IsNullOrEmpty(interactiveInfo.InitiatorID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編號無效."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(interactiveInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Blacklist/get", postData);
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<MemberInfoDto>>()
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
                this.logger.LogError($"Get Blacklist Error >>> InitiatorID:{interactiveInfo.InitiatorID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得黑名單發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得好友名單
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetFriendList(InteractiveInfoDto interactiveInfo)
        {
            if (string.IsNullOrEmpty(interactiveInfo.InitiatorID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編號無效."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(interactiveInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/list", postData);
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<MemberInfoDto>>()
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
                this.logger.LogError($"Get Friend List Error >>> MemberID:{interactiveInfo.InitiatorID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得好友名單發生錯誤."
                };
            }
        }

        /// <summary>
        /// 拒絕加入好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> RejectBeFriend(InteractiveInfoDto interactiveInfo)
        {
            if (string.IsNullOrEmpty(interactiveInfo.InitiatorID) || string.IsNullOrEmpty(interactiveInfo.PassiveID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編號無效."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(interactiveInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/reject", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.StatusCode == HttpStatusCode.OK,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Be Friend Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "拒絕加入好友發生錯誤."
                };
            }
        }

        /// <summary>
        /// 搜尋好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> SearchFriend(InteractiveInfoDto interactiveInfo)
        {
            if (string.IsNullOrEmpty(interactiveInfo.InitiatorID) || string.IsNullOrEmpty(interactiveInfo.PassiveID))
            {
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "會員編號無效."
                };
            }

            try
            {
                string postData = JsonConvert.SerializeObject(interactiveInfo);
                HttpResponseMessage httpResponseMessage = await Utility.POST(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/search", postData);
                if (httpResponseMessage.StatusCode == HttpStatusCode.OK)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<MemberInfoDto>()
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
                this.logger.LogError($"Search Friend Error >>> InitiatorID:{interactiveInfo.InitiatorID} PassiveID:{interactiveInfo.PassiveID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "搜尋好友發生錯誤."
                };
            }
        }
    }
}