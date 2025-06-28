namespace ApiShared;

public record ErrResponseObject(ErrDto[] Errs);

public record class ErrDto(
    string Message,
    ushort Code,
    string? Details
)
{
    public static ErrDto FromErr(Err err) => new(
        err.Message,
        err.Code,
        err.Details
    );
}