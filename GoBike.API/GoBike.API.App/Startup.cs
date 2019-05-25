using AutoMapper;
using GoBike.API.Core.Applibs;
using GoBike.API.Core.Resource;
using GoBike.API.Repository.Interface;
using GoBike.API.Repository.Managers;
using GoBike.API.Service.Interface.Interactive;
using GoBike.API.Service.Interface.Member;
using GoBike.API.Service.Interface.Team;
using GoBike.API.Service.Interface.Verifier;
using GoBike.API.Service.Managers.Interactive;
using GoBike.API.Service.Managers.Member;
using GoBike.API.Service.Managers.Team;
using GoBike.API.Service.Managers.Verifier;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System.IO;

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

            #region Swagger

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "GoBike API");
            });

            #endregion Swagger

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
            this.SwaggerHandler(services);
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

        /// <summary>
        /// Config 處理器
        /// </summary>
        /// <param name="services">services</param>
        private void ConfigurationHandler(IServiceCollection services)
        {
            AppSettingHelper.Appsetting = Configuration.Get<AppSettingHelper>();
            CommonFlagHelper.CommonFlag = Configuration.Get<CommonFlagHelper>();
        }

        /// <summary>
        /// Service 處理器
        /// </summary>
        /// <param name="services">services</param>
        private void DependencyInjectionHandler(IServiceCollection services)
        {
            services.AddSingleton<IMemberService, MemberService>();
            services.AddSingleton<IInteractiveService, InteractiveService>();
            services.AddSingleton<ITeamService, TeamService>();
            services.AddSingleton<IVerifierService, VerifierService>();
            services.AddSingleton<IRedisRepository, RedisRepository>();
        }

        /// <summary>
        /// Session 處理器
        /// </summary>
        /// <param name="services">services</param>
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

        /// <summary>
        /// Swagger 處理器
        /// </summary>
        /// <param name="services">services</param>
        private void SwaggerHandler(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new Info { Title = "GoBike API", Version = "v1", Description = "apigobike.zapto.org:18592" });
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var xmlPath = Path.Combine(basePath, "GoBike.API.Swagger.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}