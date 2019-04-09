using GoBike.API.Service.Email;
using GoBike.API.Service.Models.Response;
using GoBike.API.Service.Models.Verifier;
using System.Threading.Tasks;

namespace GoBike.API.Service.Interface.Verifier
{
    /// <summary>
    /// 驗證碼服務
    /// </summary>
    public interface IVerifierService
    {
        /// <summary>
        /// 取得驗證碼
        /// </summary>
        /// <param name="verifierInfo">verifierInfo</param>
        /// <returns>string</returns>
        Task<string> GetVerifierCode(VerifierInfoDto verifierInfo);

        /// <summary>
        /// 驗證碼是否有效
        /// </summary>
        /// <param name="verifierInfo">verifierInfo</param>
        /// <returns>bool</returns>
        Task<bool> IsValidVerifierCode(VerifierInfoDto verifierInfo);

        /// <summary>
        /// 發送驗證碼
        /// </summary>
        /// <param name="verifierInfo">verifierInfo</param>
        /// <param name="emailContext">emailContext</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> SendVerifierCode(VerifierInfoDto verifierInfo, EmailContext emailContext);
    }
}