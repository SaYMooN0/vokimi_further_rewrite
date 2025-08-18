using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokimiStorageKeysLib.voki_cover;

namespace CoreVokiCreationService.Infrastructure.persistence.configurations.value_converters;

public class AppUserProfilePicKeyConverter : ValueConverter<VokiCoverKey, string>
{
    public AppUserProfilePicKeyConverter() : base(
        id => id.ToString(),
        value => new VokiCoverKey(value)
    ) { }
}