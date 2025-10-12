using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using SharedKernel.common.app_users;

namespace InfrastructureShared.Base.persistence.value_converters;

public class UserUniqueNameConverter : ValueConverter<UserUniqueName, string>
{
    public UserUniqueNameConverter() : base(
        id => id.ToString(),
        value => UserUniqueName.Create(value).AsSuccess()
    ) { }
}

