using MediatR;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;
using System.Collections.Generic;
using System.Threading.Tasks;
using Todo.Api;
using Todo.Core.Common.Abstractions;
using Todo.Core.Common.Enums;
using Todo.Infrastructure;
using Xunit;

namespace Todo.IntegrationTests
{
    public class CommonFixture : IAsyncLifetime
    {
        private WebApplicationFactory<Program> _factory = null!;
        private IConfiguration _configuration = null!;
        private IServiceScopeFactory _scopeFactory = null!;
        private Checkpoint _checkpoint = null!;
        private DataBaseType _dataBaseType;

        public CommonFixture()
        {
            _factory = new TodoWebApplicationFactory();
            _scopeFactory = _factory.Services.GetRequiredService<IServiceScopeFactory>();
            _configuration = _factory.Services.GetRequiredService<IConfiguration>();
            _dataBaseType = _configuration.GetValue<DataBaseType>(nameof(DataBaseType));

            _checkpoint = new Checkpoint
            {
                
                TablesToIgnore = new[] { "__EFMigrationsHistory" },
                 
            };
        }
        public Task InitializeAsync()
        {
            return Task.Run(async () =>
            {
                using (var serviceScope = _scopeFactory.CreateScope())
                {
                    var dbContext = serviceScope.ServiceProvider.GetService<TodoContext>();
                    await dbContext.Database.EnsureDeletedAsync();
                    await dbContext.Database.EnsureCreatedAsync();
                }
            });
        }

        public async Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            using var scope = _scopeFactory.CreateScope();
            var mediator = scope.ServiceProvider.GetRequiredService<ISender>();
            return await mediator.Send(request);
        }


        public async Task Reset()
        {
            var connectionString = _configuration.GetConnectionString(_dataBaseType.ToString());
            await _checkpoint.Reset(connectionString);
        }

        public async Task<TEntity> GetAsync<TEntity>(long Id)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<TEntity>>();
            return await repository.GetAsync(Id);
        }

        public async Task AddAsync<TEntity>(TEntity entity)
            where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<TEntity>>();
            await repository.AddAsync(entity);
            await repository.SaveChangesAsync();
        }

        public async Task AddManyAsync<TEntity>(IEnumerable<TEntity> entities)
             where TEntity : class
        {
            using var scope = _scopeFactory.CreateScope();
            var repository = scope.ServiceProvider.GetRequiredService<IRepository<TEntity>>();
            await repository.AddManyAsync(entities);
            await repository.SaveChangesAsync();
        }


        public async Task DisposeAsync()
        {
            using (var serviceScope = _scopeFactory.CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<TodoContext>();
                await dbContext.Database.EnsureDeletedAsync();
            }
        }

    } 
}
