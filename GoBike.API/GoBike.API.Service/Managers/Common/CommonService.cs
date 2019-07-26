using AutoMapper;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Service.Interface.Common;
using GoBike.API.Service.Models.Common;
using GoBike.API.Service.Models.Response;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GoBike.API.Service.Managers.Common
{
    /// <summary>
    /// 共用服務
    /// </summary>
    public class CommonService : ICommonService
    {
        /// <summary>
        /// logger
        /// </summary>
        private readonly ILogger<CommonService> logger;

        /// <summary>
        /// mapper
        /// </summary>
        private readonly IMapper mapper;

        /// <summary>
        /// 建構式
        /// </summary>
        /// <param name="logger">logger</param>
        /// <param name="mapper">mapper</param>
        public CommonService(ILogger<CommonService> logger, IMapper mapper)
        {
            this.logger = logger;
            this.mapper = mapper;
        }

        /// <summary>
        /// 取得市區資料列表
        /// </summary>
        /// <returns>ResponseResultDto</returns>
        public async Task<ResponseResultDto> GetCityDataList()
        {
            try
            {
                HttpResponseMessage httpResponseMessage = await Utility.ApiGet(AppSettingHelper.Appsetting.ServiceDomain.Service, "api/Common/GetCityDataList");
                if (httpResponseMessage.IsSuccessStatusCode)
                {
                    return new ResponseResultDto()
                    {
                        Ok = true,
                        Data = await httpResponseMessage.Content.ReadAsAsync<IEnumerable<CityDto>>()
                    };
                }

                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = await httpResponseMessage.Content.ReadAsAsync<string>()
                };
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get City Data List Error\n{ex}");
                return new ResponseResultDto()
                {
                    Ok = false,
                    Data = "取得市區資料列表發生錯誤."
                };
            }
        }
    }
}