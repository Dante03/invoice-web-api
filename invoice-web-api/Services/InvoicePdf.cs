using invoice_web_api.Dtos;
using invoice_web_api.Entities;
using Microsoft.AspNetCore.Http;
using QuestPDF.Fluent;
using QuestPDF.Helpers;
using QuestPDF.Infrastructure;

namespace invoice_web_api.Services
{
    public class InvoicePdf : IDocument
    {
        private readonly CreateInvoiceDto _invoice;
        private readonly InvoiceTotals _totals;

        public InvoicePdf(CreateInvoiceDto invoice, InvoiceTotals totals)
        {
            _invoice = invoice;
            _totals = totals;
        }

        public DocumentMetadata GetMetadata() => DocumentMetadata.Default;

        public void Compose(IDocumentContainer container)
        {
            QuestPDF.Settings.License = LicenseType.Community;
            container.Page(page =>
            {
                page.Margin(40);
                page.Size(PageSizes.A4);
                page.DefaultTextStyle(x => x.FontSize(10));

                page.Content().Column(col =>
                {
                    col.Spacing(25);

                    col.Item().Element(Header);
                    col.Item().Element(ItemsTable);
                    col.Item().Element(FooterSection);
                });
            });
        }

        void Header(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem(2).Column(col =>
                {
                    col.Item().Text("Invoice")
                        .FontSize(28)
                        .Bold();


                    col.Item().Text(_invoice.CompanyName);
                    col.Item().Text($"{_invoice.FirstName}, {_invoice.LastName}");
                    col.Item().Text(_invoice.Website);
                    col.Item().Text(_invoice.CompanyAddress1);
                    col.Item().Text(_invoice.CompanyAddress2);
                    col.Item().Text(_invoice.Country);
                    col.Item().Text(_invoice.Phone);

                    col.Item().PaddingTop(15).Text("")
                        .FontSize(12)
                        .SemiBold()
                        .FontColor(Colors.Grey.Darken1);

                    col.Item().Text($"{_invoice.FirstNameClient}, {_invoice.LastNameClient}");
                    col.Item().Text(_invoice.ClientAddress1);
                    col.Item().Text(_invoice.ClientAddress2);
                    col.Item().Text(_invoice.Country);
                    col.Item().Text(_invoice.Email);
            });

                row.RelativeItem(1).AlignRight().Column(col =>
                {

                    byte[] imageBytes = GetBytesFromFormFile(_invoice.Logo);
                    col.Item()
                        .Padding(0)
                        .AlignCenter()
                        .Image(imageBytes)
                        .FitWidth();

                    col.Item().PaddingTop(20).Text($"Invoice No: {(_invoice.InvoiceNumber == 0 ? 1 : _invoice.InvoiceNumber).ToString("000")}");
                    col.Item().Text($"Invoice Date: {_invoice.InvoiceDate:dd/MM/yyyy}");
                    col.Item().Text($"Due Date: {_invoice.DueDate:dd/MM/yyyy}");
                });
            });
        }

        void ItemsTable(IContainer container)
        {
            container.Table(table =>
            {
                table.ColumnsDefinition(cols =>
                {
                    cols.ConstantColumn(40);
                    cols.RelativeColumn();
                    cols.ConstantColumn(80);
                    cols.ConstantColumn(80);
                });

                table.Header(header =>
                {
                    header.Cell().Element(HeaderCell).Text("ID");
                    header.Cell().Element(HeaderCell).Text("Description");
                    header.Cell().Element(HeaderCell).AlignCenter().Text("Quantity");
                    header.Cell().Element(HeaderCell).AlignRight().Text("Price");
                });

                int index = 1;
                foreach (var item in _invoice.Items)
                {
                    table.Cell().Text(index++.ToString());
                    table.Cell().Text(item.Description);
                    table.Cell().AlignCenter().Text(item.Qty.ToString());
                    table.Cell().AlignRight().Text(item.Price.ToString("0.00"));
                }
            });
        }

        static IContainer HeaderCell(IContainer container)
        {
            return container
                .Background(Colors.Black)
                .Padding(8)
                .DefaultTextStyle(x => x.FontColor(Colors.White).SemiBold());
        }

        void FooterSection(IContainer container)
        {
            container.Row(row =>
            {
                row.RelativeItem().PaddingRight(20).Border(1).Padding(1).Column(col =>
                {
                    col.Item().Text("Notes").SemiBold();
                    col.Item().Text(_invoice.Notes ?? "Any additional comments")
                        .FontColor(Colors.Grey.Darken1);
                });

                row.ConstantItem(200).Column(col =>
                {
                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Subtotal:");
                        r.ConstantItem(80).AlignRight().Text(_totals.Subtotal.ToString("0.00"));
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Tax:");
                        r.ConstantItem(80).AlignRight().Text(_totals.Tax.ToString("0.00"));
                    });

                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Discount:");
                        r.ConstantItem(80).AlignRight().Text(_totals.Discount.ToString("0.00"));
                    });

                    col.Item().PaddingTop(5).LineHorizontal(1);

                    col.Item().Row(r =>
                    {
                        r.RelativeItem().Text("Total:").Bold();
                        r.ConstantItem(80).AlignRight()
                            .Text(_totals.Total.ToString("0.00"))
                            .Bold();
                    });
                });
            });
        }
        public byte[] GetBytesFromFormFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return null;

            using var memoryStream = new MemoryStream();
            file.CopyTo(memoryStream);
            return memoryStream.ToArray();
        }
    }
}
