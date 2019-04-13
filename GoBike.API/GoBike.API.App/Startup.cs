using AutoMapper;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Repository.Interface;
using GoBike.API.Repository.Managers;
using GoBike.API.Service.Interface.Interactive;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Interface.Verifier;
using GoBike.API.Service.Managers.Interactive;
using GoBike.API.Service.Managers.Member;
using GoBike.API.Service.Managers.Verifier;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoBike.API.App
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
            //app.UseCors("ProductNoPolicy"); // 必須建立在  app.UseMvc 之前
            app.UseSession();
            app.UseMvc();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            services.AddAutoMapper();
            this.ConfigurationHandler(services);
            this.SessionHandler(services);
            this.DependencyInjectionHandler(services);
            //services.AddCors(options =>
            //{
            //    // CorsPolicy 是自訂的 Policy 名稱
            //    options.AddPolicy("ProductNoPolicy", policy =>
            //    {
            //        policy.AllowAnyOrigin()
            //              .AllowAnyHeader()
            //              .AllowAnyMethod()
            //              .AllowCredentials();
            //    });
            //});
        }

        private void ConfigurationHandler(IServiceCollection services)
        {
            AppSettingHelper.Appsetting = Configuration.Get<AppSettingHelper>();
            CommonFlagHelper.CommonFlag = Configuration.Get<CommonFlagHelper>();
        }

        private void DependencyInjectionHandler(IServiceCollection services)
        {
            services.AddSingleton<IMemberService, MemberService>();
            services.AddSingleton<IInteractiveService, InteractiveService>();
            services.AddSingleton<IVerifierService, VerifierService>();
            services.AddSingleton<IRedisRepository, RedisRepository>();
        }

        private void SessionHandler(IServiceCollection services)
        {
            services.AddDistributedRedisCache(o =>
            {
                o.Configuration = AppSettingHelper.Appsetting.RedisConnection;
            });
            services.AddSession(options =>
            {
                options.Cookie.HttpOnly = true;
                options.Cookie.Name = "Produce Session";
                options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
                options.Cookie.SameSite = SameSiteMode.None;
            });
        }
    }
}