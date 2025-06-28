using System.Text.Json;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace InfrastructureShared.persistence.value_converters;

public class ObjectToJsonConverter<T> : ValueConverter<T, string> where T : class
{
    public ObjectToJsonConverter(JsonSerializerOptions? options = null) : base(
        obj => JsonSerializer.Serialize(obj, options ?? new()),
        str => JsonSerializer.Deserialize<T>(str, options ?? new())!
    ) { }
}