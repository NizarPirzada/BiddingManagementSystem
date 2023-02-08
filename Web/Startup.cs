using AccountOwnerServer.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.SpaServices.ReactDevelopmentServer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NLog;
using System.IO;
using PanacealogicsSales.Entities.Models;
using Microsoft.AspNetCore.Authentication.Certificate;
using PanacealogicsSales.Web;
using Microsoft.AspNetCore.Http;
using System.Configuration;
using Microsoft.OpenApi.Models;
using System;
using System.Reflection;
using System.Linq;
using Swashbuckle.Swagger;

namespace AccountOwnerServer
{
    public class Startup
    {
        public static string originValue = "";
        public Startup(IConfiguration configuration)
        {
            //LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
            

        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureLoggerService();
            services.ConfigureMySqlContext(Configuration);
            services.ConfigureRepositoryWrapper();
            services
            .AddAuthentication(CertificateAuthenticationDefaults.AuthenticationScheme)
            .AddCertificate(options =>
            {
                options.AllowedCertificateTypes = CertificateTypes.All;
            });
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", policy =>
                {
                    policy.AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowAnyOrigin();
                });
            });


            services.AddControllers();
            ConfigureSwagger(services);
            //services.AddScoped<LogFilter>();
            services.AddSignalR();
        }

        private static void ConfigureSwagger(IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "Alpha Sales", Version = "v1", });
               

                // Set the comments path for the Swagger JSON and UI.

            });
        }

        public  void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            System.IO.Directory.SetCurrentDirectory(env.ContentRootPath);
            GlobalDiagnosticsContext.Set("configDir", "C:\\git\\damienbod\\AspNetCoreNlog\\Logs");
            GlobalDiagnosticsContext.Set("connectionString", "server=127.0.0.1;userid=root;password=root;database=errorlogs;");

            
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "TestService");
            });

            app.UseCors(builder =>
            {
                builder
                   .SetIsOriginAllowed(origin => true)
                    .AllowAnyHeader()
                    .WithMethods("GET", "POST")
                    .AllowCredentials();
            });
            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseAuthentication();
            app.UseCors("CorsPolicy");

            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            app.UseDeveloperExceptionPage();

            app.UseRouting();
            //app.UseCors("ClientPermission");
            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapHub<MessageHub>("/messagehub");
                endpoints.MapControllers();
                endpoints.MapGet("/", async context =>
                {
                    await context.Response.WriteAsync("Asp.Net Core Web Api!");
                });
            });
         
           


        }

    }
}
