using JD.CTC.Data.Repositories.DataContext;
using JD.CTC.Data.Repositories.Interfaces;
using JD.CTC.Data.Repositories.Repository;
using JD.CTC.Presentation.Blazor.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace JD.CTC.Presentation.Blazor
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();
            services.AddServerSideBlazor();
            services.AddSingleton<WeatherForecastService>();

            var conbuilder = new Microsoft.Data.SqlClient.SqlConnectionStringBuilder(Configuration["ConnectionStrings:DefaultConnection"])
            {
                ConnectTimeout = 90,
                ConnectRetryInterval = 5,
                ConnectRetryCount = 10,
                MultipleActiveResultSets = true
            };

            services.AddSingleton<IDbConnection>(x => new Microsoft.Data.SqlClient.SqlConnection(conbuilder.ToString()));
            services.AddSingleton(p => new CTCContext(Configuration["ConnectionStrings:DefaultConnection"]));

            services.AddScoped<ILegadoRepository, LegadoRepository>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapBlazorHub();
                endpoints.MapFallbackToPage("/_Host");
            });
        }
    }
}
