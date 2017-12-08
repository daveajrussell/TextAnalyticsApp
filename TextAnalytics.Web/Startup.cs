using TextAnalyticsApp.Core;
using TextAnalyticsApp.Data.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using TextAnalyticsApp.Data.Services;
using Microsoft.EntityFrameworkCore.Design;
using TextAnalyticsApp.Data.Factories;
using TextAnalyticsApp.Data.Abstractions;

namespace TextAnalyticsApp.Web
{
    public class Startup
    {
        private readonly string _connectionStringName;

        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();

            _connectionStringName = "AppSettings:ConnectionString";

            Configuration = builder.Build();
        }

        public IConfigurationRoot Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            // Add sql server
            services.AddDbContext<CoreDbContext>(options =>
                options.UseSqlServer(Configuration[_connectionStringName]));

            // Add datalayer
            services.AddScoped<RepositoryFactories>();
            services.AddScoped<IDesignTimeDbContextFactory<CoreDbContext>, CoreDbContextFactory>();
            services.AddTransient<IRepositoryProvider, RepositoryProvider>();
            services.AddTransient<IUow, Uow>();

            // Add Application Services
            services.Configure<AppSettings>(Configuration.GetSection("AppSettings"));

            // Add framework services.
            services.AddMvc();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            loggerFactory.AddConsole(Configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseBrowserLink();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
            }

            app.UseStaticFiles();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}
