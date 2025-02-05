namespace C3.Blocks.Repository.MsSql.Tests.Stubs;

public class MyUnitOfWork(TestDbContext context, ILoggerFactory loggerFactory) : UnitOfWork<TestDbContext>(context, loggerFactory)
{
    public RepositoryBase<MyEntity, MyEntityId, TestDbContext> Repository { get; } =
        new RepositoryBase<MyEntity, MyEntityId, TestDbContext>(context, loggerFactory.CreateLogger<RepositoryBase<MyEntity, MyEntityId, TestDbContext>>());

    protected override void Dispose(bool disposing)
    {
        base.Dispose(disposing);

        if (disposing)
        {
            this.Repository?.Dispose();
        }
    }
}
