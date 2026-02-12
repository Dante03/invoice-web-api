using invoice_web_api.Dtos;

namespace invoice_web_api.Services
{
    public static class InvoiceCalculator
    {
        public static InvoiceTotals Calculate(
            List<InvoiceItemDto> items,
            decimal tax,
            decimal discount,
            string type
        )
        {
            decimal subtotal = 0;
            decimal discountTotal = 0;
            decimal taxTotal = 0;

            foreach (var item in items)
            {
                var baseAmount = item.Price * item.Qty;
                var discountAmount = baseAmount * (discount / 100);
                var taxPercent = tax / 100;


                decimal taxAmount = type == "service"
                    ? (baseAmount - discountAmount) * taxPercent
                    : baseAmount * taxPercent;


                subtotal += baseAmount;
                discountTotal += discountAmount;
                taxTotal += taxAmount;
            }

            return new InvoiceTotals
            {
                Subtotal = subtotal,
                Discount = discountTotal,
                Tax = taxTotal,
                Total = subtotal - discountTotal + taxTotal
            };
        }
    }
}
