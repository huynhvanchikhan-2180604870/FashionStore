using DinkToPdf;
using DinkToPdf.Contracts;

namespace FashionStore.Services
{
    public class PDFService : IPDFService
    {
        private readonly IConverter _conventer;

        public PDFService(IConverter conventer)
        {
            _conventer = conventer;
        }
        public byte[] GenaratePDF(string contentHTML, Orientation orientation = Orientation.Portrait, PaperKind paperKind = PaperKind.A4)
        {
            var globalSetting = new GlobalSettings
            {
                ColorMode = ColorMode.Color,
                Orientation = Orientation.Portrait,
                PaperSize = PaperKind.A4,
                Margins = new MarginSettings() { Top = 10, Bottom = 10 }
            };

            var objectSetting = new ObjectSettings()
            {
                PagesCount = true,
                HtmlContent = contentHTML,
                WebSettings = { DefaultEncoding = "utf-8" }
            };

            var pdf = new HtmlToPdfDocument()
            {
                GlobalSettings = globalSetting,
                Objects = { objectSetting }
            };
            return _conventer.Convert(pdf);
        }
    }
}
