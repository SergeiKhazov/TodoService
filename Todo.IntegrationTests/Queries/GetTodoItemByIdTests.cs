using Bogus;
using FluentAssertions;
using System.Threading.Tasks;
using Todo.Core.Common.Exceptions;
using Todo.Core.Queries;
using Todo.Domain;
using Xunit;


namespace Todo.IntegrationTests
{
    [Collection("Common Collection")]
    public class GetTodoItemByIdTests : IAsyncLifetime
    {
        private readonly CommonFixture _common;

        public GetTodoItemByIdTests(CommonFixture common) => _common = common;
        public Task InitializeAsync() => Task.CompletedTask;

        [Fact]
        public async Task ShouldNotReturnNotFound()
        {
            var faker = new Faker<TodoItem>()
                .RuleFor(b => b.Name, f => f.Name.FullName())
                .RuleFor(b => b.IsComplete, f => f.Random.Bool());

            var fake = faker.Generate();
            await _common.AddAsync(fake);

            var query = new GetTodoItemByIdQuery { Id = fake.Id };
            await FluentActions.Invoking(() => _common.SendAsync(query)).Should().NotThrowAsync<NotFoundException>();
        }

        public async Task DisposeAsync()
        {
            await _common.Reset();
        }
    } 
}
