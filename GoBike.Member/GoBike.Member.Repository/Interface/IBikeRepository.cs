using GoBike.Member.Repository.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Member.Repository.Interface
{
    /// <summary>
    /// 車輛資料庫
    /// </summary>
    public interface IBikeRepository
    {
        /// <summary>
        /// 建立車輛資料
        /// </summary>
        /// <param name="bikeData">bikeData</param>
        /// <returns>bool</returns>
        Task<bool> CreateBikeData(BikeData bikeData);

        /// <summary>
        /// 刪除車輛資料
        /// </summary>
        /// <param name="bikeID">bikeID</param>
        /// <returns>bool</returns>
        Task<bool> DeleteBikeData(string bikeID);

        /// <summary>
        /// 取得車輛資料
        /// </summary>
        /// <param name="bikeID">bikeID</param>
        /// <returns>BikeData</returns>
        Task<BikeData> GetBikeData(string bikeID);

        /// <summary>
        /// 取得會員的車輛資料列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>BikeDatas</returns>
        Task<IEnumerable<BikeData>> GetBikeDataListOfMember(string memberID);

        /// <summary>
        /// 更新車輛資料
        /// </summary>
        /// <param name="bikeData">bikeData</param>
        /// <returns>Tuple(bool, string)</returns>
        Task<Tuple<bool, string>> UpdateBikeData(BikeData bikeData);
    }
}