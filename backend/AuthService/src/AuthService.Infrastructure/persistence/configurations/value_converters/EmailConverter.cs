using AuthService.Domain.common;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AuthService.Infrastructure.persistence.configurations.value_converters;

public class EmailConverter : ValueConverter<Email, string>
{
    public EmailConverter() : base(
        id => id.ToString(),
        value => Email.Create(value).AsSuccess()
    ) { }
}