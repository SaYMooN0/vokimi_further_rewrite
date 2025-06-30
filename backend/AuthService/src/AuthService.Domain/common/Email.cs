using System.Text.RegularExpressions;
using SharedKernel.domain;
using SharedKernel.errs;
using SharedKernel.errs.utils;
using SharedKernel.exceptions;

namespace AuthService.Domain.common;

public class Email : ValueObject
{
    private string _email;

    private Email(string email) {
        if (!IsStringValidEmail(email)) {
            InvalidConstructorArgumentException.ThrowErr(ErrFactory.IncorrectFormat("Email format"));
        }

        _email = email;
    }

    private const string EmailRegex = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
    public override string ToString() => _email;
    public override IEnumerable<object> GetEqualityComponents() => [_email];

    public static ErrOr<Email> Create(string email) {
        if (!IsStringValidEmail(email)) {
            return ErrFactory.IncorrectFormat("Invalid email");
        }

        return new Email(email);
    }


    public static bool IsStringValidEmail(string email) =>
        !string.IsNullOrWhiteSpace(email) && Regex.IsMatch(email, EmailRegex);
}