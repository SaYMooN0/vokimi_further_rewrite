using System.Collections.Immutable;
using DraftVokisLib;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GeneralVokiCreationService.Infrastructure.persistence.value_converters;

public class VokiCoAuthorIdsSetConverter : ValueConverter<VokiCoAuthorIdsSet, Guid[]>
{
    public VokiCoAuthorIdsSetConverter() : base(
        ids => ids.ToArray().Select(i => i.Value).ToArray(),
        value => VokiCoAuthorIdsSet.Create(
            value.Select(i => new AppUserId(i)).ToImmutableHashSet()
        ).AsSuccess()
    ) { }
}