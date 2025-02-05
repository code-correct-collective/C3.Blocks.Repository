using C3.Blocks.EntityFramework.Testing;
using C3.Blocks.Repository.MsSql.Tests.Stubs;

namespace C3.Blocks.Repository.MsSql.Tests;

public class TestBase : EntityFrameworkSqliteTestBase<TestDbContext>
{
    protected IList<MyEntity> Entities { get; } = new List<MyEntity>();

    protected async Task RunTestAsync(Func<TestDbContext, CancellationToken, Task> runnerAsync, Func<TestDbContext, CancellationToken, Task>? setupAsync = null)
    {
        await base.RunTestAsync(
            runnerAsync,
            async (context, cancellation) =>
            {
                this.PopulateEntities();
                await context.AddRangeAsync(this.Entities.ToArray());
                await context.SaveChangesAsync(cancellation);

                if (setupAsync != null)
                {
                    await setupAsync(context, cancellation);
                }
            }
        );
    }

    private void PopulateEntities()
    {
        foreach (var item in Enumerable.Range(0, 100))
        {
            this.Entities.Add(new MyEntity
            {
                Name = $"d{item:D2}",
                CreatedAt = DateTime.UtcNow.AddMinutes(item * -1)
            });
        }
    }
}
