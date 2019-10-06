using GoBike.Service.Repository.Models.Member;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Service.Repository.Interface.Member
{
    /// <summary>
    /// 互動資料庫
    /// </summary>
    public interface IInteractiveRepository
    {
        /// <summary>
        /// 建立互動資料
        /// </summary>
        /// <param name="interactiveData">interactiveData</param>
        /// <returns>bool</returns>
        Task<bool> CreateInteractiveData(InteractiveData interactiveData);

        /// <summary>
        /// 取得指定互動資料列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <param name="InteractiveID">InteractiveID</param>
        /// <returns>InteractiveDatas</returns>
        Task<InteractiveData> GetAppointInteractiveData(string memberID, string interactiveID);

        /// <summary>
        /// 取得被動性互動資料列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveDatas</returns>
        Task<IEnumerable<InteractiveData>> GetBeInteractiveDataList(string memberID);

        /// <summary>
        /// 取得主動性互動資料列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>InteractiveDatas</returns>
        Task<IEnumerable<InteractiveData>> GetInteractiveDataList(string memberID);

        /// <summary>
        /// 更新互動資料
        /// </summary>
        /// <param name="interactiveData">interactiveData</param>
        /// <returns>bool</returns>
        Task<bool> UpdateInteractiveData(InteractiveData interactiveData);
    }
}