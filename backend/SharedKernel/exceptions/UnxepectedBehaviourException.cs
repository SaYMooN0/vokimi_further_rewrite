using SharedKernel.errs;

namespace SharedKernel.exceptions;

public class UnexpectedBehaviourException : Exception
{
    public string? Details { get; }
    public ushort ErrCode { get; }

    public UnexpectedBehaviourException(
        string message,
        string? details = null,
        ushort errCode = ErrCodes.Unspecified,
        Exception? innerException = null
    ) :
        base(message, innerException) {
        Details = details;
        ErrCode = errCode;
    }

    public UnexpectedBehaviourException(Err err) : base(err.Message) {
        Details = err.Details;
        ErrCode = err.Code;
    }
}