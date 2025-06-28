namespace SharedKernel.errs;

public class ErrOrNothing
{
    private Err? _err;

    private ErrOrNothing(Err? err) {
        _err = err;
    }

    public bool IsErr() => _err != null;

    public bool IsErr(out Err err) {
        if (_err != null) {
            err = _err;
            return true;
        }

        err = new Err("No error");
        return false;
    }

    public static implicit operator ErrOrNothing(Err err) => new(err);
    public static readonly ErrOrNothing Nothing = new ErrOrNothing(null);

    public override string ToString() =>
        _err is null
            ? "[ErrOrNothing]: Nothing"
            : $"[ErrOrNothing]: \n{_err}";

    public void AddNext(Err next) {
        if (_err is null) {
            _err = next;
        }
        else {
            _err.AddNext(next);
        }
    }

    public ErrOrNothing WithNext(Err next) {
        AddNext(next);
        return this;
    }

    public void AddNextIfErr<T>(ErrOr<T> errOr) {
        if (errOr.IsErr(out var err)) {
            AddNext(err);
        }
    }

    public ErrOrNothing WithNextIfErr<T>(ErrOr<T> errOr) {
        AddNextIfErr(errOr);
        return this;
    }

    public void AddNextIfErr(ErrOrNothing errOrNothing) {
        if (errOrNothing.IsErr(out var err)) {
            AddNext(err);
        }
    }

    public ErrOrNothing WithNextIfErr(ErrOrNothing errOrNothing) {
        AddNextIfErr(errOrNothing);
        return this;
    }

    public ErrOrNothing Or(ErrOrNothing second) =>
        IsErr(out var err) ? err : second;
}