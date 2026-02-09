using invoice_web_api.Interfaces;
using Supabase;
using System.IO;

namespace invoice_web_api.Services
{
    public class SupabaseStorageService : ISupabaseStorageService
    {
        public readonly OptionsServices _option;
        public readonly Client _supabase;
        public SupabaseStorageService(OptionsServices option)
        {
            _option = option;
            _supabase = new Client(
                _option.SUPABASE_URL,
                _option.SUPABASE_SERVICE_KEY
            );
        }

        public async Task<string> UploadFile(IFormFile file, string fileName, string directory)
        {
            await _supabase.InitializeAsync();

            string path = $"{directory}/{fileName}";
            var bucket = _option.SUPABASE_BUCKET;

            using var stream = file.OpenReadStream();
            using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);

            await _supabase.Storage
                .From(bucket)
                .Upload(ms.ToArray(), path);

            return path;
        }

        public async Task<string> DownloadFile(string path)
        {
            await _supabase.InitializeAsync();

            var bucket = _option.SUPABASE_BUCKET;

            var signedUrl = await _supabase.Storage
                .From(bucket)
                .CreateSignedUrl(path, 300); // segundos
            return signedUrl;
        }
    }
}
