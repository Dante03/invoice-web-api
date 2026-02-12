namespace invoice_web_api.Dtos
{
    public class InvoiceItemDto
    {
        public string Description { get; set; } = default!;
        public string Type { get; set; } = default!;
        public int Qty { get; set; }
        public decimal Price { get; set; }
    }
}
