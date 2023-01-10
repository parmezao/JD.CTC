using JD.CTC.Data.Repositories.DataContext;
using JD.CTC.Data.Repositories.Interfaces;
using JD.CTC.Data.Repositories.Repository;
using JD.CTC.Presentation.Blazor.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using Microsoft.Data.SqlClient;

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


            // Registra os serviços de conexão (EntityFramework e Dapper).
            var tipoBanco = Configuration["TipoBanco"].ToString();

            switch (tipoBanco)
            {
                case "Oracle":
                {
                    var conbuilderOracle = new OracleConnectionStringBuilder(Configuration["ConnectionStrings:OracleConnection"])
                    {
                        ConnectionTimeout = 90,
                    };
                    services.AddSingleton<IDbConnection>(x => new OracleConnection(conbuilderOracle.ToString()));

                    break;
                };
                case "SQLServer":
                    {
                        var conbuilder = new SqlConnectionStringBuilder(Configuration["ConnectionStrings:DefaultConnection"])
                        {
                            ConnectTimeout = 90,
                            ConnectRetryInterval = 5,
                            ConnectRetryCount = 10,
                            MultipleActiveResultSets = true
                        };
                        services.AddSingleton<IDbConnection>(x => new SqlConnection(conbuilder.ToString()));

                        break;
                    };
                default:
                    break;
            }

            services.AddSingleton(p => new CTCContext(Configuration["ConnectionStrings:DefaultConnection"]));            

            // Registra os Repositórios
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
