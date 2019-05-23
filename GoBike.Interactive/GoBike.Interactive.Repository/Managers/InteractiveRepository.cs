using GoBike.Interactive.Core.Applibs;
using GoBike.Interactive.Repository.Interface;
using GoBike.Interactive.Repository.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Interactive.Repository.Managers
{
    /// <summary>
    /// 互動資料庫
    /// </summary>
    public class InteractiveRepository : IInteractiveRepository
    {
        /// <summary>
        /// friendDatas
        /// </summary>
        private readonly IMongoCollection<InteractiveData> interactiveDatas;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<InteractiveRepository> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="options">options</param>
        public InteractiveRepository(ILogger<InteractiveRepository> logger)
        {
            this.logger = logger;
            IMongoClient client = new MongoClient(AppSettingHelper.Appsetting.MongoDBConfig.ConnectionString);
            IMongoDatabase db = client.GetDatabase(AppSettingHelper.Appsetting.MongoDBConfig.MemberDatabase);
            this.interactiveDatas = db.GetCollection<InteractiveData>(AppSettingHelper.Appsetting.MongoDBConfig.CollectionFlag.Interactive);
        }

        /// <summary>
        /// 建立互動資料
        /// </summary>
        /// <param name="interactiveData">interactiveData</param>
        /// <returns>bool</returns>
        public async Task<bool> CreateInteractiveData(InteractiveData interactiveData)
        {
            try
            {
                await this.interactiveDatas.InsertOneAsync(interactiveData);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Interactive Data Error >>> Data:{JsonConvert.SerializeObject(interactiveData)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 刪除互動資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteInteractiveData(string memberID)
        {
            try
            {
                FilterDefinition<InteractiveData> filter = Builders<InteractiveData>.Filter.Eq("MemberID", memberID);
                DeleteResult result = await this.interactiveDatas.DeleteManyAsync(filter);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Delete Interactive Data Fail For IsAcknowledged >>> MemberID:{memberID}");
                    return false;
                }

                if (result.DeletedCount == 0)
                {
                    this.logger.LogError($"Delete Interactive Data Fail For DeletedCount >>> MemberID:{memberID}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Interactive Data Error >>> MemberID:{memberID}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 取得互動資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveData</returns>
        public async Task<InteractiveData> GetInteractiveData(string memberID)
        {
            try
            {
                FilterDefinition<InteractiveData> filter = Builders<InteractiveData>.Filter.Eq("MemberID", memberID);
                return await this.interactiveDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Interactive Data Error >>> MemberID:{memberID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 更新黑名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="blacklistIDs">blacklistIDs</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateBlacklist(string memberID, IEnumerable<string> blacklistIDs)
        {
            try
            {
                FilterDefinition<InteractiveData> filter = Builders<InteractiveData>.Filter.Eq("MemberID", memberID);
                UpdateDefinition<InteractiveData> update = Builders<InteractiveData>.Update.Set(data => data.BlacklistIDs, blacklistIDs);
                UpdateResult result = await this.interactiveDatas.UpdateOneAsync(filter, update);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Update Blacklist Fail For IsAcknowledged >>> MemberID:{memberID} BlacklistIDs:{JsonConvert.SerializeObject(blacklistIDs)}");
                    return false;
                }

                if (result.ModifiedCount == 0)
                {
                    this.logger.LogError($"Update Blacklist Fail For ModifiedCount >>> MemberID:{memberID} BlacklistIDs:{JsonConvert.SerializeObject(blacklistIDs)}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Blacklist Error >>> MemberID:{memberID} BlacklistIDs:{JsonConvert.SerializeObject(blacklistIDs)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 更新好友名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="friendListIDs">friendListIDs</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateFriendList(string memberID, IEnumerable<string> friendListIDs)
        {
            try
            {
                FilterDefinition<InteractiveData> filter = Builders<InteractiveData>.Filter.Eq("MemberID", memberID);
                UpdateDefinition<InteractiveData> update = Builders<InteractiveData>.Update.Set(data => data.FriendListIDs, friendListIDs);
                UpdateResult result = await this.interactiveDatas.UpdateOneAsync(filter, update);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Update Friend List Fail For IsAcknowledged >>> MemberID:{memberID} FriendListIDs:{JsonConvert.SerializeObject(friendListIDs)}");
                    return false;
                }

                if (result.ModifiedCount == 0)
                {
                    this.logger.LogError($"Update Friend List Fail For ModifiedCount >>> MemberID:{memberID} FriendListIDs:{JsonConvert.SerializeObject(friendListIDs)}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Friend List Error >>> MemberID:{memberID} FriendListIDs:{JsonConvert.SerializeObject(friendListIDs)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 更新互動資料
        /// </summary>
        /// <param name="interactiveData">interactiveData</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateInteractiveData(InteractiveData interactiveData)
        {
            try
            {
                FilterDefinition<InteractiveData> filter = Builders<InteractiveData>.Filter.Eq("_id", interactiveData.Id);
                ReplaceOneResult result = await this.interactiveDatas.ReplaceOneAsync(filter, interactiveData);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Update Interactive Data Fail For IsAcknowledged >>> Data:{JsonConvert.SerializeObject(interactiveData)}");
                    return false;
                }

                if (result.ModifiedCount == 0)
                {
                    this.logger.LogError($"Update Interactive Data Fail For ModifiedCount >>> Data:{JsonConvert.SerializeObject(interactiveData)}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Interactive Data Error >>> Data:{JsonConvert.SerializeObject(interactiveData)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 更新請求名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="requestListIDs">requestListIDs</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateRequestList(string memberID, IEnumerable<string> requestListIDs)
        {
            try
            {
                FilterDefinition<InteractiveData> filter = Builders<InteractiveData>.Filter.Eq("MemberID", memberID);
                UpdateDefinition<InteractiveData> update = Builders<InteractiveData>.Update.Set(data => data.RequestListIDs, requestListIDs);
                UpdateResult result = await this.interactiveDatas.UpdateOneAsync(filter, update);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Update Request List Fail For IsAcknowledged >>> MemberID:{memberID} RequestListIDs:{JsonConvert.SerializeObject(requestListIDs)}");
                    return false;
                }

                if (result.ModifiedCount == 0)
                {
                    this.logger.LogError($"Update Request List Fail For ModifiedCount >>> MemberID:{memberID} RequestListIDs:{JsonConvert.SerializeObject(requestListIDs)}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Request List Error >>> MemberID:{memberID} RequestListIDs:{JsonConvert.SerializeObject(requestListIDs)}\n{ex}");
                return false;
            }
        }
    }
}