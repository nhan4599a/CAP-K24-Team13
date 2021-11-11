namespace Shared.DTOs
{
    public interface DTO<TSource>
    {
        object MapFromSource(TSource originalObject);
    }
}
