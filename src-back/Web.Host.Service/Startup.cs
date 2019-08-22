using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Linq;
using Microsoft.AspNetCore.HttpOverrides;
using Web.Host.Service.Infrastructure.DI;
using Cqrs.Services;

namespace Web.Host.Service
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            // конфигурация сервиса
            services
                .AddCorsConfiguration()
                .AddDatabases(Configuration)
                .AddServiceConfig(Configuration)
                .AddHttpRequests()
                .AddSourceParsers()
                .AddWebSourceLoader()
                .AddAutomapperConfiguration()
                .AddCqrsServices(new Type[] {
                        typeof(Web.Host.Cqrs.AssemblyLinks.CqrsAssemblyLink)
                })
                .AddMvc().SetCompatibilityVersion(CompatibilityVersion.Version_2_2);
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app
                .UseCors()
                .UseForwardedHeaders(new ForwardedHeadersOptions
                {
                    ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
                })
                .InitializeDatabase()
                .UseMvc();
        }



    }
}
