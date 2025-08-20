using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokimiStorageKeysLib.concrete_keys;

namespace UserProfilesService.Infrastructure.persistence.configurations.value_converters;

public class AppUserProfilePicKeyConverter : ValueConverter<UserProfilePicKey, string>
{
    public AppUserProfilePicKeyConverter() : base(
        id => id.ToString(),
        value => new UserProfilePicKey(value)
    ) { }
}