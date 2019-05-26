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
    /// 公告資料庫
    /// </summary>
    public class AnnouncementRepository : IAnnouncementRepository
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<AnnouncementRepository> logger;

        /// <summary>
        /// announcementDatas
        /// </summary>
        private readonly IMongoCollection<AnnouncementData> announcementDatas;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public AnnouncementRepository(ILogger<AnnouncementRepository> logger)
        {
            this.logger = logger;
            IMongoClient client = new MongoClient(AppSettingHelper.Appsetting.MongoDBConfig.ConnectionString);
            IMongoDatabase db = client.GetDatabase(AppSettingHelper.Appsetting.MongoDBConfig.TeamDatabase);
            this.announcementDatas = db.GetCollection<AnnouncementData>(AppSettingHelper.Appsetting.MongoDBConfig.CollectionFlag.Announcement);
        }

        /// <summary>
        /// 建立公告資料
        /// </summary>
        /// <param name="AnnouncementData">AnnouncementData</param>
        /// <returns>bool</returns>
        public async Task<bool> CreateAnnouncementData(AnnouncementData announcementData)
        {
            try
            {
                await this.announcementDatas.InsertOneAsync(announcementData);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Announcement Data Error >>> Data:{JsonConvert.SerializeObject(announcementData)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 刪除公告資料
        /// </summary>
        /// <param name="announcementID">announcementID</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteAnnouncementData(string announcementID)
        {
            try
            {
                FilterDefinition<AnnouncementData> filter = Builders<AnnouncementData>.Filter.Eq("AnnouncementID", announcementID);
                DeleteResult result = await this.announcementDatas.DeleteManyAsync(filter);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Delete Announcement Data Fail For IsAcknowledged >>> AnnouncementID:{announcementID}");
                    return false;
                }

                if (result.DeletedCount == 0)
                {
                    this.logger.LogError($"Delete Announcement Data Fail For DeletedCount >>> AnnouncementID:{announcementID}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Team Data Error >>> AnnouncementID:{announcementID}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 取得公告資料
        /// </summary>
        /// <param name="announcementID">announcementID</param>
        /// <returns>AnnouncementData</returns>
        public async Task<AnnouncementData> GetAnnouncementData(string announcementID)
        {
            try
            {
                FilterDefinition<AnnouncementData> filter = Builders<AnnouncementData>.Filter.Eq("AnnouncementID", announcementID);
                return await this.announcementDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Announcement Data Error >>> AnnouncementID:{announcementID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得車隊公告資料列表
        /// </summary>
        /// <param name="teamID">teamID</param>
        /// <returns>AnnouncementDatas</returns>
        public async Task<IEnumerable<AnnouncementData>> GetAnnouncementDataListOfTeam(string teamID)
        {
            try
            {
                FilterDefinition<AnnouncementData> filter = Builders<AnnouncementData>.Filter.Eq("TeamID", teamID);
                return await this.announcementDatas.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get AnnouncementData List Of Team Error >>> TeamID:{teamID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 更新公告資料
        /// </summary>
        /// <param name="announcementData">announcementData</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateAnnouncementData(AnnouncementData announcementData)
        {
            try
            {
                FilterDefinition<AnnouncementData> filter = Builders<AnnouncementData>.Filter.Eq("_id", announcementData.Id);
                ReplaceOneResult result = await this.announcementDatas.ReplaceOneAsync(filter, announcementData);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Update Announcement Data Fail For IsAcknowledged >>> Data:{JsonConvert.SerializeObject(announcementData)}");
                    return false;
                }

                if (result.ModifiedCount == 0)
                {
                    this.logger.LogError($"Update Announcement Data Fail For ModifiedCount >>> Data:{JsonConvert.SerializeObject(announcementData)}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Announcement Data Error >>> Data:{JsonConvert.SerializeObject(announcementData)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 更新已閱公告名單資料
        /// </summary>
        /// <param name="announcementID">announcementID</param>
        /// <param name="memberIDs">memberIDs</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateHaveSeenPlayers(string announcementID, IEnumerable<string> memberIDs)
        {
            try
            {
                FilterDefinition<AnnouncementData> filter = Builders<AnnouncementData>.Filter.Eq("AnnouncementID", announcementID);
                UpdateDefinition<AnnouncementData> update = Builders<AnnouncementData>.Update.Set(data => data.HaveSeenPlayerIDs, memberIDs);
                UpdateResult result = await this.announcementDatas.UpdateOneAsync(filter, update);
                if (!result.IsAcknowledged)
                {
                    this.logger.LogError($"Update Have Seen Players Fail For IsAcknowledged >>> AnnouncementID:{announcementID} HaveSeenPlayerIDs:{JsonConvert.SerializeObject(memberIDs)}");
                    return false;
                }

                if (result.ModifiedCount == 0)
                {
                    this.logger.LogError($"Update Have Seen Players Fail For ModifiedCount >>> AnnouncementID:{announcementID} HaveSeenPlayerIDs:{JsonConvert.SerializeObject(memberIDs)}");
                    return false;
                }

                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Have Seen Players Error >>> AnnouncementID:{announcementID} HaveSeenPlayerIDs:{JsonConvert.SerializeObject(memberIDs)}\n{ex}");
                return false;
            }
        }
    }
}