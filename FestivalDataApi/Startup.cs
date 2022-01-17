using FestivalDataApi.BusinessLogic.Handlers;
using FestivalDataApi.Utilities;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerUI;
using System;
using System.IO;
using System.Linq;
using System.Reflection;
using Serilog;

namespace FestivalDataApi
{
    /// <summary>
    /// This class is used to configure dependancy injection and application configuration management 
    /// and other service configurations
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// Startup constructor with IWebHostEnvironment
        /// </summary>
        /// <param name="env"></param>
        public Startup(IWebHostEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", true, true)
                .AddEnvironmentVariables();
            ConfigurationRoot = builder.Build();
        }

        public IConfigurationRoot ConfigurationRoot { get; }

        /// <summary>
        /// This method gets called by the runtime. Use this method to add services to the container..
        /// </summary>
        /// <param name="services"></param>
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();

            services.AddMvc().AddJsonOptions(o =>
            {
                o.JsonSerializerOptions.PropertyNamingPolicy = null;
                o.JsonSerializerOptions.DictionaryKeyPolicy = null;
            });

            services.AddMemoryCache();

            services.AddMediatR(Assembly.GetAssembly(typeof(GetRecordLabelsHandler)));

            services.AddSingleton(ConfigurationRoot);

            services.AddHttpClientRegistry(ConfigurationRoot);

            services.AddSwaggerGen(c =>
            {
                var provider = services.BuildServiceProvider().GetRequiredService<IApiDescriptionGroupCollectionProvider>();

                foreach (var description in provider.ApiDescriptionGroups.Items.Where(x => x.GroupName != null))
                {
                    c.SwaggerDoc(description.GroupName, new OpenApiInfo { Title = "Music Festival Data Api", Version = description.GroupName });
                }
                c.CustomSchemaIds((type) => type.Name);

                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                c.IncludeXmlComments(xmlPath);
                c.DescribeAllParametersInCamelCase();
            });

            services.AddMemoryCache();
        }

        /// <summary>
        /// This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        /// </summary>
        /// <param name="app"></param>
        /// <param name="env"></param>
        /// <param name="provider"></param>
        public void Configure(IApplicationBuilder app,
                              IWebHostEnvironment env,
                              IHostApplicationLifetime appLifetime,
                              IApiDescriptionGroupCollectionProvider provider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    foreach (var description in provider.ApiDescriptionGroups.Items.Where(x => x.GroupName != null))
                    {
                        c.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName);
                    }
                    c.DocExpansion(DocExpansion.None);
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            //Ensure any buffered events are sent at shutdown

            appLifetime.ApplicationStopped.Register(Log.CloseAndFlush);

            app.UseSerilogRequestLogging();

            app.UseAuthorization();

            app.Use(async (context, next) =>
            {
                HttpRequestRewindExtensions.EnableBuffering(context.Request);
                await next();
            });

            app.UseMiddleware<ExceptionHandler>(); // This should appear before app.UseEndpoints

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
