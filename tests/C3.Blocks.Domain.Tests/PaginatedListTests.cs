namespace C3.Blocks.Domain.Tests;

public class PaginatedListTests
{
    [Fact]
    public void CreatePaginatedListTest()
    {
        // Arrange
        var expectedSize = 20;
        var expectedTotal = 3;
        var expectedPage = 1;
        List<string> list = ["one", "two", "three"];

        // Act
        var paginated = new PaginatedList<string>(list, expectedTotal, expectedSize, expectedPage);

        // Assert
        Assert.Equal(paginated.Items[0], list[0]);
        Assert.Equal(paginated.Items[1], list[1]);
        Assert.Equal(paginated.Items[2], list[2]);
        Assert.Equal(expectedSize, paginated.Size);
        Assert.Equal(expectedTotal, paginated.Total);
        Assert.Equal(expectedPage, paginated.Page);
        Assert.Equal(1, paginated.TotalPages);
    }

    [Fact]
    public void CreatePaginatedListWithNullItemsTest()
        => Assert.Throws<ArgumentNullException>(() => new PaginatedList<string>(null!, 0, 0, 0));
}
