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
    /// 會員資料庫
    /// </summary>
    public class RideRepository : IRideRepository
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<RideRepository> logger;

        /// <summary>
        /// memberDatas
        /// </summary>
        private readonly IMongoCollection<RideData> rideDatas;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public RideRepository(ILogger<RideRepository> logger)
        {
            this.logger = logger;
            IMongoClient client = new MongoClient(AppSettingHelper.Appsetting.MongoDBConfig.ConnectionString);
            IMongoDatabase db = client.GetDatabase(AppSettingHelper.Appsetting.MongoDBConfig.MemberDatabase);
            this.rideDatas = db.GetCollection<RideData>(AppSettingHelper.Appsetting.MongoDBConfig.CollectionFlag.Ride);
        }

        /// <summary>
        /// 建立騎乘資料
        /// </summary>
        /// <param name="rideData">rideData</param>
        /// <returns>bool</returns>
        public async Task<bool> CreateRideData(RideData rideData)
        {
            try
            {
                await this.rideDatas.InsertOneAsync(rideData);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Ride Data Error >>> Data:{JsonConvert.SerializeObject(rideData)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 取得最新騎乘資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>RideData</returns>
        public async Task<RideData> GetLatestRideData(string memberID)
        {
            try
            {
                FilterDefinition<RideData> filter = Builders<RideData>.Filter.Eq("MemberID", memberID);
                return await this.rideDatas.Find(filter).SortByDescending(data => data.CreateDate).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Latest Ride Data Error >>> MemberID:{memberID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得騎乘資料列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>RideDatas</returns>
        public async Task<IEnumerable<RideData>> GetRideDataList(string memberID)
        {
            try
            {
                FilterDefinition<RideData> filter = Builders<RideData>.Filter.Eq("MemberID", memberID);
                return await this.rideDatas.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Ride Data List Error >>> MemberID:{memberID}\n{ex}");
                return null;
            }
        }
    }
}