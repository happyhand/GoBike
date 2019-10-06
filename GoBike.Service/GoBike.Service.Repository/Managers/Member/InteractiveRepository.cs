using GoBike.Service.Core.Applibs;
using GoBike.Service.Repository.Interface.Member;
using GoBike.Service.Repository.Models.Member;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Service.Repository.Managers.Member
{
    /// <summary>
    /// 互動資料庫
    /// </summary>
    public class InteractiveRepository : IInteractiveRepository
    {
        /// <summary>
        /// memberDatas
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
        /// 取得指定互動資料列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="InteractiveID">InteractiveID</param>
        /// <returns>InteractiveDatas</returns>
        public async Task<InteractiveData> GetAppointInteractiveData(string memberID, string interactiveID)
        {
            try
            {
                return await this.interactiveDatas.Find(options => options.MemberID.Equals(memberID) && options.InteractiveID.Equals(interactiveID)).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Appoint Interactive Data Error >>> MemberID:{memberID} InteractiveID:{interactiveID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得被動性互動資料列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveDatas</returns>
        public async Task<IEnumerable<InteractiveData>> GetBeInteractiveDataList(string memberID)
        {
            try
            {
                return await this.interactiveDatas.Find(options => options.InteractiveID.Equals(memberID)).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Be Interactive List Error >>> MemberID:{memberID}\n{ex}");
                return new List<InteractiveData>();
            }
        }

        /// <summary>
        /// 取得主動性互動資料列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveDatas</returns>
        public async Task<IEnumerable<InteractiveData>> GetInteractiveDataList(string memberID)
        {
            try
            {
                return await this.interactiveDatas.Find(options => options.MemberID.Equals(memberID)).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Interactive List Error >>> MemberID:{memberID}\n{ex}");
                return new List<InteractiveData>();
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
    }
}