using GoBike.UploadFiles.Core.Applibs;
using GoBike.UploadFiles.Service.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace GoBike.UploadFiles.Service.Managers
{
    /// <summary>
    /// 檔案上傳服務
    /// </summary>
    public class UploadFilesService : IUploadFilesService
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<UploadFilesService> logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public UploadFilesService(ILogger<UploadFilesService> logger)
        {
            this.logger = logger;
        }

        /// <summary>
        /// 上傳圖像
        /// </summary>
        /// <param name="httpRequest">httpRequest</param>
        /// <returns>List<string></returns>
        public async Task<List<string>> UploadImages(HttpRequest httpRequest)
        {
            try
            {
                List<string> filePaths = new List<string>();
                FormValueProvider formValueProvider = await httpRequest.StreamFile((file) =>
                {
                    this.logger.LogInformation($"Start Upload Image >>> file:{file.FileName}");
                    string fileExtensionName = Path.GetExtension(file.FileName);
                    string fileName = this.GetNewFileName(fileExtensionName);
                    string fileUrl = $"images/event/{fileName}".ToLower();
                    string filePath = $"{AppSettingHelper.Appsetting.CdnPath}/{fileUrl}".ToLower();
                    string fileDirectoryName = Path.GetDirectoryName(filePath);
                    if (!Directory.Exists(fileDirectoryName))
                    {
                        Directory.CreateDirectory(fileDirectoryName);
                    }

                    filePaths.Add(fileUrl);
                    return File.Create(filePath);
                });

                this.logger.LogInformation($"Finish Upload Image >>> file:{JsonConvert.SerializeObject(filePaths)}");
                return filePaths;
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Upload Images Error\n{ex}");
                return new List<string>();
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
    }
}