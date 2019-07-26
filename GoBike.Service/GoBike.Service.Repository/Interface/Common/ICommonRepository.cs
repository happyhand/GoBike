using GoBike.Service.Repository.Models.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Service.Repository.Interface.Common
{
    /// <summary>
    /// 共用資料庫
    /// </summary>
    public interface ICommonRepository
    {
        /// <summary>
        /// 取得市區資料列表
        /// </summary>
        /// <returns>CityDatas</returns>
        Task<IEnumerable<CityData>> GetCityDataList();
    }
}