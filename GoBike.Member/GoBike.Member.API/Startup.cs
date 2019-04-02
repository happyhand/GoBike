using AutoMapper;
using GoBike.Member.App.Applibs;
using GoBike.Member.Core.Resource;
using GoBike.Member.Repository.Interface;
using GoBike.Member.Repository.Managers;
using GoBike.Member.Repository.Models.Core;
using GoBike.Member.Service.Interface;
using GoBike.Member.Service.Managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoBike.Member.API
{
	public class Startup
	{
		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public IConfiguration Configuration { get; }

		// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
		public void Configure(IApplicationBuilder app, IHostingEnvironment env)
		{
			if (env.IsDevelopment())
			{
				app.UseDeveloperExceptionPage();
			}
			else
			{
				app.UseHsts();
			}

			app.UseHttpsRedirection();
			app.UseMvc();
		}

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.AddAutoMapper();
			this.ConfigurationHandler(services);
			this.DependencyInjectionHandler(services);
		}

		private void ConfigurationHandler(IServiceCollection services)
		{
			AppSettingHelper.Appsetting = Configuration.Get<AppSettingHelper>();
			services.Configure<MongoDBSetting>(options =>
			{
				options.ConnectionString = AppSettingHelper.Appsetting.MongoDBConfig.ConnectionString;
				options.MemberDatabase = AppSettingHelper.Appsetting.MongoDBConfig.MemberDatabase;
			});
			services.Configure<SmtpSetting>(options =>
			{
				options.SmtpServer = AppSettingHelper.Appsetting.SmtpConfig.SmtpServer;
				options.SmtpMail = AppSettingHelper.Appsetting.SmtpConfig.SmtpMail;
				options.SmtpPassword = AppSettingHelper.Appsetting.SmtpConfig.SmtpPassword;
			});
		}

		private void DependencyInjectionHandler(IServiceCollection services)
		{
			services.AddSingleton<IMemberService, MemberService>();
			services.AddSingleton<IMemberRepository, MemberRepository>();
		}
	}
}