using C3.Blocks.Repository.MsSql.Tests.Stubs;

namespace C3.Blocks.Repository.MsSql.Tests;

public class RepositoryBaseTests : TestBase
{
    [Fact]
    public async Task FindAsyncMethodTest()
    {
        await this.RunTestAsync(async (u, lm, cancellationToken) =>
        {
            // Act
            var result = await u.Repository.FindAsync(this.Entities[2].Id, cancellationToken);

            // Assert
            Assert.Equal(this.Entities[2], result);
            lm.Received().LogTraceMethod(nameof(u.Repository.FindAsync), [this.Entities[2].Id]);
            lm.Received().LogFindEntity(LogLevel.Information, [this.Entities[2].Id]);
        });
    }

    [Fact]
    public async Task FindAsyncMethodNullIdTest()
    {
        await this.RunTestAsync(async (u, lm, c) =>
        {
            // Act, Assert
            await Assert.ThrowsAsync<ArgumentNullException>(async () => await u.Repository.FindAsync(null!, c));
        });
    }

    [Fact]
    public async Task AddAsyncMethodNullArgumentTest()
    {
        await this.RunTestAsync(async (u, lm, c) =>
        {
            // Act, Arrange, Assert
            await Assert.ThrowsAnyAsync<ArgumentNullException>(async () => await u.Repository.AddAsync(null!));
        });
    }

    [Fact]
    public async Task AddAsyncMethodTest()
    {
        await this.RunTestAsync(async (u, lm, c) =>
        {
            // Arrange
            var e = new MyEntity();

            // Act
            var result = await u.Repository.AddAsync(e, c);
            await u.CommitAsync();

            var result2 = await u.Repository.FindAsync(e.Id, c);

            // Assert
            Assert.NotNull(result2);
            Assert.Equal(e, result);
            Assert.Equal(e, result2);
            lm.Received().LogTraceMethod(nameof(u.Repository.AddAsync), [e]);
            lm.Received().LogAddEntityAttempt(e.Id);
        });
    }

    [Fact]
    public async Task AddRangeAsyncMethodTest()
    {
        await this.RunTestAsync(async (u, lm, c) =>
        {
            // Arrange
            var e = new MyEntity();

            // Act
            await u.Repository.AddRangeAsync([e], c);
            await u.CommitAsync();
            var result = await u.Repository.FindAsync(e.Id, c);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(e, result);
        });
    }

    [Fact]
    public async Task AddRangeAsyncMethodNullArgumentTest()
    {
        // Arrange, Act, Assert
        await this.RunTestAsync(
            (u, lm, c) =>
                Assert.ThrowsAsync<ArgumentNullException>(() => u.Repository.AddRangeAsync(null!)));

    }

    [Fact]
    public async Task UpdateAsyncMethodTest()
    {
        await this.RunTestAsync(async (u, lm, c) =>
        {
            // Arrange
            var updatedEntity = new MyEntity(this.Entities[0].Id) { Name = "Updated" };

            // Act
            await u.Repository.UpdateAsync(updatedEntity);
            var result = await u.Repository.FindAsync(updatedEntity.Id, c);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, updatedEntity);
            Assert.Equal(updatedEntity.Name, result.Name);
            lm.Received().LogUpdateEntityAttempt(updatedEntity.Id);
        });
    }

    [Fact]
    public async Task UpdateAsyncMethodNullArgumentTest()
    {
        await this.RunTestAsync(async (u, lm, c) =>
        {
            // Arrange, Act, Assert
            await Assert.ThrowsAsync<ArgumentNullException>(() => u.Repository.UpdateAsync(null!));
        });
    }

    [Fact]
    public async Task UpdateRangeAsyncMethodTest()
    {
        await this.RunTestAsync(async (u, lm, c) =>
        {
            // Arrange
            var updatedEntity = new MyEntity(this.Entities[0].Id) { Name = "Updated" };

            // Act
            await u.Repository.UpdateRangeAsync([updatedEntity], c);
            await u.CommitAsync();
            var result = await u.Repository.FindAsync(updatedEntity.Id, c);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(result, updatedEntity);
            Assert.Equal(updatedEntity.Name, result.Name);
            lm.Received().LogTraceMethod(nameof(u.Repository.UpdateRangeAsync), [updatedEntity]);
            lm.Received().LogUpdateRangeEntity(1);
        });
    }

    [Fact]
    public async Task RemoveAsyncMethodTest()
    {
        await this.RunTestAsync(async (u, lm, c) =>
        {
            // Arrange, Act
            await u.Repository.RemoveAsync(this.Entities[2]);
            await u.CommitAsync();
            var remaining = await u.Repository.FindAsync(this.Entities[2].Id, c);

            // Assert
            Assert.Null(remaining);
        });
    }

    [Fact]
    public async Task RemoveAsyncMethodWithNullEntityTest()
    {
        await this.RunTestAsync((u, lm, c) =>
            Assert.ThrowsAsync<ArgumentNullException>(() =>
                u.Repository.RemoveAsync(null!)));
    }

    [Fact]
    public async Task RemoveRangeAsyncMethodTest()
    {
        await this.RunTestAsync(async (u, lm, c) =>
        {
            // Arrange, Act
            await u.Repository.RemoveRangeAsync([this.Entities[2]], c);
            await u.CommitAsync();
            var remaining = await u.Repository.FindAsync(this.Entities[2].Id, c);

            // Assert
            Assert.Null(remaining);
        });
    }

    [Fact]
    public async Task DisposeAsyncMethodTest()
    {
        await this.RunTestAsync(async (u, lm, c) =>
        {
            await u.Repository.DisposeAsync();

            try
            {
                _ = await u.Repository.FindAsync(this.Entities[0].Id, c);
                Assert.Fail("The repository was not disposed");
            }
            catch (ObjectDisposedException)
            {
            }

        });
    }

    [Fact]
    public async Task DisposeMethodTest()
    {
        await this.RunTestAsync(async (u, lm, c) =>
        {
#pragma warning disable CA1849
            u.Repository.Dispose();
#pragma warning restore CA1849

            try
            {
                _ = await u.Repository.FindAsync(this.Entities[0].Id, c);
                Assert.Fail("The repository was not disposed");
            }
            catch (ObjectDisposedException)
            {
            }
        });
    }

    private async Task RunTestAsync(
        Func<MyUnitOfWork, ILogger, CancellationToken, Task> runnerAsync,
        Func<TestDbContext, CancellationToken, Task>? setupAsync = null)
    {
        await this.RunTestAsync(async (context, cancellationToken) =>
        {
            // Arrange
            var loggerFactory = Substitute.For<ILoggerFactory>();
            var loggerMock = Substitute.For<ILogger<RepositoryBase<MyEntity, MyEntityId, TestDbContext>>>();
            loggerFactory.CreateLogger<RepositoryBase<MyEntity, MyEntityId, TestDbContext>>().Returns(loggerMock);

            using var u = new MyUnitOfWork(context, loggerFactory);

            // Act, Assert
            await runnerAsync(u, loggerMock, cancellationToken);
        },
        setupAsync);

    }
}
