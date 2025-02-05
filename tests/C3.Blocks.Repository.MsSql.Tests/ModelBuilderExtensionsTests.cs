using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using System.Reflection;

namespace C3.Blocks.Repository.MsSql.Tests;

public class ModelBuilderExtensionsTests
{
    [Fact]
    public void SetDateTimeKindMethodTest()
    {
        // Arrange
        var modelBuilderMock = Substitute.ForPartsOf<ModelBuilder>();
        var modelMock = Substitute.For<IMutableModel>();
        var entityTypeMock = Substitute.For<IMutableEntityType>();
        var propertyMock = Substitute.For<IMutableProperty>();

        modelBuilderMock.Model.Returns(modelMock);
        modelMock.GetEntityTypes().Returns([entityTypeMock]);
        entityTypeMock.GetProperties().Returns([propertyMock]);
        propertyMock.ClrType.Returns(typeof(DateTime));

        // Act
        modelBuilderMock.SetDateTimeKind();

        // Assert
        _ = modelBuilderMock.Received(1).Model;
        modelMock.Received(1).GetEntityTypes();
        entityTypeMock.Received(1).GetProperties();
        _ = propertyMock.Received(1).ClrType;
        propertyMock.Received(1).SetValueConverter(Arg.Any<ValueConverter<DateTime, DateTime>>());
    }

    [Fact(Skip = "I am getting a cast error with the EntityTypeBuilder and there are some internal types that have me stumped")]
    public void FixDateTimeOffsetForSqliteMethodTest()
    {
        // Arrange
        var modelBuilderMock = Substitute.For<ModelBuilder>();
        var modelMock = Substitute.For<IMutableModel>();
        var entityTypeMock = Substitute.For<IMutableEntityType>();
        var clrTypeMock = Substitute.For<Type>();
        var propertyInfoMock = Substitute.For<PropertyInfo>();
        var entityTypeBuilderMock = Substitute.For<EntityTypeBuilder>(entityTypeMock);
        var propertyBuilderMock = Substitute.For<PropertyBuilder>();

        propertyInfoMock.PropertyType.Returns(typeof(DateTimeOffset));
        propertyInfoMock.Name.Returns("CreatedOn");
        clrTypeMock.GetProperties().Returns([propertyInfoMock]);
        modelMock.GetEntityTypes().Returns([entityTypeMock]);
        modelBuilderMock.Model.Returns(modelMock);
        entityTypeMock.ClrType.Returns(clrTypeMock);
        entityTypeMock.Name.Returns("MyEntity");

        modelBuilderMock.Entity(entityTypeMock.Name).Returns(entityTypeBuilderMock);
        entityTypeBuilderMock.Property(propertyInfoMock.Name).Returns(propertyBuilderMock);
        propertyBuilderMock.HasConversion(Arg.Any<DateTimeOffsetToBinaryConverter>()).Returns(propertyBuilderMock);

        const string ProviderName = "Microsoft.EntityFrameworkCore.Sqlite";

        // Act
        modelBuilderMock.FixDateTimeOffsetForSqlite(ProviderName);

        // Assert
        _ = modelBuilderMock.Received(1).Model;
        modelMock.Received(1).GetEntityTypes();
        _ = entityTypeMock.Received(1).ClrType;
        clrTypeMock.Received(1).GetProperties();
        _ = propertyInfoMock.Received(1).PropertyType;
        _ = propertyInfoMock.Received(1).Name;
        modelBuilderMock.Received(1).Entity(entityTypeMock.Name);
        entityTypeBuilderMock.Received(1).Property(propertyInfoMock.Name);
        propertyBuilderMock.Received(1).HasConversion(Arg.Any<DateTimeOffsetToBinaryConverter>());
    }
}
