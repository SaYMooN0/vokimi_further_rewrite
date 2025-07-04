using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.domain.ids;

namespace InfrastructureShared.persistence.value_converters;

public class GuidBasedIdConverter<TId> : ValueConverter<TId, Guid> where TId : GuidBasedId
{
    public GuidBasedIdConverter() : base(
        id => id.Value,
        value => (TId)Activator.CreateInstance(typeof(TId), value)
    ) { }
}