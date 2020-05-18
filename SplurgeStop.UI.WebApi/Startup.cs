using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.PurchaseTransaction;

namespace SplurgeStop.UI.WebApi
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
            var connString = Configuration.GetConnectionString("DEV");

            services.AddDbContext<SplurgeStopDbContext>(options => options.UseSqlServer(connString));

            services.AddTransient<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddTransient<IPurchaseTransactionRepository, PurchaseTransactionRepository>();
            services.AddTransient<IPurchaseTransactionService, PurchaseTransactionService>();

            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyMethod()
                                  .AllowAnyHeader()
                                  .WithOrigins("http://localhost:3000")
                                  .AllowCredentials()));

            services.AddControllers();
            services.AddHttpClient();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseCors("CorsPolicy");

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseHttpsRedirection();
            }

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
