using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Todo.Core.Common.Abstractions;
using Todo.Core.Mappings;
using Todo.Core.Models;
using Todo.Domain;
using Todo.Infrastructure;
using Todo.Infrastructure.Services;
using Todo.Api.Swagger;
using Todo.Core.Common.Enums;
using Todo.Api.Filters;
using Todo.Api.Extensions;

namespace Todo.Api
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
            ConfigureDatabase(services);
            services.AddMediatR(typeof(TodoItemDTO).Assembly);
            services.AddCustomSwagger();
            services.AddCustomControllers();
            services.AddAutoMapper(this.GetType().Assembly);
            services.AddTransient<IRepository<TodoItem>, Repository<TodoItem>>();
            services.AddAutoMapper(config => config.AddProfile<MappingProfile>());
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseCustomSwagger();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            var serviceScopeFactory = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>();
            using (var serviceScope = serviceScopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<TodoContext>();
                dbContext.Database.EnsureCreated();
            }
        }

        public void ConfigureDatabase(IServiceCollection services)
        {
            services.AddDbContext<TodoContext>(options =>
            {
                string connString;
                switch (Configuration.GetValue<DataBaseType>(nameof(DataBaseType)))
                {
                    case DataBaseType.SQLServer:
                        connString = Configuration.GetConnectionString(nameof(DataBaseType.SQLServer));
                        options.UseSqlServer(connString);
                        return;
                    case DataBaseType.InMemory:
                        options.UseInMemoryDatabase(Configuration.GetConnectionString(nameof(DataBaseType.InMemory)) ?? nameof(TodoContext));
                        return;
                    default:
                        options.UseInMemoryDatabase(Configuration.GetConnectionString(nameof(DataBaseType.InMemory)) ?? nameof(TodoContext));
                        return;
                };

            });
        }
    }
}
