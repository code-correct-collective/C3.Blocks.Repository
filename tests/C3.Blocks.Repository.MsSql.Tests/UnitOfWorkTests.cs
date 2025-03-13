using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data;

namespace C3.Blocks.Repository.MsSql.Tests;

public class UnitOfWorkTests
{
    [Fact]
    public async Task CommitAsyncMethodTest()
    {
        await RunSimpleTest(async (u, c, lm) =>
        {
            await u.CommitAsync();

            // Arrange
            lm.Received(1).LogTraceMethod(nameof(u.CommitAsync), []);
            await c.Received(1).SaveChangesAsync();
        });
    }

    [Fact]
    public async Task BeginTransactionMethodTest()
    {
        await RunTransactionTests((u, c, db, lm) =>
        {
            // Act
#pragma warning disable CA1849
            _ = u.BeginTransaction();

            // Assert
            _ = c.Received(1).Database;
            db.Received(1).BeginTransaction();
            lm.Received(1).LogTraceMethod(nameof(u.BeginTransaction), []);
            return Task.CompletedTask;
#pragma warning restore CA1849
        });
    }

    [Fact(Skip = "This test throws a SystemInvalidOperationException, Not sure how to work around it.")]
    public async Task BeginTransactionMethodWithIsolationTest()
    {
        await RunTransactionTests((u, c, db, lm) =>
        {
            // Arrange
            var i = IsolationLevel.Unspecified;

            // Act
#pragma warning disable CA1849
            _ = u.BeginTransaction(i);

            // Assert
            _ = c.Received(1).Database;
            db.Received(1).BeginTransaction(i);
            lm.Received(1).LogTraceMethod(nameof(u.BeginTransaction), [i]);
            return Task.CompletedTask;
#pragma warning restore CA1849
        });
    }

    [Fact]
    public async Task BeginTransactionAsyncMethodTest()
    {
        await RunTransactionTests(async (u, c, db, lm) =>
        {
            // Act
            _ = await u.BeginTransactionAsync();

            // Assert
            _ = c.Received(1).Database;
            _ = await db.Received(1).BeginTransactionAsync();
            lm.Received(1).LogTraceMethod(nameof(u.BeginTransaction), []);
        });
    }

    [Fact(Skip = "This test throws a SystemInvalidOperationException, Not sure how to work around it.")]
    public async Task BeginTransactionAsyncMethodWithIsolationLevelTest()
    {
        await RunTransactionTests(async (u, c, db, lm) =>
        {
            // Arrange
            var i = IsolationLevel.Unspecified;
            // Act
            _ = await u.BeginTransactionAsync(i);

            // Assert
            _ = c.Received(1).Database;
            _ = await db.Received(1).BeginTransactionAsync(i);
            lm.Received(1).LogTraceMethod(nameof(u.BeginTransaction), [i]);
        });
    }

    [Fact(Skip = "Both solutions throw NSubstitute.Exceptions.RedundantArgumentMatcherException")]
    public async Task ExecuteInResilientTransactionAsyncMethodTest()
    {
        await RunTransactionTests(async (u, c, db, lm) =>
        {
            // Arrange
            var executionStrategy = Substitute.For<IExecutionStrategy>();
            db.CreateExecutionStrategy().Returns(executionStrategy);

            // Possible Solution 1
            // executionStrategy.ExecuteAsync(Arg.Any<Func<Task>>())
            //     .Returns(async callInfo =>
            //     {
            //         var operation = callInfo.Arg<Func<Task>>();
            //         await operation!();
            //     });

            // Possible Solution 2
            // executionStrategy.When(x => x.ExecuteAsync(Arg.Any<Func<Task>>()))
            //     .Do(callInfo =>
            //     {
            //         var operation = callInfo.Arg<Func<Task>>();
            //         operation().Wait();
            //     });

            // Act
            await u.ExecuteInResilientTransactionAsync(() =>
            {
                return Task.FromResult(true);
            });

            // Assert;
            await db.Received(1).BeginTransactionAsync(default);
        });
    }

    [Fact]
    public async Task DisposeMethodTest()
    {
        await RunSimpleTest(async (u, c, lm) =>
        {
            // Act
#pragma warning disable CA1849
            u.Dispose();
#pragma warning restore CA1849

            try
            {
                await u.CommitAsync();
                Assert.Fail("Object was not properly disposed");
            }
            catch (ObjectDisposedException)
            {
            }
        });
    }

    [Fact]
    public async Task DisposeAsyncMethodTest()
    {
        await RunSimpleTest(async (u, c, lm) =>
        {
            // Act
            await u.DisposeAsync();

            try
            {
                await u.CommitAsync();
                Assert.Fail("Object was not properly disposed");
            }
            catch (ObjectDisposedException)
            {
            }
        });
    }

    private static async Task RunSimpleTest(Func<UnitOfWork<DbContext>, DbContext, ILogger, Task> operation)
    {
        // Arrange
        var c = Substitute.For<DbContext>();
        c.SaveChangesAsync().Returns(Task.FromResult(1));
        var lm = Substitute.For<ILogger<UnitOfWork<DbContext>>>();
        var lfm = Substitute.For<ILoggerFactory>();
        lfm.CreateLogger<UnitOfWork<DbContext>>().Returns(lm);
        using var u = new UnitOfWork<DbContext>(c, lfm);

        // Act
        await operation(u, c, lm);
    }
    private static async Task RunTransactionTests(Func<UnitOfWork<DbContext>, DbContext, DatabaseFacade, ILogger<UnitOfWork<DbContext>>, Task> runnerAsync)
    {
        // Arrange
        var c = Substitute.For<DbContext>();
        var db = Substitute.For<DatabaseFacade>(c);
        c.Database.Returns(db);
        var lm = Substitute.For<ILogger<UnitOfWork<DbContext>>>();
        var lfm = Substitute.For<ILoggerFactory>();
        lfm.CreateLogger<UnitOfWork<DbContext>>().Returns(lm);
        using var u = new UnitOfWork<DbContext>(c, lfm);

        await runnerAsync(u, c, db, lm);
    }
}
