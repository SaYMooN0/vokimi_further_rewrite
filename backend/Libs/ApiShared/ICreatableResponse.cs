namespace ApiShared;

public interface ICreatableResponse<TSource>
{
    static abstract ICreatableResponse<TSource> Create(TSource success);
}