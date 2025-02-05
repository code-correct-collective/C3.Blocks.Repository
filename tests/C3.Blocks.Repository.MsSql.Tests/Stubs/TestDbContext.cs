namespace C3.Blocks.Repository.MsSql.Tests.Stubs;

public class TestDbContext : DbContext
{
    public TestDbContext(DbContextOptions<TestDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder = modelBuilder ?? throw new ArgumentNullException(nameof(modelBuilder));
        modelBuilder.Entity<MyEntity>()
            .Property(e => e.Id).HasConversion(v => v.Value, v => new MyEntityId(v));
    }
}
