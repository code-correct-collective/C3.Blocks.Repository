namespace C3.Blocks.Domain.Tests;

public class ResultTests
{
    [Fact]
    public void CreateResultTest()
    {
        // Arrange
        var expectedData = "My Data";
        var expectedSuccess = true;
        var expectedMessage = "My message";

        // Act
        var result = new Result<string>(expectedData, expectedSuccess, expectedMessage);

        // Assert
        Assert.Equal(expectedData, result.Data);
        Assert.Equal(expectedSuccess, result.Success);
        Assert.Equal(expectedMessage, result.Message);
    }
}
