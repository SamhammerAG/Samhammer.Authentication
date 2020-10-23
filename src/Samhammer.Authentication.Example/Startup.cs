using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Samhammer.Authentication.Api.Guest;
using Samhammer.Authentication.Api.Jwt;
using Samhammer.Authentication.Api.Keycloak;
using Samhammer.Swagger.Authentication;
using Samhammer.Swagger.Default;
using Swashbuckle.AspNetCore.Filters;
using Swashbuckle.AspNetCore.SwaggerGen;

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
            
            // TODO move this to Samhammer.Swagger.Authentication
            services.Configure<SwaggerGenOptions>(c =>
            {
                var apiKeyRef = new OpenApiReference
                {
                    Id = GuestAuthenticationDefaults.AuthenticationScheme,
                    Type = ReferenceType.SecurityScheme
                };

                var apiKeyScheme = new OpenApiSecurityScheme
                {
                    Reference = apiKeyRef,
                    Type = SecuritySchemeType.ApiKey,
                    In = ParameterLocation.Header,
                    Name = GuestAuthenticationDefaults.HeaderKey
                };

                c.AddSecurityDefinition(apiKeyRef.Id, apiKeyScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement { { apiKeyScheme, new string[] {} } });

                c.OperationFilter<SecurityRequirementsOperationFilter>(true, apiKeyScheme.Reference.Id);
            });
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
