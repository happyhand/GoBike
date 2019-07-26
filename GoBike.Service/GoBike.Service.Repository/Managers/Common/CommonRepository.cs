using GoBike.Service.Core.Applibs;
using GoBike.Service.Repository.Interface.Common;
using GoBike.Service.Repository.Models.Common;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Service.Repository.Managers.Common
{
    /// <summary>
    /// 共用資料庫
    /// </summary>
    public class CommonRepository : ICommonRepository
    {
        /// <summary>
        /// memberDatas
        /// </summary>
        private readonly IMongoCollection<CityData> cityDatas;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<CommonRepository> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public CommonRepository(ILogger<CommonRepository> logger)
        {
            this.logger = logger;
            IMongoClient client = new MongoClient(AppSettingHelper.Appsetting.MongoDBConfig.ConnectionString);
            IMongoDatabase db = client.GetDatabase(AppSettingHelper.Appsetting.MongoDBConfig.CommonDatabase);
            this.cityDatas = db.GetCollection<CityData>(AppSettingHelper.Appsetting.MongoDBConfig.CollectionFlag.City);
        }

        /// <summary>
        /// 取得市區資料列表
        /// </summary>
        /// <returns>CityDatas</returns>
        public async Task<IEnumerable<CityData>> GetCityDataList()
        {
            try
            {
                return await this.cityDatas.Find(data => true).ToListAsync();
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get City Data List Error\n{ex}");
                return null;
            }
        }
    }
}