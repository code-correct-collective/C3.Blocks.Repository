using System.Diagnostics.CodeAnalysis;

namespace C3.Blocks.Repository.MsSql;

/// <summary>
/// Provides extension methods for the ModelBuilder class.
/// </summary>
public static class ModelBuilderExtensions
{
    /// <summary>
    /// Sets the DateTimeKind for all DateTime properties in the model.
    /// </summary>
    /// <param name="modelBuilder">The ModelBuilder instance.</param>
    /// <param name="kind">The DateTimeKind to set. Default is DateTimeKind.Utc.</param>
    /// <returns>The updated ModelBuilder instance.</returns>
    /// <exception cref="ArgumentNullException" />
    public static ModelBuilder SetDateTimeKind(this ModelBuilder modelBuilder, DateTimeKind kind = DateTimeKind.Utc)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));
        var dateTimeConverter = new ValueConverter<DateTime, DateTime>(v => v, v => DateTime.SpecifyKind(v, kind));

        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            foreach (var property in entityType.GetProperties())
            {
                if (property.ClrType == typeof(DateTime) || property.ClrType == typeof(DateTime?))
                {
                    property.SetValueConverter(dateTimeConverter);
                }
            }
        }

        return modelBuilder;
    }

    /// <summary>
    /// Fixes the `DateTimeOffset` properties ONLY when the `DbContext.Database.ProviderName` is for SQLite by using the `DateTimeOffsetToBinaryConverter`.
    /// </summary>
    /// <param name="modelBuilder">The ModelBuilder instance.</param>
    /// <param name="databaseProviderName">The name of the database provider.</param>
    /// <exception cref="ArgumentNullException" />
    [ExcludeFromCodeCoverage(Justification = "This is used only for testing/sqlite")]
    public static void FixDateTimeOffsetForSqlite(this ModelBuilder modelBuilder, string? databaseProviderName)
    {
        ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));


        if (databaseProviderName is string providerName && providerName == "Microsoft.EntityFrameworkCore.Sqlite")
        {
            // SQLite does not have proper support for DateTimeOffset via Entity Framework Core, see the limitations
            // here: https://docs.microsoft.com/en-us/ef/core/providers/sqlite/limitations#query-limitations
            // To work around this, when the Sqlite database provider is used, all model properties of type DateTimeOffset
            // use the DateTimeOffsetToBinaryConverter
            // Based on: https://github.com/aspnet/EntityFrameworkCore/issues/10784#issuecomment-415769754
            // This only supports millisecond precision, but should be sufficient for most use cases.
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                var properties = entityType.ClrType.GetProperties().Where(p => p.PropertyType == typeof(DateTimeOffset)
                                                                            || p.PropertyType == typeof(DateTimeOffset?));
                foreach (var property in properties)
                {
                    modelBuilder
                        .Entity(entityType.Name)
                        .Property(property.Name)
                        .HasConversion(new DateTimeOffsetToBinaryConverter());
                }
            }
        }
    }
}
