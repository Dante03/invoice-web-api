using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace invoice_web_api.Dtos
{
    public class CreateInvoiceDto
    {
        public string InvoiceNumber { get; set; }
        public string Name { get; set; }
        public string Directory { get; set; }
    }
}
