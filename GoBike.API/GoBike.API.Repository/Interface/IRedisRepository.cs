using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.API.Repository.Interface
{
    public interface IRedisRepository
    {
        /// <summary>
        /// 讀取快取資料
        /// </summary>
        /// <param name="cacheKey">cacheKey</param>
        /// <returns>string</returns>
        Task<string> GetCache(string cacheKey);

        /// <summary>
        /// 取得 RedisKeys
        /// </summary>
        /// <param name="cacheKey">cacheKey</param>
        /// <returns>RedisKeys</returns>
        IEnumerable<RedisKey> GetRedisKeys(string cacheKey);

        /// <summary>
        /// 寫入快取資料
        /// </summary>
        /// <param name="cacheKey">cacheKey</param>
        /// <param name="dataJSON">dataJSON</param>
        /// <param name="cacheTimes">cacheTimes</param>
        /// <returns>bool</returns>
        Task<bool> SetCache(string cacheKey, string dataJSON, TimeSpan cacheTimes);
    }
}