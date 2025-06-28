namespace SharedKernel.domain.ids;

public abstract class GuidBasedId : ValueObject, IEntityId
{
    private GuidBasedId() { }
    public Guid Value { get; }
    protected GuidBasedId(Guid value) => Value = value;
    public override string ToString() => Value.ToString();

    public override IEnumerable<object> GetEqualityComponents() {
        yield return Value;
    }

    public int CompareTo(object? obj) => obj switch {
        IEntityId ed => ToString().CompareTo(ed.ToString()),
        Guid guid => guid.CompareTo(Value),
        _ => -1
    };
}