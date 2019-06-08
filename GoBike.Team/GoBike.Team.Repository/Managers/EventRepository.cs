using GoBike.Team.Core.Applibs;
using GoBike.Team.Repository.Interface;
using GoBike.Team.Repository.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoBike.Team.Repository.Managers
{
    /// <summary>
    /// 活動資料庫
    /// </summary>
    public class EventRepository : IEventRepository
    {
        /// <summary>
        /// eventDatas
        /// </summary>
        private readonly IMongoCollection<EventData> eventDatas;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<EventRepository> logger;

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
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Delete Event Data Fail For IsAcknowledged >>> EventID:{eventID}");
                    return false;
                }

                if (result.DeletedCount == 0)
                {
                    this.logger.LogError($"Delete Event Data Fail For DeletedCount >>> EventID:{eventID}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Event Data Error >>> EventID:{eventID}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 刪除車隊所有活動資料
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteEventDataListOfTeam(string teamID)
        {
            try
            {
                FilterDefinition<EventData> filter = Builders<EventData>.Filter.Eq("TeamID", teamID);
                DeleteResult result = await this.eventDatas.DeleteManyAsync(filter);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Delete Event Data List Of Team Fail For IsAcknowledged >>> TeamID:{teamID}");
                    return false;
                }

                if (result.DeletedCount == 0)
                {
                    this.logger.LogError($"Delete Event Data List Of Team Fail For DeletedCount >>> TeamID:{teamID}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Event Data List Of Team Error >>> TeamID:{teamID}\n{ex}");
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
        /// 取得車隊活動資料列表
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>EventDatas</returns>
        public async Task<IEnumerable<EventData>> GetEventDataListOfTeam(string teamID)
        {
            try
            {
                FilterDefinition<EventData> filter = Builders<EventData>.Filter.Eq("TeamID", teamID);
                return await this.eventDatas.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Event Data List Of Team Error >>> TeamID:{teamID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得會員活動資料列表
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>EventDatas</returns>
        public async Task<IEnumerable<EventData>> GetEventDataListOfMember(string member)
        {
            try
            {
                return await this.eventDatas.Find(data => data.JoinMemberList.Contains(member)).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Event Data List Of Member Error >>> Member:{member}\n{ex}");
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
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Update Event Data Fail For IsAcknowledged >>> Data:{JsonConvert.SerializeObject(eventData)}");
                    return false;
                }

                if (result.ModifiedCount == 0)
                {
                    this.logger.LogError($"Update Event Data Fail For ModifiedCount >>> Data:{JsonConvert.SerializeObject(eventData)}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Event Data Error >>> Data:{JsonConvert.SerializeObject(eventData)}\n{ex}");
                return false;
            }
        }
    }
}