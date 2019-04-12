using GoBike.UploadFiles.Applibs;
using GoBike.UploadFiles.Filters;
using GoBike.UploadFiles.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GoBike.UploadFiles.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class UploadFilesController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<UploadFilesController> logger;

        public UploadFilesController(ILogger<UploadFilesController> logger)
        {
            this.logger = logger;
        }

        [HttpPost]
        [DisableFormValueModelBindingFilter]
        public async Task<ResultModel> Images()
        {
            this.logger.LogInformation($"Start Upload Image");
            try
            {
                List<string> filePaths = new List<string>();
                FormValueProvider formValueProvider = await Request.StreamFile((file) =>
                {
                    this.logger.LogInformation($"Start Upload Image >>> file:{file.FileName}");
                    string fileExtensionName = Path.GetExtension(file.FileName);
                    string fileName = this.GetNewFileName(fileExtensionName);
                    string filePath = $"{AppSettingHelper.Appsetting.CdnPath}/images/event/{fileName}";
                    string fileDirectoryName = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(fileDirectoryName))
                    {
                        Directory.CreateDirectory(fileDirectoryName);
                    }

                    filePaths.Add(filePath.ToLower());
                    return System.IO.File.Create(filePath.ToLower());
                });

                return new ResultModel() { ResultCode = 1, ResultMessage = "Upload Images Success", ResultData = filePaths };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Upload Images Error\n{ex}");
                return new ResultModel() { ResultCode = -999, ResultMessage = "Upload Images Error", ResultData = string.Empty };
            }
        }

        /// <summary>
        /// 取得新的檔案名稱
        /// </summary>
        /// <param name="fileExtensionName">fileExtensionName</param>
        /// <returns>string</returns>
        private string GetNewFileName(string fileExtensionName)
        {
            string guid = Guid.NewGuid().ToString().Replace("-", string.Empty).Substring(0, 6);
            return $"{guid}-{DateTime.Now:yyyyMMdd-HHmmss}{fileExtensionName}";
        }

        /// <summary>
        /// 回應資料
        /// </summary>
        public class ResultModel
        {
            /// <summary>
            /// Gets or sets ResultCode
            /// </summary>
            public int ResultCode { get; set; }

            /// <summary>
            /// Gets or sets ResultData
            /// </summary>
            public dynamic ResultData { get; set; }

            /// <summary>
            /// Gets or sets ResultMessage
            /// </summary>
            public string ResultMessage { get; set; }
        }
    }
}