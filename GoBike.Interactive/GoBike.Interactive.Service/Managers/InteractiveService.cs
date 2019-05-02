using AutoMapper;
using GoBike.Interactive.Repository.Interface;
using GoBike.Interactive.Repository.Models;
using GoBike.Interactive.Service.Interface;
using GoBike.Interactive.Service.Models.Command;
using GoBike.Interactive.Service.Models.Data;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoBike.Interactive.Service.Managers
{
    /// <summary>
    /// 互動服務
    /// </summary>
    public class InteractiveService : IInteractiveService
    {
        /// <summary>
        /// interactiveRepository
        /// </summary>
        private readonly IInteractiveRepository interactiveRepository;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<InteractiveService> logger;

        /// <summary>
        /// mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="interactiveRepository">interactiveRepository</param>
        /// <param name="memberRepository">memberRepository</param>
        public InteractiveService(ILogger<InteractiveService> logger, IMapper mapper, IInteractiveRepository interactiveRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.interactiveRepository = interactiveRepository;
        }

        /// <summary>
        /// 加入黑名單
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>string</returns>
        public async Task<string> AddBlacklist(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyInteractiveCommandResult))
                {
                    return verifyInteractiveCommandResult;
                }

                //// 處理發起者互動資料
                Tuple<InteractiveData, string> getInitiatorInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.InitiatorID, true);
                if (!string.IsNullOrEmpty(getInitiatorInteractiveDataReuslt.Item2))
                {
                    return getInitiatorInteractiveDataReuslt.Item2;
                }

                InteractiveData initiatorInteractiveData = getInitiatorInteractiveDataReuslt.Item1;
                bool updateInitiatorBlacklistResult = this.UpdateListHandler(initiatorInteractiveData.BlacklistIDs, interactiveCommand.ReceiverID, true);
                bool updateInitiatorFriendListResult = this.UpdateListHandler(initiatorInteractiveData.FriendListIDs, interactiveCommand.ReceiverID, false);
                bool updateInitiatorRequestListResult = this.UpdateListHandler(initiatorInteractiveData.RequestListIDs, interactiveCommand.ReceiverID, false);
                if (updateInitiatorBlacklistResult || updateInitiatorFriendListResult || updateInitiatorRequestListResult)
                {
                    Tuple<bool, string> initiatorUpdateResult = await this.interactiveRepository.UpdateInteractiveData(initiatorInteractiveData);
                    if (!initiatorUpdateResult.Item1)
                    {
                        this.logger.LogError($"Add Blacklist Fail For Update Initiator Interactive Data >>> Data:{JsonConvert.SerializeObject(initiatorInteractiveData)}");
                        return initiatorUpdateResult.Item2;
                    }
                }

                //// 處理接收者互動資料
                Tuple<InteractiveData, string> getReceiverInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.ReceiverID, false);
                if (!string.IsNullOrEmpty(getReceiverInteractiveDataReuslt.Item2))
                {
                    return getReceiverInteractiveDataReuslt.Item2;
                }

                InteractiveData receiverInteractiveData = getReceiverInteractiveDataReuslt.Item1;
                if (receiverInteractiveData != null)
                {
                    bool updateReceiverFriendListResult = this.UpdateListHandler(receiverInteractiveData.FriendListIDs, interactiveCommand.InitiatorID, false);
                    bool updateReceiverRequestListResult = this.UpdateListHandler(receiverInteractiveData.RequestListIDs, interactiveCommand.InitiatorID, false);
                    if (updateReceiverFriendListResult || updateReceiverRequestListResult)
                    {
                        Tuple<bool, string> receiverUpdateResult = await this.interactiveRepository.UpdateInteractiveData(receiverInteractiveData);
                        if (!receiverUpdateResult.Item1)
                        {
                            this.logger.LogError($"Add Blacklist Fail For Update Receiver Interactive Data >>> Data:{JsonConvert.SerializeObject(receiverInteractiveData)}");
                            return receiverUpdateResult.Item2;
                        }
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Blacklist Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return "加入黑名單發生錯誤.";
            }
        }

        /// <summary>
        /// 加入好友
        /// </summary>
        /// <param name="interactiveInfo">interactiveInfo</param>
        /// <returns>string</returns>
        public async Task<string> AddFriend(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyInteractiveCommandResult))
                {
                    return verifyInteractiveCommandResult;
                }

                //// 驗證成為好友資格
                Tuple<InteractiveData, string> getInitiatorInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.InitiatorID, false);
                if (!string.IsNullOrEmpty(getInitiatorInteractiveDataReuslt.Item2))
                {
                    return getInitiatorInteractiveDataReuslt.Item2;
                }

                InteractiveData initiatorInteractiveData = getInitiatorInteractiveDataReuslt.Item1;
                Tuple<InteractiveData, string> getReceiverInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.ReceiverID, false);
                if (!string.IsNullOrEmpty(getReceiverInteractiveDataReuslt.Item2))
                {
                    return getReceiverInteractiveDataReuslt.Item2;
                }

                InteractiveData receiverInteractiveData = getReceiverInteractiveDataReuslt.Item1;
                string verifyBeFriendQualificationResult = this.VerifyBeFriendQualification(initiatorInteractiveData, receiverInteractiveData);
                if (!string.IsNullOrEmpty(verifyBeFriendQualificationResult))
                {
                    return verifyBeFriendQualificationResult;
                }

                //// 更新發起者互動資料
                bool updateInitiatorFriendListResult = this.UpdateListHandler(initiatorInteractiveData.FriendListIDs, interactiveCommand.ReceiverID, true);
                bool updateInitiatorRequestListResult = this.UpdateListHandler(initiatorInteractiveData.RequestListIDs, interactiveCommand.ReceiverID, false);
                if (updateInitiatorFriendListResult || updateInitiatorRequestListResult)
                {
                    Tuple<bool, string> initiatorUpdateResult = await this.interactiveRepository.UpdateInteractiveData(initiatorInteractiveData);
                    if (!initiatorUpdateResult.Item1)
                    {
                        this.logger.LogError($"Add Friend Fail For Update Initiator Interactive Data >>> Data:{JsonConvert.SerializeObject(initiatorInteractiveData)}");
                        return initiatorUpdateResult.Item2;
                    }
                }

                //// 更新接收者互動資料
                bool updateReceiveFriendListResult = this.UpdateListHandler(receiverInteractiveData.FriendListIDs, interactiveCommand.InitiatorID, true);
                if (updateReceiveFriendListResult)
                {
                    Tuple<bool, string> receiverUpdateResult = await this.interactiveRepository.UpdateFriendList(receiverInteractiveData.MemberID, receiverInteractiveData.FriendListIDs);
                    if (!receiverUpdateResult.Item1)
                    {
                        this.logger.LogError($"Add Friend Fail For Update Receiver Friend List >>> MemberID:{receiverInteractiveData.MemberID} FriendListIDs:{JsonConvert.SerializeObject(receiverInteractiveData.FriendListIDs)}");
                        return receiverUpdateResult.Item2;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Friend Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return "加入好友發生錯誤.";
            }
        }

        /// <summary>
        /// 加入好友請求
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>string</returns>
        public async Task<string> AddFriendRequest(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyInteractiveCommandResult))
                {
                    return verifyInteractiveCommandResult;
                }

                //// 驗證發送加入好友請求資格
                Tuple<InteractiveData, string> getInitiatorInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.InitiatorID, true);
                if (!string.IsNullOrEmpty(getInitiatorInteractiveDataReuslt.Item2))
                {
                    return getInitiatorInteractiveDataReuslt.Item2;
                }

                InteractiveData initiatorInteractiveData = getInitiatorInteractiveDataReuslt.Item1;
                Tuple<InteractiveData, string> getReceiverInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.ReceiverID, true);
                if (!string.IsNullOrEmpty(getReceiverInteractiveDataReuslt.Item2))
                {
                    return getReceiverInteractiveDataReuslt.Item2;
                }

                InteractiveData receiverInteractiveData = getReceiverInteractiveDataReuslt.Item1;
                string verifyFriendRequestQualificationResult = this.VerifyFriendRequestQualification(initiatorInteractiveData, receiverInteractiveData);
                if (!string.IsNullOrEmpty(verifyFriendRequestQualificationResult))
                {
                    return verifyFriendRequestQualificationResult;
                }

                //// 更新接收者互動資料
                bool updateReceiveRequestListResult = this.UpdateListHandler(receiverInteractiveData.RequestListIDs, interactiveCommand.InitiatorID, true);
                if (updateReceiveRequestListResult)
                {
                    Tuple<bool, string> receiverUpdateResult = await this.interactiveRepository.UpdateRequestList(receiverInteractiveData.MemberID, receiverInteractiveData.RequestListIDs);
                    if (!receiverUpdateResult.Item1)
                    {
                        this.logger.LogError($"Add Friend Request Fail For Update Receiver Request List >>> MemberID:{receiverInteractiveData.MemberID} RequestListIDs:{JsonConvert.SerializeObject(receiverInteractiveData.RequestListIDs)}");
                        return receiverUpdateResult.Item2;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Friend Request Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return "加入好友請求發生錯誤.";
            }
        }

        /// <summary>
        /// 刪除黑名單
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>string</returns>
        public async Task<string> DeleteBlacklist(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyInteractiveCommandResult))
                {
                    return verifyInteractiveCommandResult;
                }

                Tuple<InteractiveData, string> getInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.InitiatorID, false);
                if (!string.IsNullOrEmpty(getInteractiveDataReuslt.Item2))
                {
                    return getInteractiveDataReuslt.Item2;
                }

                InteractiveData interactiveData = getInteractiveDataReuslt.Item1;
                if (interactiveData == null)
                {
                    return "無互動資料.";
                }

                bool updateInitiatorBlacklistResult = this.UpdateListHandler(interactiveData.BlacklistIDs, interactiveCommand.ReceiverID, false);
                if (updateInitiatorBlacklistResult)
                {
                    Tuple<bool, string> updateInteractiveDataResult = await this.interactiveRepository.UpdateBlacklist(interactiveData.MemberID, interactiveData.BlacklistIDs);
                    if (!updateInteractiveDataResult.Item1)
                    {
                        this.logger.LogError($"Delete Blacklist Fail For Update Black list >>> MemberID:{interactiveData.MemberID} BlacklistIDs:{JsonConvert.SerializeObject(interactiveData.BlacklistIDs)}");
                        return updateInteractiveDataResult.Item2;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Blacklist Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return "刪除黑名單發生錯誤.";
            }
        }

        /// <summary>
        /// 刪除好友
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>string</returns>
        public async Task<string> DeleteFriend(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyInteractiveCommandResult))
                {
                    return verifyInteractiveCommandResult;
                }

                Tuple<InteractiveData, string> getInitiatorInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.InitiatorID, false);
                if (!string.IsNullOrEmpty(getInitiatorInteractiveDataReuslt.Item2))
                {
                    return getInitiatorInteractiveDataReuslt.Item2;
                }

                InteractiveData initiatorInteractiveData = getInitiatorInteractiveDataReuslt.Item1;
                if (initiatorInteractiveData == null)
                {
                    return "無發起者的互動資料.";
                }

                Tuple<InteractiveData, string> getReceiverInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.ReceiverID, false);
                if (!string.IsNullOrEmpty(getReceiverInteractiveDataReuslt.Item2))
                {
                    return getReceiverInteractiveDataReuslt.Item2;
                }

                InteractiveData receiverInteractiveData = getReceiverInteractiveDataReuslt.Item1;
                if (receiverInteractiveData == null)
                {
                    return "無接收者的互動資料.";
                }

                //// 更新發起者互動資料
                bool updateInitiatorFriendListResult = this.UpdateListHandler(initiatorInteractiveData.FriendListIDs, interactiveCommand.ReceiverID, false);
                if (updateInitiatorFriendListResult)
                {
                    Tuple<bool, string> initiatorUpdateResult = await this.interactiveRepository.UpdateFriendList(initiatorInteractiveData.MemberID, initiatorInteractiveData.FriendListIDs);
                    if (!initiatorUpdateResult.Item1)
                    {
                        this.logger.LogError($"Delete Friend Fail For Update Initiator Friend List >>> MemberID:{initiatorInteractiveData.MemberID} FriendListIDs:{JsonConvert.SerializeObject(initiatorInteractiveData.FriendListIDs)}");
                        return initiatorUpdateResult.Item2;
                    }
                }

                //// 更新接收者互動資料
                bool updateReceiverFriendListResult = this.UpdateListHandler(receiverInteractiveData.FriendListIDs, interactiveCommand.InitiatorID, false);
                if (updateReceiverFriendListResult)
                {
                    Tuple<bool, string> receiverUpdateResult = await this.interactiveRepository.UpdateFriendList(receiverInteractiveData.MemberID, receiverInteractiveData.FriendListIDs);
                    if (!receiverUpdateResult.Item1)
                    {
                        this.logger.LogError($"Delete Friend Fail For Update Receiver Friend List >>> MemberID:{receiverInteractiveData.MemberID} FriendListIDs:{JsonConvert.SerializeObject(receiverInteractiveData.FriendListIDs)}");
                        return receiverUpdateResult.Item2;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Friend Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return "刪除好友發生錯誤.";
            }
        }

        /// <summary>
        /// 刪除加入好友請求
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>string</returns>
        public async Task<string> DeleteRequestForAddFriend(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyInteractiveCommandResult))
                {
                    return verifyInteractiveCommandResult;
                }

                Tuple<InteractiveData, string> getReceiverInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.ReceiverID, false);
                if (!string.IsNullOrEmpty(getReceiverInteractiveDataReuslt.Item2))
                {
                    return getReceiverInteractiveDataReuslt.Item2;
                }

                InteractiveData receiverInteractiveData = getReceiverInteractiveDataReuslt.Item1;
                if (receiverInteractiveData == null)
                {
                    return "無接收者的互動資料.";
                }

                //// 更新接收者互動資料
                bool updateReceiverRequestListResult = this.UpdateListHandler(receiverInteractiveData.RequestListIDs, interactiveCommand.InitiatorID, false);
                if (updateReceiverRequestListResult)
                {
                    Tuple<bool, string> receiverUpdateResult = await this.interactiveRepository.UpdateRequestList(receiverInteractiveData.MemberID, receiverInteractiveData.RequestListIDs);
                    if (!receiverUpdateResult.Item1)
                    {
                        this.logger.LogError($"Delete Request For Add Friend Fail For Update Receiver Request List >>> MemberID:{receiverInteractiveData.MemberID} RequestListIDs:{JsonConvert.SerializeObject(receiverInteractiveData.RequestListIDs)}");
                        return receiverUpdateResult.Item2;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Request For Add Friend Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return "刪除加入好友請求發生錯誤.";
            }
        }

        /// <summary>
        /// 取得加入好友請求名單
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>Tuple(strings, string)</returns>
        public async Task<Tuple<IEnumerable<string>, string>> GetAddFriendRequestList(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, false);
                if (!string.IsNullOrEmpty(verifyInteractiveCommandResult))
                {
                    return Tuple.Create<IEnumerable<string>, string>(null, verifyInteractiveCommandResult);
                }

                Tuple<InteractiveData, string> getInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.InitiatorID, true);
                if (!string.IsNullOrEmpty(getInteractiveDataReuslt.Item2))
                {
                    return Tuple.Create<IEnumerable<string>, string>(null, getInteractiveDataReuslt.Item2);
                }

                InteractiveData interactiveData = getInteractiveDataReuslt.Item1;
                return Tuple.Create(interactiveData.RequestListIDs, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Add Friend Request List Error >>> InitiatorID:{interactiveCommand.InitiatorID}\n{ex}");
                return Tuple.Create<IEnumerable<string>, string>(null, "取得加入好友請求名單發生錯誤.");
            }
        }

        /// <summary>
        /// 取得黑名單
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>Tuple(strings, string)</returns>
        public async Task<Tuple<IEnumerable<string>, string>> GetBlacklist(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, false);
                if (!string.IsNullOrEmpty(verifyInteractiveCommandResult))
                {
                    return Tuple.Create<IEnumerable<string>, string>(null, verifyInteractiveCommandResult);
                }

                Tuple<InteractiveData, string> getInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.InitiatorID, true);
                if (!string.IsNullOrEmpty(getInteractiveDataReuslt.Item2))
                {
                    return Tuple.Create<IEnumerable<string>, string>(null, getInteractiveDataReuslt.Item2);
                }

                InteractiveData interactiveData = getInteractiveDataReuslt.Item1;
                return Tuple.Create(interactiveData.BlacklistIDs, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Blacklist Error >>> InitiatorID:{interactiveCommand.InitiatorID}\n{ex}");
                return Tuple.Create<IEnumerable<string>, string>(null, "取得黑名單發生錯誤.");
            }
        }

        /// <summary>
        /// 取得好友名單
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>Tuple(strings, string)</returns>
        public async Task<Tuple<IEnumerable<string>, string>> GetFriendList(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, false);
                if (!string.IsNullOrEmpty(verifyInteractiveCommandResult))
                {
                    return Tuple.Create<IEnumerable<string>, string>(null, verifyInteractiveCommandResult);
                }

                Tuple<InteractiveData, string> getInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.InitiatorID, true);
                if (!string.IsNullOrEmpty(getInteractiveDataReuslt.Item2))
                {
                    return Tuple.Create<IEnumerable<string>, string>(null, getInteractiveDataReuslt.Item2);
                }

                InteractiveData interactiveData = getInteractiveDataReuslt.Item1;
                return Tuple.Create(interactiveData.FriendListIDs, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Friend List Error >>> InitiatorID:{interactiveCommand.InitiatorID}\n{ex}");
                return Tuple.Create<IEnumerable<string>, string>(null, "取得好友名單發生錯誤.");
            }
        }

        /// <summary>
        /// 拒絕加入好友
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>string</returns>
        public async Task<string> RejectBeFriend(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyInteractiveCommandResult))
                {
                    return verifyInteractiveCommandResult;
                }

                Tuple<InteractiveData, string> getInitiatorInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.InitiatorID, false);
                if (!string.IsNullOrEmpty(getInitiatorInteractiveDataReuslt.Item2))
                {
                    return getInitiatorInteractiveDataReuslt.Item2;
                }

                InteractiveData initiatorInteractiveData = getInitiatorInteractiveDataReuslt.Item1;
                if (initiatorInteractiveData == null)
                {
                    return "無發起者的互動資料.";
                }

                //// 更新發起者互動資料
                bool updateInitiatorRequestListResult = this.UpdateListHandler(initiatorInteractiveData.RequestListIDs, interactiveCommand.ReceiverID, false);
                if (updateInitiatorRequestListResult)
                {
                    Tuple<bool, string> initiatorUpdateResult = await this.interactiveRepository.UpdateRequestList(initiatorInteractiveData.MemberID, initiatorInteractiveData.RequestListIDs);
                    if (!initiatorUpdateResult.Item1)
                    {
                        this.logger.LogError($"Reject Be Friend Fail For Update Initiator Request List >>> MemberID:{initiatorInteractiveData.MemberID} RequestListIDs:{JsonConvert.SerializeObject(initiatorInteractiveData.RequestListIDs)}");
                        return initiatorUpdateResult.Item2;
                    }
                }

                return string.Empty;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Reject Be Friend Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return "拒絕加入好友發生錯誤.";
            }
        }

        /// <summary>
        /// 搜尋好友
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>Tuple(InteractiveInfoDto, string)</returns>
        public async Task<Tuple<InteractiveInfoDto, string>> SearchFriend(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                string verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!string.IsNullOrEmpty(verifyInteractiveCommandResult))
                {
                    return Tuple.Create<InteractiveInfoDto, string>(null, verifyInteractiveCommandResult);
                }

                Tuple<InteractiveData, string> getReceiverInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.ReceiverID, false);
                if (!string.IsNullOrEmpty(getReceiverInteractiveDataReuslt.Item2))
                {
                    return Tuple.Create<InteractiveInfoDto, string>(null, getReceiverInteractiveDataReuslt.Item2);
                }

                InteractiveData receiverInteractiveData = getReceiverInteractiveDataReuslt.Item1;
                if (receiverInteractiveData != null)
                {
                    if (receiverInteractiveData.BlacklistIDs.Contains(interactiveCommand.InitiatorID))
                    {
                        return Tuple.Create<InteractiveInfoDto, string>(null, "對方已設該會員為黑名單.");
                    }

                    if (receiverInteractiveData.FriendListIDs.Contains(interactiveCommand.InitiatorID))
                    {
                        return Tuple.Create(new InteractiveInfoDto() { MemberID = interactiveCommand.ReceiverID, Status = (int)InteractiveStatusType.Friend }, string.Empty);
                    }

                    if (receiverInteractiveData.RequestListIDs.Contains(interactiveCommand.InitiatorID))
                    {
                        return Tuple.Create(new InteractiveInfoDto() { MemberID = interactiveCommand.ReceiverID, Status = (int)InteractiveStatusType.Request }, string.Empty);
                    }
                }

                Tuple<InteractiveData, string> getInitiatorInteractiveDataReuslt = await this.GetInteractiveData(interactiveCommand.InitiatorID, true);
                if (!string.IsNullOrEmpty(getInitiatorInteractiveDataReuslt.Item2))
                {
                    return Tuple.Create<InteractiveInfoDto, string>(null, getInitiatorInteractiveDataReuslt.Item2);
                }

                InteractiveData initiatorInteractiveData = getInitiatorInteractiveDataReuslt.Item1;
                if (initiatorInteractiveData.BlacklistIDs.Contains(interactiveCommand.ReceiverID))
                {
                    return Tuple.Create(new InteractiveInfoDto() { MemberID = interactiveCommand.ReceiverID, Status = (int)InteractiveStatusType.Black }, string.Empty);
                }

                if (initiatorInteractiveData.FriendListIDs.Contains(interactiveCommand.ReceiverID))
                {
                    return Tuple.Create(new InteractiveInfoDto() { MemberID = interactiveCommand.ReceiverID, Status = (int)InteractiveStatusType.Friend }, string.Empty);
                }

                if (initiatorInteractiveData.RequestListIDs.Contains(interactiveCommand.ReceiverID))
                {
                    return Tuple.Create(new InteractiveInfoDto() { MemberID = interactiveCommand.ReceiverID, Status = (int)InteractiveStatusType.RequestHandler }, string.Empty);
                }

                return Tuple.Create(new InteractiveInfoDto() { MemberID = interactiveCommand.ReceiverID, Status = (int)InteractiveStatusType.None }, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Search Friend Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return Tuple.Create<InteractiveInfoDto, string>(null, "搜尋好友發生錯誤.");
            }
        }

        /// <summary>
        /// 創建新互動資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveData</returns>
        private InteractiveData CreateInteractiveData(string memberID)
        {
            return new InteractiveData()
            {
                MemberID = memberID,
                BlacklistIDs = new List<string>(),
                FriendListIDs = new List<string>(),
                RequestListIDs = new List<string>()
            };
        }

        /// <summary>
        /// 取得互動資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="isCreate">isCreate</param>
        /// <returns>Tuple(InteractiveData, string)</returns>
        private async Task<Tuple<InteractiveData, string>> GetInteractiveData(string memberID, bool isCreate)
        {
            if (string.IsNullOrEmpty(memberID))
            {
                return Tuple.Create<InteractiveData, string>(null, "會員編號無效.");
            }

            InteractiveData interactiveData = await this.interactiveRepository.GetInteractiveData(memberID);
            if (interactiveData == null && isCreate)
            {
                interactiveData = this.CreateInteractiveData(memberID);
                bool isCreateSuccess = await this.interactiveRepository.CreateInteractiveData(interactiveData);
                if (!isCreateSuccess)
                {
                    this.logger.LogError($"Get Interactive Data Fail For Create Interactive Data >>> InteractiveData:{JsonConvert.SerializeObject(interactiveData)}");
                    return Tuple.Create<InteractiveData, string>(null, "建立互動資料失敗.");
                }
            }

            return Tuple.Create<InteractiveData, string>(interactiveData, string.Empty);
        }

        /// <summary>
        /// 名單更新處理
        /// </summary>
        /// <param name="list">list</param>
        /// <param name="targetID">targetID</param>
        /// <param name="isAdd">isAdd</param>
        /// <returns>bool</returns>
        private bool UpdateListHandler(IEnumerable<string> list, string targetID, bool isAdd)
        {
            if (isAdd)
            {
                if (!list.Contains(targetID))
                {
                    (list as List<string>).Add(targetID);
                    return true;
                }

                return false;
            }
            else
            {
                if (list.Contains(targetID))
                {
                    (list as List<string>).Remove(targetID);
                    return true;
                }

                return false;
            }
        }

        /// <summary>
        /// 驗證成為好友資格
        /// </summary>
        /// <param name="initiatorData">initiatorData</param>
        /// <param name="receiveData">receiveData</param>
        /// <returns>string</returns>
        private string VerifyBeFriendQualification(InteractiveData initiatorData, InteractiveData receiveData)
        {
            if (initiatorData == null)
            {
                return "無發起者的互動資料.";
            }

            if (receiveData == null)
            {
                return "無接收者的互動資料.";
            }

            if (initiatorData.BlacklistIDs.Contains(receiveData.MemberID))
            {
                return "對方已設為黑名單.";
            }

            if (!initiatorData.RequestListIDs.Contains(receiveData.MemberID))
            {
                return "對方未提出加入好友請求.";
            }

            if (receiveData.BlacklistIDs.Contains(initiatorData.MemberID))
            {
                return "對方已設該會員為黑名單.";
            }

            return string.Empty;
        }

        /// <summary>
        /// 驗證發送加入好友請求資格
        /// </summary>
        /// <param name="initiatorData">initiatorData</param>
        /// <param name="receiveData">receiveData</param>
        /// <returns>string</returns>
        private string VerifyFriendRequestQualification(InteractiveData initiatorData, InteractiveData receiveData)
        {
            if (initiatorData == null)
            {
                return "無發起者的互動資料.";
            }

            if (receiveData == null)
            {
                return "無接收者的互動資料.";
            }

            if (initiatorData.BlacklistIDs.Contains(receiveData.MemberID))
            {
                return "對方已設為黑名單.";
            }

            if (initiatorData.FriendListIDs.Contains(receiveData.MemberID))
            {
                return "已為好友關係.";
            }

            if (receiveData.BlacklistIDs.Contains(initiatorData.MemberID))
            {
                return "對方已設該會員為黑名單.";
            }

            if (receiveData.FriendListIDs.Contains(initiatorData.MemberID))
            {
                return "對方已與該會員為好友關係.";
            }

            return string.Empty;
        }

        /// <summary>
        /// 驗證互動指令資料
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <param name="isVerifyInitiator">isVerifyInitiator</param>
        /// <param name="isVerifyReceiver">isVerifyReceiver</param>
        /// <returns>string</returns>
        private string VerifyInteractiveCommand(InteractiveCommandDto interactiveCommand, bool isVerifyInitiator, bool isVerifyReceiver)
        {
            if (interactiveCommand == null)
            {
                return "互動指令資料不存在.";
            }

            if (isVerifyInitiator)
            {
                if (string.IsNullOrEmpty(interactiveCommand.InitiatorID))
                {
                    return "發起者會員編號無效.";
                }
            }

            if (isVerifyReceiver)
            {
                if (string.IsNullOrEmpty(interactiveCommand.ReceiverID))
                {
                    return "接收者會員編號無效.";
                }
            }

            return string.Empty;
        }
    }
}