using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel;
using SharedKernel.common;
using SharedKernel.common.app_users;

namespace InfrastructureShared.persistence.value_converters;

public class AppUserNameConverter : ValueConverter<AppUserName, string>
{
    public AppUserNameConverter() : base(
        id => id.ToString(),
        value => AppUserName.Create(value).AsSuccess()
    ) { }
}

