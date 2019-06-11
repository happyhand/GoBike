using AutoMapper;
using GoBike.Team.Core.Applibs;
using GoBike.Team.Repository.Interface;
using GoBike.Team.Repository.Managers;
using GoBike.Team.Service.Interface;
using GoBike.Team.Service.Managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using System;

namespace GoBike.Team
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
            this.MongoDBSettingHandler();
        }

        private void ConfigurationHandler(IServiceCollection services)
        {
            AppSettingHelper.Appsetting = Configuration.Get<AppSettingHelper>();
        }

        private void DependencyInjectionHandler(IServiceCollection services)
        {
            services.AddSingleton<ITeamService, TeamService>();
            services.AddSingleton<IInteractiveService, InteractiveService>();
            services.AddSingleton<IAnnouncementService, AnnouncementService>();
            services.AddSingleton<IEventService, EventService>();
            services.AddSingleton<ITeamRepository, TeamRepository>();
            services.AddSingleton<IAnnouncementRepository, AnnouncementRepository>();
            services.AddSingleton<IEventRepository, EventRepository>();
        }

        private void MongoDBSettingHandler()
        {
            DateTimeSerializer dateTimeSerializer = new DateTimeSerializer(DateTimeKind.Local, BsonType.DateTime);
            BsonSerializer.RegisterSerializer(typeof(DateTime), dateTimeSerializer);
        }
    }
}