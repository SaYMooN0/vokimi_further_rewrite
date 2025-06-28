namespace SharedKernel.auth;

public class JwtTokenString
{
    private readonly string _value;

    public JwtTokenString(string value) {
        _value = value;
    }

    public override string ToString() => _value;
}