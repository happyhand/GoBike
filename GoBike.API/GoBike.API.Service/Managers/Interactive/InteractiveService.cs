using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Interactive;
using GoBike.API.Service.Models.Command;
using GoBike.API.Service.Models.Data;
using GoBike.API.Service.Models.Member;
using GoBike.API.Service.Models.Response;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> AddBlacklist(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(interactiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Blacklist/AddBlacklist", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Blacklist Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
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
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> AddFriend(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(interactiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/AddFriend", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Friend Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
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
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> AddFriendRequest(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(interactiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/AddFriendRequest", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Friend Request Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
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
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> DeleteBlacklist(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(interactiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Blacklist/DeleteBlacklist", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Blacklist Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
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
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> DeleteFriend(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(interactiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/DeleteFriend", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Friend Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
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
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> DeleteRequestForAddFriend(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(interactiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/DeleteRequestForAddFriend", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Request For Add Friend Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
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
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetAddFriendRequestList(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(interactiveCommand, true, false);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(interactiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/GetAddFriendRequestList", postData);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                    };
                }

                IEnumerable<string> memberIDs = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<string>>();
                Tuple<IEnumerable<MemberInfoDto>, string> getMemebrInfoListResult = await this.GetMemebrInfoList(memberIDs, (int)InteractiveStatusType.Request);
                if (!string.IsNullOrEmpty(getMemebrInfoListResult.Item2))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = getMemebrInfoListResult.Item2
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = true,
                    Data = getMemebrInfoListResult.Item1
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Add Friend Request List Error >>> InitiatorID:{interactiveCommand.InitiatorID}\n{ex}");
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
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetBlacklist(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(interactiveCommand, true, false);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(interactiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Blacklist/GetBlacklist", postData);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                    };
                }

                IEnumerable<string> memberIDs = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<string>>();
                Tuple<IEnumerable<MemberInfoDto>, string> getMemebrInfoListResult = await this.GetMemebrInfoList(memberIDs, (int)InteractiveStatusType.Black);
                if (!string.IsNullOrEmpty(getMemebrInfoListResult.Item2))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = getMemebrInfoListResult.Item2
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = true,
                    Data = getMemebrInfoListResult.Item1
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Blacklist Error >>> InitiatorID:{interactiveCommand.InitiatorID}\n{ex}");
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
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetFriendList(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(interactiveCommand, true, false);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(interactiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/GetFriendList", postData);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                    };
                }

                IEnumerable<string> memberIDs = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<string>>();
                Tuple<IEnumerable<MemberInfoDto>, string> getMemebrInfoListResult = await this.GetMemebrInfoList(memberIDs, (int)InteractiveStatusType.Friend);
                if (!string.IsNullOrEmpty(getMemebrInfoListResult.Item2))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = getMemebrInfoListResult.Item2
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = true,
                    Data = getMemebrInfoListResult.Item1
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Friend List Error >>> InitiatorID:{interactiveCommand.InitiatorID}\n{ex}");
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
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> RejectBeFriend(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(interactiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/RejectBeFriend", postData);
                return new ResponseResultDto()
                {
                    Ok = httpResponseMessage.IsSuccessStatusCode,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Be Friend Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
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
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> SearchFriend(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyMemberResult = await this.VerifyMemebr(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyMemberResult))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = verifyMemberResult
                    };
                }

                string postData = JsonConvert.SerializeObject(interactiveCommand);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.InteractiveService, "api/Friend/SearchFriend", postData);
                if (!httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                    };
                }

                InteractiveInfoDto interactiveInfo = await httpResponseMessage.Content.ReadAsAsync<InteractiveInfoDto>();
                Tuple<MemberInfoDto, string> getMemebrInfoResult = await this.GetMemebrInfo(interactiveInfo.MemberID, interactiveInfo.Status);
                if (!string.IsNullOrEmpty(getMemebrInfoResult.Item2))
                {
                    return new ResponseResultDto()
                    {
                        Ok = false,
                        Data = getMemebrInfoResult.Item2
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = true,
                    Data = getMemebrInfoResult.Item1
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Friend Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "搜尋好友發生錯誤."
                };
            }
        }

        /// <summary>
        /// 取得會員資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="interactiveStatus">interactiveStatus</param>
        /// <returns>Tuple(MemberInfoDto, string)</returns>
        private async Task<Tuple<MemberInfoDto, string>> GetMemebrInfo(string memberID, int interactiveStatus)
        {
            if (string.IsNullOrEmpty(memberID))
            {
                return Tuple.Create<MemberInfoDto, string>(null, "會員編號無效.");
            }

            try
            {
                string postData = JsonConvert.SerializeObject(new MemberBaseDto() { MemberID = memberID });
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/GetMemberInfo", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    MemberInfoDto memberInfo = await httpResponseMessage.Content.ReadAsAsync<MemberInfoDto>();
                    memberInfo.InteractiveStatus = interactiveStatus;
                    return Tuple.Create(memberInfo, string.Empty);
                }

                return Tuple.Create<MemberInfoDto, string>(null, await httpResponseMessage.Content.ReadAsAsync<string>());
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Memebr Info Error >>> MemberID:{memberID} InteractiveStatus:{interactiveStatus}\n{ex}");
                return Tuple.Create<MemberInfoDto, string>(null, "取得會員資料發生錯誤.");
            }
        }

        /// <summary>
        /// 取得會員資料列表
        /// </summary>
        /// <param name="memberIDs">memberIDs</param>
        /// <param name="interactiveStatus">interactiveStatus</param>
        /// <returns>Tuple(MemberInfoDtos, string)</returns>
        private async Task<Tuple<IEnumerable<MemberInfoDto>, string>> GetMemebrInfoList(IEnumerable<string> memberIDs, int interactiveStatus)
        {
            if (memberIDs == null || memberIDs.Count() == 0)
            {
                return Tuple.Create<IEnumerable<MemberInfoDto>, string>(new List<MemberInfoDto>(), string.Empty);
            }

            try
            {
                string postData = JsonConvert.SerializeObject(memberIDs);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/GetMemberInfo/List", postData);
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    IEnumerable<MemberInfoDto> memberInfos = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<MemberInfoDto>>();
                    foreach (MemberInfoDto memberInfo in memberInfos)
                    {
                        memberInfo.InteractiveStatus = interactiveStatus;
                    }

                    return Tuple.Create(memberInfos, string.Empty);
                }

                return Tuple.Create<IEnumerable<MemberInfoDto>, string>(null, await httpResponseMessage.Content.ReadAsAsync<string>());
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Memebr Info List Error >>> MemberIDs:{JsonConvert.SerializeObject(memberIDs)} InteractiveStatus:{interactiveStatus}\n{ex}");
                return Tuple.Create<IEnumerable<MemberInfoDto>, string>(null, "取得會員資料列表發生錯誤.");
            }
        }

        /// <summary>
        /// 驗證會員資料
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <param name="isVerifyInitiator">isVerifyInitiator</param>
        /// <param name="isVerifyReceiver">isVerifyReceiver</param>
        /// <returns>string</returns>
        private async Task<string> VerifyMemebr(InteractiveCommandDto interactiveCommand, bool isVerifyInitiator, bool isVerifyReceiver)
        {
            try
            {
                if (interactiveCommand == null)
                {
                    return "互動指令資料不存在.";
                }

                List<string> verifyMemberIDList = new List<string>();
                if (isVerifyInitiator)
                {
                    if (string.IsNullOrEmpty(interactiveCommand.InitiatorID))
                    {
                        return "發起者會員編號無效.";
                    }

                    if (interactiveCommand.InitiatorID.Equals(interactiveCommand.ReceiverID))
                    {
                        return "互動對象不得為會員本身.";
                    }

                    verifyMemberIDList.Add(interactiveCommand.InitiatorID);
                }

                if (isVerifyReceiver)
                {
                    if (string.IsNullOrEmpty(interactiveCommand.ReceiverID))
                    {
                        return "接收者會員編號無效.";
                    }

                    if (interactiveCommand.ReceiverID.Equals(interactiveCommand.InitiatorID))
                    {
                        return "互動對象不得為會員本身.";
                    }

                    verifyMemberIDList.Add(interactiveCommand.ReceiverID);
                }

                if (verifyMemberIDList.Count == 0)
                {
                    return "無查詢資料.";
                }

                string postData = JsonConvert.SerializeObject(verifyMemberIDList);
                HttpResponseMessage httpResponseMessage = await Utility.ApiPost(AppSettingHelper.Appsetting.ServiceDomain.MemberService, "api/VerifyMember", postData);
                return httpResponseMessage.IsSuccessStatusCode ? string.Empty : "無會員資料.";
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Verify Memebr Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID} IsVerifyInitiator:{isVerifyInitiator} IsVerifyReceiver:{isVerifyReceiver}\n{ex}");
                return "驗證會員資料發生錯誤.";
            }
        }
    }
}