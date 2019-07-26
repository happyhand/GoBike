using GoBike.Service.Service.Models.Common;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Service.Service.Interface.Common
{
    /// <summary>
    /// 共用服務
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// 取得市區資料列表
        /// </summary>
        /// <returns>Tuple(CityDtos, string)</returns>
        Task<Tuple<IEnumerable<CityDto>, string>> GetCityDataList();
    }
}