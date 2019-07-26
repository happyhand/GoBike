using GoBike.API.Service.Models.Response;
using System.Threading.Tasks;

namespace GoBike.API.Service.Interface.Common
{
    /// <summary>
    /// 共用服務
    /// </summary>
    public interface ICommonService
    {
        /// <summary>
        /// 取得市區資料列表
        /// </summary>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> GetCityDataList();
    }
}