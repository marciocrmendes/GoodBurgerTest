namespace GoodBurger.Api.Domain.Abstractions;

public abstract class BaseEntity
{
    public DateTime CreatedAt { get; set; }
    public Guid CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public Guid? UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public Guid? DeletedBy { get; set; }

    public bool IsDeleted => DeletedAt.HasValue;
}

public abstract class BaseEntity<TId> : BaseEntity
    where TId : struct
{
    public TId Id { get; set; }
}
