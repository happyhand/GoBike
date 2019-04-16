using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.UploadFiles.Service.Interface
{
    /// <summary>
    /// 檔案上傳服務
    /// </summary>
    public interface IUploadFilesService
    {
        /// <summary>
        /// 上傳圖像
        /// </summary>
        /// <param name="httpRequest">httpRequest</param>
        /// <returns>List<string></returns>
        Task<List<string>> UploadImages(HttpRequest httpRequest);
    }
}