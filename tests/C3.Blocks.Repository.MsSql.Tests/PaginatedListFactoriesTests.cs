using C3.Blocks.Repository.MsSql.Tests.Stubs;
using Microsoft.VisualStudio.TestPlatform.CoreUtilities.Tracing.Interfaces;

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
    public async Task CreateKeysetPaginatedListAsyncMethodTest()
    {
        await this.RunTestAsync(
            async (context, c) =>
            {
                // Arrange
                var size = 20;
                var expectedItemsOne = this.Entities.OrderBy(e => e.CreatedAt).Take(size).ToList();
                var expectedItemsTwo = this.Entities.OrderBy(e => e.CreatedAt).Skip(20).Take(size);

                // Act
                var itemsOne = await context.Set<MyEntity>().CreateKeysetPaginatedListAsync(
                    e => e.CreatedAt,
                    size,
                    default);
                var itemsTwo = await context.Set<MyEntity>().CreateKeysetPaginatedListAsync(
                    e => e.CreatedAt,
                    size,
                    default,
                    itemsOne.Items.Max(e => e.CreatedAt)
                );

                // Assert
                foreach (var index in Enumerable.Range(0, 20))
                {
                    Assert.Equal(expectedItemsOne[index], itemsOne.Items[index]);
                }
                foreach (var index in Enumerable.Range(0, 20))
                {
                    Assert.Equal(expectedItemsOne[index], itemsOne.Items[index]);
                }
            });
    }

    [Fact]
    public async Task CreateKeysetPaginatedListAsyncWithNoResultsMethodTest()
    {
        await this.RunTestAsync(
            async (context, c) =>
            {
                var empty = await context.Set<MyEntity>()
                    .Where(d => d.Name == "I do not exist")
                    .CreateKeysetPaginatedListDescendingAsync(d => d.CreatedAt, 20, c);
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
                    .CreateKeysetPaginatedListDescendingAsync(d => d.CreatedAt, 20, c);
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
                var expectedItemsOne = this.Entities.OrderByDescending(e => e.CreatedAt).Take(size).ToList();
                var expectedItemsTwo = this.Entities.OrderByDescending(e => e.CreatedAt).Skip(20).Take(size);

                // Act
                var itemsOne = await context.Set<MyEntity>().CreateKeysetPaginatedListDescendingAsync(
                    e => e.CreatedAt,
                    size,
                    default);
                var itemsTwo = await context.Set<MyEntity>().CreateKeysetPaginatedListDescendingAsync(
                    e => e.CreatedAt,
                    size,
                    default,
                    itemsOne.Items.Max(e => e.CreatedAt)
                );

                // Assert
                foreach (var index in Enumerable.Range(0, 20))
                {
                    Assert.Equal(expectedItemsOne[index], itemsOne.Items[index]);
                }
                foreach (var index in Enumerable.Range(0, 20))
                {
                    Assert.Equal(expectedItemsOne[index], itemsOne.Items[index]);
                }
            });
    }
}
