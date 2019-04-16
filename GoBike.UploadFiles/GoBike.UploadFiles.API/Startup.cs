using GoBike.UploadFiles.Core.Applibs;
using GoBike.UploadFiles.Service.Interface;
using GoBike.UploadFiles.Service.Managers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GoBike.UploadFiles
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
            this.ConfigurationHandler(services);
            this.DependencyInjectionHandler(services);
        }

        private void ConfigurationHandler(IServiceCollection services)
        {
            AppSettingHelper.Appsetting = Configuration.Get<AppSettingHelper>();
        }

        private void DependencyInjectionHandler(IServiceCollection services)
        {
            services.AddSingleton<IUploadFilesService, UploadFilesService>();
        }
    }
}