using System;
using AutoMapper;
using GoBike.API.Service.Models.Member.Data;
using GoBike.API.Service.Models.Member.View;

namespace GoBike.API.App.AutoMapperProfile.Member
{
    /// <summary>
    /// MemberProfile
    /// </summary>
    public class MemberProfile : Profile
    {
        /// <summary>
        /// MemberProfile
        /// </summary>
        public MemberProfile()
        {
            CreateMap<MemberDto, MemberSimpleInfoViewDto>();
            CreateMap<MemberDto, MemberDetailInfoViewDto>();
            CreateMap<MemberDto, MemberSettingInfoViewDto>();
            CreateMap<RideDto, RideSimpleInfoViewDto>();
            CreateMap<RideDto, RideDetailInfoViewDto>();
            CreateMap<RideRouteDto, RideRouteInfoViewDto>();
            CreateMap<RideContentDto, RideContentInfoViewDto>();

            //CreateMap<MemberDto, MemberSimpleInfoViewDto>().ForMember(view => view.LastOnlineTime, info => info.MapFrom(data => this.CalculationLastOnlineTimeToHour(data.LoginDate)));
            //CreateMap<MemberInfoDto, MemberCardInfoViewDto>().ForMember(view => view.LastOnlineTime, info => info.MapFrom(data => this.CalculationLastOnlineTimeToHour(data.LoginDate)));
            //CreateMap<MemberInfoDto, TeamMemberInfoViewDto>().ForMember(view => view.LastOnlineTime, info => info.MapFrom(data => this.CalculationLastOnlineTimeToHour(data.LoginDate)));
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