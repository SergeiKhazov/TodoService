using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using Todo.Core.Configuration;

namespace Todo.Api.Swagger
{
    public static class AddCustomSwaggerExtensions
	{
		public static IServiceCollection AddCustomSwagger(this IServiceCollection services)
		{
			services.AddSwaggerGen(cfg =>
			{
				var serviceProvider = services.BuildServiceProvider();
				var microserviceConfiguration = serviceProvider.GetService<IOptions<MicroserviceConfiguration>>()?.Value;
				cfg.SwaggerDoc("v1", new OpenApiInfo
				{
					Title = microserviceConfiguration?.Name ?? String.Empty,
					Version = microserviceConfiguration?.Version ?? "0.0.0",
					Contact = new OpenApiContact
					{
						Name = "Sergei Khazov",
						Email = "hazerbrook@gmail.com",
					},
				});
            });

			return services;
		}

		public static IApplicationBuilder UseCustomSwagger(this IApplicationBuilder app)
		{
			app.UseSwagger().UseSwaggerUI(options =>
			{
				var microserviceConfiguration = app.ApplicationServices.GetService<IOptions<MicroserviceConfiguration>>()?.Value;
				options.SwaggerEndpoint("/swagger/v1/swagger.json", microserviceConfiguration?.Name ?? String.Empty);
				options.DocumentTitle = microserviceConfiguration?.Name ?? String.Empty;
			});

			return app;
		}
	}
}
