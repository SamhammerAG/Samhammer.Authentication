using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Samhammer.Authentication.Api.Guest;
using Samhammer.Authentication.Api.Jwt;
using Samhammer.Authentication.Api.Keycloak;
using Samhammer.Swagger.Authentication;
using Samhammer.Swagger.Default;

namespace Samhammer.Authentication.Example
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
            services.AddControllers();

            services.AddJwtAuthentication()
                .AddKeycloak(Configuration)
                .AddGuest(Configuration);

            services.AddSwaggerGen();
            services.AddSwaggerDefaultApi();
            services.AddSwaggerAuthentication(Configuration);
            services.AddSwaggerGuest(Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseRouting();

            app.UseSwagger();
            app.UseSwaggerUI();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
