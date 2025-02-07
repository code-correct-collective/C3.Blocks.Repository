namespace C3.Blocks.Domain.Tests;

public class PaginatedListFactoriesTests
{
    [Fact]
    public void CreatePaginatedListMethodTest()
    {
        // arrange
        List<string> expected = ["one", "two", "three"];
        var expectedTotal = 3;
        var expectedSize = 20;
        var expectedPage = 1;

        // Act
        var paginated = expected.CreatePaginatedList(expectedTotal, expectedPage, expectedSize);

        // Assert
        Assert.Equal(expected[0], paginated.Items[0]);
        Assert.Equal(expected[1], paginated.Items[1]);
        Assert.Equal(expected[2], paginated.Items[2]);
        Assert.Equal(expectedPage, paginated.Page);
        Assert.Equal(expectedSize, paginated.Size);
        Assert.Equal(expectedTotal, paginated.Total);
        Assert.Equal(1, paginated.TotalPages);
    }

    [Fact]
    public void CreateKeysetPaginatedListMethodTests()
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
        var keysetPaginated = expectedItems.CreateKeysetPaginatedList(expectedMin, expectedMax, expectedSize);

        // Assert
        Assert.Equal(expectedItems[0], keysetPaginated.Items[0]);
        Assert.Equal(expectedItems[1], keysetPaginated.Items[1]);
        Assert.Equal(expectedItems[2], keysetPaginated.Items[2]);
        Assert.Equal(expectedSize, keysetPaginated.Size);
        Assert.Equal(expectedMax, keysetPaginated.MaxValue);
        Assert.Equal(expectedMin, keysetPaginated.MinValue);
    }
}
