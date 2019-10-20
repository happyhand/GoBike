using GoBike.MGT.Repository.Models.Data;
using System.Threading.Tasks;

namespace GoBike.MGT.Repository.Interface
{
    /// <summary>
    /// 後台資料庫服務
    /// </summary>
    public interface IMgtRepository
    {
        /// <summary>
        /// 新增代理商資料
        /// </summary>
        /// <param name="agentData">agentData</param>
        void AddAgent(AgentData agentData);

        /// <summary>
        /// 取得代理商資料
        /// </summary>
        /// <param name="id">id</param>
        /// <returns>AgentData</returns>
        Task<AgentData> GetAgent(long id);
    }
}