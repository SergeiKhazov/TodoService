using Xunit;

namespace Todo.IntegrationTests
{
    [CollectionDefinition("Common Collection")]
    public class CommonCollection: ICollectionFixture<CommonFixture>
    { }
}
