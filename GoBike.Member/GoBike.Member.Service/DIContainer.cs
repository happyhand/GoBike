using GoBike.Member.Core.Interface.Repository;
using GoBike.Member.Repository.Managers;
using Microsoft.Extensions.DependencyInjection;

namespace GoBike.Member.Service
{
	public class DIContainer
	{
		public static void RunDI(IServiceCollection services)
		{
			services.AddScoped<IMemberRepository, MemberRepository>();
		}
	}
}