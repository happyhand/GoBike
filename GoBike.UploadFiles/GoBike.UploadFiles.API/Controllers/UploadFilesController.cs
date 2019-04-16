using GoBike.UploadFiles.API.Filters;
using GoBike.UploadFiles.Service.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.UploadFiles.API.Controllers
{
    /// <summary>
    /// 上傳檔案
    /// </summary>
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadFilesController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<UploadFilesController> logger;

        /// <summary>
        /// uploadFilesService
        /// </summary>
        private readonly IUploadFilesService uploadFilesService;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="uploadFilesService">uploadFilesService</param>
        public UploadFilesController(ILogger<UploadFilesController> logger, IUploadFilesService uploadFilesService)
        {
            this.logger = logger;
            this.uploadFilesService = uploadFilesService;
        }

        /// <summary>
        /// POST - Images
        /// </summary>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [DisableFormValueModelBindingFilter]
        public async Task<IActionResult> Images()
        {
            List<string> filePaths = await this.uploadFilesService.UploadImages(this.Request);
            if (filePaths.Count == 0)
            {
                return BadRequest("上傳圖片檔案失敗.");
            }

            return Ok(filePaths);
        }
    }
}