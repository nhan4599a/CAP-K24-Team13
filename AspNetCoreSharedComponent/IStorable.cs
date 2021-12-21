namespace AspNetCoreSharedComponent
{
    public interface IStorable
    {
        Task<string[]> SaveFilesAsync(IFormFileCollection files);

        Task<string[]> EditFilesAsync(string[] oldFilesName, IFormFileCollection files);
    }
}
