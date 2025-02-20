namespace C3.Blocks.Domain.Tests;

public class ResultFactoriesTests
{
    const string ExpectedData = "my data";
    const string ExpectedMessage = "my message";

    [Fact]
    public void MakeUnsuccessfulResultWithMessage()
    {
        // Arrange, Act
        var result = ExpectedData.CreateUnsuccessfulResult(ExpectedMessage);

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ExpectedData, result.Data);
        Assert.Equal(ExpectedMessage, result.Message);
    }

    [Fact]
    public void MakeUnsuccessfulResultWithoutMessage()
    {
        // Arrange, Act
        var result = ExpectedData.CreateUnsuccessfulResult();

        // Assert
        Assert.False(result.Success);
        Assert.Equal(ExpectedData, result.Data);
        Assert.Equal(string.Empty, result.Message);
    }

    [Fact]
    public void MakeUnsuccessfulResultWithoutData()
    {
        // Arrange, Act
        var result = ResultFactories.CreateUnsuccessfulResult<string>(message: ExpectedMessage);

        // Act
        Assert.False(result.Success);
        Assert.Null(result.Data);
        Assert.Equal(ExpectedMessage, result.Message);
    }

    [Fact]
    public void MakeSuccessfulResultWithoutData()
    {
        // Arrange, Act
        var result = ExpectedData.CreateSuccessfulResult(ExpectedMessage);

        // Act
        Assert.True(result.Success);
        Assert.Equal(ExpectedData, result.Data);
        Assert.Equal(ExpectedMessage, result.Message);
    }

    [Fact]
    public void MakeSuccessfulResultWithoutMessage()
    {
        // Arrange, Act
        var result = ExpectedData.CreateSuccessfulResult();

        // Act
        Assert.True(result.Success);
        Assert.Equal(ExpectedData, result.Data);
        Assert.Equal(string.Empty, result.Message);
    }
}
