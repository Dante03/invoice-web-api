namespace invoice_web_api.Services
{
    public class OptionsServices
    {
        public string SUPABASE_URL { get; set; }
        public string SUPABASE_SERVICE_KEY { get; set; }
        public string SUPABASE_BUCKET { get; set; }
        public string JWTKEY { get; set; }
        public string JWTISSUER { get; set; }
        public string JWTAUDIENCE { get; set; }
        public string ConnectionString { get; set; }
        public double ExpireMinutes { get; set; }
    }
}
