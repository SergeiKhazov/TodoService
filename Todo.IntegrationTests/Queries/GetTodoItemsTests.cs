using Bogus;
using FluentAssertions;
using System.Threading.Tasks;
using Todo.Core.Queries;
using Todo.Domain;
using Xunit;


namespace Todo.IntegrationTests
{
    [Collection("Common Collection")]
    public class GetTodoItemsTests : IAsyncLifetime
    {
        private readonly CommonFixture _common;

        public GetTodoItemsTests(CommonFixture common)
        {
            _common = common;
        }
        public Task InitializeAsync() => Task.CompletedTask;

        [Fact]
        public async Task ShouldGetTodoItems()
        {
            const int count = 7;
            var faker = new Faker<TodoItem>()
                .RuleFor(b => b.Name, f => f.Name.FullName())
                .RuleFor(b => b.IsComplete, f => f.Random.Bool());

            var fakes = faker.Generate(count);

            await _common.AddManyAsync(fakes);

            var query = new GetTodoItemsQuery();

            var result = await _common.SendAsync(query);

            result.Should().HaveCount(count);
        }

        public async Task DisposeAsync()
        {
            await _common.Reset();
        }
    } 
}
