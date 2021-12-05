namespace Shared.DTOs
{
    public interface IDTO<TSource>
    {
        object MapFromSource(TSource originalObject);
    }
}
