using System.Runtime.CompilerServices;

namespace SharedKernel.exceptions;

public class InvalidConstructorArgumentException : Exception
{
    public Err Err { get; }
    public string Caller { get; }

    private InvalidConstructorArgumentException(Err err, string caller) : base(err.Message) {
        Err = err;
        Caller = caller;
    }

    public static void ThrowIfErr(
        ErrOrNothing possibleErr,
        [CallerMemberName] string memberName = ""
    ) {
        if (possibleErr.IsErr(out var err)) {
            throw new InvalidConstructorArgumentException(err, memberName);
        }
    }

    public static void ThrowErr(
        Err err, [CallerMemberName] string memberName = ""
    ) => throw new InvalidConstructorArgumentException(err, memberName);
}