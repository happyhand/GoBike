using AutoMapper;
using GoBike.API.App.Models.Member;
using GoBike.API.Service.Models.Member.Data;
using System;

namespace GoBike.API.App.AutoMapperProfile.Member
{
    public class MemberProfile : Profile
    {
        public MemberProfile()
        {
            CreateMap<MemberInfoDto, MemberSimpleViewDto>().ForMember(view => view.LastOnlineTime, info => info.MapFrom(data => this.CalculationLastOnlineTimeToHour(data.LoginDate)));
            CreateMap<MemberInfoDto, MemberDetailViewDto>().ForMember(view => view.LastOnlineTime, info => info.MapFrom(data => this.CalculationLastOnlineTimeToHour(data.LoginDate)));
        }

        /// <summary>
        /// 計算最後上線時間
        /// </summary>
        /// <param name="loginDate">loginDate</param>
        /// <returns>int</returns>
        private int CalculationLastOnlineTimeToHour(DateTime loginDate)
        {
            if (loginDate == null)
            {
                return 0;
            }

            return new TimeSpan((DateTime.Now - loginDate).Ticks).Hours;
        }
    }
}