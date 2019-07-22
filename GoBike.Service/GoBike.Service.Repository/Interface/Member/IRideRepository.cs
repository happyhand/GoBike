using GoBike.Service.Repository.Models.Member;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Service.Repository.Interface.Member
{
    /// <summary>
    /// 騎乘資料庫
    /// </summary>
    public interface IRideRepository
    {
        /// <summary>
        /// 建立騎乘資料
        /// </summary>
        /// <param name="rideData">rideData</param>
        /// <returns>bool</returns>
        Task<bool> CreateRideData(RideData rideData);

        /// <summary>
        /// 取得最新騎乘資料
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>RideData</returns>
        Task<RideData> GetLatestRideData(string memberID);

        /// <summary>
        /// 取得騎乘資料
        /// </summary>
        /// <param name="rideID">rideID</param>
        /// <returns>RideData</returns>
        Task<RideData> GetRideData(string rideID);

        /// <summary>
        /// 取得騎乘資料列表
        /// </summary>
        /// <param name="memberID">memberID</param>
        /// <returns>RideDatas</returns>
        Task<IEnumerable<RideData>> GetRideDataList(string memberID);
    }
}