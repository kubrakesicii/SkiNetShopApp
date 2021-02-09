using API.Extensions;
using API.Helpers;
using API.Middleware;
using AutoMapper;
using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Identity;
using Infrastructure.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Redis;

namespace API
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            _config = configuration;
        }

        public IConfiguration _config { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddDbContext<StoreContext>(x => x.UseSqlServer(_config.GetConnectionString("DefaultConnection")));
            services.AddAutoMapper(typeof(MappingProfiles));

            services.AddDbContext<AppIdentityDbContext>(x => x.UseSqlServer(_config.GetConnectionString("IdentityConnection")));

            services.AddSingleton<IConnectionMultiplexer>(c => {
                var configuration = ConfigurationOptions.Parse(_config.GetConnectionString("Redis"));
                return ConnectionMultiplexer.Connect(configuration);
            });

            services.AddApplicationServices();
            services.AddSwaggerDocumentation();
            services.AddIdentityServices(_config);
            services.AddCors(opt => {
                opt.AddPolicy("CorsPolicy",policy => {
                    policy.AllowAnyHeader().AllowAnyMethod().WithOrigins("https://localhost:4200");
                });
            });
            services.AddScoped<ITokenService,TokenService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseMiddleware<ExceptionMiddleware>();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseStaticFiles();

            app.UseCors("CorsPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseSwaggerDocumentation();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
