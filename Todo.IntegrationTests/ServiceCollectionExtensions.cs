using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using Todo.Core.Common.Enums;
using Todo.Infrastructure;

namespace Todo.IntegrationTests
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection Remove<TService>(this IServiceCollection services)
        {
            var serviceDescriptor = services.FirstOrDefault(d =>
                d.ServiceType == typeof(TService));

            if (serviceDescriptor != null)
            {
                services.Remove(serviceDescriptor);
            }

            return services;
        }

        public static IServiceCollection AddDb(this IServiceCollection services, WebHostBuilderContext builderContext)
        {
            services.AddDbContext<TodoContext>(options =>
            {
                string connString;
                var dataBaseType = builderContext.Configuration.GetValue<DataBaseType>(nameof(DataBaseType));
                switch (dataBaseType)
                {
                    case DataBaseType.SQLServer:
                        connString = builderContext.Configuration.GetConnectionString(nameof(DataBaseType.SQLServer));
                        options.UseSqlServer(connString,
                            b => b.MigrationsAssembly(typeof(TodoContext).Assembly.FullName));
                        return;
                    case DataBaseType.InMemory:
                        options.UseInMemoryDatabase(builderContext.Configuration.GetConnectionString(nameof(DataBaseType.InMemory)) ?? nameof(TodoContext));
                        return;
                    default:
                        options.UseInMemoryDatabase(builderContext.Configuration.GetConnectionString(nameof(DataBaseType.InMemory)) ?? nameof(TodoContext));
                        return;
                };

            });
            return services;
        }

    } 
}
