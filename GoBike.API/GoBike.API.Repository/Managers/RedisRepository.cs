using GoBike.API.Core.Applibs;
using GoBike.API.Repository.Interface;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoBike.API.Repository.Managers
{
    /// <summary>
    /// Redis 資料庫
    /// </summary>
    public class RedisRepository : IRedisRepository
    {
        /// <summary>
        /// connectionMultiplexer
        /// </summary>
        private readonly IDatabase database;

        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<RedisRepository> logger;

        /// <summary>
        /// connectionMultiplexer
        /// </summary>
        private readonly IServer redisServer;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public RedisRepository(ILogger<RedisRepository> logger)
        {
            this.logger = logger;
            Lazy<ConnectionMultiplexer> lazyConnection = new Lazy<ConnectionMultiplexer>(() =>
            {
                return ConnectionMultiplexer.Connect(AppSettingHelper.Appsetting.RedisConnection);
            });

            ConnectionMultiplexer connectionMultiplexer = lazyConnection.Value;
            this.database = connectionMultiplexer.GetDatabase(1);
            this.redisServer = connectionMultiplexer.GetServer(connectionMultiplexer.GetEndPoints().First());
        }

        /// <summary>
        /// 刪除快取資料
        /// </summary>
        /// <param name="cacheKey">cacheKey</param>
        /// <returns>bool</returns>
        public async Task<bool> DeleteCache(string cacheKey)
        {
            try
            {
                return await this.database.KeyDeleteAsync(cacheKey);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Delete Cache Error >>> CacheKey:{cacheKey}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 讀取快取資料
        /// </summary>
        /// <param name="cacheKey">cacheKey</param>
        /// <returns>string</returns>
        public async Task<string> GetCache(string cacheKey)
        {
            try
            {
                return await this.database.StringGetAsync(cacheKey);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Cache Error >>> CacheKey:{cacheKey}\n{ex}");
                return string.Empty;
            }
        }

        /// <summary>
        /// 取得 RedisKeys
        /// </summary>
        /// <param name="cacheKey">cacheKey</param>
        /// <returns>RedisKeys</returns>
        public IEnumerable<RedisKey> GetRedisKeys(string cacheKey)
        {
            return this.redisServer.Keys(1, pattern: cacheKey);
        }

        /// <summary>
        /// 寫入快取資料
        /// </summary>
        /// <param name="cacheKey">cacheKey</param>
        /// <param name="dataJSON">dataJSON</param>
        /// <param name="cacheTimes">cacheTimes</param>
        /// <returns>bool</returns>
        public async Task<bool> SetCache(string cacheKey, string dataJSON, TimeSpan cacheTimes)
        {
            try
            {
                return await this.database.StringSetAsync(cacheKey, dataJSON, cacheTimes);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Set Cache Error >>> CacheKey:{cacheKey} DataJSON:{dataJSON}\n{ex}");
                return false;
            }
        }

        /// <summary>
        /// 更新快取資料到期時間
        /// </summary>
        /// <param name="cacheKey">cacheKey</param>
        /// <returns>bool</returns>
        public async Task<bool> UpdateCacheExpire(string cacheKey, TimeSpan time)
        {
            try
            {
                return await this.database.KeyExpireAsync(cacheKey, time);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Update Cache Expire Error >>> CacheKey:{cacheKey} TimeSpan:{time}\n{ex}");
                return false;
            }
        }
    }
}