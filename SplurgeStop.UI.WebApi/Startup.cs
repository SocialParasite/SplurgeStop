using System.Text.Json.Serialization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SplurgeStop.Data.EF;
using SplurgeStop.Data.EF.Repositories;
using SplurgeStop.Domain;
using SplurgeStop.Domain.CityProfile;
using SplurgeStop.Domain.DA_Interfaces;
using SplurgeStop.Domain.LocationProfile;
using SplurgeStop.Domain.ProductProfile;
using SplurgeStop.Domain.ProductProfile.BrandProfile;
using SplurgeStop.Domain.ProductProfile.SizeProfile;
using SplurgeStop.Domain.ProductProfile.TypeProfile;
using SplurgeStop.Domain.PurchaseTransaction;
using SplurgeStop.Domain.StoreProfile;

namespace SplurgeStop.UI.WebApi
{
    public class Startup
    {
        private IConfiguration Configuration { get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connString = Configuration.GetConnectionString("DEV");

            services.AddDbContext<SplurgeStopDbContext>(options => options.UseSqlServer(connString));

            services.AddTransient<IUnitOfWork, EfCoreUnitOfWork>();
            services.AddTransient<IPurchaseTransactionRepository, PurchaseTransactionRepository>();
            services.AddTransient<IPurchaseTransactionService, PurchaseTransactionService>();
            services.AddTransient<IStoreRepository, StoreRepository>();
            services.AddTransient<IStoreService, StoreService>();
            services.AddTransient<ICityRepository, CityRepository>();
            services.AddTransient<ICityService, CityService>();
            services.AddTransient<ILocationRepository, LocationRepository>();
            services.AddTransient<ILocationService, LocationService>();
            services.AddTransient<IBrandRepository, BrandRepository>();
            services.AddTransient<IBrandService, BrandService>();
            services.AddTransient<IProductRepository, ProductRepository>();
            services.AddTransient<IProductService, ProductService>();
            services.AddTransient<IProductTypeRepository, ProductTypeRepository>();
            services.AddTransient<IProductTypeService, ProductTypeService>();
            services.AddTransient<ISizeRepository, SizeRepository>();
            services.AddTransient<ISizeService, SizeService>();

            services.AddCors(options => options.AddPolicy("CorsPolicy",
                builder => builder.AllowAnyMethod()
                                  .AllowAnyHeader()
                                  .WithOrigins("http://localhost:3000")
                                  .AllowCredentials()));

            services.AddControllers();
            services.AddHttpClient();

            services.AddControllers().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
            });

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
