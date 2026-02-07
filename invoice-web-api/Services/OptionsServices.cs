namespace invoice_web_api.Services
{
    public class OptionsServices
    {
        public string JWTKEY { get; set; }
        public string JWTISSUER { get; set; }
        public string JWTAUDIENCE { get; set; }
        public string ConnectionString { get; set; }
        public double ExpireMinutes { get; set; }
    }
}
