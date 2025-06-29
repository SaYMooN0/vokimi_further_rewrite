using ApiShared;
using AuthService.Domain.common;
using AuthService.Domain.rules;
using SharedKernel.common;
using SharedKernel.errs;
using SharedKernel.errs.utils;

namespace AuthService.Api.contracts;

public class RegisterUserRequest : IRequestWithValidationNeeded
{
    public string Username { get; init; }
    public string Email { get; init; }
    public string Password { get; init; }

    public ErrOrNothing Validate() {
        ErrOrNothing errs = ErrOrNothing.Nothing;

        if (!Domain.common.Email.IsStringValidEmail(Email)) {
            errs.AddNext(ErrFactory.IncorrectFormat($"'{Email}' is not a invalid email"));
        }

        return errs
            .WithNextIfErr(PasswordRules.CheckForErr(Password))
            .WithNextIfErr(AppUserName.CheckForErr(Username));
    }

    public AppUserName ParsedUsername => AppUserName.Create(Username).AsSuccess();
    public Email ParsedEmail => Domain.common.Email.Create(Email).AsSuccess();
}