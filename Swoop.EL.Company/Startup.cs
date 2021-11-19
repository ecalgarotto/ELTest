using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Swoop.EL.Company.BAL.Interfaces;
using Swoop.EL.Company.BAL.Services;
using Swoop.EL.Company.Common;
using Swoop.EL.Company.Common.Cache;
using Swoop.EL.Company.DAL.Interfaces;
using Swoop.EL.Company.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace Swoop.EL.Company
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
            services.AddControllers();

            services.AddTransient<ICacheProvider>(p =>
            {
                CacheAppSettings settings = new CacheAppSettings();
                Configuration.GetSection("CacheAppSettings").Bind(settings);

                Assembly assembly = Assembly.Load(settings.CacheProvider);

                Type assemblyType = assembly.GetType(settings.Provider);
                if (assemblyType != null)
                {
                    Type[] argTypes = new Type[] { };

                    ConstructorInfo cInfo = assemblyType.GetConstructor(argTypes);

                    ICacheProvider cacheProvider = (ICacheProvider)cInfo.Invoke(null);

                    return cacheProvider;
                }
                else
                {
                    // Error checking is needed to help catch instances where
                    throw new NotImplementedException();
                }

            });

            services.AddTransient<ICustomAppSettings>(p =>
            {
                CustomAppSettings settings = new CustomAppSettings();
                Configuration.GetSection("CustomAppSettings").Bind(settings);                
                return settings;
            });

            

            services.AddSwaggerGen();

            services.AddHttpContextAccessor();
            services.AddHttpClient();

            services.AddScoped<ICompanyService, CompanyService>();
            services.AddScoped<IOfficerService, OfficerService>();

            services.AddScoped<ICompanyRepository, CompanyRepository>();
            services.AddScoped<IOfficerRepository, OfficerRepository>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                    options.RoutePrefix = string.Empty;
                });
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
