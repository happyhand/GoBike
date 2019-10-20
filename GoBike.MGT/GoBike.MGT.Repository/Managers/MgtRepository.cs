using GoBike.MGT.Repository.Interface;
using GoBike.MGT.Repository.Models.Context;
using GoBike.MGT.Repository.Models.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace GoBike.MGT.Repository.Managers
{
    /// <summary>
    /// 後台資料庫服務
    /// </summary>
    public class MgtRepository : IMgtRepository
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<MgtRepository> logger;

        /// <summary>
        /// mgtdb
        /// </summary>
        private readonly IServiceProvider serviceProvider;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public MgtRepository(ILogger<MgtRepository> logger, IServiceProvider serviceProvider)
        {
            this.logger = logger;
            this.serviceProvider = serviceProvider;
        }

        /// <summary>
        /// 新增代理商資料
        /// </summary>
        /// <param name="agentData">agentData</param>
        public void AddAgent(AgentData agentData)
        {
            try
            {
                using (var mgtdb = new Mgtdb(this.serviceProvider.GetRequiredService<DbContextOptions<Mgtdb>>()))
                {
                    mgtdb.Agent.AddAsync(agentData);
                    mgtdb.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Agent Error >>> AgentData:{JsonConvert.SerializeObject(agentData)}\n{ex}");
            }
        }

        /// <summary>
        /// 取得代理商資料
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>AgentData</returns>
        public async Task<AgentData> GetAgent(long id)
        {
            try
            {
                using (var mgtdb = new Mgtdb(this.serviceProvider.GetRequiredService<DbContextOptions<Mgtdb>>()))
                {
                    return await mgtdb.Agent.FirstOrDefaultAsync(options => options.Id.Equals(id));
                }
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Agent Error >>> ID:{id}\n{ex}");
                return null;
            }
        }
    }
}