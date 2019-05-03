using GoBike.Member.Core.Applibs;
using GoBike.Member.Repository.Interface;
using GoBike.Member.Repository.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Member.Repository.Managers
{
    /// <summary>
    /// 車輛資料庫
    /// </summary>
    public class BikeRepository : IBikeRepository
    {
        /// <summary>
        /// bikeDatas
        /// </summary>
        private readonly IMongoCollection<BikeData> bikeDatas;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<BikeRepository> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public BikeRepository(ILogger<BikeRepository> logger)
        {
            this.logger = logger;
            IMongoClient client = new MongoClient(AppSettingHelper.Appsetting.MongoDBConfig.ConnectionString);
            IMongoDatabase db = client.GetDatabase(AppSettingHelper.Appsetting.MongoDBConfig.MemberDatabase);
            this.bikeDatas = db.GetCollection<BikeData>(AppSettingHelper.Appsetting.MongoDBConfig.CollectionFlag.Bike);
        }

        /// <summary>
        /// 建立車輛資料
        /// </summary>
        /// <param name="bikeData">bikeData</param>
        /// <returns>bool</returns>
        public async Task<bool> CreateBikeData(BikeData bikeData)
        {
            try
            {
                await this.bikeDatas.InsertOneAsync(bikeData);
                return true;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Create Bike Data Error >>> Data:{JsonConvert.SerializeObject(bikeData)}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 刪除車輛資料
        /// </summary>
        /// <param name="bikeID">bikeID</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteBikeData(string bikeID)
        {
            try
            {
                FilterDefinition<BikeData> filter = Builders<BikeData>.Filter.Eq("BikeID", bikeID);
                DeleteResult result = await this.bikeDatas.DeleteManyAsync(filter);
                return result.IsAcknowledged && result.DeletedCount > 0;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Bike Data Error >>> BikeID:{bikeID}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 取得車輛資料
        /// </summary>
        /// <param name="bikeID">bikeID</param>
        /// <returns>BikeData</returns>
        public async Task<BikeData> GetBikeData(string bikeID)
        {
            try
            {
                FilterDefinition<BikeData> filter = Builders<BikeData>.Filter.Eq("BikeID", bikeID);
                return await this.bikeDatas.Find(filter).FirstOrDefaultAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Bike Data Error >>> BikeID:{bikeID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 取得會員的車輛資料列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>BikeDatas</returns>
        public async Task<IEnumerable<BikeData>> GetBikeDataListOfMember(string memberID)
        {
            try
            {
                FilterDefinition<BikeData> filter = Builders<BikeData>.Filter.Eq("MemberID", memberID);
                return await this.bikeDatas.Find(filter).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Bike Data List Of Member Error >>> MemberID:{memberID}\n{ex}");
                return null;
            }
        }

        /// <summary>
        /// 更新車輛資料
        /// </summary>
        /// <param name="bikeData">bikeData</param>
        /// <returns>Tuple(bool, string)</returns>
        public async Task<Tuple<bool, string>> UpdateBikeData(BikeData bikeData)
        {
            try
            {
                FilterDefinition<BikeData> filter = Builders<BikeData>.Filter.Eq("_id", bikeData.Id);
                ReplaceOneResult result = await this.bikeDatas.ReplaceOneAsync(filter, bikeData);
                if (!result.IsAcknowledged)
                {
                    return Tuple.Create(false, "無法更新車輛資料.");
                }

                if (result.ModifiedCount == 0)
                {
                    return Tuple.Create(false, "車輛資料未更改.");
                }

                return Tuple.Create(true, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Bike Data Error >>> Data:{JsonConvert.SerializeObject(bikeData)}\n{ex}");
                return Tuple.Create(false, "更新車輛資料發生錯誤.");
            }
        }
    }
}