using C3.Blocks.Domain.Tests.Stubs;

namespace C3.Blocks.Domain.Tests;

public class EntityTests
{
    [Fact]
    public void CreateEntityTest()
    {
        // Arrange
        var id = Guid.NewGuid().ToString();

        // Act
        var entity = new MyEntity(id);

        // Assert
        Assert.Equal(id, entity.Id);
        Assert.Equal(id.GetHashCode(StringComparison.Ordinal) + 17, entity.GetHashCode());
#pragma warning disable CA1508 // Avoid dead conditional code
        Assert.False(entity.Equals(null));
#pragma warning restore CA1508 // Avoid dead conditional code
        Assert.False(entity.Equals(12));
        Assert.True(entity.Equals(entity));
        Assert.True(entity.Equals(new MyEntity(id)));
    }

    [Fact]
    public void CreateEntityIdExceptionTest()
        => Assert.Throws<ArgumentNullException>(() => new MyEntity(null!));
}
