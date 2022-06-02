using FluentAssertions;
using System.Threading.Tasks;
using Todo.Core.Commands;
using Todo.Core.Common.Exceptions;
using Todo.Core.Models;
using Todo.Domain;
using Xunit;


namespace Todo.IntegrationTests
{
    [Collection("Common Collection")]
    public class DeleteTodoItemTests : IAsyncLifetime
    {
        private readonly CommonFixture _common;

        public DeleteTodoItemTests(CommonFixture common) => _common = common;

        [Fact]
        public async Task ShouldRequireValidTodoItemId()
        {
            const long id = 1;

            var command = new DeleteTodoItemCommand { Id = id};

            await FluentActions.Invoking(() =>_common.SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ShouldDeleteTodoItem()
        {
            var createdItem = await _common.SendAsync(new CreateTodoItemCommand { TodoItemDTO = new TodoItemDTO { Name = "Test Name", IsComplete = true } });


            var command = new DeleteTodoItemCommand{ Id = createdItem.Id };

            await _common.SendAsync(command);

            var item = await _common.GetAsync<TodoItem>(createdItem.Id);

            item.Should().BeNull();
        }


        public Task InitializeAsync() => Task.CompletedTask;
        public async Task DisposeAsync()
        {
            await _common.Reset();
        }
    } 
}
