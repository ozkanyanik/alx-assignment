using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using EFDataService;
namespace PromoCodeManagerApi
{
    public class Startup
    {
        private IConfiguration Configuration { get; }
        private const string CorsSettings = "_corsSettings";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(CorsSettings,
                    builder => builder
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .WithOrigins("http://localhost:4200")
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .AllowAnyHeader()
                        .WithExposedHeaders("Content-Disposition")
                        .Build()
                );
            });
            services.AddLocalization(options =>
            {
                options.ResourcesPath = "Resources";
            });

            services.AddControllers();
            
            //Adding Entitfy Framework Data Service Extension
            services.AddEFDataService();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseCors(CorsSettings);
            app.UseEFCore();

            app.UseHttpsRedirection();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
