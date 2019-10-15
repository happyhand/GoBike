using AutoMapper;
using GoBike.MGT.Core.Applibs;
using GoBike.MGT.Core.Resource;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;

namespace GoBike.MGT.APP
{
    /// <summary>
    /// Startup
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup
        /// </summary>
        /// <param name="configuration"></param>
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        /// <summary>
        /// Configuration
        /// </summary>
        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// <summary>
        /// Configure
        /// </summary>
        /// <param name="app">app</param>
        /// <param name="env">env</param>
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
        /// <summary>
        /// ConfigureServices
        /// </summary>
        /// <param name="services">services</param>
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
            //services.AddSingleton<IInteractiveService, InteractiveService>();
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
                options.IdleTimeout = TimeSpan.FromMinutes(5);
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
                c.SwaggerDoc("v1", new Info { Title = "GoBike MGT", Version = "v1", Description = "apigobike.zapto.org:18592" });
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);
                var xmlPath = Path.Combine(basePath, "GoBike.MGT.Swagger.xml");
                c.IncludeXmlComments(xmlPath);
            });
        }
    }
}