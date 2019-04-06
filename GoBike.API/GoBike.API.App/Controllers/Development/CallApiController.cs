using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace GoBikeAPI.App.Controllers.Development
{
    [Route("api/[controller]")]
    [ApiController]
    public class CallApiController : ControllerBase
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger logger;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        public CallApiController(ILogger<CallApiController> logger)
        {
            this.logger = logger;
        }

        //[HttpGet]
        //public IActionResult Get()
        //{
        //    HttpClient client = new HttpClient();
        //    client.BaseAddress = new Uri("http://uploadfilesgobike.hopto.org:18594");
        //    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        //    HttpResponseMessage response = client.GetAsync("api/Values").Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var data = response.Content.ReadAsAsync<dynamic>().Result;
        //        return Ok(data);
        //    }
        //    else
        //    {
        //        return Ok("Call Fail");
        //    }
        //}

        //[HttpPost]
        //public IActionResult Post()
        //{
        //    //// 建立 HttpClient
        //    HttpClient client = new HttpClient();
        //    //// 設定站台 url (api url)
        //    client.BaseAddress = new Uri("http://uploadfilesgobike.hopto.org:18594");
        //    //// 讀取 Request 中的檔案，並轉換成 byte 型式
        //    byte[] dataBytes;
        //    MultipartFormDataContent multiContent = new MultipartFormDataContent();
        //    foreach (var file in Request.Form.Files)
        //    {
        //        using (BinaryReader binaryReader = new BinaryReader(file.OpenReadStream()))
        //        {
        //            dataBytes = binaryReader.ReadBytes((int)file.OpenReadStream().Length);
        //            ByteArrayContent bytes = new ByteArrayContent(dataBytes);
        //            multiContent.Add(bytes, "file", file.FileName);
        //        }
        //    }
        //    //// 呼叫 api 並接收回應
        //    HttpResponseMessage response = client.PostAsync("api/UploadFiles/Images", multiContent).Result;
        //    if (response.IsSuccessStatusCode)
        //    {
        //        var result = response.Content.ReadAsAsync<ResultModel>().Result;
        //        return Ok(result);
        //    }
        //    else
        //    {
        //        return Ok("Call Fail");
        //    }
        //}
    }
}