using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InfrastructureShared.Base.persistence.value_converters.guid_based_ids;

public class GuidBasedIdConverter<TId> : ValueConverter<TId, Guid> where TId : GuidBasedId
{
    public GuidBasedIdConverter() : base(
        id => id.Value,
        value => (TId)Activator.CreateInstance(typeof(TId), value)
    ) { }
}