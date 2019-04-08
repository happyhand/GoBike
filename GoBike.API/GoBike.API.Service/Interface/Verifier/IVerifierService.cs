using GoBike.API.Service.Models.Response;
using System.Threading.Tasks;

namespace GoBike.API.Service.Interface.Verifier
{
    /// <summary>
    /// 驗證碼服務
    /// </summary>
    public interface IVerifierService
    {
        /// <summary>
        /// 驗證碼是否有效
        /// </summary>
        /// <param name="email">email</param>
        /// <param name="verifierCode">verifierCode</param>
        /// <returns>bool</returns>
        Task<bool> IsValidVerifierCode(string email, string verifierCode);

        /// <summary>
        /// 發送驗證碼
        /// </summary>
        /// <param name="email">email</param>
        /// <returns>ResponseResultDto</returns>
        Task<ResponseResultDto> SendVerifierCode(string email);
    }
}