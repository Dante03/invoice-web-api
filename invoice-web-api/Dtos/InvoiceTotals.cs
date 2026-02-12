namespace invoice_web_api.Dtos
{
    public class InvoiceTotals
    {
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
    }
}
