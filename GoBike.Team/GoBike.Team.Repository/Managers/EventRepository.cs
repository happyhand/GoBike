using GoBike.Team.Core.Applibs;
using GoBike.Team.Repository.Interface;
using GoBike.Team.Repository.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Team.Repository.Managers
{
    /// <summary>
    /// 活動資料庫
    /// </summary>
    public class EventRepository : IEventRepository
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<EventRepository> logger;

        /// <summary>
        /// eventDatas
        /// </summary>
        private readonly IMongoCollection<EventData> eventDatas;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public EventRepository(ILogger<EventRepository> logger)
        {
            this.logger = logger;
            IMongoClient client = new MongoClient(AppSettingHelper.Appsetting.MongoDBConfig.ConnectionString);
            IMongoDatabase db = client.GetDatabase(AppSettingHelper.Appsetting.MongoDBConfig.TeamDatabase);
            this.eventDatas = db.GetCollection<EventData>(AppSettingHelper.Appsetting.MongoDBConfig.CollectionFlag.Event);
        }

        /// <summary>
        /// 建立活動資料
        /// </summary>
        /// <param name="eventData">eventData</param>
        /// <returns>bool</returns>
        public async Task<bool> CreateEventData(EventData eventData)
        {
            try
            {
                await this.eventDatas.InsertOneAsync(eventData);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Event Data Error >>> Data:{JsonConvert.SerializeObject(eventData)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 刪除活動資料
        /// </summary>
        /// <param name="eventID">eventID</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteEventData(string eventID)
        {
            try
            {
                FilterDefinition<EventData> filter = Builders<EventData>.Filter.Eq("EventID", eventID);
                DeleteResult result = await this.eventDatas.DeleteManyAsync(filter);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Event Data Error >>> EventID:{eventID}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 取得活動資料
        /// </summary>
        /// <param name="eventID">eventID</param>
        /// <returns>EventData</returns>
        public async Task<EventData> GetEventData(string eventID)
        {
            try
            {
                FilterDefinition<EventData> filter = Builders<EventData>.Filter.Eq("EventID", eventID);
                return await this.eventDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Event Data Error >>> EventID:{eventID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得活動列表資料
        /// </summary>
        /// <param name="eventIDs">eventIDs</param>
        /// <returns>EventDatas</returns>
        public async Task<IEnumerable<EventData>> GetEventDataList(IEnumerable<string> eventIDs)
        {
            try
            {
                FilterDefinition<EventData> filter = Builders<EventData>.Filter.In("EventID", eventIDs);
                return await this.eventDatas.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Event Data List Error >>> TeamIDs:{JsonConvert.SerializeObject(eventIDs)}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 更新活動資料
        /// </summary>
        /// <param name="eventData">eventData</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateEventData(EventData eventData)
        {
            try
            {
                FilterDefinition<EventData> filter = Builders<EventData>.Filter.Eq("_id", eventData.Id);
                ReplaceOneResult result = await this.eventDatas.ReplaceOneAsync(filter, eventData);
                return result.IsAcknowledged && result.ModifiedCount > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Event Data Error >>> Data:{JsonConvert.SerializeObject(eventData)}\n{ex}");
                return false;
            }
        }
    }
}