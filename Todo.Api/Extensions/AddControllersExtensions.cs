using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using System;
using Todo.Api.Filters;

namespace Todo.Api.Extensions
{
    public static class AddControllersExtensions
    {
        public static IServiceCollection AddCustomControllers(this IServiceCollection services, Action<MvcOptions> configure = null)
        {
            services.AddControllers(options =>
                {
                    options.Filters.Add<ExtendedExceptionFilterAttribute>();
                    if (configure != null)
                    {
                        configure(options);
                    }
                });

            return services;
        }
    }
}
