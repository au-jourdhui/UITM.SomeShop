using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace SomeShop.Web.Extensions
{
    public static class ConfigureApplicationExtensions
    {
        private const int IdleTimeoutDays = 365;

        public static IServiceCollection AddApplicationConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            configuration.Bind(nameof(SiteSettings), GlobalVariables.SiteSettings);
            services
                .Configure<Microsoft.AspNetCore.Http.Features.FormOptions>(x =>
                {
                    x.ValueLengthLimit = int.MaxValue;
                    x.MultipartBodyLengthLimit = int.MaxValue;
                    x.MemoryBufferThreshold = int.MaxValue;
                })
                .AddSession(options => { options.IdleTimeout = TimeSpan.FromDays(IdleTimeoutDays); })
                .AddMvc()
                .SetCompatibilityVersion(CompatibilityVersion.Version_2_1);
            return services;
        }
    }
}