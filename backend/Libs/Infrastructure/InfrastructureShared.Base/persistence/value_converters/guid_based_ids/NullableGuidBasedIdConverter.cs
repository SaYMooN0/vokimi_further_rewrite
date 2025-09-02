using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InfrastructureShared.Base.persistence.value_converters.guid_based_ids;

public class NullableGuidBasedIdConverter<TId> : ValueConverter<TId?, Guid?> where TId : GuidBasedId
{
    public NullableGuidBasedIdConverter() : base(
        id => NullableEntityIdToGuid(id),
        value => value.HasValue
            ? (TId)Activator.CreateInstance(typeof(TId), value.Value)
            : null
    ) { }

    private static Guid? NullableEntityIdToGuid(GuidBasedId? id) => id?.Value;
}