using AutoMapper;
using GoBike.Interactive.Repository.Interface;
using GoBike.Interactive.Repository.Models;
using GoBike.Interactive.Service.Interface;
using GoBike.Interactive.Service.Models.Command;
using Microsoft.Extensions.Logging;
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
                bool verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!verifyInteractiveCommandResult)
                {
                    this.logger.LogError($"Add Blacklist Fail For Verify InteractiveCommand >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}");
                    return "加入黑名單失敗.";
                }

                //// 處理發起者互動資料
                InteractiveData initiatorInteractiveData = await this.GetInteractiveData(interactiveCommand.InitiatorID, true);
                if (initiatorInteractiveData == null)
                {
                    this.logger.LogError($"Add Blacklist Fail For Get Interactive Data >>> InitiatorID:{interactiveCommand.InitiatorID}");
                    return "加入黑名單失敗.";
                }

                bool updateInitiatorBlacklistResult = this.UpdateListHandler(initiatorInteractiveData.BlacklistIDs, interactiveCommand.ReceiverID, true);
                bool updateInitiatorFriendListResult = this.UpdateListHandler(initiatorInteractiveData.FriendListIDs, interactiveCommand.ReceiverID, false);
                bool updateInitiatorRequestListResult = this.UpdateListHandler(initiatorInteractiveData.RequestListIDs, interactiveCommand.ReceiverID, false);
                if (updateInitiatorBlacklistResult || updateInitiatorFriendListResult || updateInitiatorRequestListResult)
                {
                    bool initiatorUpdateResult = await this.interactiveRepository.UpdateInteractiveData(initiatorInteractiveData);
                    if (!initiatorUpdateResult)
                    {
                        return "黑名單更新失敗.";
                    }
                }

                //// 處理接收者互動資料
                InteractiveData receiverInteractiveData = await this.GetInteractiveData(interactiveCommand.ReceiverID, false);
                if (receiverInteractiveData != null)
                {
                    bool updateReceiverFriendListResult = this.UpdateListHandler(receiverInteractiveData.FriendListIDs, interactiveCommand.InitiatorID, false);
                    bool updateReceiverRequestListResult = this.UpdateListHandler(receiverInteractiveData.RequestListIDs, interactiveCommand.InitiatorID, false);
                    if (updateReceiverFriendListResult || updateReceiverRequestListResult)
                    {
                        bool receiverUpdateResult = await this.interactiveRepository.UpdateInteractiveData(receiverInteractiveData);
                        if (!receiverUpdateResult)
                        {
                            return "黑名單更新失敗.";
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
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>string</returns>
        public async Task<string> AddFriend(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                bool verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!verifyInteractiveCommandResult)
                {
                    this.logger.LogError($"Add Friend Fail For Verify InteractiveCommand >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}");
                    return "加入好友失敗.";
                }

                //// 驗證成為好友資格
                InteractiveData initiatorInteractiveData = await this.GetInteractiveData(interactiveCommand.InitiatorID, false);
                if (initiatorInteractiveData == null)
                {
                    this.logger.LogError($"Add Friend Fail For Get Initiator Interactive Data >>> InitiatorID:{interactiveCommand.InitiatorID}");
                    return "加入好友失敗.";
                }

                InteractiveData receiverInteractiveData = await this.GetInteractiveData(interactiveCommand.ReceiverID, false);
                if (receiverInteractiveData == null)
                {
                    this.logger.LogError($"Add Friend Fail For Get Receiver Interactive Data >>> ReceiverID:{interactiveCommand.ReceiverID}");
                    return "加入好友失敗.";
                }

                bool verifyBeFriendQualificationResult = this.VerifyBeFriendQualification(initiatorInteractiveData, receiverInteractiveData);
                if (!verifyBeFriendQualificationResult)
                {
                    return "加入好友失敗.";
                }

                //// 更新發起者互動資料
                bool updateInitiatorFriendListResult = this.UpdateListHandler(initiatorInteractiveData.FriendListIDs, interactiveCommand.ReceiverID, true);
                bool updateInitiatorRequestListResult = this.UpdateListHandler(initiatorInteractiveData.RequestListIDs, interactiveCommand.ReceiverID, false);
                if (updateInitiatorFriendListResult || updateInitiatorRequestListResult)
                {
                    bool initiatorUpdateResult = await this.interactiveRepository.UpdateInteractiveData(initiatorInteractiveData);
                    if (!initiatorUpdateResult)
                    {
                        return "好友名單更新失敗.";
                    }
                }

                //// 更新接收者互動資料
                bool updateReceiveFriendListResult = this.UpdateListHandler(receiverInteractiveData.FriendListIDs, interactiveCommand.InitiatorID, true);
                if (updateReceiveFriendListResult)
                {
                    bool receiverUpdateResult = await this.interactiveRepository.UpdateFriendList(receiverInteractiveData.MemberID, receiverInteractiveData.FriendListIDs);
                    if (!receiverUpdateResult)
                    {
                        return "好友名單更新失敗.";
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
                bool verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!verifyInteractiveCommandResult)
                {
                    this.logger.LogError($"Add Friend Request Fail For Verify InteractiveCommand >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}");
                    return "加入好友請求失敗.";
                }

                //// 驗證發送加入好友請求資格
                InteractiveData initiatorInteractiveData = await this.GetInteractiveData(interactiveCommand.InitiatorID, true);
                if (initiatorInteractiveData == null)
                {
                    this.logger.LogError($"Add Friend Request Fail For Get Initiator Interactive Data >>> InitiatorID:{interactiveCommand.InitiatorID}");
                    return "加入好友請求失敗.";
                }

                InteractiveData receiverInteractiveData = await this.GetInteractiveData(interactiveCommand.ReceiverID, true);
                if (receiverInteractiveData == null)
                {
                    this.logger.LogError($"Add Friend Request Fail For Get Receiver Interactive Data >>> ReceiverID:{interactiveCommand.ReceiverID}");
                    return "加入好友請求失敗.";
                }

                bool verifyFriendRequestQualificationResult = this.VerifyFriendRequestQualification(initiatorInteractiveData, receiverInteractiveData);
                if (!verifyFriendRequestQualificationResult)
                {
                    return "加入好友請求失敗.";
                }

                //// 更新接收者互動資料
                bool updateReceiveRequestListResult = this.UpdateListHandler(receiverInteractiveData.RequestListIDs, interactiveCommand.InitiatorID, true);
                if (updateReceiveRequestListResult)
                {
                    bool receiverUpdateResult = await this.interactiveRepository.UpdateRequestList(receiverInteractiveData.MemberID, receiverInteractiveData.RequestListIDs);
                    if (!receiverUpdateResult)
                    {
                        return "加入好友請求名單更新失敗.";
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
                bool verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!verifyInteractiveCommandResult)
                {
                    this.logger.LogError($"Delete Blacklist Fail For Verify InteractiveCommand >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}");
                    return "刪除黑名單失敗.";
                }

                InteractiveData interactiveData = await this.GetInteractiveData(interactiveCommand.InitiatorID, false);
                if (interactiveData == null)
                {
                    this.logger.LogError($"Delete Blacklist Fail For Get Interactive Data >>> InitiatorID:{interactiveCommand.InitiatorID}");
                    return "刪除黑名單失敗.";
                }

                bool updateInitiatorBlacklistResult = this.UpdateListHandler(interactiveData.BlacklistIDs, interactiveCommand.ReceiverID, false);
                if (updateInitiatorBlacklistResult)
                {
                    bool updateBlacklistResult = await this.interactiveRepository.UpdateBlacklist(interactiveData.MemberID, interactiveData.BlacklistIDs);
                    if (!updateBlacklistResult)
                    {
                        return "黑名單更新失敗.";
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
                bool verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!verifyInteractiveCommandResult)
                {
                    this.logger.LogError($"Delete Friend Fail For Verify InteractiveCommand >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}");
                    return "刪除好友失敗.";
                }

                InteractiveData initiatorInteractiveData = await this.GetInteractiveData(interactiveCommand.InitiatorID, false);
                if (initiatorInteractiveData == null)
                {
                    this.logger.LogError($"Delete Friend Fail For Get Initiator Interactive Data >>> InitiatorID:{interactiveCommand.InitiatorID}");
                    return "刪除好友失敗.";
                }

                InteractiveData receiverInteractiveData = await this.GetInteractiveData(interactiveCommand.ReceiverID, false);
                if (receiverInteractiveData == null)
                {
                    this.logger.LogError($"Delete Friend Fail For Get Receiver Interactive Data >>> ReceiverID:{interactiveCommand.ReceiverID}");
                    return "刪除好友失敗.";
                }

                //// 更新發起者互動資料
                bool updateInitiatorFriendListResult = this.UpdateListHandler(initiatorInteractiveData.FriendListIDs, interactiveCommand.ReceiverID, false);
                if (updateInitiatorFriendListResult)
                {
                    bool initiatorUpdateResult = await this.interactiveRepository.UpdateFriendList(initiatorInteractiveData.MemberID, initiatorInteractiveData.FriendListIDs);
                    if (!initiatorUpdateResult)
                    {
                        return "好友名單更新失敗.";
                    }
                }

                //// 更新接收者互動資料
                bool updateReceiverFriendListResult = this.UpdateListHandler(receiverInteractiveData.FriendListIDs, interactiveCommand.InitiatorID, false);
                if (updateReceiverFriendListResult)
                {
                    bool receiverUpdateResult = await this.interactiveRepository.UpdateFriendList(receiverInteractiveData.MemberID, receiverInteractiveData.FriendListIDs);
                    if (!receiverUpdateResult)
                    {
                        return "好友名單更新失敗.";
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
                bool verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!verifyInteractiveCommandResult)
                {
                    this.logger.LogError($"Delete Request For Add Friend Fail For Verify InteractiveCommand >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}");
                    return "刪除加入好友請求失敗.";
                }

                InteractiveData receiverInteractiveData = await this.GetInteractiveData(interactiveCommand.ReceiverID, false);
                if (receiverInteractiveData == null)
                {
                    this.logger.LogError($"Delete Request For Add Friend Fail For Get Interactive Data >>> ReceiverID:{interactiveCommand.ReceiverID}");
                    return "刪除加入好友請求失敗.";
                }

                //// 更新接收者互動資料
                bool updateReceiverRequestListResult = this.UpdateListHandler(receiverInteractiveData.RequestListIDs, interactiveCommand.InitiatorID, false);
                if (updateReceiverRequestListResult)
                {
                    bool receiverUpdateResult = await this.interactiveRepository.UpdateRequestList(receiverInteractiveData.MemberID, receiverInteractiveData.RequestListIDs);
                    if (!receiverUpdateResult)
                    {
                        return "加入好友請求名單更新失敗.";
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
                bool verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, false);
                if (!verifyInteractiveCommandResult)
                {
                    this.logger.LogError($"Get Add Friend Request List Fail For Verify InteractiveCommand >>> InitiatorID:{interactiveCommand.InitiatorID}");
                    return Tuple.Create<IEnumerable<string>, string>(null, "取得加入好友請求名單失敗.");
                }

                InteractiveData interactiveData = await this.GetInteractiveData(interactiveCommand.InitiatorID, false);
                return Tuple.Create(interactiveData == null ? null : interactiveData.RequestListIDs, string.Empty);
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
                bool verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, false);
                if (!verifyInteractiveCommandResult)
                {
                    this.logger.LogError($"Get Blacklist Fail For Verify InteractiveCommand >>> InitiatorID:{interactiveCommand.InitiatorID}");
                    return Tuple.Create<IEnumerable<string>, string>(null, "取得黑名單失敗.");
                }

                InteractiveData interactiveData = await this.GetInteractiveData(interactiveCommand.InitiatorID, false);
                return Tuple.Create(interactiveData == null ? null : interactiveData.BlacklistIDs, string.Empty);
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
                bool verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, false);
                if (!verifyInteractiveCommandResult)
                {
                    this.logger.LogError($"Get Friend List Fail For Verify InteractiveCommand >>> InitiatorID:{interactiveCommand.InitiatorID}");
                    return Tuple.Create<IEnumerable<string>, string>(null, "取得好友名單失敗.");
                }

                InteractiveData interactiveData = await this.GetInteractiveData(interactiveCommand.InitiatorID, false);
                return Tuple.Create(interactiveData == null ? null : interactiveData.FriendListIDs, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Friend List Error >>> InitiatorID:{interactiveCommand.InitiatorID}\n{ex}");
                return Tuple.Create<IEnumerable<string>, string>(null, "取得好友名單發生錯誤.");
            }
        }

        /// <summary>
        /// 取得會員互動狀態
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <returns>Tuple(int, string)</returns>
        public async Task<Tuple<int, string>> GetMemberInteractiveStatus(InteractiveCommandDto interactiveCommand)
        {
            try
            {
                bool verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!verifyInteractiveCommandResult)
                {
                    this.logger.LogError($"Get Member Interactive Status Fail For Verify InteractiveCommand >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}");
                    return Tuple.Create((int)InteractiveStatusType.None, "取得會員互動狀態失敗.");
                }

                InteractiveData receiverInteractiveData = await this.GetInteractiveData(interactiveCommand.ReceiverID, false);
                if (receiverInteractiveData != null)
                {
                    if (receiverInteractiveData.BlacklistIDs.Contains(interactiveCommand.InitiatorID))
                    {
                        return Tuple.Create((int)InteractiveStatusType.None, "對方已設該會員為黑名單.");
                    }

                    if (receiverInteractiveData.FriendListIDs.Contains(interactiveCommand.InitiatorID))
                    {
                        return Tuple.Create((int)InteractiveStatusType.Friend, string.Empty);
                    }

                    if (receiverInteractiveData.RequestListIDs.Contains(interactiveCommand.InitiatorID))
                    {
                        return Tuple.Create((int)InteractiveStatusType.Request, string.Empty);
                    }
                }

                InteractiveData initiatorInteractiveData = await this.GetInteractiveData(interactiveCommand.InitiatorID, true);
                if (initiatorInteractiveData != null)
                {
                    if (initiatorInteractiveData.BlacklistIDs.Contains(interactiveCommand.ReceiverID))
                    {
                        return Tuple.Create((int)InteractiveStatusType.Black, string.Empty);
                    }

                    if (initiatorInteractiveData.FriendListIDs.Contains(interactiveCommand.ReceiverID))
                    {
                        return Tuple.Create((int)InteractiveStatusType.Friend, string.Empty);
                    }

                    if (initiatorInteractiveData.RequestListIDs.Contains(interactiveCommand.ReceiverID))
                    {
                        return Tuple.Create((int)InteractiveStatusType.RequestHandler, string.Empty);
                    }
                }

                return Tuple.Create((int)InteractiveStatusType.None, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Member Interactive Status Error >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}\n{ex}");
                return Tuple.Create((int)InteractiveStatusType.None, "取得會員互動狀態發生錯誤.");
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
                bool verifyInteractiveCommandResult = this.VerifyInteractiveCommand(interactiveCommand, true, true);
                if (!verifyInteractiveCommandResult)
                {
                    this.logger.LogError($"Reject Be Friend Fail For Verify InteractiveCommand >>> InitiatorID:{interactiveCommand.InitiatorID} ReceiverID:{interactiveCommand.ReceiverID}");
                    return "拒絕加入好友失敗.";
                }

                InteractiveData initiatorInteractiveData = await this.GetInteractiveData(interactiveCommand.InitiatorID, false);
                if (initiatorInteractiveData == null)
                {
                    this.logger.LogError($"Reject Be Friend Fail For Get Initiator Interactive Data >>> InitiatorID:{interactiveCommand.InitiatorID}");
                    return "拒絕加入好友失敗.";
                }

                //// 更新發起者互動資料
                bool updateInitiatorRequestListResult = this.UpdateListHandler(initiatorInteractiveData.RequestListIDs, interactiveCommand.ReceiverID, false);
                if (updateInitiatorRequestListResult)
                {
                    bool initiatorUpdateResult = await this.interactiveRepository.UpdateRequestList(initiatorInteractiveData.MemberID, initiatorInteractiveData.RequestListIDs);
                    if (!initiatorUpdateResult)
                    {
                        return "加入好友請求名單更新失敗.";
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
        /// <returns>InteractiveData</returns>
        private async Task<InteractiveData> GetInteractiveData(string memberID, bool isCreate)
        {
            if (string.IsNullOrEmpty(memberID))
            {
                return null;
            }

            InteractiveData interactiveData = await this.interactiveRepository.GetInteractiveData(memberID);
            if (interactiveData == null && isCreate)
            {
                interactiveData = this.CreateInteractiveData(memberID);
                bool isCreateSuccess = await this.interactiveRepository.CreateInteractiveData(interactiveData);
                if (!isCreateSuccess)
                {
                    return null;
                }
            }

            return interactiveData;
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
        /// 驗證互動指令資料
        /// </summary>
        /// <param name="interactiveCommand">interactiveCommand</param>
        /// <param name="isVerifyInitiator">isVerifyInitiator</param>
        /// <param name="isVerifyReceiver">isVerifyReceiver</param>
        /// <returns>bool</returns>
        private bool VerifyInteractiveCommand(InteractiveCommandDto interactiveCommand, bool isVerifyInitiator, bool isVerifyReceiver)
        {
            if (interactiveCommand == null)
            {
                return false;
            }

            if (isVerifyInitiator)
            {
                if (string.IsNullOrEmpty(interactiveCommand.InitiatorID))
                {
                    return false;
                }
            }

            if (isVerifyReceiver)
            {
                if (string.IsNullOrEmpty(interactiveCommand.ReceiverID))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// 驗證成為好友資格
        /// </summary>
        /// <param name="initiatorData">initiatorData</param>
        /// <param name="receiverData">receiverData</param>
        /// <returns>bool</returns>
        private bool VerifyBeFriendQualification(InteractiveData initiatorData, InteractiveData receiverData)
        {
            if (initiatorData.BlacklistIDs.Contains(receiverData.MemberID))
            {
                this.logger.LogError($"Verify Be Friend Qualification Error For Initiator BlacklistIDs >>> InitiatorID:{initiatorData.MemberID} ReceiverID:{receiverData.MemberID}");
                return false;
            }

            if (!initiatorData.RequestListIDs.Contains(receiverData.MemberID))
            {
                this.logger.LogError($"Verify Be Friend Qualification Error For Initiator RequestListIDs >>> InitiatorID:{initiatorData.MemberID} ReceiverID:{receiverData.MemberID}");
                return false;
            }

            if (receiverData.BlacklistIDs.Contains(initiatorData.MemberID))
            {
                this.logger.LogError($"Verify Be Friend Qualification Error For Receive BlacklistIDs >>> InitiatorID:{initiatorData.MemberID} ReceiverID:{receiverData.MemberID}");
                return false;
            }

            return true;
        }

        /// <summary>
        /// 驗證發送加入好友請求資格
        /// </summary>
        /// <param name="initiatorData">initiatorData</param>
        /// <param name="receiverData">receiverData</param>
        /// <returns>bool</returns>
        private bool VerifyFriendRequestQualification(InteractiveData initiatorData, InteractiveData receiverData)
        {
            if (initiatorData.BlacklistIDs.Contains(receiverData.MemberID))
            {
                this.logger.LogError($"Verify Friend Request Qualification Error For Initiator BlacklistIDs >>> InitiatorID:{initiatorData.MemberID} ReceiverID:{receiverData.MemberID}");
                return false;
            }

            if (initiatorData.FriendListIDs.Contains(receiverData.MemberID))
            {
                this.logger.LogError($"Verify Friend Request Qualification Error For Initiator FriendListIDs >>> InitiatorID:{initiatorData.MemberID} ReceiverID:{receiverData.MemberID}");
                return false;
            }

            if (initiatorData.RequestListIDs.Contains(receiverData.MemberID))
            {
                this.logger.LogError($"Verify Friend Request Qualification Error For Initiator RequestListIDs >>> InitiatorID:{initiatorData.MemberID} ReceiverID:{receiverData.MemberID}");
                return false;
            }

            if (receiverData.BlacklistIDs.Contains(initiatorData.MemberID))
            {
                this.logger.LogError($"Verify Friend Request Qualification Error For Receiver BlacklistIDs >>> InitiatorID:{initiatorData.MemberID} ReceiverID:{receiverData.MemberID}");
                return false;
            }

            if (receiverData.FriendListIDs.Contains(initiatorData.MemberID))
            {
                this.logger.LogError($"Verify Friend Request Qualification Error For Receiver FriendListIDs >>> InitiatorID:{initiatorData.MemberID} ReceiverID:{receiverData.MemberID}");
                return false;
            }

            return true;
        }
    }
}