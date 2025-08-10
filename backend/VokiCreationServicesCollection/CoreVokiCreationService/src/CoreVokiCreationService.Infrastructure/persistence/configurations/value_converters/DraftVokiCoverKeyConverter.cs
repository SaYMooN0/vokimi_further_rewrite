using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokimiStorageKeysLib.voki_cover;

namespace CoreVokiCreationService.Infrastructure.persistence.configurations.value_converters;

public class DraftVokiCoverKeyConverter : ValueConverter<VokiCoverKey, string>
{
    public DraftVokiCoverKeyConverter() : base(
        id => id.ToString(),
        value => new VokiCoverKey(value)
    ) { }
}