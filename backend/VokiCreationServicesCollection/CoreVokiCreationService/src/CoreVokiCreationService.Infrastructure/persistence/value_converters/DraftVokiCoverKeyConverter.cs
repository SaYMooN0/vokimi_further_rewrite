using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokimiStorageKeysLib.draft_voki_cover;

namespace CoreVokiCreationService.Infrastructure.persistence.value_converters;

public class DraftVokiCoverKeyConverter : ValueConverter<DraftVokiCoverKey, string>
{
    public DraftVokiCoverKeyConverter() : base(
        id => id.Value,
        value => new DraftVokiCoverKey(value)
    ) { }
}