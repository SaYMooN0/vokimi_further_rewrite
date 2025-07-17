using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokimiStorageKeysLib.draft_voki_cover;

namespace CoreVokiCreationService.Infrastructure.persistence.configurations.value_converters;

public class DraftVokiCoverKeyConverter : ValueConverter<DraftVokiCoverKey, string>
{
    public DraftVokiCoverKeyConverter() : base(
        id => id.ToString(),
        value => new DraftVokiCoverKey(value)
    ) { }
}