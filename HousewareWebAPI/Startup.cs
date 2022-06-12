using Houseware.WebAPI.Data;
using Houseware.WebAPI.Helpers.Middleware;
using HousewareWebAPI.Helpers.Common;
using HousewareWebAPI.Helpers.Filter;
using HousewareWebAPI.Helpers.Services;
using HousewareWebAPI.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;

namespace HousewareWebAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            var builder = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();
            Configuration = builder.Build();
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var appSettingsSection = Configuration.GetSection("AppSettings");
            services.Configure<AppSettings>(appSettingsSection);
            var appSettings = appSettingsSection.Get<AppSettings>();

            // Enable custom validation with model state
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddDbContext<HousewareContext>(options => options.UseSqlServer(appSettings.DBConnectionString));

            services.AddControllers(options =>
            {
                options.Filters.Add(new ResponseValidationActionFilter());
                options.Filters.Add(new ResponseExceptionActionFilter());
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HousewareWebAPI", Version = "v1" });
            });

            // Dependency servirces
            //services.AddScoped<HttpContext, HttpContext>();
            services.AddScoped<IOAuthService, OAuthService>();
            services.AddScoped<IImageService, ImageService>();
            services.AddScoped<IClassificationService, ClassificationService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<ISpecificationService, SpecificationService>();
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<IAddressService, AddressService>();
            services.AddScoped<IStoreService, StoreService>();
            services.AddScoped<IStoredService, StoredService>();
            services.AddScoped<IGHNService, GHNService>();
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IVNPayService, VNPayService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IOptions<AppSettings> appSettings)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "HousewareWebAPI v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //Enable CORS
            app.UseCors(builder => builder
                .AllowAnyOrigin()
                .AllowAnyHeader()
                .AllowAnyMethod());

            if (appSettings.Value.UsingJWT)
            {
                app.UseMiddleware<JwtMiddleware>();
            }

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
