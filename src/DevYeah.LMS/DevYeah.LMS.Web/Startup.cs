using DevYeah.LMS.Business;
using DevYeah.LMS.Business.ConfigurationModels;
using DevYeah.LMS.Business.Interfaces;
using DevYeah.LMS.Data;
using DevYeah.LMS.Data.Interfaces;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DevYeah.LMS.Web
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
            #region Configuration Setting
            var lmsConfig = Configuration.GetSection("LMSConfig");
            var dbConnection = lmsConfig.GetSection("DbConfig").GetValue<string>("DefaultConnection");
            services.AddDbContext<LMSContext>(options => options.UseSqlServer(dbConnection));
            services.AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2)
                .AddJsonOptions(options => {
                    options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                });
            services.Configure<AppSettings>(lmsConfig);
            services.AddSpaStaticFiles(configuration =>
            {
                configuration.RootPath = "app/build";
            });
            #endregion

            #region IOC Setting
            services.AddScoped<DbContext, LMSContext>();
            services.AddScoped<IEmailClient, EmailClient>();
            services.AddScoped<IBlobStorage, BlobStorage>();
            services.AddScoped<IAccountRepository, AccountRepository>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<ICourseRepository, CourseRepository>();
            services.AddScoped<ISystemErrorsRepository, SystemErrorsRepository>();
            services.AddScoped<ICourseService, CourseService>();
            #endregion
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
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseSpaStaticFiles();
            app.UseMvc();

            app.UseSpa(spa =>
            {
                spa.Options.SourcePath = "app";

                if (env.IsDevelopment())
                {
                    spa.UseReactDevelopmentServer(npmScript: "start");
                }
            });
        }
    }
}
