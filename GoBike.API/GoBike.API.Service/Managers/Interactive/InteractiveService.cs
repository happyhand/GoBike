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

        public async Task<ResponseResultDto> AddBlacklist(InteractiveInfoDto interactiveInfo)
        {
            throw new System.NotImplementedException();
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

        public async Task<ResponseResultDto> AddFriendRequest(InteractiveInfoDto interactiveInfo)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseResultDto> DeleteBlacklist(InteractiveInfoDto interactiveInfo)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseResultDto> DeleteFriend(InteractiveInfoDto interactiveInfo)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseResultDto> DeleteRequestForAddFriend(InteractiveInfoDto interactiveInfo)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseResultDto> GetAddFriendRequestList(InteractiveInfoDto interactiveInfo)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseResultDto> GetBlacklist(InteractiveInfoDto interactiveInfo)
        {
            throw new System.NotImplementedException();
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

        public async Task<ResponseResultDto> RejectBeFriend(InteractiveInfoDto interactiveInfo)
        {
            throw new System.NotImplementedException();
        }

        public async Task<ResponseResultDto> SearchFriend(InteractiveInfoDto interactiveInfo)
        {
            throw new System.NotImplementedException();
        }
    }
}