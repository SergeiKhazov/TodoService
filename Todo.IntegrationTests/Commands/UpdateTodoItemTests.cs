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
    public class UpdateTodoItemTests : IAsyncLifetime
    {
        private readonly CommonFixture _common;

        public UpdateTodoItemTests(CommonFixture common) => _common = common;

        [Fact]
        public async Task ShouldRequireEqualTodoItemId()
        {
            var command = new UpdateTodoItemCommand { Id = 1, TodoItemDTO = new TodoItemDTO {Id = 2, Name = "Test Name", IsComplete = true } };
            await FluentActions.Invoking(() => _common.SendAsync(command)).Should().ThrowAsync<BadRequestException>();
        }

        [Fact]
        public async Task ShouldRequireValidTodoItemId()
        {
            var command = new UpdateTodoItemCommand { Id = 1, TodoItemDTO = new TodoItemDTO { Id = 1, Name = "Test Name", IsComplete = true } };
            await FluentActions.Invoking(() => _common.SendAsync(command)).Should().ThrowAsync<NotFoundException>();
        }

        [Fact]
        public async Task ShouldUpdateTodoItem()
        {
            var createdItem = await _common.SendAsync(new CreateTodoItemCommand { TodoItemDTO = new TodoItemDTO { Name = "Bad Name", IsComplete = false } } );

            var itemUpdate = new TodoItemDTO { Id = createdItem.Id, Name = "Test Name", IsComplete = true };
            var command = new UpdateTodoItemCommand{Id = createdItem.Id, TodoItemDTO = itemUpdate };

            await _common.SendAsync(command);

            var item = await _common.GetAsync<TodoItem>(createdItem.Id);

            item.Should().NotBeNull();
            item.Name.Should().Be(itemUpdate.Name);
            item.IsComplete.Should().Be(itemUpdate.IsComplete);
        }

        public Task InitializeAsync() => Task.CompletedTask;
        public async Task DisposeAsync()
        {
            await _common.Reset();
        }
    } 
}
