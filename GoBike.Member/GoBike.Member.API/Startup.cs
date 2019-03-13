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

		// This method gets called by the runtime. Use this method to add services to the container.
		public void ConfigureServices(IServiceCollection services)
		{
			services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
			services.Configure<DBSetting>(options =>
			{
				options.ConnectionString = Configuration.GetSection("MongoConnection:ConnectionString").Value;
				options.MemberDatabase = Configuration.GetSection("MongoConnection:MemberDatabase").Value;
			});
			services.AddScoped<IMemberService, MemberService>();
			services.AddScoped<IMemberRepository, MemberRepository>();

			services.AddCors(options =>
			{
				// CorsPolicy 是自訂的 Policy 名稱
				options.AddPolicy("ProductNoPolicy", policy =>
				{
					policy.WithOrigins("http://www.6stest.com")
						  .AllowAnyHeader()
						  .AllowAnyMethod()
						  .AllowCredentials();
				});
			});
		}

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
			app.UseCors("ProductNoPolicy"); // 必須建立在  app.UseMvc 之前
			app.UseMvc();
		}
	}
}