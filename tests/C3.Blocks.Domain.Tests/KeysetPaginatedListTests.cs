namespace C3.Blocks.Domain.Tests;

public class KeysetPaginatedListTests
{
    [Fact]
    public void CreateKeysetPaginatedListConstructorTests()
    {
        // Arrange
        var expectedItems = new List<string>
        {
            "one",
            "two",
            "three"
        };
        var expectedMin = DateTime.MinValue;
        var expectedMax = DateTime.MaxValue;
        var expectedSize = 20;

        // Act
        var keysetPaginated = new KeysetPaginatedList<string, DateTime>(
            expectedItems,
            expectedMin,
            expectedMax,
            expectedSize);

        // Assert
        Assert.Equal(expectedItems[0], keysetPaginated.Items[0]);
        Assert.Equal(expectedItems[1], keysetPaginated.Items[1]);
        Assert.Equal(expectedItems[2], keysetPaginated.Items[2]);
        Assert.Equal(expectedSize, keysetPaginated.Size);
        Assert.Equal(expectedMax, keysetPaginated.MaxValue);
        Assert.Equal(expectedMin, keysetPaginated.MinValue);
    }
}
