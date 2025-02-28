using C3.Blocks.Repository.MsSql.Tests.Stubs;
using Xunit.Sdk;

namespace C3.Blocks.Repository.MsSql.Tests;

public class PaginatedListFactoriesTests : TestBase
{
    [Fact]
    public async Task CreatePaginatedListMethodTest()
    {
        await this.RunTestAsync(async (context, cancellationToken) =>
        {
            // Arrange
            var page = 1;
            var size = 20;
            //Act
            var result = await context.Set<MyEntity>().OrderBy(e => e.Name).CreatePaginatedListAsync(page, size, cancellationToken);

            // Assert
            Assert.Equal(size, result.Items.Count);

            foreach (var index in Enumerable.Range(0, 20))
            {
                Assert.Equal(this.Entities[index], result.Items[index]);
            }
            Assert.Equal(page, result.Page);
            Assert.Equal(size, result.Size);
            Assert.Equal(this.Entities.Count, result.Total);
        });
    }

    [Fact]
    public async Task CreateKeysetPaginatedListAsyncMethodBeforeAfterTest()
    {
        await this.RunTestAsync(
            async (context, c) =>
            {
                // Arrange
                var size = 1;
                var expectedItems = this.Entities.OrderBy(e => e.CreatedAt).Take(3).ToList();

                var expectedBefore = expectedItems[0];
                var expectedMiddle = expectedItems[1];
                var expectedAfter = expectedItems[2];

                // Act
                var before = await context.Set<MyEntity>().CreateKeysetPaginatedListAsync(
                    e => e.CreatedAt,
                    size,
                    before: expectedMiddle.CreatedAt,
                    after: default,
                    cancellationToken: default
                );

                var after = await context.Set<MyEntity>().CreateKeysetPaginatedListAsync(
                    e => e.CreatedAt,
                    size,
                    before: default,
                    after: expectedMiddle.CreatedAt,
                    c
                );

                // Assert
                Assert.True(expectedBefore.CreatedAt < expectedMiddle.CreatedAt);
                Assert.True(expectedMiddle.CreatedAt < expectedAfter.CreatedAt);
                Assert.Equal(size, before.Size);
                Assert.Equal(expectedBefore, before.Items[0]);
                Assert.Equal(size, after.Size);
                Assert.Equal(expectedAfter, after.Items[0]);
            }
        );
    }

    [Fact]
    public async Task CreateKeysetPaginatedListAsyncMethodTest()
    {
        await this.RunTestAsync(
            async (context, c) =>
            {
                // Arrange
                var size = 20;
                var expectedPage1 = this.Entities.OrderBy(e => e.CreatedAt).Take(size).ToList(); // before
                var expectedPage2 = this.Entities.OrderBy(e => e.CreatedAt).Skip(20).Take(size).ToList(); // after

                // Act
                var defaultPage1 = await context.Set<MyEntity>().CreateKeysetPaginatedListAsync(
                    e => e.CreatedAt,
                    size,
                    cancellationToken: default);

                var page2StartDate = expectedPage2[0].CreatedAt;
                var page1 = await context.Set<MyEntity>().CreateKeysetPaginatedListAsync(
                    e => e.CreatedAt,
                    size,
                    before: page2StartDate,
                    cancellationToken: default
                );

                var page1EndDate = expectedPage1[19].CreatedAt;
                var page2 = await context.Set<MyEntity>().CreateKeysetPaginatedListAsync(
                    e => e.CreatedAt,
                    size,
                    after: page1EndDate,
                    cancellationToken: default
                );

                // Assert
                foreach (var index in Enumerable.Range(0, 20))
                {
                    Assert.Equal(expectedPage1[index], defaultPage1.Items[index]);
                }

                foreach (var index in Enumerable.Range(0, 20))
                {
                    Assert.Equal(expectedPage1[index], page1.Items[index]);
                }

                foreach (var index in Enumerable.Range(0, 20))
                {
                    Assert.Equal(expectedPage2[index], page2.Items[index]);
                }

            }
        );
    }

    [Fact]
    public async Task CreateKeysetPaginatedListAsyncWithNoResultsMethodTest()
    {
        await this.RunTestAsync(
            async (context, c) =>
            {
                var empty = await context.Set<MyEntity>()
                    .Where(d => d.Name == "I do not exist")
                    .CreateKeysetPaginatedListDescendingAsync(d => d.CreatedAt, 20, cancellationToken: c);
            }
        );
    }

    [Fact]
    public async Task CreateKeysetPaginatedListAsyncMethodInvalidArgumentsTest()
    {
        await RunTestAsync(
            async (context, c) =>
            {
                await Assert.ThrowsAsync<ArgumentException>(() => context.Set<MyEntity>().CreateKeysetPaginatedListAsync(
                    e => e.CreatedAt,
                    20,
                    before: DateTime.UtcNow,
                    after: DateTime.UtcNow,
                    c
                ));
            }
        );
    }

    [Fact]
    public async Task CreateKeysetPaginatedListDescendingAsyncWithNoResultsMethodTest()
    {
        await this.RunTestAsync(
            async (context, c) =>
            {
                var empty = await context.Set<MyEntity>()
                    .Where(d => d.Name == "I do not exist")
                    .CreateKeysetPaginatedListDescendingAsync(d => d.CreatedAt, 20, cancellationToken: c);
            }
        );

    }


    [Fact]
    public async Task CreateKeysetPaginatedListDescendingAsyncMethodBeforeAfterTest()
    {
        await this.RunTestAsync(
            async (context, c) =>
            {
                // Arrange
                var size = 1;
                var expectedItems = this.Entities.OrderByDescending(e => e.CreatedAt).Take(3).ToList();

                var expectedBefore = expectedItems[2];
                var expectedMiddle = expectedItems[1];
                var expectedAfter = expectedItems[0];

                // Act
                var before = await context.Set<MyEntity>().CreateKeysetPaginatedListDescendingAsync(
                    e => e.CreatedAt,
                    size,
                    before: expectedMiddle.CreatedAt,
                    after: default,
                    cancellationToken: default
                );

                var after = await context.Set<MyEntity>().CreateKeysetPaginatedListDescendingAsync(
                    e => e.CreatedAt,
                    size,
                    before: default,
                    after: expectedMiddle.CreatedAt,
                    c
                );

                // Assert
                Assert.True(expectedBefore.CreatedAt < expectedMiddle.CreatedAt);
                Assert.True(expectedMiddle.CreatedAt < expectedAfter.CreatedAt);
                Assert.Equal(size, before.Size);
                Assert.Equal(expectedBefore, before.Items[0]);
                Assert.Equal(size, after.Size);
                Assert.Equal(expectedAfter, after.Items[0]);
            }
        );
    }
    [Fact]
    public async Task CreateKeysetPaginatedListDescendingAsyncMethodTest()
    {
        await this.RunTestAsync(
            async (context, c) =>
            {
                // Arrange
                var size = 20;
                var expectedPage1 = this.Entities.OrderByDescending(e => e.CreatedAt).Take(size).ToList(); // after
                var expectedPage2 = this.Entities.OrderByDescending(e => e.CreatedAt).Skip(20).Take(size).ToList(); // before

                // Act
                var defaultPage = await context.Set<MyEntity>().CreateKeysetPaginatedListDescendingAsync(
                    e => e.CreatedAt,
                    size,
                    cancellationToken: default);

                var page2StartDate = expectedPage2[0].CreatedAt;
                var page1 = await context.Set<MyEntity>().CreateKeysetPaginatedListDescendingAsync(
                    e => e.CreatedAt,
                    size,
                    after: page2StartDate,
                    cancellationToken: default
                );

                var page1EndDate = expectedPage1[19].CreatedAt;
                var page2 = await context.Set<MyEntity>().CreateKeysetPaginatedListDescendingAsync(
                    e => e.CreatedAt,
                    size,
                    before: page1EndDate,
                    cancellationToken: default
                );

                // Assert
                foreach (var index in Enumerable.Range(0, 20))
                {
                    Assert.Equal(expectedPage1[index], defaultPage.Items[index]);
                }

                foreach (var index in Enumerable.Range(0, 20))
                {
                    Assert.Equal(expectedPage1[index], page1.Items[index]);
                }

                foreach (var index in Enumerable.Range(0, 20))
                {
                    Assert.Equal(expectedPage2[index], page2.Items[index]);
                }

            }
        );
    }

    [Fact]
    public async Task CreateKeysetPaginatedListDescendingAsyncMethodInvalidArgumentsTest()
    {
        await RunTestAsync(
            async (context, c) =>
            {
                await Assert.ThrowsAsync<ArgumentException>(() => context.Set<MyEntity>().CreateKeysetPaginatedListDescendingAsync(
                    e => e.CreatedAt,
                    20,
                    before: DateTime.UtcNow,
                    after: DateTime.UtcNow,
                    c
                ));
            }
        );
    }
}
