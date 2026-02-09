namespace invoice_web_api.Interfaces
{
    public interface ISupabaseStorageService
    {
        Task<string> UploadFile(IFormFile file, string fileName, string directory);
        Task<string> DownloadFile(string path);
    }
}
