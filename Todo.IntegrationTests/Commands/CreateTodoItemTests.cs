using FluentAssertions;
using System;
using System.Threading.Tasks;
using Todo.Core.Commands;
using Todo.Core.Models;
using Todo.Domain;
using Xunit;
using Xunit.Abstractions;


namespace Todo.IntegrationTests
{
    [Collection("Common Collection")]
    public class CreateTodoItemTests: IAsyncLifetime
    {
        private readonly CommonFixture _common;

        public CreateTodoItemTests(CommonFixture common)
        {
            _common = common;
        }
        public Task InitializeAsync() => Task.CompletedTask;

        [Fact]
        public async Task ShouldCreateTodoItem()
        {
            var command = new CreateTodoItemCommand{ TodoItemDTO = new TodoItemDTO { Name = "Test Name", IsComplete = true } };

            var id = (await _common.SendAsync(command)).Id;

            var item = await _common.GetAsync<TodoItem>(id);

            item.Should().NotBeNull();
            item.Name.Should().Be(command.TodoItemDTO.Name);
            item.IsComplete.Should().Be(command.TodoItemDTO.IsComplete);
        }

        public async Task DisposeAsync()
        {
            await _common.Reset();
        }
    } 
}
