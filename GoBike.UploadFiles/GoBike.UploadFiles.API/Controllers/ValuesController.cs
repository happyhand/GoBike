using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;

namespace GoBike.UploadFiles.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<ValuesController> logger;

        private IHostingEnvironment hostingEnv;

        public ValuesController(ILogger<ValuesController> logger, IHostingEnvironment hostingEnv)
        {
            this.logger = logger;
            this.hostingEnv = hostingEnv;
        }

        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            this.logger.LogInformation("Get Log321");
            return new string[] { "value1", "value2" };
        }

        [HttpPost]
        public IActionResult Post(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            List<string> filePathResultList = new List<string>();
            foreach (IFormFile file in files)
            {
                string fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
                this.logger.LogInformation($"fileName:{fileName}");
                string filePath = $@"D:\CDN\Html\UploadFiles\Event\";
                if (!Directory.Exists(filePath))
                {
                    Directory.CreateDirectory(filePath);
                }
                string suffix = fileName.Split('.')[1];
                fileName = Guid.NewGuid() + "." + suffix;
                string fileFullName = filePath + fileName;
                this.logger.LogInformation($"fileFullName:{fileFullName}");
                using (FileStream fs = System.IO.File.Create(fileFullName))
                {
                    file.CopyTo(fs);
                    fs.Flush();
                }
            }

            return Ok("Success");
        }

        //[HttpPost]
        //public IActionResult Post()
        //{
        //    this.logger.LogInformation("Start");
        //    var files = Request.Form.Files;
        //    long size = files.Sum(f => f.Length);
        //    //size > 100MB refuse upload !
        //    if (size > 104857600)
        //    {
        //        return Ok("pictures total size > 100MB , server refused !");
        //    }
        //    List<string> filePathResultList = new List<string>();
        //    foreach (var file in files)
        //    {
        //        var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim('"');
        //        this.logger.LogInformation($"fileName:{fileName}");
        //        string filePath = $@"D:\CDN\Html\UploadFiles\Event\";
        //        if (!Directory.Exists(filePath))
        //        {
        //            Directory.CreateDirectory(filePath);
        //        }
        //        string suffix = fileName.Split('.')[1];
        //        fileName = Guid.NewGuid() + "." + suffix;
        //        string fileFullName = filePath + fileName;
        //        this.logger.LogInformation($"fileFullName:{fileFullName}");
        //        using (FileStream fs = System.IO.File.Create(fileFullName))
        //        {
        //            file.CopyTo(fs);
        //            fs.Flush();
        //        }
        //    }

        //    return Ok("Success");
        //}
    }
}