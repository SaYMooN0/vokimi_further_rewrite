using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using VokimiStorageKeysLib;
using VokimiStorageKeysLib.base_keys;

namespace VokiCreationServicesLib.Infrastructure.persistence.value_converters;

public class StorageKeyConverter<T> : ValueConverter<T, string> where T : BaseStorageKey
{
    public StorageKeyConverter() : base(
        id => id.ToString(),
        value => (T)Activator.CreateInstance(typeof(T), value)
    ) { }
}