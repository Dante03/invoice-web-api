namespace invoice_web_api.Interfaces
{
    public interface ISupabaseStorageService
    {
        Task<string> UploadFile(IFormFile file, string fileName, string directory);
        Task<byte[]> DownloadFile(string path);
        Task<string> ViewFile(string path);
    }
}
