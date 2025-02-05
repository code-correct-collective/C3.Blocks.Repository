using Microsoft.EntityFrameworkCore.Storage;

namespace C3.Blocks.Repository.MsSql.Tests;

public class TransactionWrapperTests
{
    [Fact]
    public async Task RollbackMethodTest()
    {
        await RunTest((w, t) =>
        {
            // Act
#pragma warning disable CA1849
            w.Rollback();

            // Assert
            t.Received(1).Rollback();
#pragma warning restore CA1849
            return Task.CompletedTask;
        });

    }
    [Fact]
    public async Task RollbackAsyncMethodTest()
    {
        await RunTest(async (w, t) =>
        {
            // Act
            await w.RollbackAsync();

            // Assert
            await t.Received(1).RollbackAsync();
        });

    }

    [Fact]
    public async Task TransactionIdPropertyTest()
    {
        await RunTest((w, t) =>
        {
            // Arrange
            var expectedId = Guid.NewGuid();
            t.TransactionId.Returns(expectedId);

            // Act
            var id = w.TransactionId;

            // Assert
            Assert.Equal(expectedId, id);

            return Task.CompletedTask;
        });
    }

    [Fact]
    public async Task CommitMethodTest()
    {
        await RunTest((w, t) =>
        {
            // Act
#pragma warning disable CA1849
            w.Commit();

            // Assert
            t.Received(1).Commit();
#pragma warning restore CA1849
            return Task.CompletedTask;
        });
    }

    [Fact]
    public async Task CommitAsyncMethodTest()
    {
        await RunTest(async (w, t) =>
        {
            // Act
            await w.CommitAsync();

            // Assert
            await t.Received(1).CommitAsync();
        });
    }

    [Fact]
    public async Task DisposeAsyncMethodTest()
    {
        await RunTest(async (w, t) =>
        {
            // Act
            await w.DisposeAsync();

            // Assert
            try
            {
                await w.CommitAsync();
                Assert.Fail("Object was not disposed");
            }
            catch (ObjectDisposedException)
            {
            }

        });
    }

    [Fact]
    public async Task DisposeMethodTest()
    {
        await RunTest(async (w, t) =>
        {
            // Act
#pragma warning disable CA1849
            w.Dispose();
#pragma warning restore CA1849

            // Assert
            try
            {
                await w.RollbackAsync();
                Assert.Fail("Object was not disposed");
            }
            catch (ObjectDisposedException)
            {
            }
        });
    }

    private static async Task RunTest(Func<TransactionWrapper, IDbContextTransaction, Task> runnerAsync)
    {
        var t = Substitute.For<IDbContextTransaction>();
        using var w = new TransactionWrapper(t);

        await runnerAsync(w, t);
    }
}
