using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokimiStorageKeysLib.voki_cover;

namespace CoreVokiCreationService.Infrastructure.persistence.configurations.value_converters;

public class VokiCoverKeyConverter : ValueConverter<VokiCoverKey, string>
{
    public VokiCoverKeyConverter() : base(
        id => id.ToString(),
        value => new VokiCoverKey(value)
    ) { }
}