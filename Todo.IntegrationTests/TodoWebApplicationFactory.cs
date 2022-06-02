using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Todo.Api;
using Todo.Infrastructure;

namespace Todo.IntegrationTests
{
    public class TodoWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureAppConfiguration(configurationBuilder =>
            {
                var integrationConfig = new ConfigurationBuilder()
                    .AddJsonFile("appsettings.json")
                    .AddEnvironmentVariables()
                    .Build();
            });

            builder.ConfigureServices((builder, services) =>
            {
                services.Remove<DbContextOptions<TodoContext>>().AddDb(builder);
                //using (var serviceScope = builder..CreateScope())
                //{
                //    var dbContext = serviceScope.ServiceProvider.GetService<TodoContext>();
                //    dbContext.Database.EnsureDeleted();
                //    dbContext.Database.EnsureCreated();
                //}
            });
        }

    }
}
