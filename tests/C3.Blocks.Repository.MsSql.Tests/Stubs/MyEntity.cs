using C3.Blocks.Domain;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics;

namespace C3.Blocks.Repository.MsSql.Tests.Stubs;

[PrimaryKey(nameof(Id))]
[DebuggerDisplay("Name: {" + nameof(Name) + "}; Created: {" + nameof(CreatedAt) + "}")]
public class MyEntity(MyEntityId id) : EntityBase<MyEntityId>(id)
{
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;

    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public override MyEntityId Id { get; protected set; }

    public DateTime CreatedAt { get; set; }

    public MyEntity()
        : this(new MyEntityId(Guid.NewGuid()))
    {
    }
}
