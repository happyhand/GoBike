using AutoMapper;
using GoBike.MGT.Repository.Interface;
using GoBike.MGT.Repository.Models.Data;
using GoBike.MGT.Service.Interface;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace GoBike.MGT.Service.Managers
{
    /// <summary>
    /// 後台服務
    /// </summary>
    public class MgtService : IMgtService
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<MgtService> logger;

        /// <summary>
        /// mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// mgtRepository
        /// </summary>
        private readonly IMgtRepository mgtRepository;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        /// <param name="commonRepository">commonRepository</param>
        public MgtService(ILogger<MgtService> logger, IMapper mapper, IMgtRepository mgtRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.mgtRepository = mgtRepository;
        }

        /// <summary>
        /// 新增代理商資料
        /// </summary>
        /// <returns>Tuple(AgentData, string)</returns>
        public void AddAgent(string nickname, string password)
        {
            try
            {
                this.mgtRepository.AddAgent(new AgentData() { Nickname = nickname, Password = password });
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Add Agent Error >>> Nickname:{nickname} Password:{password}\n{ex}");
            }
        }

        /// <summary>
        /// 取得代理商資料
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>Tuple(AgentData, string)</returns>
        public async Task<Tuple<AgentData, string>> GetAgent(long id)
        {
            try
            {
                AgentData agentData = await this.mgtRepository.GetAgent(id);
                return Tuple.Create(agentData, string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get Agent Error >>> ID:{id}\n{ex}");
                return Tuple.Create<AgentData, string>(null, "取得代理商資料發生錯誤.");
            }
        }
    }
}