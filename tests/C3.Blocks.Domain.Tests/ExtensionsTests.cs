namespace C3.Blocks.Domain.Tests;

public class ExtensionsTests
{
    [Fact]
    public void ToSlugMethodTest()
    {
        // Arrange
        var toProcess = "it'll work on this!";

        // Act
        var result = toProcess.ToSlug();

        // Assert
        Assert.Equal("it-ll-work-on-this-", result);
    }
}
