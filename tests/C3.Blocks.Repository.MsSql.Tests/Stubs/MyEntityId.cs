using System;

namespace C3.Blocks.Repository.MsSql.Tests.Stubs;

public record struct MyEntityId
{
    public Guid Value { get; }

    public MyEntityId(Guid value)
    {
        if (value.Equals(Guid.Empty))
        {
            throw new ArgumentException($"An empty Guid is not a valid ${nameof(MyEntityId)}");
        }
        this.Value = value;
    }
}
