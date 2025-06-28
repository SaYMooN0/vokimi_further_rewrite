using System.Text;

namespace SharedKernel.errs;

public class Err
{
    public string Message { get; init; }
    public ushort Code { get; init; }
    public string? Details { get; init; }
    public Err? Next { get; private set; }

    public Err(
        string message,
        ushort code = ErrCodes.Unspecified,
        string? details = null
    ) {
        Message = message;
        Code = code;
        Details = details;
    }

    public override string ToString() {
        StringBuilder sb = new();
        AppendToString(sb, 1);
        return sb.ToString();
    }

    private void AppendToString(StringBuilder sb, int index) {
        sb.AppendLine($"[Error {index}]");
        sb.AppendLine($"Code: {Code}");
        sb.AppendLine($"Message: {Message}");

        if (!string.IsNullOrEmpty(Details))
            sb.AppendLine($"Details: {Details}");

        if (Next is not null) {
            sb.AppendLine();
            Next.AppendToString(sb, index + 1);
        }
    }

    public void AddNext(Err next) {
        var current = this;
        while (current.Next != null) {
            current = current.Next;
        }

        current.Next = next;
    }

    public Err WithNext(Err next) {
        AddNext(next);
        return this;
    }

    public void AddNextIfErr<T>(ErrOr<T> errOr) {
        if (errOr.IsErr(out var err)) {
            AddNext(err);
        }
    }

    public Err WithNextIfErr<T>(ErrOr<T> errOr) {
        if (errOr.IsErr(out var err)) {
            AddNext(err);
        }

        return this;
    }
}