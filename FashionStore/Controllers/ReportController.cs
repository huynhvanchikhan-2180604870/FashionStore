
using DinkToPdf;
using DinkToPdf.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace FashionStore.Controllers
{
    public class ReportController : Controller
    {
        private readonly IConverter _converter;
        public ReportController(IConverter converter)
        {
            _converter = converter;
        }
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult GenerateInvoice()
        {
            var htmlContent = @"
            <html>
                <head>
                    <title>Invoice</title>
                </head>
                <body>
                    <h1>Invoice</h1>
                    <p>This is a sample invoice.</p>
                    <table border='1'>
                        <tr>
                            <th>Item</th>
                            <th>Quantity</th>
                            <th>Price</th>
                        </tr>
                        <tr>
                            <td>Sample Item</td>
                            <td>1</td>
                            <td>$100.00</td>
                        </tr>
                    </table>
                    <p>Total: $100.00</p>
                </body>
            </html>";
            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4
            },
                Objects = {
                new ObjectSettings() {
                    PagesCount = true,
                    HtmlContent = htmlContent,
                    WebSettings = { DefaultEncoding = "utf-8" },
                    HeaderSettings = { FontName = "Arial", FontSize = 9, Right = "Page [page] of [toPage]", Line = true },
                    FooterSettings = { FontName = "Arial", FontSize = 9, Line = true, Center = "Invoice Footer" }
                }
            }
            };

            var file = _converter.Convert(pdf);
            return File(file, "application/pdf", "Invoice.pdf");
        }

        public IActionResult DisplayReport()
        {
            return View();
        }
    }
}
