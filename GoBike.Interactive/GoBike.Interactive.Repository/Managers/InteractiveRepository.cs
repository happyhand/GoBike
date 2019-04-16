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
        /// <param name="initiatorID">initiatorID</param>
        /// <param name="passiveID">passiveID</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteInteractiveData(string initiatorID, string passiveID)
        {
            try
            {
                FilterDefinitionBuilder<InteractiveData> builder = Builders<InteractiveData>.Filter;
                List<FilterDefinition<InteractiveData>> filters = new List<FilterDefinition<InteractiveData>>()
                    {
                        builder.Eq("InitiatorID", initiatorID),
                        builder.Eq("PassiveID", passiveID),
                    };
                DeleteResult result = await this.interactiveDatas.DeleteManyAsync(builder.And(filters));
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Interactive Data Error >>> InitiatorID:{initiatorID} PassiveID:{passiveID}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 取得加入好友請求列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveDatas</returns>
        public async Task<IEnumerable<InteractiveData>> GetAddFriendRequestList(string memberID)
        {
            try
            {
                FilterDefinitionBuilder<InteractiveData> builder = Builders<InteractiveData>.Filter;
                List<FilterDefinition<InteractiveData>> filters = new List<FilterDefinition<InteractiveData>>()
                    {
                        builder.Eq("PassiveID", memberID),
                        builder.Eq("Status", (int)FriendStatusType.Request),
                    };
                return await this.interactiveDatas.Find(builder.And(filters)).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Add Friend Request List Error >>> MemberID:{memberID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得黑名單列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveDatas</returns>
        public async Task<IEnumerable<InteractiveData>> GetBlacklist(string memberID)
        {
            try
            {
                FilterDefinitionBuilder<InteractiveData> builder = Builders<InteractiveData>.Filter;
                List<FilterDefinition<InteractiveData>> filters = new List<FilterDefinition<InteractiveData>>()
                    {
                        builder.Eq("InitiatorID", memberID),
                        builder.Eq("Status", (int)FriendStatusType.Black),
                    };
                return await this.interactiveDatas.Find(builder.And(filters)).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Blacklist Error >>> MemberID:{memberID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得好友列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveDatas</returns>
        public async Task<IEnumerable<InteractiveData>> GetFriendList(string memberID)
        {
            try
            {
                FilterDefinitionBuilder<InteractiveData> builder = Builders<InteractiveData>.Filter;
                List<FilterDefinition<InteractiveData>> filters = new List<FilterDefinition<InteractiveData>>()
                    {
                        builder.Eq("InitiatorID", memberID),
                        builder.Eq("Status", (int)FriendStatusType.Friend),
                    };
                return await this.interactiveDatas.Find(builder.And(filters)).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Friend List Error >>> MemberID:{memberID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得指定互動資料
        /// </summary>
        /// <param name="initiatorID">initiatorID</param>
        /// <param name="passiveID">passiveID</param>
        /// <returns>InteractiveData</returns>
        public async Task<InteractiveData> GetInteractiveData(string initiatorID, string passiveID)
        {
            try
            {
                FilterDefinitionBuilder<InteractiveData> builder = Builders<InteractiveData>.Filter;
                List<FilterDefinition<InteractiveData>> filters = new List<FilterDefinition<InteractiveData>>()
                    {
                        builder.Eq("InitiatorID", initiatorID),
                        builder.Eq("PassiveID", passiveID),
                    };
                return await this.interactiveDatas.Find(builder.And(filters)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Interactive Data Error >>> InitiatorID:{initiatorID} PassiveID:{passiveID}\n{ex}");
                return null;
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
                FilterDefinitionBuilder<InteractiveData> builder = Builders<InteractiveData>.Filter;
                List<FilterDefinition<InteractiveData>> filters = new List<FilterDefinition<InteractiveData>>()
                {
                    builder.Eq("InitiatorID", interactiveData.InitiatorID),
                    builder.Eq("PassiveID", interactiveData.PassiveID),
                };
                ReplaceOneResult result = await this.interactiveDatas.ReplaceOneAsync(builder.And(filters), interactiveData);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Interactive Data Error >>> Data:{JsonConvert.SerializeObject(interactiveData)}\n{ex}");
                return false;
            }
        }
    }
}