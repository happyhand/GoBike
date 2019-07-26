using AutoMapper;
using GoBike.Service.Repository.Interface.Common;
using GoBike.Service.Repository.Models.Common;
using GoBike.Service.Service.Interface.Common;
using GoBike.Service.Service.Models.Common;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GoBike.Service.Service.Managers.Common
{
    /// <summary>
    /// 共用服務
    /// </summary>
    public class CommonService : ICommonService
    {
        /// <summary>
        /// memberRepository
        /// </summary>
        private readonly ICommonRepository commonRepository;

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
        /// <param name="commonRepository">commonRepository</param>
        public CommonService(ILogger<CommonService> logger, IMapper mapper, ICommonRepository commonRepository)
        {
            this.logger = logger;
            this.mapper = mapper;
            this.commonRepository = commonRepository;
        }

        /// <summary>
        /// 取得市區資料列表
        /// </summary>
        /// <returns>Tuple(CityDtos, string)</returns>
        public async Task<Tuple<IEnumerable<CityDto>, string>> GetCityDataList()
        {
            try
            {
                IEnumerable<CityData> cityDatas = await this.commonRepository.GetCityDataList();
                return Tuple.Create(this.mapper.Map<IEnumerable<CityDto>>(cityDatas), string.Empty);
            }
            catch (Exception ex)
            {
                this.logger.LogError($"Get City Data List Error\n{ex}");
                return Tuple.Create<IEnumerable<CityDto>, string>(null, "取得市區資料列表發生錯誤.");
            }
        }
    }
}