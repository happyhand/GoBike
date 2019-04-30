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
                return result.IsAcknowledged && result.DeletedCount > 0;
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
        /// <returns>Tuple(bool, string)</returns>
        public async Task<Tuple<bool, string>> UpdateBlacklist(string memberID, IEnumerable<string> blacklistIDs)
        {
            try
            {
                FilterDefinition<InteractiveData> filter = Builders<InteractiveData>.Filter.Eq("MemberID", memberID);
                UpdateDefinition<InteractiveData> update = Builders<InteractiveData>.Update.Set(data => data.BlacklistIDs, blacklistIDs);
                UpdateResult result = await this.interactiveDatas.UpdateOneAsync(filter, update);
                if (!result.IsAcknowledged)
                {
                    return Tuple.Create(false, "無法更新黑名單.");
                }

                if (result.ModifiedCount == 0)
                {
                    return Tuple.Create(false, "黑名單未更改.");
                }

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Blacklist Error >>> MemberID:{memberID} BlacklistIDs:{JsonConvert.SerializeObject(blacklistIDs)}\n{ex}");
                return Tuple.Create(false, "更新黑名單發生錯誤.");
            }
        }

        /// <summary>
        /// 更新好友名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="friendListIDs">friendListIDs</param>
        /// <returns>Tuple(bool, string)</returns>
        public async Task<Tuple<bool, string>> UpdateFriendList(string memberID, IEnumerable<string> friendListIDs)
        {
            try
            {
                FilterDefinition<InteractiveData> filter = Builders<InteractiveData>.Filter.Eq("MemberID", memberID);
                UpdateDefinition<InteractiveData> update = Builders<InteractiveData>.Update.Set(data => data.FriendListIDs, friendListIDs);
                UpdateResult result = await this.interactiveDatas.UpdateOneAsync(filter, update);
                if (!result.IsAcknowledged)
                {
                    return Tuple.Create(false, "無法更新好友名單.");
                }

                if (result.ModifiedCount == 0)
                {
                    return Tuple.Create(false, "好友名單未更改.");
                }

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Friend List Error >>> MemberID:{memberID} FriendListIDs:{JsonConvert.SerializeObject(friendListIDs)}\n{ex}");
                return Tuple.Create(false, "更新好友名單發生錯誤.");
            }
        }

        /// <summary>
        /// 更新互動資料
        /// </summary>
        /// <param name="interactiveData">interactiveData</param>
        /// <returns>Tuple(bool, string)</returns>
        public async Task<Tuple<bool, string>> UpdateInteractiveData(InteractiveData interactiveData)
        {
            try
            {
                FilterDefinition<InteractiveData> filter = Builders<InteractiveData>.Filter.Eq("_id", interactiveData.Id);
                ReplaceOneResult result = await this.interactiveDatas.ReplaceOneAsync(filter, interactiveData);
                if (!result.IsAcknowledged)
                {
                    return Tuple.Create(false, "無法更新互動資料.");
                }

                if (result.ModifiedCount == 0)
                {
                    return Tuple.Create(false, "互動資料未更改.");
                }

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Interactive Data Error >>> Data:{JsonConvert.SerializeObject(interactiveData)}\n{ex}");
                return Tuple.Create(false, "更新互動資料發生錯誤.");
            }
        }

        /// <summary>
        /// 更新請求名單
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="requestListIDs">requestListIDs</param>
        /// <returns>Tuple(bool, string)</returns>
        public async Task<Tuple<bool, string>> UpdateRequestList(string memberID, IEnumerable<string> requestListIDs)
        {
            try
            {
                FilterDefinition<InteractiveData> filter = Builders<InteractiveData>.Filter.Eq("MemberID", memberID);
                UpdateDefinition<InteractiveData> update = Builders<InteractiveData>.Update.Set(data => data.RequestListIDs, requestListIDs);
                UpdateResult result = await this.interactiveDatas.UpdateOneAsync(filter, update);
                if (!result.IsAcknowledged)
                {
                    return Tuple.Create(false, "無法更新請求名單.");
                }

                if (result.ModifiedCount == 0)
                {
                    return Tuple.Create(false, "請求名單未更改.");
                }

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Request List Error >>> MemberID:{memberID} RequestListIDs:{JsonConvert.SerializeObject(requestListIDs)}\n{ex}");
                return Tuple.Create(false, "更新請求名單發生錯誤.");
            }
        }
    }
}