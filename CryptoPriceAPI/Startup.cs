using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Caching.StackExchangeRedis;
using StackExchange.Redis;
using CryptoPriceAPI.Repositories;

namespace CryptoPriceAPI
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // Bu metod, hizmetleri containere ekler
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            // DbContext yapılandırması
            services.AddDbContext<CryptoDbContext>(options =>
                options.UseSqlServer("Server=localhost,1433;Database=my_database;User Id=sa;Password=YourStrong!Passw0rd;"));

            // Repository yapılandırması
            services.AddScoped<ICryptoRepository, CryptoRepository>();

            // Redis yapılandırması
            services.AddStackExchangeRedisCache(options =>
            {
                options.Configuration = Configuration.GetSection("Redis:Configuration").Value;
            });

            // IConnectionMultiplexer yapılandırması
            services.AddSingleton<IConnectionMultiplexer>(sp =>
            {
                var configuration = Configuration.GetSection("Redis:Configuration").Value;
                return ConnectionMultiplexer.Connect(configuration);
            });

            // RedisCacheService'i ekleyin
            services.AddSingleton<IRedisCacheService, RedisCacheService>();

            // CORS yapılandırması
            services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", builder =>
                {
                    builder.AllowAnyOrigin()
                           .AllowAnyMethod()
                           .AllowAnyHeader();
                });
            });

            // Swagger yapılandırması
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CryptoPriceAPI", Version = "v1" });
            });
        }

        // Bu metod, HTTP istek boru hattını yapılandırır
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "CryptoPriceAPI v1"));
            }

            app.UseRouting();

            // CORS yapılandırması
            app.UseCors("AllowAll");

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
